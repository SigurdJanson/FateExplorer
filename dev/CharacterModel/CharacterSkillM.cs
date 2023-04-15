using FateExplorer.GameData;
using FateExplorer.Shared;

namespace FateExplorer.CharacterModel
{


    public class CharacterSkillM
    {
        public ICharacterM Hero { get; protected set; }

        public CharacterSkillM(SkillDbEntryBase gameData, int value, ICharacterM character)
        {
            Hero = character;
            //
            Id = gameData.Id;
            Name = gameData.Name;
            ClassId = gameData.ClassId;
            Abilities = new string[3];
            Abilities[0] = gameData.Ab1;
            Abilities[1] = gameData.Ab2;
            Abilities[2] = gameData.Ab3;
            //
            if (gameData is ArcaneSkillDbEntry Arcane)
            {
                ModifyAgainst = Arcane.ModAgainst;
                Tradition = new string[1] { Arcane.Property.ToString() };
                Domain = gameData.Domain;
            }
            else if (gameData is KarmaSkillDbEntry Karma)
            {
                ModifyAgainst = Karma.ModAgainst;
                Tradition = Karma.Tradition.Clone() as string[];
                Domain = gameData.Domain;
            }
            //
            Value = value;
        }

        public string Id { get; protected set; }

        public string Name { get; protected set; }

        public int ClassId { get; protected set; }

        public int Value { get; protected set; }

        public string[] Abilities { get; protected set; }

        /// <summary>
        /// Resilience that sets the modifier against which any rolls 
        /// have to be tested.
        /// </summary>
        public string ModifyAgainst { get; protected set; } // modagainst: SK / ZK

        /// <summary>
        /// Divine or arcane tradtion of a liturgy or spell
        /// </summary>
        public string[] Tradition { get; protected set; } // tradition in karmaskills; Category in arcane skills

        //
        public Check.Skill Domain { get; protected set; }
    }
}
