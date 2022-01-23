using FateExplorer.GameData;
using System;

namespace FateExplorer.GameLogic
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
        /// 
        /// </summary>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        public int ResolveValue(int newValue, int EffMax, int EffMin)
        {
            if (newValue > EffMax) return EffMax;
            if (newValue < EffMin) return EffMin;
            return newValue;
        }

        /// <summary>
        /// Apply a modifier to the given value using an operation and a modifier.
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
    }
}
