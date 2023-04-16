using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    /// <summary>
    /// The most common modifier. It simply adds the modifier to all rolled
    /// dice. Used for basic ability, skill or combat checks.
    /// </summary>
    public class SimpleCheckModificatorM : ICheckModificatorM
    {
        /// <inheritdoc />
        public Modifier Modifier { get; protected set; }


        /// <inheritdoc />
        public int[] LastEffectiveDelta { get; protected set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The modifier's mod to be applied to a roll</param>
        public SimpleCheckModificatorM(Modifier mod)
        {
            Modifier = mod;
        }




        /// <inheritdoc/>
        public int[] Apply(int[] Before) // TODO: check for min/max???
        {
            int[] After = new int[Before.Length];
            LastEffectiveDelta = new int[Before.Length];

            for (int i = 0; i < Before.Length; i++)
            {
                After[i] = Before[i] + Modifier;
                LastEffectiveDelta[i] = After[i] - Before[i];
            }
                
            return After;
        }


        /// <inheritdoc/>
        public int Apply(int Before)
        {
            int After = Before + Modifier;
            LastEffectiveDelta = new int[1] { After - Before };
            return After;
        }


        /// <inheritdoc/>
        public int Delta(int Before)
        {
            int After = Before + Modifier;
            return After - Before;
        }


        /// <inheritdoc/>
        public void Set(Modifier mod)
        {
            var old = Modifier;
            Modifier = mod;
            if (Modifier != old) NotifyStateChange();
        }


        /// <inheritdoc/>
        public event Action OnStateChanged;

        private void NotifyStateChange() => OnStateChanged.Invoke();
    }
}
