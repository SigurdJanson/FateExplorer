using FateExplorer.CharacterImport;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// The job of the converter is to take the identified Activatables from the importer
/// and find those which can be wrapped by a dedicated class to handle the effects 
/// of that Dis-/Advantage.
/// </summary>
/// <param name="gameData"></param>
/// <param name="characterImportM"></param>
public class DisAdvantagesConverter(IGameDataService gameData, ICharacterImporter characterImportM)
{
    IGameDataService GameData { get; init; } = gameData;
    ICharacterImporter Importer { get; init; } = characterImportM;


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ChrImportException"></exception>
    public (Dictionary<string, IActivatableM>, Dictionary<string, IActivatableM>) GetDisAdvantages()
    {
        Dictionary<string, (string id, int tier)> disadvantages;
        Dictionary<string, IActivatableM> DisadvResult = new();
        Dictionary<string, IActivatableM> AdvResult = new();

        try
        {
            disadvantages = Importer.GetDisAdvantages();
        }
        catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.DisAdvantage); }

        // get all available types of (dis-) disadvantages
        // and populate with data from FE game data base
        Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in allTypes)
        {
            if (typeof(IActivatableM).IsAssignableFrom(type) && !type.IsAbstract)
            {
                var attr = (DisAdvantageAttribute)Attribute.GetCustomAttribute(type, typeof(DisAdvantageAttribute));

                if (attr is not null && disadvantages.TryGetValue(attr.Name, out (string id, int tier) ImportAttr))
                { // Skip if this ability id is not in the list of imported disadvantages
                    IActivatableM instance;

                    bool IsRecognized;
                    string[] Reference;
                    var DbEntry = GameData.DisAdvantages[ImportAttr.id];
                    if (GameData.DisAdvantages.Contains(ImportAttr.id))
                    {
                        
                        IsRecognized = DbEntry.Recognized;
                        Reference = DbEntry.Reference ?? [];
                        instance = (IActivatableM)Activator.CreateInstance(type, [ImportAttr.id, ImportAttr.tier, Reference, IsRecognized]);
                    }
                    else
                    {
                        instance = (IActivatableM)Activator.CreateInstance(type, [ImportAttr.id, ImportAttr.tier]);
                    }

                    if (DbEntry.Type == AdvType.Advantage)
                    {
                        AdvResult.Add(attr.Name, instance);
                    }
                    else
                    {
                        DisadvResult.Add(attr.Name, instance);
                    }
                    disadvantages.Remove(attr.Name);
                }
            }
        }

        // Add "anonymous" (dis-) disadvantages that do not have a specific class with game logic
        foreach (var (id, tier) in disadvantages.Values)
        {
            TieredActivatableM instance;

            var DbEntry = GameData.DisAdvantages[id];
            if (GameData?.DisAdvantages.Contains(id) ?? false)
            {
                instance = new TieredActivatableM(id, tier, DbEntry.Reference, DbEntry.Recognized);
            }
            else
            {
                instance = new TieredActivatableM(id, tier, []);
            }

            if (DbEntry.Type == AdvType.Advantage)
            {
                AdvResult.Add(id, instance);
            }
            else
            {
                DisadvResult.Add(id, instance);
            }
        }


        return (AdvResult, DisadvResult);
    }

}

