using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public class RollCheckResultViMo
    {
        protected CheckBaseM RollCheck { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rollCheck">Shall not be null</param>
        public RollCheckResultViMo(CheckBaseM rollCheck)
        {
            RollCheck = rollCheck ?? throw new ArgumentNullException(nameof(rollCheck));
        }



        // CHECK ///////////////////////////////
        #region Check

        /// <summary>
        /// Describing name
        /// </summary>
        public string Name
        {
            get => RollCheck?.Name ?? "";
            private set => RollCheck.Name = value;
        }



        /// <summary>
        /// The sum of the modifiers put into the roll.
        /// </summary>
        public ICheckModificatorM CheckModifier
        {
            get => RollCheck.CheckModificator;
            set => RollCheck.CheckModificator = value;
        }



        /// <summary>
        /// Success level of the complete check
        /// </summary>
        public RollSuccess.Level SuccessLevel
        {
            get => RollCheck.Success.CurrentLevel;
        }


        /// <summary>
        /// Get the summarized additive modifier after it has been applied to the
        /// roll. See <seealso cref="ICheckModificatorM.Total"/>.
        /// </summary>
        public int SummarizedModifier => RollCheck?.CheckModificator?.LastEffectiveDelta[0] ?? 0;//TODO: prüfen: war mal: RollCheck?.CheckModificator?.Total ?? 0;


        /// <summary>
        /// Return the names of the checks roll attribute(s)
        /// </summary>
        public string[] RollAttrName { get => RollCheck.RollAttrName; }


        /// <summary>
        /// The value of the target attribute. If a check does not support the 
        /// target attribute this is null.
        /// </summary>
        public int? TargetAttr { get => RollCheck.TargetAttr; }

        /// <summary>
        /// The name of the target attribute. If a check does not support the 
        /// target attribute this is null.
        /// </summary>
        public string TargetAttrName { get => RollCheck.TargetAttrName; }

        /// <summary>
        /// The remaining eyes of the target attribute after evaluating the check
        /// </summary>
        public int Remainder { get => RollCheck.Remainder; }


        public string ClassificationDescr => RollCheck.ClassificationDescr;
        public string ClassificationLabel => RollCheck.ClassificationLabel;

        public string Classification => RollCheck.Classification;

        #endregion /////////////////////


        // ROLLS ///////////////////////

        /// <summary>
        /// Get remaining eyes for a particular roll after it's evaluation (i.e. roll attribute 
        /// minus roll result).
        /// </summary>
        /// <param name="Which">The desired roll type</param>
        /// <returns>A list of remainders one for each die of the roll</returns>
        public int[] RollRemainder(RollType Which) => RollCheck.RollRemainder(Which);


        /// <summary>
        /// Returns the primary roll itself
        /// </summary>
        /// <returns></returns>
        public IRollM GetPrimaryRoll() // TODO: model is exposed
            => RollCheck?.GetRoll(RollType.Primary);


        /// <summary>
        /// Indicates if the check requires a confirmation roll as immediate next step.
        /// </summary>
        /// <returns>true/false</returns>
        public bool PrimaryNeedsConfirmation() => RollCheck.NeedsConfirmation;


        /// <summary>
        /// Indicates if the check requires a botch effect roll as immediate next step.
        /// </summary>
        /// <returns>true/false</returns>
        public bool NeedsBotchEffect() => RollCheck.NeedsBotchEffect;

        /// <summary>
        /// Needs a roll to determine the damage caused by a combat roll. By default 
        /// this is false.
        /// </summary>
        public bool NeedsDamage() => RollCheck.NeedsDamage;

        /// <summary>
        /// Returns the analysed result of the primary roll
        /// </summary>
        /// <returns></returns>
        public RollResultViMo GetPrimaryResult() => GetRollResult(RollType.Primary);


        /// <summary>
        /// Returns the analysed result of the confirmation roll
        /// </summary>
        /// <returns></returns>
        public RollResultViMo GetConfirmationResult() => GetRollResult(RollType.Confirm);


        /// <summary>
        /// Returns the analysed result of the botch effect roll
        /// </summary>
        /// <returns></returns>
        public RollResultViMo GetBotchResult() => GetRollResult(RollType.Botch);


        /// <summary>
        /// Return a results object for an individual roll
        /// </summary>
        /// <param name="Step">The roll in the order they were executed.</param>
        /// <returns></returns>
        public RollResultViMo GetRollResult(RollType Which, bool ForceRoll = false)
        {
            IRollM CurrentRoll = RollCheck.GetRoll(Which, ForceRoll);
            if (CurrentRoll is null)
                return null;

            RollResultViMo Result = new(RollCheck.Id, CurrentRoll.Sides, FreeDiceCupViMo.CupType.None)
            {
                RollResult = CurrentRoll.OpenRoll,
                SuccessLevel = RollCheck.SuccessOfRoll(Which),
                Modifier = RollCheck.CheckModificator.LastEffectiveDelta,
                CombinedResult = CurrentRoll.OpenRollCombined(),
                RollAgainst = RollCheck.RollAttr
            };

            return Result;
        }


    }
}
