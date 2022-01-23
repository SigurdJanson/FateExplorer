using System;

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

        private int[] openRoll;
        /// <inheritdoc/>
        public int[] OpenRoll
        { 
            get
            {
                if (openRoll is null || openRoll[0] == 0)
                    InitRoll();
                return openRoll;
            }
            protected set => openRoll = value;
        }

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
            RNG = new RandomMersenne();
            OpenRoll = new int[1] { 0 };
            PrevRoll = new int[1] { 0 };
        }

        /// <summary>
        /// Rolls an initlialising role; is only to called once in the rolls
        /// lifetime.
        /// </summary>
        protected void InitRoll()
        {
            openRoll[0] = RNG.IRandom(1, Sides[0]);
        }

        /// <inheritdoc/>
        public virtual int[] Roll()
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


        private int[] modifiedBy;
        /// <summary>
        /// The modifier that has effectively been applied to the roll.
        /// It must be set by <see cref="Roll"/>.
        /// </summary>
        /// <remarks>The base class DieRollM does not modifiers 
        /// and will return an array of zeroes.</remarks>
        public int[] ModifiedBy
        {
            get
            {
                if (modifiedBy == null) modifiedBy = new int[Sides.Length];
                return modifiedBy;
            }
            protected set => modifiedBy = value;
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
