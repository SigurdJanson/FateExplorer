using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.WPA.RollLogic
{
    public class MultiDieRoll : IRoll
    {
        protected IRandomNG RNG { get; set; }

        public int DieCount { get; protected set; }

        /// <inheritdoc/>
        public int Sides { get; protected set; }

        public Func<int, int, int> AggregateFunc = (x, y) => x + y;

        /// <inheritdoc/>
        public int OpenRoll { get; protected set; }

        /// <inheritdoc/>
        public int PrevRoll { get; protected set; }

        public int[] OpenRollVals { get; protected set; }
        public int[] PrevRollVals { get; protected set; }

        public MultiDieRoll(int sides, int dieCount, Func<int, int, int> aggregate = null)
        {
            if (sides < 2) throw new ArgumentOutOfRangeException(nameof(sides), "A die with less than 2 sides makes no sense");
            Sides = sides;
            if (sides < 1) throw new ArgumentOutOfRangeException(nameof(dieCount), "Less than 1 dice make no sense");
            DieCount = dieCount;
            OpenRollVals = new int[DieCount];
            PrevRollVals = new int[DieCount];
            RNG = new RandomMersenne();

            if (aggregate is not null)
                AggregateFunc = aggregate;
        }

        /// <inheritdoc/>
        public int Roll()
        {
            for(int i = 0; i < DieCount; i++)
            {
                PrevRollVals[i] = OpenRollVals[i];
                OpenRollVals[i] = RNG.IRandom(1, Sides);
            }
            PrevRoll = OpenRoll;
            OpenRoll = OpenRollVals.Aggregate(AggregateFunc);

            return OpenRoll;
        }
    }
}
