using FateExplorer.RollLogic;

namespace FateExplorer.RollLogic
{
    public interface ICheckModifierM
    {
        /// <summary>
        /// Creates a copy before modifying it.
        /// </summary>
        /// <param name="Before">A roll</param>
        /// <returns>The modified roll as array of modified die points</returns>
        int[] Apply(IRollM Before);

        /// <summary>
        /// The total additive modifier after applying it. This value can
        /// be dynamic and depend on the input (when a modifier e.g. cuts
        /// a skill in half).
        /// </summary>
        int Total { get; }
    }
}
