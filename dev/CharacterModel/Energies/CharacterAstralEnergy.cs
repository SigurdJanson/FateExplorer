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
        //-Next statement removed bauce it is now handled by `GetDisAdvantageModifier`
        //protected const int AstralBaseEnergy = 20;


        public CharacterAstralEnergy(EnergiesDbEntry gameData, CharacterEnergyClass _Class, int AddedEnergy, ICharacterM hero) 
            : base(gameData, _Class, AddedEnergy, hero)
        {
            if (_Class != CharacterEnergyClass.AE)
                throw new ArgumentException($"Class has been instantiated with the wrong type of energy", nameof(_Class));

            Max = gameData.RaceBaseValue.First(bv => bv.RaceId == Hero.SpeciesId).Value;
            // Some traditions (coded as special abilities) allow adding the value of an
            // basic ability (COU, SAG, ...) to the energy level
            foreach (var (specialability, ability) in gameData.TraditionBonus)
                if (hero.HasSpecialAbility(specialability))
                    Max += Hero.Abilities[ability].Value;
            //-Next statement removed bauce it is now handled by `GetDisAdvantageModifier`
            //-Max += AstralBaseEnergy; // does not use the enegergies.json file because it is a must
            Max += AddedEnergy;
            Max += GetDisAdvantageModifier(gameData);

            Min = 0;

            CalcThresholds(Max);
        }
    }
}
