using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System.Collections.Generic;

namespace UnitTests.CharacterModel
{
    /// <summary>
    /// Data source class for the character Arbosch, Son of Angrax (taken from the quick start guide)
    /// </summary>
    internal static class HeroArbosch
    {
        public const int UnarmedSkill = 6;

        public static WeaponDTO ArboschsDagger
        {
            get => new()
            {
                Id = "ITEMTPL_2",
                Name = "Arbosch Dagger",
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


        public static WeaponDTO ArboschsDwarfCudgel
            => new()
            {
                Id = "ITEMTPL_56",
                Name = "Arboschs Dwarf Cudgel",
                CombatTechId = "CT_15",
                IsImprovised = false,
                AttackMod = 0,
                ParryMod = -1,
                DamageThreshold = 13,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 6,
                Range = null,
                Branch = CombatBranch.Melee,
                IsParry = false,
                IsRanged = false,
                IsTwohanded = true,
                PrimaryAbilityId = new string[1] { "ATTR_8" },
                Reach = 2
            };


        public static Dictionary<string, CombatTechM> CombatTechs(ICharacterM mockCharacter)
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
            result.Add("CT_3", new CombatTechM(CT_3, 6, mockCharacter));

            CombatTechDbEntry CT_15 = new()
            {
                Id = "CT_15",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Melee,
                Name = "Zweihandhiebwaffen",
                PrimeAttrID = "ATTR_8"
            };
            result.Add("CT_15", new CombatTechM(CT_15, 12, mockCharacter));

            return result;
        }


        /// <summary>
        /// The abilities of Arbosch
        /// </summary>
        public static Dictionary<string, int> AbilityValues
        {
            get
            {
                Dictionary<string, int> Result = new();
                Result.Add(AbilityM.COU, 14);
                Result.Add(AbilityM.SGC, 10);
                Result.Add(AbilityM.INT, 13);
                Result.Add(AbilityM.CHA, 9);
                Result.Add(AbilityM.DEX, 13);
                Result.Add(AbilityM.AGI, 11);
                Result.Add(AbilityM.CON, 15);
                Result.Add(AbilityM.STR, 15);
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
    }
}
