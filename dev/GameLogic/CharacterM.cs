using FateExplorer.CharacterData;
using FateExplorer.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
// TODO
using System.Net.Http;
using System.Net.Http.Json;

namespace FateExplorer.GameLogic
{
    public class CharacterM : ICharacterM
    {
        public CharacterM(IGameDataService gameData, CharacterImportOptM characterImportOptM)
        {
            // TEMPORARY IMPORT //TODO
            //string fileName = $"data/Character_Junis_20200629.json";
            //CharacterImportOptM characterImportOptM = await DataSource.GetFromJsonAsync<KarmaSkillsDB>(fileName);
            //string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            //string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{Filename}.json"));

            //CharacterImportOptM characterImportOptM = JsonSerializer.Deserialize<CharacterImportOptM>("jsonString");
            //

            Abilities = new();
            foreach(var AbImport in characterImportOptM.GetAbilities())
            {
                string AbilityName = gameData.Abilities[AbImport.Key].Name;
                string AbilityShortName = gameData.Abilities[AbImport.Key].ShortName;
                AbilityM ab = new(AbImport.Key, AbilityName, AbilityShortName, AbImport.Value);
                Abilities.Add(AbImport.Key, ab);
            }

            Skills = new(this, characterImportOptM, gameData);
        }

        public Dictionary<string, AbilityM> Abilities { get; set; }

        public Dictionary<string, CharacterResourceM> Resources { get; set; }

        public Dictionary<string, ResilienceM> Resiliences { get; set; }

        public Dictionary<string, CombatTechM> CombatTechs{ get; set; }

        public CharacterSkillsM Skills { get; set; }

        public int GetAbility(string Id)
        {
            return Abilities[Id].EffectiveValue;
        }

        public int GetEffectiveAbility(string Id)
        {
            return Abilities[Id].EffectiveValue;
        }

        public int GetEffectiveResilience(string Id)
        {
            return Resiliences[Id].Value;
        }
    }
}
