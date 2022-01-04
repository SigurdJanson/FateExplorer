using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.GameLogic
{
    public class CharacterHealth : CharacterResourceM
    {

        public int MaxHealth { get; set; }

        /// <summary>
        /// Effective health points
        /// </summary>
        public int Health { get; set; }


        public CharacterHealth(int max, CharacterM hero) 
            : base(CharacterResourceClass.Health, max, hero)
        {
            CalcThresholds();

            // If a character’s LP total ever drops below zero by an amount equal to
            // or greater than the character’s Constitution stat, the character dies
            // immediately. (Core Rules, p. 340)
            Min = - Hero.GetAbility(AbilityM.CON);
        }

        protected void CalcThresholds()
        {
            // Since the lowest level is fixed at 5 we may not need all thresholds.
            if (Max >= 5.5 * 4) // we need all levels then
                Thresholds = new int[] { (int)Math.Round((double)Max * 3 / 4), (int)Math.Round((double)Max / 2), (int)Math.Round((double)Max / 4), 5 };
            else if (Max >= 5.5 * 2)
                Thresholds = new int[] { (int)Math.Round((double)Max * 3 / 4), (int)Math.Round((double)Max / 2), 5 };
            else if (Max >= 5.5 * 4 / 3)
                Thresholds = new int[] { (int)Math.Round((double)Max * 3 / 4), 5 };
        }
    }
}
