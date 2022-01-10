using FateExplorer.Shared;
using FateExplorer.ViewModel;
using System;

namespace FateExplorer.RollLogic
{
    public class AbilityCheckM : CheckBaseM
    {
        public new const string checkTypeId = "DSA5/0/ability";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="abilityValue">Effective value of the ability</param>
        /// <param name="modifier">An additional modifier</param>
        public AbilityCheckM(int abilityValue, int modifier) // TODO: Check if necessary in the long run
        {
            // inherited properties
            AttributeId = "";
            Attribute = new int[1];

            AbilityValue = abilityValue;
            Modifier = modifier;
            RollSeries = new();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="modifier"></param>
        public AbilityCheckM(AbilityDTO ability, ICheckModifierM modifier)
        {
            // inherited properties
            AttributeId = ability.Id;
            Attribute = new int[1];

            AbilityValue = ability.EffectiveValue;
            //Modifier = modifier; // TODO modify
            Name = ability.Name;
            RollSeries = new();
        }

        private enum TRoll { Primary = 0, Confirm = 1 }


        /// <summary>
        /// The ability value to roll against
        /// </summary>
        private int AbilityValue {  get => Attribute[0]; set => Attribute[0] = value; }


        /// <summary>
        /// A simple additive modifier
        /// </summary>
        private int Modifier { get; set; }


        /// <inheritdoc />
        public override RollSuccessLevel Success
        {
            get => RollSeries.Count switch
            {
                0 => RollSuccessLevel.na,
                1 => SuccessHelpers.PrimaryD20Success(RollSeries[0].OpenRoll[0], AbilityValue + Modifier),
                2 => SuccessHelpers.CheckSuccess(RollSeries[0].OpenRoll[0], RollSeries[1].OpenRoll[0], AbilityValue + Modifier),
                _ => RollSuccessLevel.na
            };
        }


        /// <inheritdoc />
        public override RollSuccessLevel RollSuccess(int Roll)
        {
            if (Roll >= RollSeries.Count)
                throw new ArgumentOutOfRangeException(nameof(Roll));

            return RollSeries.Count switch
            {
                0 => RollSuccessLevel.na,
                1 => SuccessHelpers.PrimaryD20Success(RollSeries[0].OpenRoll[0], AbilityValue + Modifier),
                2 => SuccessHelpers.D20Success(RollSeries[1].OpenRoll[0], AbilityValue + Modifier),
                _ => RollSuccessLevel.na
            };
        }


        private IRollM NextStep(TRoll Which) =>
            Which switch
            {
                TRoll.Primary => new D20Roll(null),
                TRoll.Confirm => new D20Roll(new D20ConfirmEntry()),
                _ => throw new ArgumentException()
            };


        /// <inheritdoc/>
        public override IRollM RollNextStep()
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

    }
}
