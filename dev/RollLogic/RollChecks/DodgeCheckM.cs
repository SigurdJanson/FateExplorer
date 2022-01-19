using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public class DodgeCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/dodge";


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
        /// Constructor
        /// </summary>
        /// <param name="dodge"></param>
        /// <param name="modifier"></param>
        public DodgeCheckM(DodgeDTO dodge, ICheckModifierM modifier)
        {
            // inherited properties
            AttributeId = dodge.Id;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            CheckModifier = modifier ?? new SimpleCheckModifierM(0);

            RollAttr[0] = dodge.EffectiveValue;
            RollAttrName[0] = dodge.Name;
            Name = dodge.Name;

            RollList = new();
            ThrowCup(RollType.Primary); // directly roll first roll and add
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
                        CheckModifier.Apply(RollAttr[0]));

                else if (RollList[RollType.Primary] is not null)
                    return SuccessHelpers.PrimaryD20Success(
                        RollList[RollType.Primary].OpenRoll[0],
                        CheckModifier.Apply(RollAttr[0]));
                else return RollSuccessLevel.na;
            }
        }


        /// <inheritdoc />
        /// <remarks>Dodge rolls do not provide a classification</remarks>
        public override string ClassificationLabel => null;

        /// <inheritdoc />
        /// <remarks>Dodge rolls do not provide a classification</remarks>
        public override string Classification => null;



        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder
        {
            get => throw new NotImplementedException();
        }



        // ROLL /////////////////////////////////

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException" />
        public override RollSuccessLevel RollSuccess(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => RollList[RollType.Confirm] is not null ?
                    SuccessHelpers.PrimaryD20Success(
                        RollList[RollType.Primary].OpenRoll[0],
                        CheckModifier.Apply(RollAttr[0]))
                    : RollSuccessLevel.na,
                RollType.Confirm => RollList[RollType.Primary] is not null ?
                    SuccessHelpers.D20Success(
                        RollList[RollType.Confirm].OpenRoll[0],
                        CheckModifier.Apply(RollAttr[0]))
                    : RollSuccessLevel.na,
                _ => RollSuccessLevel.na
            };
        }



        /// <summary>
        /// Roll the dice for the selected roll.
        /// </summary>
        /// <param name="Which">The desired roll</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private IRollM ThrowCup(RollType Which)
        {
            IRollM roll = Which switch
            {
                RollType.Primary => new D20Roll(null),
                RollType.Confirm => NeedsConfirmation ? new D20Roll(new D20ConfirmEntry()) : null,
                RollType.Botch => NeedsBotchEffect ? new BotchEffectRoll() : null,
                _ => throw new ArgumentException("Dodge rolls only support primary, confirmation, and botch rolls")
            };
            RollList[Which] = roll;
            return roll;
        }



        /// <inheritdoc />
        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            if (AutoRoll && RollList[Which] is null)
                RollList[Which] = ThrowCup(Which);

            return RollList[Which];
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int[] RollRemainder(RollType Which)
        {
            throw new NotImplementedException();
        }
    }
}
