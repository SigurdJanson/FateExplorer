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

        /// <inheritdoc />
        public int[] LastEffectiveApply { get; protected set; }


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
            LastEffectiveApply = new int[Before.Length];

            for (int i = 0; i < Before.Length; i++)
            {
                After[i] = Before[i] / 2 + Before[i] % 2;
                LastEffectiveApply[i] = Before[i] - After[i];
            }
                
            return After;
        }

        /// <inheritdoc/>
        public int Apply(int Before) // TODO: check for min/max???
        {
            int After = Before / 2 + Before % 2;
            LastEffectiveApply = new int[1];
            LastEffectiveApply[0] = Before - After;
            return After;
        }



        /// <inheritdoc/>
        /// <remarks>Not implemented for this type of modifier</remarks>
        public void Set(int value)
        {
            throw new NotImplementedException("User-defined values not supported for half modifier");
        }


        /// <inheritdoc/>
        /// <remarks>Not used for this type of modifier because <see cref="Set"/> is not implemented.</remarks>
#pragma warning disable CS0067 // The event is never used
        public event Action OnStateChanged;
#pragma warning restore CS0067
    }
}

