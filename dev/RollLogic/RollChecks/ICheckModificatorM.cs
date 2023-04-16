using FateExplorer.Shared;

namespace FateExplorer.RollLogic;


/// <summary>
/// This is the common interface for different kinds of roll/check modifiers.
/// </summary>
public interface ICheckModificatorM : IStateContainer
{


    /// <summary>
    /// Apply the modifier to a single attributes.
    /// </summary>
    /// <param name="Before">An attribute value</param>
    /// <returns>The modified value</returns>
    public int Apply(int Before);

    /// <summary>
    /// This is the common interface for different kinds of roll/check modifiers.
    /// The result of the last modification as (effective) as 
    /// array of additive modifiers. Can be null if this modifier 
    /// has not been applied, yet..
    /// </summary>
        /// <summary>
        /// The total additive modifier after applying it. This value can
        /// be dynamic and depend on the input (when a modifier e.g. cuts
        /// a skill in half).
        /// </summary>
        int Total { get; }

    int[] LastEffectiveApply { get; }

    /// <summary>
    /// Sets the modifier using an int. Each class implementing this interface has
    /// to respond to this accordingly.
    /// </summary>
    /// <param name="value"></param>
    void Set(Modifier value);

}
