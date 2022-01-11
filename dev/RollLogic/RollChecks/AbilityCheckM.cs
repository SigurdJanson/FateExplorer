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
        /// <param name="ability"></param>
        /// <param name="modifier"></param>
        public AbilityCheckM(AbilityDTO ability, SimpleCheckModifierM modifier)
        {
            // inherited properties
            AttributeId = ability.Id;
            Attribute = new int[1];
            CheckModifier = modifier ?? new SimpleCheckModifierM(0);

            AbilityValue = ability.EffectiveValue;
            Name = ability.Name;
            RollSeries = new();
            RollNextStep(); // directly roll first roll and add
        }


        /// <summary>
        /// The ability value to roll against
        /// </summary>
        private int AbilityValue {  get => Attribute[0]; set => Attribute[0] = value; }


        /// <inheritdoc />
        public override RollSuccessLevel Success
        {
            get => RollSeries.Count switch
            {
                0 => RollSuccessLevel.na,
                1 => SuccessHelpers.PrimaryD20Success(RollSeries[0].OpenRoll[0], 
                    AbilityValue + CheckModifier.Total),
                2 => SuccessHelpers.CheckSuccess(RollSeries[0].OpenRoll[0], RollSeries[1].OpenRoll[0], 
                    AbilityValue + CheckModifier.Total),
                _ => RollSuccessLevel.na
            };
        }


        /// <inheritdoc />
        public override RollSuccessLevel RollSuccess(int Roll)
        {
            if (Roll >= RollSeries.Count)
                throw new ArgumentOutOfRangeException(nameof(Roll));

            return Roll switch
            {
                (int)RollType.Primary => 
                    SuccessHelpers.PrimaryD20Success(RollSeries[0].OpenRoll[0], AbilityValue + (CheckModifier?.Total ?? 0)),
                (int)RollType.Confirm => 
                    SuccessHelpers.D20Success(RollSeries[1].OpenRoll[0], AbilityValue + (CheckModifier?.Total ?? 0)),
                _ => RollSuccessLevel.na
            };
        }


        private IRollM NextStep(RollType Which) =>
            Which switch
            {
                RollType.Primary => new D20Roll(null),
                RollType.Confirm => new D20Roll(new D20ConfirmEntry()),
                _ => throw new ArgumentException()
            };


        /// <inheritdoc/>
        public override IRollM RollNextStep()
        {
            IRollM NextRoll;
            if (RollSeries.Count == 0)
                NextRoll = NextStep(RollType.Primary);
            else
            {
                NextRoll = NextStep(RollType.Confirm);
                if (!NextRoll.EntryConfirmed())
                    NextRoll = null; // TODO: instantiate and throw away immediately!!! URGS
            }
            RollSeries.Add(NextRoll);
            return NextRoll;
        }


        // inherited bool HasNextStep();


    }
}
