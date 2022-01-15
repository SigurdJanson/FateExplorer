using FateExplorer.Shared;
using FateExplorer.ViewModel;
using System;

namespace FateExplorer.RollLogic
{
    public class AbilityCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/ability";


        /// <inheritdoc />
        /// <remarks>In this context it is the skill value.</remarks>
        public override int? TargetAttr { get; protected set; }

        /// <inheritdoc />
        /// <remarks>In this context it is the skill.</remarks>
        public override string TargetAttrName { get; protected set; }


        /// <inheritdoc />
        /// <remarks>In this context the ability values</remarks>
        public override int[] RollAttr { get; protected set; }

        /// <inheritdoc />
        /// <remarks>In this context the ability values</remarks>
        public override string[] RollAttrName { get; protected set; }


        /// <summary>
        /// Access to the roll attribute, i.e. the ability value to roll against
        /// </summary>
        private int AbilityValue { get => RollAttr[0]; set => RollAttr[0] = value; }



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="modifier"></param>
        public AbilityCheckM(AbilityDTO ability, ICheckModifierM modifier)
        {
            // inherited properties
            AttributeId = ability.Id;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            CheckModifier = modifier ?? new SimpleCheckModifierM(0);

            AbilityValue = ability.EffectiveValue; // implicitely sets `this.Attribute` 
            RollAttrName[0] = ability.Name;
            Name = ability.Name;

            RollList = new();
            NextStep(RollType.Primary); // directly roll first roll and add
        }


        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException" />
        public override RollSuccessLevel Success
        {
            get
            {
                if (RollList[RollType.Confirm] is not null)
                    return SuccessHelpers.CheckSuccess(
                        RollList[RollType.Primary].OpenRoll[0],
                        RollList[RollType.Confirm].OpenRoll[0],
                        CheckModifier.Apply(AbilityValue));

                else if (RollList[RollType.Primary] is not null)
                    return SuccessHelpers.PrimaryD20Success(
                        RollList[RollType.Primary].OpenRoll[0],
                        CheckModifier.Apply(AbilityValue));
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
                    SuccessHelpers.PrimaryD20Success(
                        RollList[RollType.Primary].OpenRoll[0],
                        CheckModifier.Apply(AbilityValue))
                    : RollSuccessLevel.na,
                RollType.Confirm => RollList[RollType.Primary] is not null ?
                    SuccessHelpers.D20Success(
                        RollList[RollType.Confirm].OpenRoll[0],
                        CheckModifier.Apply(AbilityValue)) 
                    : RollSuccessLevel.na,
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



        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            if (AutoRoll && RollList[Which] is null)
                RollList[Which] = NextStep(Which);

            return RollList[Which];
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int[] RollRemainder(RollType Which)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder 
        { 
            get => throw new NotImplementedException(); 
        }


        public override bool NeedsBotchEffect
        { get => false; }
    }
}
