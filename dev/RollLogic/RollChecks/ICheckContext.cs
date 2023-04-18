﻿using FateExplorer.Shared;

namespace FateExplorer.RollLogic;

/// <summary>
/// The main purpose of the check context is to collect all data needed to calculate a check modifier
/// that depends on context variables. Most important context of the <see cref="BattlegroundM">battle ground</see>.
/// </summary>
public interface ICheckContext : IStateContainer
{
    /// <summary>
    /// Computes and returns the total modifier of the given context
    /// </summary>
    /// <param name="before">A proficiency value before modification.</param>
    /// <param name="action">An action evaluated by a roll check.</param>
    /// <returns></returns>
    Modifier GetTotalMod(int before, Check.Combat action);

    /// <summary>
    /// Modifies the proficiency value before rolling a check given the current context.
    /// </summary>
    /// <param name="before">A proficiency value before modification.</param>
    /// <param name="action">An action evaluated by a roll check.</param>
    /// <returns>The modified proficiency value.</returns>
    int ApplyTotalMod(int before, Check.Combat action);

}
