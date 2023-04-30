using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
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
        /// The wielded Weapon int this check
        /// </summary>
        public WeaponM Weapon { get; protected set; }
        /// <summary>
        /// The Weapon carried in the other hand 
        /// </summary>
        public WeaponM OtherWeapon { get; protected set; }

        /// <summary>
        /// The combat technique
        /// </summary>
        public string CombatTech { get; protected set; }
        /// <summary>
        /// The kind of combat technique behind the Weapon
        /// </summary>
        public CombatBranch CombatTechType { get; protected set; }

        /// <summary>
        /// Is the Weapon improvised (affects botch rolls)?
        /// </summary>
        public bool IsImprovised { get; protected set; }

        /// <summary>
        /// The number of dice used to determine hit points; i.e. the 2 in 2W6+3
        /// </summary>
        public int DamageDieCount { get; protected set; }
        /// <summary>
        /// The die used to determine hit points; i.e. the 6 in 2W6+3
        /// </summary>
        public int DamageDieSides { get; protected set; }
        /// <summary>
        /// The added constant used to determine hit points; i.e. the 3 in 2W6+3
        /// </summary>
        public int DamageBonus { get; protected set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">A context for the roll check determining the modifier</param>
        /// <param name="gameData">Access to the data base</param>
        public AttackCheckM(WeaponM weapon, WeaponM otherWeapon, bool isMainHand, BattlegroundM context, IGameDataService gameData)
            : base(gameData)
        {
            Weapon = weapon; // the Weapon to use
            OtherWeapon = otherWeapon;
            // inherited properties
            Context = context;
            Context.OnStateChanged += UpdateAfterModifierChange;
            AttributeId = Weapon.CombatTechId;
            RollAttr = new int[1];
            RollAttrName = new string[1];

            RollAttr[0] = Weapon.AtSkill(isMainHand, OtherWeapon.Branch);
            RollAttrName[0] = ResourceId.AttackLabelId;
            Name = Weapon.Name;

            // improvised weapons: 19 AND 20 are botches
            if (Weapon.IsImprovised)
                Success.BotchThreshold = 19;

            // specialised properties
            DamageDieCount = Weapon.DamageDieCount;
            DamageDieSides = Weapon.DamageDieSides;
            DamageBonus = Weapon.DamageBonus;
            CombatTech = Weapon.CombatTechId;
            CombatTechType = Weapon.Branch;
            IsImprovised = Weapon.IsImprovised;

            RollList = new();
            ThrowCup(RollType.Primary); // directly roll first roll and add
        }


        /// <summary>
        /// Update the check assessment after a modifier update
        /// </summary>
        public override void UpdateAfterModifierChange() 
            => Success.Update(
                RollList[RollType.Primary], 
                RollList[RollType.Confirm], 
                Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Attack, CombatTech), Weapon));

        protected override void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // release unmanaged resources
                Context.OnStateChanged -= UpdateAfterModifierChange;

                if (disposedStatus) {/*Released managed resources*/}
            }
        }


        /// <inheritdoc />
        public override RollSuccess Success { get; protected set; }
 


        /// <inheritdoc />
        public override string ClassificationLabel
        {
            get
            {
                if (RollList[RollType.Damage] is not null)
                {
                    return ResourceId.HitPoints;
                }
                else if (RollList[RollType.Botch] is not null)
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
        /// <remarks>Returns the damage in case of combat attack checks 
        /// or the botch effect</remarks>
        public override string Classification
        {
            get
            {
                int? Result = null;
                if (RollList[RollType.Damage] is not null)
                {
                    int RollResult;
                    RollResult = RollList[RollType.Damage].OpenRollCombined();
                    // TODO: Move the damage assessment to the Weapon
                    if (Success.CurrentLevel == RollSuccess.Level.Success)
                        Result = RollResult + DamageBonus;
                    else if (Success.CurrentLevel == RollSuccess.Level.Critical)
                        Result = (RollResult + DamageBonus) * 2;

                    string BonusStr = (DamageDieCount, DamageBonus) switch
                    {
                        (DamageDieCount: 1, 0) 
                            => "",
                        (DamageDieCount: 1, DamageBonus: var y) when y > 0
                            => $" ({RollResult} + {DamageBonus})",
                        (DamageDieCount: var x, 0) when x > 1 
                            => $" ({DamageDieCount} x{RollResult / DamageDieCount})",
                        (DamageDieCount: var x, DamageBonus: var y) when x > 1 && y > 0 
                            => $" ({DamageDieCount} x {RollResult / DamageDieCount} + {DamageBonus})",
                        _ => throw new NotImplementedException(),
                    };
                    return Result is null ? null : $"{Result}{BonusStr}";
                }
                else if (RollList[RollType.Botch] is not null)
                {
                    Result = RollList[RollType.Botch].OpenRollCombined();
                    
                    return $"({Result} = {string.Join(" + ", RollList[RollType.Botch].OpenRoll)})";
                }
                else
                    return null;
            }
        }


        /// <inheritdoc />
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
            get => (Success.CurrentLevel == RollSuccess.Level.Success ||
                Success.CurrentLevel == RollSuccess.Level.Critical) &&
                RollList[RollType.Damage] is null;
        }


        // ROLL /////////////////////////////////

        /// <inheritdoc />
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
                _ => throw new ArgumentException("Combat rolls support primary, confirmation, and botch rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary || Which == RollType.Confirm)
                Success.Update(
                    RollList[RollType.Primary], 
                    RollList[RollType.Confirm], 
                    Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Attack, CombatTech), Weapon));

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
        public override Modifier RollModifier(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => Context.GetTotalMod(RollAttr[0], new Check(Check.Combat.Attack, CombatTech), Weapon),
                _ => throw new NotImplementedException()
            };
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment for combat rolls</remarks>
        public override int[] RollRemainder(RollType Which)
        {
            throw new NotImplementedException();
        }

    }
}
