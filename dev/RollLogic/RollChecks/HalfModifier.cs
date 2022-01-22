using System;

namespace FateExplorer.RollLogic
{

    /// <summary>
    /// Modifier that reduces the value to half of it's input
    /// </summary>
    public class HalfModifier : ICheckModifierM
    {

        /// <inheritdoc/>
        /// <remarks>Total cannot be interpreted for this type of modifier</remarks>
        public int Total { get => 0; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HalfModifier()
        {}


        /// <inheritdoc/>
        public int[] Apply(IRollM Before) // TODO: check for min/max???
            => Apply(Before.OpenRoll);


        /// <inheritdoc/>
        public int[] Apply(int[] Before) // TODO: check for min/max???
        {
            int[] After = new int[Before.Length];

            for (int i = 0; i < Before.Length; i++)
                After[i] = Before[i] / 2 + Before[i] % 2;
            return After;
        }

        /// <inheritdoc/>
        public int Apply(int Before) // TODO: check for min/max???
        {
            return Before / 2 + Before % 2;
        }



        /// <inheritdoc/>
        /// <remarks>Not implemented for this type of modifier</remarks>
        public void Set(int value)
        {
            throw new NotImplementedException(nameof(value));
        }
    }
}

