using System;
using System.Linq;

namespace FateExplorer.RollLogic
{
    /// <summary>
    /// This roll uses 2d6 but uses only the higher value; in case of a doublet it uses the sum.
    /// </summary>
    public class BestOf2d6 : IRollM
    {
        /// <summary>
        /// The source for random numbers; by default a mersenne twister
        /// </summary>
        public IRandomNG RNG { get; set; }

        /// <inheritdoc/>
        public int DieCount { get; protected set; }

        /// <inheritdoc/>
        public int[] Sides { get; protected set; }

        /// <inheritdoc/>
        public int[] OpenRoll { get; protected set; }

        /// <inheritdoc/>
        public int[] PrevRoll { get; protected set; }


        private int[] modifiedBy;

        /// <value>Is the open roll the result of a doublet?</value>
        public bool IsDoublet { get; protected set; } = false;

        public BestOf2d6()
        {
            DieCount = 1;

            OpenRoll = new int[DieCount];
            PrevRoll = new int[DieCount];

            Sides = new int[DieCount];
            for (int i = 0; i < DieCount; i++) Sides[i] = 6;

            RNG = new RandomMersenne();
            Roll();
        }

        /// <summary>
        /// The modifier that has effectively been applied to the roll.
        /// It must be set by <see cref="Roll"/>.
        /// </summary>
        public int[] ModifiedBy
        {
            get
            {
                if (modifiedBy == null) modifiedBy = new int[Sides.Length];
                return modifiedBy;
            }
            protected set => modifiedBy = value;
        }


        /// <inheritdoc/>
        public int OpenRollCombined() => OpenRoll[0];


        /// <inheritdoc/>
        public int[] Roll()
        {
            PrevRoll[0] = OpenRoll[0];

            int Roll1 = RNG.IRandom(1, Sides[0]);
            int Roll2 = RNG.IRandom(1, Sides[0]);
            if (Roll1 == Roll2)
            {
                OpenRoll[0] = Roll1 + Roll2;
                IsDoublet = true;
            }
            else
            {
                OpenRoll[0] = Math.Max(Roll1, Roll2);
                IsDoublet = false;
            }

            return OpenRoll;
        }
    }
}
