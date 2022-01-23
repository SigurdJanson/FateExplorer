namespace FateExplorer.RollLogic
{
    /// Modifier that forces a roll to a certain value. Used mostly when rolls yield no 
    /// result (like no regeneration during the night, ...).
    /// </summary>
    public class ForcefulModifier : ICheckModifierM
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
        public ForcefulModifier(int value)
        {
            Value = value;
        }


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
                After[i] = Value;
                LastEffectiveApply[i] = Before[i] - After[i];
            }
                
            return After;
        }

        /// <inheritdoc/>
        public int Apply(int Before) // TODO: check for min/max???
        {
            int After = Value;

            LastEffectiveApply = new int[1];
            LastEffectiveApply[0] = Before - After;

            return After;
        }



        /// <inheritdoc/>
        public void Set(int value)
        {
            Value = value;
        }

    }
}
