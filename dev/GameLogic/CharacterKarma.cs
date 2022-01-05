using FateExplorer.GameData;
using System;

namespace FateExplorer.GameLogic
{
    public class CharacterKarma : CharacterEnergyM
    {

        public CharacterKarma(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero)
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.KP)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy", nameof(_Class));

            Max = gameData.RaceBaseValue[0].Value; // TODO: pick the right value
            foreach (var a in gameData.DependantAbilities)
                Max += Hero.Abilities[a].Value;
            Max += AddedEnergy;

            Min = 0;

            CalcThresholds();
        }



        protected override void CalcThresholds()
        {
            // We may not need all thresholds when Max is low
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
