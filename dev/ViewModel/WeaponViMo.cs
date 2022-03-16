using FateExplorer.CharacterModel;
using System.Collections.Generic;

namespace FateExplorer.ViewModel
{
    public class WeaponViMo
    {
        protected WeaponM WeaponM { get; set; }

        public WeaponViMo(WeaponM weapon)
        {
            WeaponM = weapon;
        }

        /// <summary>
        /// Allow conversion of the view model to the model
        /// </summary>
        /// <returns>The weapon as model class</returns>
        public WeaponM ToWeaponM() => WeaponM;


        public string[] PrimaryAbilityId { get => WeaponM.PrimaryAbilityId; }

        public int BaseAtSkill { get => WeaponM.BaseAtSkill; }

        public int AtSkill(HandsViMo Hands, CombatBranch otherHand)
            => WeaponM.AtSkill(Hands.MainWeapon == this, otherHand);

        public int BasePaSkill { get => WeaponM.BasePaSkill; }

        public int PaSkill(HandsViMo Hands, CombatBranch otherHand)
        {
            bool IsMainWeapon = Hands.MainWeapon == this;
            WeaponViMo OtherWeapon = IsMainWeapon ? Hands.OffWeapon : Hands.MainWeapon;
            return WeaponM.PaSkill(IsMainWeapon, otherHand, OtherWeapon.IsParry, OtherWeapon.ParryMod);
        }


        public string Name { get => WeaponM.Name; }

        public string CombatTechId { get => WeaponM.CombatTechId; }

        public CombatBranch Branch { get => WeaponM.Branch; }

        public int DamageThreshold { get => WeaponM.DamageThreshold; }

        public int DamageDieCount { get => WeaponM.DamageDieCount; }

        public int DamageDieSides { get => WeaponM.DamageDieSides; }

        public int DamageBonus { get => WeaponM.DamageBonus; }

        public int AttackMod { get => WeaponM.AttackMod; }

        public int ParryMod { get => WeaponM.ParryMod; }

        public int Reach { get => WeaponM.Reach; }

        public int[] Range { get => WeaponM.Range; }

        public bool IsImprovised { get => WeaponM.IsImprovised; }

        public bool IsRanged { get => WeaponM.IsRanged; }

        public bool IsTwohanded { get => WeaponM.IsTwoHanded; }

        public bool IsParry { get => WeaponM.IsParry; }

        public bool CanParry { get => WeaponM.CanParry; }
    }
}