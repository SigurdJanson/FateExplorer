using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Linq;

namespace FateExplorer.CharacterModel
{
    public class CharacterHealth : CharacterEnergyM
    {

        public CharacterHealth(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero)
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.LP)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy",
                    nameof(_Class));

            Max = gameData.RaceBaseValue.FirstOrDefault(bv => bv.RaceId == Hero.SpeciesId)?.Value ?? 0;
            foreach (var a in gameData.DependantAbilities)
                Max += Hero.Abilities[a].Value;
            Max += AddedEnergy;
            Max += GetDisAdvantageModifier(gameData);

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
            decimal dEffMax = (decimal)EffMax;
            if (EffMax >= 22) // 22 == 5.5 * 4 // we need all levels then because (EffMax * 3/4 > 5)
                Thresholds = new int[]
                {
                    (int)Math.Floor(dEffMax * 3 / 4 + 0.5m), // use `Floor`(x + 0.5m)` to avoid round-to-even: (int)Math.Floor(X + 0.5m)
				    (int)Math.Floor(dEffMax / 2 + 0.5m),
                    (int)Math.Floor(dEffMax / 4 + 0.5m),
                    5
                };
            else if (EffMax >= 11) // 11 == 5.5 * 2 - here EffMax/2 > 5
                Thresholds = new int[]
                {
                    (int)Math.Floor(dEffMax * 3 / 4 + 0.5m),
                    (int)Math.Floor(dEffMax / 2 + 0.5m),
                    5
                };
            else if (EffMax >= 8) // 8 > 5.5 * 4 / 3 - here EffMax/4 > 5
                Thresholds = new int[]
                {
                    (int)Math.Floor(dEffMax * 3 / 4 + 0.5m),
                    5
                };
            else
                Thresholds = new int[] { 5 };
        }
    }
}
