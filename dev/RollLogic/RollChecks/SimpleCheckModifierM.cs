using System;

namespace FateExplorer.RollLogic
{
    /// <summary>
    /// The most common modifier. It simply adds the modifier to all rolled
    /// dice. Used for basic ability, skill or combat checks.
    /// </summary>
    public class SimpleCheckModifierM : ICheckModifierM
    {
        /// <summary>
        /// An additive modifier
        /// </summary>
        public int Value { get; protected set; }

        /// <inheritdoc/>
        public int Total { get => Value; }

        /// <inheritdoc />
        public int[] LastEffectiveApply { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The modifier's value to be applied to a roll</param>
        public SimpleCheckModifierM(int value)
        {
            Value = value;
        }


        /// <inheritdoc/>
        public int[] Apply(IRollM Before) // TODO: check for min/max???
        {
            int[] After = new int[Before.OpenRoll.Length];
            LastEffectiveApply = new int[Before.OpenRoll.Length];

            for (int i = 0; i < Before.OpenRoll.Length; i++)
            {
                After[i] = Before.OpenRoll[i] + Value;
                LastEffectiveApply[i] = Value;
            }
                
            return After;
        }


        /// <inheritdoc/>
        public int[] Apply(int[] Before) // TODO: check for min/max???
        {
            int[] After = new int[Before.Length];
            LastEffectiveApply = new int[Before.Length];

            for (int i = 0; i < Before.Length; i++)
            {
                After[i] = Before[i] + Value;
                LastEffectiveApply[i] = Value;
            }
                
            return After;
        }


        /// <inheritdoc/>
        public int Apply(int Before) // TODO: check for min/max???
        {
            LastEffectiveApply = new int[1] { Value };
            return Before + Value; ;
        }



        /// <inheritdoc/>
        public void Set(int value)
        {
            if (value < -30 || value > +30) throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }
    }
}
