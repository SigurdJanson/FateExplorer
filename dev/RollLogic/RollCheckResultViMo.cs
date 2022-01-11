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


        /// <summary>
        /// Describing name
        /// </summary>
        public string Name
        {
            get => RollCheck?.Name ?? "";
            private set => RollCheck.Name = value;
        }


        /// <summary>
        /// Success level of the complete check
        /// </summary>
        public RollSuccessLevel SuccessLevel 
        {
            get => RollCheck.Success;
        }


        /// <summary>
        /// The sum of the modifiers put into the roll.
        /// </summary>
        public ICheckModifierM CheckModifier
        {
            get => RollCheck.CheckModifier;
            set => RollCheck.CheckModifier = value;
        }

        /// <summary>
        /// Get the summarized additive modifier after it has been applied to the
        /// roll. See <seealso cref="ICheckModifierM.Total"/>.
        /// </summary>
        public int SummarizedModifier => RollCheck?.CheckModifier?.Total ?? 0;



        /// <summary>
        /// Returns the primary roll itself
        /// </summary>
        /// <returns></returns>
        public IRollM GetPrimaryRoll() // TODO: model is exposed
            => RollCheck?.RollSeries[0];

        /// <summary>
        /// Returns the analysed result of the primary roll
        /// </summary>
        /// <returns></returns>
        /// <remarks>Assumes that the primary roll is at index 0.</remarks>
        public RollResultViMo GetPrimaryResult()
            => GetRollResult(0);


        public bool PrimaryNeedsConfirmation() => RollCheck.NeedsConfirmation;
        public bool NeedsBotchEffect() => RollCheck.NeedsBotchEffect;


        /// <summary>
        /// Return a results object for an individual roll
        /// </summary>
        /// <param name="Step">The roll in the order they were executed.</param>
        /// <returns></returns>
        public RollResultViMo GetRollResult(int Step = -1)
        {
            IRollM CurrentRoll;

            if (RollCheck.RollSeries.Count == 0 || RollCheck.RollSeries is null)
            {
                CurrentRoll = RollCheck.RollNextStep();
                Step = 0;
            }
            else if (Step == -1)
            {
                CurrentRoll = RollCheck.RollSeries[^1];
                Step = RollCheck.RollSeries.Count - 1;
            }
            else
                CurrentRoll = RollCheck.RollSeries[Step];

            RollResultViMo Result = new(RollCheck.Id, CurrentRoll.Sides, FreeDiceCupViMo.CupType.None);
            Result.RollResult = CurrentRoll.OpenRoll;

            Result.SuccessLevel = RollCheck.RollSuccess(Step);
            Result.Modifier = RollCheck.CheckModifier.Total;
            Result.CombinedResult = null;
            Result.RollAgainst = RollCheck.Attribute;

            return Result;
        }


    }
}
