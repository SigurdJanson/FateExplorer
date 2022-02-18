using FateExplorer.GameData;
using System;
using System.Linq;

namespace FateExplorer.CharacterModel
{
    public class CharacterAstralEnergy : CharacterEnergyM
    {
        /// <summary>
        /// Basic amount of astral energy granted by advantage "spellcaster"
        /// </summary>
        protected const int AstralBaseEnergy = 20;


        public CharacterAstralEnergy(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero) 
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.AE)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy", nameof(_Class));

            int RaceBaseValue = gameData.RaceBaseValue.First(bv => bv.RaceId == Hero.SpeciesId).Value;
            Max = RaceBaseValue;
            foreach (var a in gameData.DependantAbilities)
                Max += Hero.Abilities[a].Value;
            Max += AstralBaseEnergy;
            Max += AddedEnergy;

            Min = 0;

            CalcThresholds(Max);
        }
    }
}
