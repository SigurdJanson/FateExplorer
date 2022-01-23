using FateExplorer.GameData;
using System;
using System.Linq;

namespace FateExplorer.GameLogic
{
    public class CharacterKarma : CharacterEnergyM
    {

        public CharacterKarma(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero)
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.KP)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy", nameof(_Class));

            int RaceBaseValue = gameData.RaceBaseValue.First(bv => bv.RaceId == Hero.SpeciesId).Value;
            Max = RaceBaseValue;
            foreach (var a in gameData.DependantAbilities)
                Max += Hero.Abilities[a].Value;
            Max += AddedEnergy;

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
