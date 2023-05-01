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
        /// <remarks>In this context it is the parry skill value.</remarks>
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
        /// The wielded weapon int this check
        /// </summary>
        public WeaponM Weapon { get; protected set; }
        /// <summary>
        /// The weapon carried in the other hand 
        /// </summary>
        public WeaponM OtherWeapon { get; protected set; }

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
        public ParryCheckM(WeaponM weapon, WeaponM otherWeapon, bool isMainHand, BattlegroundM context, IGameDataService gameData)
            : base(context, gameData)
        {
            Weapon = weapon; // the Weapon to use
            OtherWeapon = otherWeapon;
            // inherited properties
            //Context = context; //Already assigned through base
            Context.OnStateChanged += UpdateAfterModifierChange;
            AttributeId = weapon.CombatTechId;
            RollAttr = new int[1];
            RollAttrName = new string[1];

            RollAttr[0] = weapon.PaSkill(isMainHand, otherWeapon.Branch, otherWeapon.IsParry, otherWeapon.ParryMod);
            RollAttrName[0] = ResourceId.ParryLabelId;
            Name = weapon.Name;

            // improvised weapons: 19 AND 20 are botches
            if (weapon.IsImprovised)
                Success.BotchThreshold = 19;

            // specialised properties
            CombatTechType = weapon.Branch;
            CombatTech = weapon.CombatTechId;
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
                Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Parry, CombatTech), Weapon));

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

        /// <inheritdoc/>
        public override int ModDelta => Context.ModDelta(RollAttr[0], new Check(Check.Combat.Parry, CombatTech), Weapon);


        // inherited: public override bool NeedsBotchEffect



        /// <summary>
        /// Needs a roll to determine the damage caused by a combat roll. By default 
        /// this is false.
        /// </summary>
        public override bool NeedsDamage
        {
            get => false;
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
                _ => throw new ArgumentException("Combat rolls support primary, confirmation, and botch rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary || Which == RollType.Confirm)
                Success.Update(
                    RollList[RollType.Primary], 
                    RollList[RollType.Confirm], 
                    Context.ApplyTotalMod(RollAttr[0], new Check(Check.Combat.Parry, CombatTech), Weapon));

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
                RollType.Primary => Context.GetTotalMod(RollAttr[0], new Check(Check.Combat.Parry, CombatTech), Weapon),
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
