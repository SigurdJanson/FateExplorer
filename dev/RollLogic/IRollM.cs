﻿namespace FateExplorer.RollLogic
{
    /// <summary>
    /// A roll yields one or many random numbers between 1 and the number of die sides.
    /// </summary>
    public interface IRollM
    {
        /// <summary>
        /// Number of dice.
        /// </summary>
        int DieCount { get; }


        /// <summary>
        /// The number of sides for the die. Array length must be <see cref="DieCount"/>.
        /// </summary>
        int[] Sides { get; }

        /// <summary>
        /// The current roll value. Array length must be <see cref="DieCount"/>.
        /// </summary>
        int[] OpenRoll { get; }

        /// <summary>
        /// The previous roll value. Array length must be <see cref="DieCount"/>.
        /// </summary>
        int[] PrevRoll { get; }

        /// <summary>
        /// Roll the dice and return the value. Array length must be <see cref="DieCount"/>.
        /// </summary>
        /// <returns>The result of the die roll</returns>
        int[] Roll();

        /// <summary>
        /// Used to aggregate several dice used to roll.
        /// </summary>
        /// <returns>An aggregated form of all dice</returns>
        int OpenRollCombined();


        /// <summary>
        /// The modifier that has effectively been applied to the roll.
        /// It must be set by <see cref="Roll"/>.
        /// </summary>
        int[] ModifiedBy { get; }

    }
}
