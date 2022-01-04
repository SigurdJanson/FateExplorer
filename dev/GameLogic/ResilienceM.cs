using FateExplorer.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FateExplorer.GameLogic
{
    public class ResilienceM
    {
        private ICharacterM Hero { get; set; }

        /// <summary>
        /// For character generation each race has a base value.
        /// </summary>
        public int BaseValue { get; protected set; }

        /// <summary>
        /// Extra points from dis-/advantages added to the base value.
        /// </summary>
        public int? ExtraValue { get; protected set; } = null;

        /// <summary>
        /// List of ids to identify the attributes needed to compute this attribute.
        /// </summary>
        public string[] DependentAbilities { get; set; }

        private int? value = null;
        public int Value 
        { 
            get
            {
                if (this.value is null)
                    this.value = ComputeValue();

                return (int)this.value;
            }
            protected set
            {
                this.value = value;
            }
        }



        /// <summary>
        /// Computes the resilience value.
        /// </summary>
        /// <param name="Abilities">
        /// If not given, the method will work with the character's base values
        /// </param>
        /// <returns></returns>
        public int ComputeValue(Dictionary<string, int> Abilities = null)
        {
            int AbSum = 0;
            foreach (var a in DependentAbilities)
                AbSum += Abilities?[a] ?? Hero.GetAbility(a);

            int V = BaseValue + (int)Math.Round(AbSum / 6.0) + (ExtraValue ?? 0);

            return V;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameData">Resilience object taken from <see cref="IGameDataService"/>.</param>
        /// <param name="hero">The hero this resilience belongs to.</param>
        public ResilienceM(ResilienceDbEntry gameData, ICharacterM hero)
        {
            Hero = hero;
            DependentAbilities = gameData.DependantAbilities.Clone() as string[];
            BaseValue = gameData.RaceBaseValue[0].Value; // TODO: pick the right race
            ExtraValue = 0; // Unknown at this points, because we do not know special abilities
        }


        public ResilienceM(ICharacterM hero, int baseValue, int? extraValue, string[] dependentAbilities)
        {
            Hero = hero;
            DependentAbilities = dependentAbilities;

            BaseValue = baseValue;
            ExtraValue = extraValue ?? 0;
        }

        public ResilienceM(ICharacterM hero, int value, string[] dependentAbilities)
        {
            Hero = hero;
            DependentAbilities = dependentAbilities;

            int V = 0;
            foreach (var a in DependentAbilities)
                V += Hero.GetAbility(a);
            Value = value;
            BaseValue = (int)(Value - Math.Round(V / 6.0));
            ExtraValue = 0;
        }
    }
}
