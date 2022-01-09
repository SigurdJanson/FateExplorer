using System;

namespace FateExplorer.RollLogic
{
    public class AbilityCheckM : CheckBaseM
    {
        public new const string checkTypeId = "DSA5/0/ability";

        public AbilityCheckM(int abilityValue, int modifier)
        {
            AbilityValue = abilityValue;
            Modifier = modifier;
            RollSeries = new();
        }


        private enum TRoll { Primary = 0, Confirm = 1 }


        /// <summary>
        /// The ability value to roll against
        /// </summary>
        private int AbilityValue {  get; set; }


        /// <summary>
        /// A simple additive modifier
        /// </summary>
        private int Modifier { get; set; }



        private IRollM NextStep(TRoll Which) =>
            Which switch
            {
                TRoll.Primary => new D20Roll(null),
                TRoll.Confirm => new D20Roll(new D20ConfirmEntry()),
                _ => throw new ArgumentException()
            };


        /// <inheritdoc/>
        public override IRollM NextStep()
        {
            IRollM NextRoll;
            if (RollSeries.Count == 0)
                NextRoll = NextStep(TRoll.Primary);
            else
            {
                NextRoll = NextStep(TRoll.Confirm);
                if (!NextRoll.EntryConfirmed())
                    NextRoll = null; // TODO: instantiate and throw away immediately!!! URGS
            }
            RollSeries.Add(NextRoll);
            return NextRoll;
        }


        // inherited bool HasNextStep();


        /// <inheritdoc/>
        public override RollResultViMo GetRollResult(int Step = -1)
        {
            IRollM CurrentRoll;
            if (Step == -1)
                CurrentRoll = RollSeries[^1];
            else
                CurrentRoll = RollSeries[Step];
            
            RollResultViMo Result = new(this.Id, CurrentRoll.Sides, FreeDiceCupViMo.CupType.None);
            Result.RollResult = CurrentRoll.OpenRoll;

            if (CurrentRoll.OpenRoll[0] == 1)
                Result.SuccessLevel = RollSuccessLevel.Critical;
            else if (CurrentRoll.OpenRoll[0] == 20)
                Result.SuccessLevel = RollSuccessLevel.Botch;
            else if (CurrentRoll.OpenRoll[0] > AbilityValue)
                Result.SuccessLevel = RollSuccessLevel.Success;
            else
                Result.SuccessLevel = RollSuccessLevel.Fail;

            Result.Modifier = Modifier;

            Result.CombinedResult = null;

            Result.RollAgainst = new int[1] { AbilityValue };

            return Result;
        }


        /// <inheritdoc/>
        public override RollResultViMo GetCheckResult()
        {
            throw new NotImplementedException();
        }
    }
}
