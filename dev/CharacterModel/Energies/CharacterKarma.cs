using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Linq;

namespace FateExplorer.CharacterModel
{
    /// <summary>
    /// Instantiate this class only when the character has the advantage blessed.
    /// </summary>
    public class CharacterKarma : CharacterEnergyM
    {


        public CharacterKarma(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero)
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.KP)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy", nameof(_Class));

            Max = gameData.RaceBaseValue.FirstOrDefault(bv => bv.RaceId == Hero.SpeciesId)?.Value ?? 0;
            // Some traditions (coded as special abilities) allow adding the value of an
            // basic ability (COU, SAG, ...) to the energy level
            foreach (var (specialability, ability) in gameData.TraditionBonus)
                if (hero.HasSpecialAbility(specialability))
                    Max += Hero.Abilities[ability].Value;

            Max += AddedEnergy;
            Max += GetDisAdvantageModifier(gameData);

            Min = 0;

            CalcThresholds();
        }



        public override void CalcThresholds(int EffMax = -1)
        {
            if (EffMax < 0) EffMax = Max;

            // We may not need all thresholds when Max is low
            if (EffMax >= 41) // we need all levels then
                Thresholds = new int[] { EffMax - 10, EffMax - 20, EffMax - 30, EffMax - 40 };
            else if (EffMax >= 31)
                Thresholds = new int[] { EffMax - 10, EffMax - 20, EffMax - 30 };
            else if (EffMax >= 21)
                Thresholds = new int[] { EffMax - 10, EffMax - 20 };
            else if (EffMax >= 11)
                Thresholds = new int[] { EffMax - 10 };
        }
    }
}
