﻿using FateExplorer.CharacterModel;
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


        public string[] PrimaryAbilityId { get => WeaponM.PrimaryAbilityId; }

        public int AtSkill { get => WeaponM.AtSkill; }

        public int PaSkill { get => WeaponM.PaSkill; }


        public string Name { get => WeaponM.Name; }

        public string CombatTechId { get => WeaponM.CombatTechId; }

        public CombatBranch Branch { get => WeaponM.Branch; }

        public int DamageThreshold { get => WeaponM.DamageThreshold; }

        public int DamageDieCount { get => WeaponM.DamageDieCount; }

        public int DamageDieSides { get => WeaponM.DamageDieSides; }

        public int DamageBonus { get => WeaponM.DamageBonus; }

        public int AttackMod { get => WeaponM.AttackMod; }

        public int ParryMod { get => WeaponM.ParryMod; }

        public int Range { get => WeaponM.Range; }

        public bool IsImprovised { get => WeaponM.Improvised; }

        public bool Ranged { get => WeaponM.Ranged; }

        public bool CanParry { get => WeaponM.CanParry; }

        public bool IsTwohanded { get => WeaponM.TwoHanded; }
    }
}