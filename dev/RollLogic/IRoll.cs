using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.WPA.RollLogic
{
    public interface IRoll
    {
        /// <summary>
        /// The number of sides for the die
        /// </summary>
        int Sides { get; }

        /// <summary>
        /// The current roll value
        /// </summary>
        int OpenRoll { get; }

        /// <summary>
        /// The previous roll value
        /// </summary>
        int PrevRoll { get; }

        /// <summary>
        /// Roll the dice and return the value
        /// </summary>
        /// <returns>The result of the die roll</returns>
        int Roll();
    }
}
