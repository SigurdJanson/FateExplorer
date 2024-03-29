﻿using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public class DodgeCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/dodge";

        /// <inheritdoc />
        public override Check WhichCheck => new(Check.Roll.Dodge);


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
        /// Does the dodging character carry a weapon?
        /// </summary>
        /// <remarks>Needed for botch effects.</remarks>
        public bool CarriesWeapon { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <inheritdoc />
        /// <param name="dodge"></param>
        /// <param name="carriesWeapon">Does the character who dodges carry a weapon? Needed for botch rolls.</param>
        /// <param name="context"></param>
        public DodgeCheckM(CharacterAttrDTO dodge, bool carriesWeapon, BattlegroundM context, IGameDataService gameData)
            : base(context, gameData)
        {
            // inherited properties
            //Context = context; //Already assigned through base
            Context.OnStateChanged += UpdateAfterModifierChange;
            AttributeId = dodge.Id;
            RollAttr = new int[1];
            RollAttrName = new string[1];

            RollAttr[0] = dodge.EffectiveValue;
            RollAttrName[0] = dodge.Name;
            Name = dodge.Name;
            CarriesWeapon = carriesWeapon;

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
                Context.ApplyTotalMod(RollAttr[0], new Check(Check.Roll.Dodge), null));

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
                    var WeaponBranch = CarriesWeapon ? CombatBranch.Melee : CombatBranch.Unarmed;
                    var Botch = GameData.GetDodgeBotch(WeaponBranch, Result);

                    return Botch.Label;
                }
                else
                    return null;
            }
        }


        /// <inheritdoc />
        /// <remarks>Dodge rolls do not provide a classification</remarks>
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
                    var WeaponBranch = CarriesWeapon ? CombatBranch.Melee : CombatBranch.Unarmed;
                    var Botch = GameData.GetDodgeBotch(WeaponBranch, Result);

                    return Botch.Descr;
                }
                return null;
            }
        }


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder => throw new NotImplementedException();

        /// <inheritdoc/>
        public override int ModDelta => Context.ModDelta(RollAttr[0], new Check(Check.Roll.Dodge), null);



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
                _ => throw new ArgumentException("Dodge rolls only support primary, confirmation, and botch rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary || Which == RollType.Confirm)
                Success.Update(
                    RollList[RollType.Primary],
                RollList[RollType.Confirm],
                Context.ApplyTotalMod(RollAttr[0], new Check(Check.Roll.Dodge), null));

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
                RollType.Primary => Context.GetTotalMod(RollAttr[0], new Check(Check.Roll.Dodge), null),
                _ => throw new NotImplementedException()
            };
        }
        
        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int[] RollRemainder(RollType Which)
        {
            throw new NotImplementedException();
        }
    }
}
