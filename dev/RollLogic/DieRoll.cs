using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.WPA.RollLogic
{
    /// <summary>
    /// Represents a single die to roll
    /// </summary>
    public class DieRoll
    {
        protected IRandomNG RNG;

        /// <summary>
        /// The number of sides for the die
        /// </summary>
        public int Sides { get;  protected set; }

        /// <summary>
        /// The current roll value
        /// </summary>
        public int OpenRoll { get; protected set; } = 0;

        /// <summary>
        /// The previous roll value
        /// </summary>
        public int PrevRoll { get; protected set; } = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sides">The sides of the die</param>
        public DieRoll(int sides)
        {
            Sides = sides;
            RNG = new RandomMersenne();
        }

        /// <summary>
        /// Roll the die and return the value
        /// </summary>
        /// <returns>The result of the die roll</returns>
        public int Roll()
        {
            PrevRoll = OpenRoll;
            OpenRoll = RNG.IRandom(1, Sides);
            return OpenRoll;
        }
    }
}
