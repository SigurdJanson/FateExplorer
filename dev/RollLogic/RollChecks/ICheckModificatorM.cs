using FateExplorer.Shared;

namespace FateExplorer.RollLogic;


/// <summary>
/// This is the common interface for different kinds of roll/check modifiers.
/// </summary>
public interface ICheckModificatorM : IStateContainer
{
    /// <summary>
    /// The modifier applied by this modificator
    /// </summary>
    public Modifier Modifier { get; }

    /// <summary>
    /// Apply the modifier to a list of proficiency values (skill, ability, ...).
    /// Creates a copy before modifying it.
    /// </summary>
    /// <param name="Before">An array of proficiency values</param>
    /// <returns>The modified values as array</returns>
    public int[] Apply(int[] Before);


    /// <summary>
    /// Apply the modifier to a single proficiency value (skill, ability, ...).
    /// </summary>
    /// <param name="Before">A proficiency value</param>
    /// <returns>The modified value</returns>
    public int Apply(int Before);


    /// <summary>
    /// Returns the effective delta between a proficiency value (skill, ability, ...)
    /// and the effective value after the modifier has been applied.
    /// </summary>
    /// <param name="Before"><inheritdoc cref="Apply(int)"/></param>
    /// <returns>The delta</returns>
    public int Delta(int Before);


    /// <summary>
    /// The result of the last modification as (effective) as 
    /// array of additive modifiers. Can be null if this modifier 
    /// has not been applied, yet..
    /// </summary>
    int[] LastEffectiveDelta { get; }

    /// <summary>
    /// Sets the modifier using an int. Each class implementing this interface has
    /// to respond to this accordingly.
    /// </summary>
    /// <param name="value"></param>
    void Set(Modifier value);

}
