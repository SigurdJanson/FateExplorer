using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.WPA.RollLogic
{
    /// <summary>
    /// Represents a single die to roll
    /// </summary>
    public class DieRoll : IRoll
    {
        protected IRandomNG RNG;

        /// <inheritdoc/>
        public int Sides { get; protected set; }

        /// <inheritdoc/>
        public int OpenRoll { get; protected set; } = 0;

        /// <inheritdoc/>
        public int PrevRoll { get; protected set; } = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sides">The sides of the die</param>
        public DieRoll(int sides)
        {
            if (sides < 2) throw new ArgumentOutOfRangeException(nameof(sides), "A die with less than 2 sides makes no sense");
            Sides = sides;
            RNG = new RandomMersenne();
        }

        /// <inheritdoc/>
        public int Roll()
        {
            PrevRoll = OpenRoll;
            OpenRoll = RNG.IRandom(1, Sides);
            return OpenRoll;
        }
    }
}
