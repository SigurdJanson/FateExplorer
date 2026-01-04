using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Linq;

namespace FateExplorer.CharacterModel
{
    public class CharacterAstralEnergy : CharacterEnergyM
    {

        public CharacterAstralEnergy(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero) 
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.AE)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy", nameof(_Class));

            Max = gameData.RaceBaseValue.FirstOrDefault(bv => bv.RaceId == Hero.SpeciesId)?.Value ?? 0;

            // Some traditions (coded as special abilities) allow adding the value of an
            // basic ability (COU, SAG, ...) to the energy level
            foreach (var (specialability, ability) in gameData.TraditionBonus)
                if (hero.HasSpecialAbility(specialability))
                    Max += Hero.Abilities[ability].Effective;

            Max += AddedEnergy;
            Max += GetDisAdvantageModifier(gameData);

            Min = 0;

            CalcThresholds(Max);
        }
    }
}
