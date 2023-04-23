using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public class ParryCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/combat/parry";


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
        /// The combat technique
        /// </summary>
        public string CombatTech { get; protected set; }

        /// <summary>
        /// The kind of combat technique behind the weapon
        /// </summary>
        public CombatBranch CombatTechType { get; protected set; }

        /// <summary>
        /// Is the weapon improvised (affects botch rolls)?
        /// </summary>
        public bool IsImprovised { get; protected set; }



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">A context for the roll check determining the modifier</param>
        /// <param name="gameData">Access to the data base</param>
        public ParryCheckM(BattlegroundM context, IGameDataService gameData)//(WeaponM weapon, bool mainHand, WeaponM otherWeapon, ICheckModificatorM modifier, IGameDataService gameData)
            : base(gameData)
        {
            WeaponM weapon = context.UseMainHand ? context.MainWeapon : context.OffWeapon; // the weapon to use
            WeaponM otherWeapon = context.UseMainHand ? context.OffWeapon : context.MainWeapon;
            // inherited properties
            AttributeId = weapon.CombatTechId;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            Context = context;
            Context.OnStateChanged += UpdateAfterModifierChange;

            RollAttr[0] = weapon.PaSkill(context.UseMainHand, otherWeapon.Branch, otherWeapon.IsParry, otherWeapon.ParryMod);
            RollAttrName[0] = ResourceId.ParryLabelId;
            Name = weapon.Name;

            // improvised weapons: 19 AND 20 are botches
            if (weapon.IsImprovised)
                Success.BotchThreshold = 19;

            // specialised properties
            CombatTechType = weapon.Branch;
            IsImprovised = weapon.IsImprovised;

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
                Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Parry, CombatTech)));

        /// <inheritdoc />
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
                if (RollList[RollType.Botch] is not null)
                {
                    int Result = RollList[RollType.Botch].OpenRollCombined();
                    var Botch = GameData.GetParryBotch(CombatTechType, Result);

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
                if (RollList[RollType.Botch] is not null)
                {
                    int Result = RollList[RollType.Botch].OpenRollCombined();
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
                    var Botch = GameData.GetParryBotch(CombatTechType, (int)Result);

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
                _ => throw new ArgumentException("Ability rolls only support primary and confirmation rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary || Which == RollType.Confirm)
                Success.Update(RollList[RollType.Primary], RollList[RollType.Confirm], Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Parry, CombatTech)));

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
            get => false;
        }
    }
}
