using FateExplorer.CharacterData;
using FateExplorer.GameData;
using System.Collections.Generic;



namespace FateExplorer.GameLogic
{
    public class CharacterM : ICharacterM
    {
        public CharacterM(IGameDataService gameData, CharacterImportOptM characterImportOptM)
        {
            Name = characterImportOptM.GetName();
            PlaceOfBirth = characterImportOptM.GetPlaceOfBirth();
            DateOfBirth = characterImportOptM.GetDateOfBirth();

            // ABILITIES
            Abilities = new();
            foreach(var AbImport in characterImportOptM.GetAbilities())
            {
                string AbilityName = gameData.Abilities[AbImport.Key].Name;
                string AbilityShortName = gameData.Abilities[AbImport.Key].ShortName;
                AbilityM ab = new(AbImport.Key, AbilityName, AbilityShortName, AbImport.Value);
                Abilities.Add(AbImport.Key, ab);
            }

            // SKILLS
            Skills = new(this, characterImportOptM, gameData);

            // RESOURCES
            Resources = new();
            Resources.Add(
                CharacterResourceClass.Health.ToString(), 
                new CharacterHealth(0, this));
            if (characterImportOptM.CountArcaneSkills() > 0)
            {
                Resources.Add(
                    CharacterResourceClass.Magic.ToString(),
                    new CharacterResourceM(CharacterResourceClass.Magic, 0, this));
            }
            if (characterImportOptM.CountKarmaSkills() > 0)
            {
                Resources.Add(
                    CharacterResourceClass.Karma.ToString(),
                    new CharacterKarma(0, this));
            }
        }



        public string Name { get; protected set; }

        public string PlaceOfBirth { get; protected set; }

        public string DateOfBirth { get; protected set; }


        public Dictionary<string, AbilityM> Abilities { get; set; }

        public Dictionary<string, CharacterResourceM> Resources { get; set; }

        public Dictionary<string, ResilienceM> Resiliences { get; set; }

        public Dictionary<string, CombatTechM> CombatTechs{ get; set; }

        public CharacterSkillsM Skills { get; set; }

        public int GetAbility(string Id) => Abilities[Id].Value;
    }
}
