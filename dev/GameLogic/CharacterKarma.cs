using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.GameLogic
{
    public class CharacterKarma : CharacterEnergyM
    {
        public CharacterKarma(int max, CharacterM hero) 
            : base(CharacterEnergyClass.KP, max, hero)
        {
            CalcThresholds();

            Min = 0;
        }

        protected override void CalcThresholds()
        {
            // We may not need all thresholds.
            if (Max >= 41) // we need all levels then
                Thresholds = new int[] { Max-10, Max-20, Max-30, Max-40 };
            else if (Max >= 31)
                Thresholds = new int[] { Max - 10, Max - 20, Max - 30 };
            else if (Max >= 21)
                Thresholds = new int[] { Max - 10, Max - 20 };
            else if (Max >= 11)
                Thresholds = new int[] { Max - 10 };
        }
    }
}
