using FateExplorer.GameData;
using System;
using System.Linq;

namespace FateExplorer.GameLogic
{
    public class CharacterHealth : CharacterEnergyM
    {

        public CharacterHealth(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero)
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.LP)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy",
                    nameof(_Class));

            int RaceBaseValue = gameData.RaceBaseValue.First(bv => bv.RaceId == Hero.SpeciesId).Value;
            Max = RaceBaseValue;
            foreach (var a in gameData.DependantAbilities)
                Max += Hero.Abilities[a].Value;
            Max += AddedEnergy;

            // If a character’s LP total ever drops below zero by an amount equal to
            // or greater than the character’s Constitution stat, the character dies
            // immediately. (Core Rules, p. 340)
            Min = -Hero.GetAbility(AbilityM.CON);

            CalcThresholds();
        }

        public override void CalcThresholds(int EffMax = -1)
        {
            if (EffMax < 0) EffMax = Max;

            // Since the lowest level is fixed at 5 we may not need all thresholds.
            if (EffMax >= 5.5 * 4) // we need all levels then
                Thresholds = new int[] { (int)Math.Round((double)EffMax * 3 / 4), (int)Math.Round((double)EffMax / 2), (int)Math.Round((double)EffMax / 4), 5 };
            else if (EffMax >= 5.5 * 2)
                Thresholds = new int[] { (int)Math.Round((double)EffMax * 3 / 4), (int)Math.Round((double)EffMax / 2), 5 };
            else if (EffMax >= 5.5 * 4 / 3)
                Thresholds = new int[] { (int)Math.Round((double)EffMax * 3 / 4), 5 };
        }
    }
}
