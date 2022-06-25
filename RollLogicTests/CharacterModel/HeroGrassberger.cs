using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System.Collections.Generic;

namespace UnitTests.CharacterModel
{
    /// <summary>
    /// Data source class for the character Ulf Grassberger (a self-made character based on an Winhall Fighter, )
    /// </summary>
    internal static class HeroGrassberger
    {
        public const int UnarmedSkill = 10;
        public const int Dodge = 8;

        public static WeaponDTO Sword
        {
            get => new()
            {
                Id = "ITEMTPL_35",
                Name = "Langschwert",
                CombatTechId = "CT_12",
                IsImprovised = false,
                AttackMod = 0,
                ParryMod = 0,
                DamageThreshold = 15,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 4,
                Reach = 2,
                Branch = CombatBranch.Melee,
                IsParry = false,
                IsRanged = false,
                IsTwohanded = false,
                PrimaryAbilityId = new string[2] { "ATTR_6", "ATTR_8" },
                Range = null
            };
        }


        public static WeaponDTO WoodenShield
            => new()
            {
                Id = "ITEMTPL_26",
                Name = "Holzschild",
                CombatTechId = "CT_10",
                IsImprovised = false,
                AttackMod = -4,
                ParryMod = 1,
                DamageThreshold = 16,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 0,
                Range = null,
                Branch = CombatBranch.Shield,
                IsParry = false,
                IsRanged = false,
                IsTwohanded = false,
                PrimaryAbilityId = new string[1] { "ATTR_8" },
                Reach = 1
            };


        public static Dictionary<string, CombatTechM> CombatTechs(ICharacterM mockCharacter)
        {
            Dictionary<string, CombatTechM> result = new();
            CombatTechDbEntry CT_12 = new()
            {
                Id = "CT_12",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Melee,
                Name = "Schwerter",
                PrimeAttrID = "ATTR_6/ATTR_8"
            };
            result.Add("CT_12", new CombatTechM(CT_12, 12, mockCharacter));

            CombatTechDbEntry CT_10 = new()
            {
                Id = "CT_10",
                CanAttack = true,
                CanParry = true,
                IsRanged = false,
                WeaponsBranch = CombatBranch.Shield,
                Name = "Schilde",
                PrimeAttrID = "ATTR_8"
            };
            result.Add("CT_10", new CombatTechM(CT_10, 12, mockCharacter));

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
                Result.Add(AbilityM.CHA, 10);
                Result.Add(AbilityM.DEX, 10);
                Result.Add(AbilityM.AGI, 15);
                Result.Add(AbilityM.CON, 14);
                Result.Add(AbilityM.STR, 14);
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
