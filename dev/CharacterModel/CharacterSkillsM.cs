using FateExplorer.CharacterImport;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FateExplorer.CharacterModel
{


    public class CharacterSkillsM
    {
        public ICharacterM Hero { get; protected set; }

        public Dictionary<string, CharacterSkillM> Skills { get; set; }

        public Dictionary<Check.Skill, bool> MasteredDomains { get; protected set; }


        public CharacterSkillsM(ICharacterM character, ICharacterImporter import, IGameDataService gameData)
        {
            Hero = character;

            Skills = new();
            MasteredDomains = new();

            // BASIC (mundane) skills
            // Create ALL available skills (unlike magic and karma)
            MasteredDomains.Add(Check.Skill.Skill, true);
            foreach (var dbentry in gameData.Skills.Data)
            {
                CharacterSkillM skill = new(dbentry, import.GetTalentSkill(dbentry.Id), character);
                Skills.Add(dbentry.Id, skill);
            }
            // ARCANE skills
            var ToImport = import.GetArcaneSkills();
            MasteredDomains.Add(Check.Skill.Arcane, ToImport?.Any() ?? false);
            foreach (var v in ToImport)
            {
                CharacterSkillM skill = new(gameData.ArcaneSkills[v.Key], v.Value, character);
                Skills.Add(v.Key, skill);
            }

            // KARMA skills
            ToImport = import.GetKarmaSkills();
            MasteredDomains.Add(Check.Skill.Karma, ToImport?.Any() ?? false);
            foreach (var v in ToImport)
            {
                CharacterSkillM skill = new(gameData.KarmaSkills[v.Key], v.Value, character);
                Skills.Add(v.Key, skill);
            }
        }


        public List<string> GetSkillNames(Check.Skill? Domain, string Filter)
        {
            List<string> names = new();
            foreach (var s in Skills.Values)
            {
                if (s.Name.Contains(Filter) && s.Domain == Domain)
                    names.Add(s.Name);
            }
            return names;
        }
    }
}
