using FateExplorer.Shared;

namespace FateExplorer.RollLogic;

/// <summary>
/// The main purpose of the check context is to collect all data needed to calculate a check modifier
/// that depends on context variables. Most important context of the <see cref="BattlegroundM">battle ground</see>.
/// </summary>
public interface ICheckContextM : IStateContainer
{
    /// <summary>
    /// Computes and returns the total modifier of the given context.
    /// </summary>
    /// <param name="before">A proficiency value before modification.</param>
    /// <param name="action">An action evaluated by a roll check.</param>
    /// <returns>A modifier that can be applied to a proficiency value.</returns>
    Modifier GetTotalMod(int before, Check.Combat action);

    /// <summary>
    /// Modifies the proficiency value before rolling a check given the current context.
    /// </summary>
    /// <param name="before">A proficiency value before modification.</param>
    /// <param name="action">An action evaluated by a roll check.</param>
    /// <returns>The modified proficiency value.</returns>
    int ApplyTotalMod(int before, Check.Combat action);


    /// <summary>
    /// Provides the difference between the original and the effective value (after modification).
    /// </summary>
    /// <param name="before">A proficiency value before modification.</param>
    /// <param name="action">An action evaluated by a roll check.</param>
    /// <returns>The difference between the original and the effective value (after modification).</returns>
    int ModDelta(int before, Check.Combat action);
}
