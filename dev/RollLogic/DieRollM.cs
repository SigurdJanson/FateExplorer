using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.RollLogic
{
    /// <summary>
    /// Represents a single die to roll
    /// </summary>
    public class DieRollM : IRollM
    {
        /// <summary>
        /// The source for random numbers; by default a mersenne twister
        /// </summary>
        public IRandomNG RNG;

        /// <inheritdoc/>
        public int DieCount { get; protected set; } = 1;

        /// <inheritdoc/>
        public int[] Sides { get; protected set; }

        /// <inheritdoc/>
        public int[] OpenRoll { get; protected set; }

        /// <inheritdoc/>
        public int[] PrevRoll { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sides">The sides of the die</param>
        public DieRollM(int sides)
        {
            if (sides < 2) throw new ArgumentOutOfRangeException(nameof(sides), "A die with less than 2 sides makes no sense");
            Sides = new int[1] { sides };
            OpenRoll = new int[1] { 0 };
            PrevRoll = new int[1] { 0 };
            RNG = new RandomMersenne();
        }

        /// <inheritdoc/>
        public int[] Roll()
        {
            PrevRoll[0] = OpenRoll[0];
            OpenRoll[0] = RNG.IRandom(1, Sides[0]);
            return OpenRoll;
        }

        /// <summary>
        /// Used to aggregate the roll result for multi dice rolls. For single die
        /// rolls this merely returns the roll result.
        /// </summary>
        /// <returns>The current (i.e. openly visible) roll</returns>
        public int OpenRollCombined()
        {
            return OpenRoll[0];
        }


        //
        #region ENTRY CONDITION 
        /// <summary>
        /// The class to verify entry
        /// </summary>
        protected IEntryCondition EntryCondition { get; set; }

        /// <inheritdoc/>
        public bool EntryConfirmed(params object[] Args)
        {
            return EntryCondition?.MeetsCondition(this, null) ?? true;
        }
        #endregion
    }
}
