namespace FateExplorer.RollLogic
{
    /// <summary>
    /// This is the common interface for different kinds of roll/check modifiers.
    /// </summary>
    public interface ICheckModifierM
    {
        /// <summary>
        /// Creates a copy before modifying it.
        /// </summary>
        /// <param name="Before">A roll</param>
        /// <returns>The modified roll as array of modified die points</returns>
        int[] Apply(IRollM Before); // ???????????? does it even make sense?

        /// <summary>
        /// Apply the modifier to a list of attributes.
        /// Creates a copy before modifying it.
        /// </summary>
        /// <param name="Before">An array of attribute values</param>
        /// <returns>The modified values as array</returns>
        public int[] Apply(int[] Before);


        /// <summary>
        /// Apply the modifier to a single attributes.
        /// </summary>
        /// <param name="Before">An attribute value</param>
        /// <returns>The modified value</returns>
        public int Apply(int Before);


        /// <summary>
        /// The total additive modifier after applying it. This value can
        /// be dynamic and depend on the input (when a modifier e.g. cuts
        /// a skill in half).
        /// </summary>
        int Total { get; }

        /// <summary>
        /// The result of the last modification as (effective) as 
        /// array of additive modifiers. Can be null if this modifier 
        /// has not been applied, yet..
        /// </summary>
        int[] LastEffectiveApply { get; }

        /// <summary>
        /// Sets the modifier using an int. Each class implementing this interface has
        /// to respond to this accordingly.
        /// </summary>
        /// <param name="value"></param>
        void Set(int value);
    }
}
