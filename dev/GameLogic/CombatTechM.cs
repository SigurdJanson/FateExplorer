﻿using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.GameLogic
{

    public class CombatTechM
    {
        private ICharacterM Hero;

        public CombatTechM(CombatTechDbEntry gameData, int value, ICharacterM hero)
        {
            if (gameData is null)
                throw new ArgumentNullException(nameof(gameData));

            Hero = hero ?? throw new ArgumentNullException(nameof(hero));
            Branch = gameData.WeaponsBranch;

            PrimeAbilityId = gameData.PrimeAttrID ?? throw new Exception(nameof(gameData.PrimeAttrID));
            Id = gameData.Id ?? throw new Exception(nameof(gameData.Id));
            CanAttack = gameData.CanAttack;
            CanParry = gameData.CanParry;
            IsRanged = gameData.IsRanged;

            Value = value; // set before computing the dependent values
            AttackValue = ComputeAttackValue();
            ParryValue = ComputeParryValue();
        }

        /// <summary>
        /// THe type of weapon
        /// </summary>
        public CombatTechniques Branch { get; protected set; }


        /// <summary>
        /// Reference to the primary ability of the combat technique
        /// (reference by id).
        /// </summary>
        public string PrimeAbilityId { get; protected set; }

        /// <summary>
        /// The id of the combat technique.
        /// </summary>
        public string Id { get; protected set; }


        /// <summary>
        /// The skill value of the combat technique.
        /// </summary>
        public int Value { get; protected set; }

        /// <summary>
        /// The basic attack value of the combat technique according to the 
        /// characters skills and not modified by damage threshold.
        /// </summary>
        public int AttackValue { get; protected set; }

        /// <summary>
        /// The basic parry value of the combat technique according to the 
        /// characters skills and not modified by damage threshold.
        /// Yields 0 if the combat technique does not allow parrying.
        /// </summary>
        public int ParryValue { get; protected set; }


        public bool CanAttack { get; protected set; }
        public bool CanParry { get; protected set; }
        public bool IsRanged { get; protected set; }



        public int ComputeAttack(int EffectiveBase)
        {
            return Value + (EffectiveBase - 8) / 3;
        }

        public int ComputeParry(int EffectivePrimary)
        {
            if (!CanParry) return 0;

            int Result = Value / 2 + Value % 2;
            Result += (EffectivePrimary - 8) / 3;
            return Result;
        }


        /// <summary>
        /// Get the characters standard value of the primary ability
        /// </summary>
        /// <returns>Value</returns>
        protected int GetPrimaryAbilityValue()
        {
            int PrimaryAbility = 0;
            foreach (var s in PrimeAbilityId.Split("/"))
                PrimaryAbility = Math.Max(PrimaryAbility, Hero.GetAbility(s));
            return PrimaryAbility;
        }


        /// <summary>
        /// Computes the attack value according to DSA5 rules without the
        /// damage threshold modifier.
        /// </summary>
        /// <returns>Basic attack value</returns>
        protected int ComputeAttackValue()
        {
            int BaseAbility = Branch switch
            {
                CombatTechniques.Ranged => Hero.GetAbility(AbilityM.DEX),
                _ => Hero.GetAbility(AbilityM.COU)
            };
            //int Result = Value + (BaseAbility - 8) / 3;
            return ComputeAttack(BaseAbility);
        }


        /// <summary>
        /// Computes the parry value according to DSA5 rules without the
        /// damage threshold modifier.
        /// </summary>
        /// <returns>Basic parry value</returns>
        protected int ComputeParryValue() 
            => ComputeParry(GetPrimaryAbilityValue());
        //{
        //    ////if (!CanParry) return 0;

        //    ////int PrimaryAbility = GetPrimaryAbilityValue();
        //    ////int Result = Value / 2 + Value % 2;
        //    ////Result += (PrimaryAbility - 8) / 3;
        //    return ComputeParry(GetPrimaryAbilityValue());
        //}


    }
}
