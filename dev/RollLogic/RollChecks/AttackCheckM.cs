using FateExplorer.Shared;
using FateExplorer.ViewModel;
using System;

namespace FateExplorer.RollLogic
{
    public class AttackCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/combat/attack";


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


        // COMBAT SPECIFIC PROPERTIES
        /// <summary>
        /// The kind of combat technique behind the weapon
        /// </summary>
        public CombatTechniques CombatTechType { get; protected set; }

        /// <summary>
        /// Is the weapon improvised (affects botch rolls)?
        /// </summary>
        public bool IsImprovised { get; protected set; }

        public int DamageDieCount { get; protected set; }
        public int DamageDieSides { get; protected set; }
        public int DamageBonus { get; protected set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="modifier"></param>
        public AttackCheckM(WeaponViMo weapon, ICheckModifierM modifier)
        {
            // inherited properties
            AttributeId = weapon.CombatTechId;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            CheckModifier = modifier ?? new SimpleCheckModifierM(0);

            RollAttr[0] = weapon.AtSkill;
            RollAttrName[0] = weapon.Name;
            Name = weapon.Name;

            DamageDieCount = weapon.DamageDieCount;
            DamageDieSides = weapon.DamageDieSides;
            DamageBonus = weapon.DamageBonus;
            IsImprovised = weapon.Improvised;

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
        public override string ClassificationLabel => "abbrvHP"; // TODO: is a string like that for l10n the best way?

        /// <inheritdoc />
        /// <remarks>Returns the damage in case of combat attack checks</remarks>
        public override string Classification
        {
            get
            {
                int RollResult;
                int? Result = null;
                if (RollList[RollType.Damage] is null)
                    return null;
                else
                    RollResult = RollList[RollType.Damage].OpenRollCombined();

                if (Success == RollSuccessLevel.Success)
                    Result = RollResult + DamageBonus;
                if (Success == RollSuccessLevel.Critical)
                    Result = (RollResult + DamageBonus) * 2;

                return Result is null ? null : $"{Result}";
            }
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder
        {
            get => throw new NotImplementedException();
        }

        // inherited: public override bool NeedsBotchEffect



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
                RollType.Primary => new D20Roll(),
                RollType.Confirm => NeedsConfirmation ? new D20Roll() : null,
                RollType.Botch => NeedsBotchEffect ? new BotchEffectRoll() : null,
                RollType.Damage => NeedsDamage ? new MultiDieRoll(DamageDieSides, DamageDieCount) : null,
                _ => throw new ArgumentException("Ability rolls only support primary and confirmation rolls")
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
        /// <remarks>Not needed at the moment for combat rolls</remarks>
        public override int[] RollRemainder(RollType Which)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Needs a roll to determine the damage caused by a combat roll. By default 
        /// this is false.
        /// </summary>
        public override bool NeedsDamage
        {
            get => (Success == RollSuccessLevel.Success ||
                Success == RollSuccessLevel.Critical) &&
                RollList[RollType.Damage] is null;
        }
    }
}
