﻿using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.CharacterModel
{
    /// <summary>
    /// A characters energy, either life, arcane or karma energy.
    /// </summary>
    public class CharacterEnergyM
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameData">Access to the game data base</param>
        /// <param name="_Class">Type of energy</param>
        /// <param name="AddedEnergy">Maximum energy for this character</param>
        /// <param name="hero">The character</param>
        public CharacterEnergyM(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero)
        {
            Hero = hero;
            Class = _Class;

            Max = AddedEnergy;
        }

        /// <summary>
        /// The character behind this energy
        /// </summary>
        public ICharacterM Hero { get; protected set; }

        /// <summary>
        /// Which tye of energy (arcane, life, karma)
        /// </summary>
        public CharacterEnergyClass Class { get; protected set; }

        /// <summary>
        /// Minimum possible points (which is not always 0).
        /// </summary>
        public int Min { get; protected set; } = 0;

        protected int max;
        /// <summary>
        /// Maximum points of the character, i.e. health, karma or astral energy.
        /// Changing it updates the thresholds, too.
        /// </summary>
        public int Max
        {
            get => max;
            set
            {
                max = value;
                CalcThresholds(max);
            }
        }


        /// <summary>
        /// Determine a new value without exceeding lower/upper limits
        /// </summary>
        /// <param name="newValue">The value to be set</param>
        /// <param name="EffMin">The effective minimum that cannot be crossed</param>
        /// <param name="EffMax">The effective maximum that cannot be crossed</param>
        /// <returns>The new value</returns>
        public int ResolveValue(int newValue, int EffMax, int EffMin)
        {
            if (newValue > EffMax) return EffMax;
            if (newValue < EffMin) return EffMin;
            return newValue;
        }


        /// <summary>
        /// Apply a modificator to the given value using an operation and a modificator.
        /// </summary>
        /// <param name="Value">Original energy value to be modified</param>
        /// <param name="Modifier">A value to be used in the modification</param>
        /// <param name="Operation">A delegate <c>int Operation(int, int)</c>; if null 
        /// addition is used as operation by default.</param>
        /// <returns>THe new value</returns>
        public int Compute(int Value, int Modifier, Func<int, int, int> Operation = null)
        {
            if (Operation is null)
                Value += Modifier;
            else
                Value = Operation(Value, Modifier);

            if (Value < Min)
                Value = Min;
            else if (Value > Max)
                Value = Max;

            return Value;
        }


        /// <summary>
        /// An array defining meaningful thresholds this energy crosses wehn being depleted.
        /// </summary>
        public int[] Thresholds { get; protected set; }

        /// <summary>
        /// Generates the <see cref="Thresholds"/> given the effective maximum.
        /// </summary>
        public virtual void CalcThresholds(int EffMax)
        {
            Thresholds = new int[] { 5 };
        }

        /// <summary>
        /// Returns the number of thresholds have been crossed.
        /// </summary>
        /// <param name="Value">Current value to be compared against thresholds</param>
        /// <returns>Number of crossed thresholds</returns>
        public int CountCrossedThresholds(int Value)
        {
            if (Thresholds is null) return 0;
            for (int i = 0; i < Thresholds.Length; i++)
                if (Value > Thresholds[i]) return i;
            return Thresholds.Length;
        }


        
        /// <summary>
        /// SETUP HELPER <br/>
        /// Get the dis-advantages from the game data base that affect the maximum
        /// energy value and return the modifier that must be added to the max.
        /// </summary>
        /// <param name="gameData"></param>
        /// <returns>The modifier that must be added to max.</returns>
        protected int GetDisAdvantageModifier(EnergiesDbEntry gameData)
        {
            int modifier = 0;
            foreach (var da in gameData.DisAdvBaseValue)
            {
                IActivatableM DisAdv = null;
                if (Hero.HasAdvantage(da.Id))
                    DisAdv = Hero.Advantages[da.Id];
                else if (Hero.HasDisadvantage(da.Id))
                    DisAdv = Hero.Disadvantages[da.Id];

                if (DisAdv is not null)
                {
                    int Multiplier = da.MultiTier ? DisAdv.Tier : 1;
                    modifier += Multiplier * da.Value;
                }
            }
            return modifier;
        }
    }
}
