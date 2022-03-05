using FateExplorer.CharacterModel;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vmCode_UnitTests.CharacterModel
{
    /// <summary>
    /// Data source class for the character Layariel Wipfelglanz (taken from the quick start guide, not VR1)
    /// </summary>
    internal static class HeroWipfelglanz
    {
        public static WeaponDTO LayarielsDagger
            => new()
            {
                Id = "ITEM_9999",
                Name = "Layariels Dagger",
                CombatTechId = "CT_3",
                Improvised = false,
                AttackMod = 0,
                ParryMod = 0,
                DamageThreshold = 14,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 1,
                Reach = 1
            };

        public static WeaponDTO LayarielsElvenBow
            => new()
            {
                Id = "ITEM_9998",
                Name = "Layariels Elven Bow",
                CombatTechId = "CT_2",
                Improvised = false,
                AttackMod = 0,
                ParryMod = 0,
                DamageThreshold = 0,
                DamageDieCount = 1,
                DamageDieSides = 6,
                DamageBonus = 5,
                Range = new int[3] { 50, 100, 200 }
            };



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
    }
}
