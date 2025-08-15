using FateExplorer.CharacterImport;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FateExplorer.CharacterModel.SpecialAbilities;


public class SpecialAbilityConverter
{
    IGameDataService GameData { get; init; }
    ICharacterImporter Importer { get; init; }

    public SpecialAbilityConverter(IGameDataService gameData, ICharacterImporter characterImportM)
    {
        GameData = gameData;
        Importer = characterImportM;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ChrImportException"></exception>
    public Dictionary<string, IActivatableM> GetAbilities()
    {
        Dictionary<string, (string id, int tier)> abilities;
        Dictionary<string, IActivatableM> result = new();

        try
        {
            abilities = Importer._GetSpecialAbilities();
        }
        catch (System.Exception e) { throw new ChrImportException("", e, ChrImportException.Property.SpecialAbility); }

        // get all available types of special abilities
        // and populate with data from FE game data base
        Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in allTypes)
        {
            if (typeof(IActivatableM).IsAssignableFrom(type) && !type.IsAbstract)
            {
                var attr = (SpecialAbilityAttribute)Attribute.GetCustomAttribute(type, typeof(SpecialAbilityAttribute));

                if (attr is not null && abilities.TryGetValue(attr.Name, out (string id, int tier) ImportAttr))
                { // Skip if this ability id is not in the list of imported abilities
                    IActivatableM instance;

                    bool IsRecognized;
                    string[] Reference;
                    if (GameData.SpecialAbilities.Contains(ImportAttr.id))
                    {
                        var x = GameData.SpecialAbilities[ImportAttr.id];
                        IsRecognized = x.Recognized;
                        Reference = x.Reference ?? [];
                        instance = (IActivatableM)Activator.CreateInstance(type, [ImportAttr.id, ImportAttr.tier, Reference, IsRecognized]);
                    }
                    else
                    {
                        instance = (IActivatableM)Activator.CreateInstance(type, [ImportAttr.id, ImportAttr.tier]);
                    }

                    result.Add(attr.Name, instance);
                    abilities.Remove(attr.Name);
                }
            }
        }

        // Add "anonymous" special abilities without a specific class
        foreach (var (id, tier) in abilities.Values)
        {
            TieredActivatableM instance;

            if (GameData?.SpecialAbilities.Contains(id) ?? false)
            {
                var x = GameData.SpecialAbilities[id];
                instance = new TieredActivatableM(id, tier, x.Reference, x.Recognized);
            }
            else
            {
                instance = new TieredActivatableM(id, tier, []);
            }
            result.Add(id, instance);
        }

        return result;
    }


}

