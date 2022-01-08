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
        public int Value { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The modifier's value to be applied to a roll</param>
        public SimpleCheckModifierM(int value)
        {
            Value = value;
        }


        /// <inheritdoc/>
        public void Apply(IRollM Before)
        {
            for (int i = 0; i < Before.OpenRoll.Length; i++)
                Before.OpenRoll[i] -= Value;
        }
    }
}
