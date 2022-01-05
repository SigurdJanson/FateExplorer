using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.GameLogic
{
    public enum CharacterEnergyClass
    {
        Health = 1, Magic = 2, Karma = 3
    }
    public class CharacterEnergyM
    {
        public CharacterEnergyM(CharacterEnergyClass _Class, int max, ICharacterM hero)
        {
            Class = _Class;
            Max = max;
            Hero = hero;
        }

        public ICharacterM Hero { get; protected set; }

        public CharacterEnergyClass Class { get; protected set; }

        /// <summary>
        /// Minimum possible points (which is not always 0).
        /// </summary>
        public int Min { get; protected set; } = 0;

        /// <summary>
        /// Maximum points of the character, i.e. health, karma or astral energy.
        /// </summary>
        public int Max { get; set; }


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


        public int[] Thresholds { get; protected set; }


        public int CountCrossedThresholds(int Value)
        {
            if (Thresholds is null) return 0;
            for (int i = 0; i < Thresholds.Length; i++)
                if (Value > Thresholds[i]) return i;
            return Thresholds.Length;
        }
    }
}
