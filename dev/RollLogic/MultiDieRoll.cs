﻿using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Linq;

namespace FateExplorer.RollLogic
{
    /// <summary>
    /// Represents a collection of dice, all dice identical (equivalent to <see cref="CupType.Multi"/>).
    /// </summary>
    public class MultiDieRoll : IRollM
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


        public Func<int, int, int> AggregateFunc = (x, y) => x + y;

        public MultiDieRoll(int sides, int dieCount, Func<int, int, int> aggregate = null)
        {
            if (sides < 2) throw new ArgumentOutOfRangeException(nameof(sides), "A die with less than 2 sides makes no sense");
            Sides = new int[dieCount];
            for (int i = 0; i < dieCount; i++) Sides[i] = sides;

            OpenRoll = new int[dieCount];
            PrevRoll = new int[dieCount];

            if (dieCount < 1) throw new ArgumentOutOfRangeException(nameof(dieCount), "Less than 1 dice make no sense");
            DieCount = dieCount;

            RNG = new RandomMersenne();

            if (aggregate is not null)
                AggregateFunc = aggregate;

            Roll();
        }

        /// <inheritdoc/>
        public int[] Roll()
        {
            for (int i = 0; i < DieCount; i++)
            {
                PrevRoll[i] = OpenRoll[i];
                OpenRoll[i] = RNG.IRandom(1, Sides[i]);
            }
            PrevRoll = OpenRoll;

            return OpenRoll;
        }

        /// <inheritdoc/>
        public virtual int OpenRollCombined()
        {
            return OpenRoll.Aggregate(AggregateFunc);
        }


        private int[] modifiedBy;
        /// <summary>
        /// The modifier that has effectively been applied to the roll.
        /// It must be set by <see cref="Roll"/>.
        /// </summary>
        public int[] ModifiedBy
        {
            get
            {
                modifiedBy ??= new int[Sides.Length];
                return modifiedBy;
            }
            protected set => modifiedBy = value;
        }

    }
}
