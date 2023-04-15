using System;

namespace FateExplorer.RollLogic
{
    /// Value that forces a roll to a certain value. Used mostly when rolls yield no 
    /// result (like no regeneration during the night, ...).
    /// </summary>
    public class ForcefulModificator : ICheckModificatorM
    {
        /// <summary>
        /// A value to force a roll/check result to
        /// </summary>
        public int Value { get; protected set; }


        /// <inheritdoc/>
        /// <remarks>Total cannot be interpreted for this type of modifier</remarks>
        public int Total { get => 0; }

        /// <inheritdoc />
        public int[] LastEffectiveApply { get; protected set; }



        /// <summary>
        /// Constructor
        /// </summary>
        public ForcefulModificator(int value)
        {
            Value = value;
        }


        /// <inheritdoc/>
        public int[] Apply(IRollM Before)
            => Apply(Before.OpenRoll);


        /// <inheritdoc/>
        public int[] Apply(int[] Before) 
        {
            int[] After = new int[Before.Length];
            LastEffectiveApply = new int[Before.Length];

            for (int i = 0; i < Before.Length; i++)
            {
                After[i] = Value;
                LastEffectiveApply[i] = After[i] - Before[i];
            }
                
            return After;
        }

        /// <inheritdoc/>
        public int Apply(int Before)
        {
            LastEffectiveApply = new int[1];
            LastEffectiveApply[0] = Value - Before;

            return Value;
        }



        /// <inheritdoc/>
        public void Set(int value)
        {
            var old = Value;
            Value = value;
            if (Value != old) NotifyStateChange();
        }


        /// <inheritdoc/>
        public event Action OnStateChanged;

        private void NotifyStateChange() => OnStateChanged.Invoke();
    }
}
