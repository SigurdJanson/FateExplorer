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
            RollList = new();
            NextStep(RollType.Primary); // directly roll first roll and add
        }


        /// <summary>
        /// The ability value to roll against
        /// </summary>
        private int AbilityValue {  get => Attribute[0]; set => Attribute[0] = value; }


        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException" />
        public override RollSuccessLevel Success
        {
            get
            {
                if (RollList[RollType.Confirm] is not null)
                    return SuccessHelpers.CheckSuccess(RollList[RollType.Confirm].OpenRoll[0],
                        RollList[RollType.Confirm].OpenRoll[0],
                        AbilityValue + CheckModifier.Total);
                else if (RollList[RollType.Primary] is not null)
                    return SuccessHelpers.PrimaryD20Success(
                        RollList[RollType.Primary].OpenRoll[0],
                        AbilityValue + CheckModifier.Total);
                else return RollSuccessLevel.na;
            }
        }


        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException" />
        public override RollSuccessLevel RollSuccess(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => RollList[RollType.Confirm] is not null ?
                    SuccessHelpers.PrimaryD20Success(RollList[RollType.Primary].OpenRoll[0], 
                        AbilityValue + (CheckModifier?.Total ?? 0)) : RollSuccessLevel.na,
                RollType.Confirm => RollList[RollType.Primary] is not null ?
                    SuccessHelpers.D20Success(RollList[RollType.Confirm].OpenRoll[0], 
                        AbilityValue + (CheckModifier?.Total ?? 0)) : RollSuccessLevel.na,
                _ => RollSuccessLevel.na
            };
        }


        private IRollM NextStep(RollType Which)
        {
            IRollM roll = Which switch
            {
                RollType.Primary => new D20Roll(null),
                RollType.Confirm => NeedsConfirmation ? new D20Roll(new D20ConfirmEntry()) : null,
                _ => throw new ArgumentException("Ability rolls only support primary and confirmation rolls")
            };
            RollList[Which] = roll;
            return roll;
        }



        ///// <inheritdoc/>
        //public override IRollM RollNextStep()
        //{
        //    IRollM NextRoll;
        //    if (RollList[RollType.Primary] is null)
        //    {
        //        NextRoll = NextStep(RollType.Primary);
        //        RollList[RollType.Primary] = NextRoll;
        //    }

        //    else
        //    {
        //        NextRoll = NextStep(RollType.Confirm);
        //        if (!NextRoll.EntryConfirmed())
        //            NextRoll = null; // TODO: instantiate and throw away immediately!!! URGS
        //    }
        //    RollList.Add(NextRoll);
        //    return NextRoll;
        //}


        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            if (AutoRoll && RollList[Which] is null)
                RollList[Which] = NextStep(Which);

            return RollList[Which];
        }



        // inherited bool HasNextStep();


    }
}
