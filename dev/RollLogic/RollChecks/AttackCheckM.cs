﻿using FateExplorer.GameData;
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
        public CombatBranch CombatTechType { get; protected set; }

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
        public AttackCheckM(WeaponViMo weapon, ICheckModifierM modifier, IGameDataService gameData)
            : base(gameData)
        {
            // inherited properties
            AttributeId = weapon.CombatTechId;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            CheckModifier = modifier ?? new SimpleCheckModifierM(0);

            RollAttr[0] = weapon.AtSkill;
            RollAttrName[0] = weapon.Name;
            Name = weapon.Name;

            // specialised properties
            DamageDieCount = weapon.DamageDieCount;
            DamageDieSides = weapon.DamageDieSides;
            DamageBonus = weapon.DamageBonus;
            CombatTechType = weapon.Branch;
            IsImprovised = weapon.Improvised;

            RollList = new();
            ThrowCup(RollType.Primary); // directly roll first roll and add
        }



        /// <inheritdoc />
        public override RollSuccess Success { get; protected set; }
 


        /// <inheritdoc />
        public override string ClassificationLabel
        {
            get
            {
                if (RollList[RollType.Botch] is not null)
                {
                    int Result = RollList[RollType.Botch].OpenRollCombined();
                    var Botch = GameData.GetAttackBotch(CombatTechType, Result);

                    return Botch.Label;
                }
                else
                    return null;
            }
        }

        /// <inheritdoc />
        /// <remarks>Returns the damage in case of combat attack checks</remarks>
        public override string Classification
        {
            get
            {
                int? Result = null;
                if (RollList[RollType.Damage] is not null)
                {
                    int RollResult;
                    RollResult = RollList[RollType.Damage].OpenRollCombined();
                    // TODO: Move the damage assessment to the weapon
                    if (Success == RollSuccess.Level.Success)
                        Result = RollResult + DamageBonus;
                    if (Success == RollSuccess.Level.Critical)
                        Result = (RollResult + DamageBonus) * 2;

                    return Result is null ? null : $"{Result}";
                }
                else if (RollList[RollType.Botch] is not null)
                {
                    Result = RollList[RollType.Botch].OpenRollCombined();
                    var Botch = GameData.GetAttackBotch(CombatTechType, (int)Result);
                    
                    return $"({Result} = {string.Join(" + ", RollList[RollType.Botch].OpenRoll)})";
                }
                else
                    return null;
            }
        }

        public override string ClassificationDescr
        {
            get
            {
                if (RollList[RollType.Botch] is not null)
                {
                    int Result = RollList[RollType.Botch].OpenRollCombined();
                    var Botch = GameData.GetAttackBotch(CombatTechType, (int)Result);

                    return Botch.Descr;
                }
                return null;
            }
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder
        {
            get => throw new NotImplementedException();
        }


        // inherited: public override bool NeedsBotchEffect



        /// <summary>
        /// Needs a roll to determine the damage caused by a combat roll. By default 
        /// this is false.
        /// </summary>
        public override bool NeedsDamage
        {
            get => (Success == RollSuccess.Level.Success ||
                Success == RollSuccess.Level.Critical) &&
                RollList[RollType.Damage] is null;
        }


        // ROLL /////////////////////////////////

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException" />
        public override RollSuccess.Level SuccessOfRoll(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => Success.PrimaryLevel,
                RollType.Confirm => Success.ConfirmationLevel,
                _ => RollSuccess.Level.na
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

            if (Which == RollType.Primary || Which == RollType.Confirm)
                Success.Update(RollList[RollType.Primary], RollList[RollType.Confirm], CheckModifier.Apply(RollAttr[0]));

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

    }
}
