using FateExplorer.GameData;
using FateExplorer.CharacterData;
using System.Collections.Generic;
using System.Linq;

namespace FateExplorer.GameLogic
{


    public class CharacterSkillsM
    {
        public ICharacterM Hero { get; protected set; }

        public Dictionary<string, CharacterSkillM> Skills { get; set; }

        public Dictionary<SkillDomain,bool> MasteredDomains { get; protected set; }


        public CharacterSkillsM(ICharacterM character, ICharacterImporter import, IGameDataService gameData)
        {
            Hero = character;

            Skills = new();
            MasteredDomains = new();

            // BASIC (mundane) skills
            // Create ALL available skills (unlike magic and karma)
            MasteredDomains.Add(SkillDomain.Basic, true);
            foreach (var dbentry in gameData.Skills.Data)
            {
                CharacterSkillM skill = new(dbentry, import.GetTalentSkill(dbentry.Id), character);
                Skills.Add(dbentry.Id, skill);
            }
            // ARCANE skills
            var ToImport = import.GetArcaneSkills();
            MasteredDomains.Add(SkillDomain.Arcane, ToImport?.Any() ?? false);
            foreach (var v in ToImport)
            {
                CharacterSkillM skill = new(gameData.ArcaneSkills[v.Key], v.Value, character);
                Skills.Add(v.Key, skill);
            }

            // KARMA skills
            ToImport = import.GetKarmaSkills();
            MasteredDomains.Add(SkillDomain.Karma, ToImport?.Any() ?? false);
            foreach (var v in ToImport)
            {
                CharacterSkillM skill = new(gameData.KarmaSkills[v.Key], v.Value, character);
                Skills.Add(v.Key, skill);
            }
        }


        public List<string> GetSkillNames(SkillDomain? Domain, string Filter)
        {
            List<string> names = new();
            foreach(var s in Skills.Values)
            {
                if (s.Name.Contains(Filter) && s.Domain == Domain)
                    names.Add(s.Name);
            }
            return names;
        }
    }
}
