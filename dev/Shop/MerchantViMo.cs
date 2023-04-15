using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.RollLogic;
using FateExplorer.Shared;
using System;

namespace FateExplorer.Shop
{
    public class MerchantViMo
    {
        private readonly IGameDataService GameData;

        private const string CommerceId = "TAL_46";
        private const int IncompleteBargain = -1;
        private const int AbilityDefault = 13;


        public MerchantViMo(IGameDataService gameData)
        {
            GameData = gameData ?? throw new ArgumentNullException(nameof(gameData));
            Sagacity = 0;
            Intuition = 0;
            Charisma = 0;
        }



        #region CHARACTER DATA

        private int sagacity, intuition, charisma;

        /// <summary>
        /// The merchant's sagacity. Use -3 to +3 from a merchant's default.
        /// </summary>
        public int Sagacity { get => sagacity - AbilityDefault; set => sagacity = AbilityDefault + value; }
        /// <summary>
        /// The merchant's intuition. Use -3 to +3 from a merchant's default.
        /// </summary>
        public int Intuition { get => intuition - AbilityDefault; set => intuition = AbilityDefault + value; }
        /// <summary>
        /// The merchant's charme. Use -3 to +3 from a merchant's default.
        /// </summary>
        public int Charisma { get => charisma - AbilityDefault; set => charisma = AbilityDefault + value; }


        public enum ExperienceLevel { Novice = 0, Advanced, Competent, Proficient, Expert, Legend }
        public int TradeSkillValue { get; protected set; } = 9;

        public ExperienceLevel TradeExperience
        {
            get
            {
                if (TradeSkillValue <= 3)
                    return ExperienceLevel.Novice;
                else if (TradeSkillValue <= 6)
                    return ExperienceLevel.Advanced;
                else if (TradeSkillValue <= 9)
                    return ExperienceLevel.Competent;
                else if (TradeSkillValue <= 12)
                    return ExperienceLevel.Proficient;
                else if (TradeSkillValue <= 15)
                    return ExperienceLevel.Expert;
                else
                    return ExperienceLevel.Legend;
            }
            set
            {
                TradeSkillValue = value switch
                {
                    ExperienceLevel.Novice => 3,
                    ExperienceLevel.Advanced => 6,
                    ExperienceLevel.Competent => 9,
                    ExperienceLevel.Proficient => 12,
                    ExperienceLevel.Expert => 15,
                    ExperienceLevel.Legend => 18,
                    _ => 0
                };
            }
        }

        #endregion




        /// <summary>
        /// The merchant's quality level of the last haggle check.
        /// Roll a new check with <see cref="Haggle()"/>.
        /// </summary>
        public int HaggleQL { get; protected set; } = IncompleteBargain;



        /// <summary>
        /// Roll a new haggle check and determine the QL
        /// </summary>
        /// <returns></returns>
        public int Haggle()
        {
            // Create skill "Commerce"
            SkillsDTO skill = new ()
            {
                Id = CommerceId,
                EffectiveValue = TradeSkillValue,
                Min = 0,
                Max = 20,
                Name = "Commerce (Haggle)", 
                Domain = Check.Skill.Skill
            };

            // Create set of abilities
            string[] FindAbs = new string[] { AbilityM.SGC, AbilityM.INT, AbilityM.CHA };
            AbilityDTO[] ability = new AbilityDTO[3];
            for (int a = 0; a < FindAbs.Length; a++)
            {
                var aData = GameData.Abilities[FindAbs[a]];
                int AbilityValue = FindAbs[a] switch
                {
                    AbilityM.SGC => this.sagacity, // use value *incl.* ability default
                    AbilityM.INT => this.intuition,
                    AbilityM.CHA => this.charisma,
                    _ => 0
                };
                ability[a] = new AbilityDTO()
                {
                    Id = aData.Id,
                    Name = aData.Name,
                    ShortName = aData.ShortName,
                    Min = 0,
                    Max = 20,
                    EffectiveValue = AbilityValue
                };
            }

            // Roll check
            var CheckResult = new SkillCheckM(skill, ability, null, GameData);
            HaggleQL = SkillCheckM.ComputeSkillQuality(CheckResult.Remainder);
            return HaggleQL;
        }


        /// <summary>
        /// Competitive check [Trade (Haggling) against Trade (Haggling)]; 
        /// subtract the loser’s SP from the winner’s SP and calculate QL; the
        /// price rises or falls 10% per point of QL(but not by more than 50%)
        /// </summary>
        /// <param name="ListPrice">Original price</param>
        /// <param name="BuyerHaggleQL">Quality level of the buyer's Trade (Haggling) check</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public decimal DeterminePrice(decimal ListPrice, int BuyerHaggleQL)
        {
            if (BuyerHaggleQL < 0) throw new ArgumentOutOfRangeException(nameof(BuyerHaggleQL));
            if (HaggleQL == IncompleteBargain) return ListPrice;

            decimal Correction = (HaggleQL - BuyerHaggleQL) * 0.1M + 1;
            if (Correction < 0.5M) Correction = 0.5M;
            if (Correction > 1.5M) Correction = 1.5M;

            return ListPrice * Correction;
        }
    }
}
