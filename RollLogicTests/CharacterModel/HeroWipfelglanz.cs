using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System.Collections.Generic;

namespace UnitTests.CharacterModel
{
    /// <summary>
    /// Data source class for the character Layariel Wipfelglanz (taken from the quick start guide, not VR1)
    /// </summary>
    internal static class HeroWipfelglanz
    {
        public const int UnarmedSkill = 6;
        public const int Dodge = 8;

        public static WeaponDTO LayarielsDagger
        { 
            get => new()
            {
                Id = "ITEM_9999",
                Name = "Layariels Dagger",
                CombatTechId = "CT_3",
                IsImprovised = false,
                AttackMod = 0,
                ParryMod = 0,
                DamageThreshold = 14,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 1,
                Reach = 1,
                Branch = CombatBranch.Melee,
                IsParry = false,
                IsRanged = false,
                IsTwohanded = false,
                PrimaryAbilityId = new string[1] { "ATTR_6" }, 
                Range = null
            };
        }


        public static WeaponDTO LayarielsElvenBow
            => new()
            {
                Id = "ITEM_9998",
                Name = "Layariels Elven Bow",
                CombatTechId = "CT_2",
                IsImprovised = false,
                AttackMod = 0,
                ParryMod = 0,
                DamageThreshold = 0,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 5,
                Range = new int[3] { 50, 100, 200 },
                Branch = CombatBranch.Ranged,
                IsParry= false, IsRanged = true, IsTwohanded = true, 
                PrimaryAbilityId = new string[1] { "ATTR_5" },
                Reach = 0
            };


        public static Dictionary<string,CombatTechM> CombatTechs(ICharacterM mockCharacter)
        {
            Dictionary<string, CombatTechM> result = new();
            CombatTechDbEntry CT_3 = new()
            {
                Id = "CT_3",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Melee,
                Name = "Dolche",
                PrimeAttrID = "ATTR_6"
            };
            result.Add("CT_3", new CombatTechM(CT_3, 8, mockCharacter) );

            CombatTechDbEntry CT_2 = new()
            {
                Id = "CT_2",
                CanAttack = true,
                CanParry = false,
                IsRanged = true,
                WeaponsBranch = CombatBranch.Ranged,
                Name = "Bögen",
                PrimeAttrID = "ATTR_6"
            };
            result.Add("CT_2", new CombatTechM(CT_2, 12, mockCharacter));

            return result;
        }


        /// <summary>
        /// The abilities of Layariel Wipfelglanz
        /// </summary>
        public static Dictionary<string, int> AbilityValues
        {
            get
            {
                Dictionary<string, int> Result = new();
                Result.Add(AbilityM.COU, 11);
                Result.Add(AbilityM.SGC, 10);
                Result.Add(AbilityM.INT, 15);
                Result.Add(AbilityM.CHA, 13);
                Result.Add(AbilityM.DEX, 14);
                Result.Add(AbilityM.AGI, 15);
                Result.Add(AbilityM.CON, 11);
                Result.Add(AbilityM.STR, 11);
                return Result;
            }
        }

        public static Dictionary<string, AbilityM> Abilities
        {
            get
            {
                var Result = new Dictionary<string, AbilityM>();
                foreach (var a in AbilityValues)
                    Result.Add(a.Key, new AbilityM(a.Key, "Ability" + a.Key, "ABBR", a.Value));
                return Result;
            }
        }

        public static string[] Advantages = { ADV.Spellcaster, ADV.ResistantToAging,
            ADV.Darksight, ADV.SenseOfRange, ADV.GoodLooks, ADV.ExceptionalSense, ADV.BeautifulVoice,
            ADV.Nimble, ADV.NeedsNoSleep, ADV.TwoVoicedSinging };
        public static string[] Disadvantages = { DISADV.AfraidOf/* Angst vor engen Räumen */, DISADV.AnnoyedByMinorSpirits,
            DISADV.BadLuck, DISADV.PersonalityFlaw/* Weltfremd gegenüber Religion */, DISADV.SensitiveNose, 
            DISADV.Incompetent /* Brett- & Glücksspiel und Zechen */ };
        public static string[] SpecialAbilities = {
            "SA_22"/*Ortskenntnis*/, "SA_9"/*Fertigkeitsspezialisierung*/,  SA.TraditionElf};
    }
}
