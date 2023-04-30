using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public class AbilityCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/ability";


        /// <inheritdoc />
        /// <remarks>Not used in this context</remarks>
        public override int? TargetAttr { get; protected set; }

        /// <inheritdoc />
        /// <remarks>Not used in this context</remarks>
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
        /// <param name="gameData"></param>
        public AbilityCheckM(AbilityDTO ability, ICheckModificatorM modifier, IGameDataService gameData)
            :base(gameData)
        {
            // inherited properties
            AttributeId = ability.Id;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            CheckModificator = modifier ?? new SimpleCheckModificatorM(Modifier.Neutral);
            CheckModificator.OnStateChanged += UpdateAfterModifierChange;

            AbilityValue = ability.EffectiveValue; // implicitely sets `this.Attribute` 
            RollAttrName[0] = ability.Name;
            Name = ability.Name;

            RollList = new();
            NextStep(RollType.Primary); // directly roll first roll and add
        }


        /// <inheritdoc />
        public override void UpdateAfterModifierChange()
            => Success.Update(RollList[RollType.Primary], RollList[RollType.Confirm], CheckModificator.Apply(AbilityValue));


        /// <inheritdoc />
        protected override void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // release unmanaged resources
                CheckModificator.OnStateChanged -= UpdateAfterModifierChange;

                if (disposedStatus) {/*Released managed resources*/}
            }
        }



        /// <inheritdoc />
        public override RollSuccess Success { get; protected set; }



        /// <inheritdoc />
        /// <remarks>Ability rolls do not provide a classification</remarks>
        public override string ClassificationLabel => null;

        /// <inheritdoc />
        /// <remarks>Ability rolls do not provide a classification</remarks>
        public override string Classification => null;

        /// <inheritdoc />
        /// <remarks>Ability rolls do not provide a classification</remarks>
        public override string ClassificationDescr => null;


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder
        {
            get => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool NeedsBotchEffect
        { get => false; }




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


        private IRollM NextStep(RollType Which)
        {
            IRollM roll = Which switch
            {
                RollType.Primary => new D20Roll(),
                RollType.Confirm => NeedsConfirmation ? new D20Roll() : null,
                _ => throw new ArgumentException("Ability rolls only support primary and confirmation rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary || Which == RollType.Confirm)
                Success.Update(RollList[RollType.Primary], RollList[RollType.Confirm], CheckModificator.Apply(AbilityValue));

            return roll;
        }


        /// <inheritdoc />
        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            if (AutoRoll && RollList[Which] is null)
                RollList[Which] = NextStep(Which);

            return RollList[Which];
        }


        /// <inheritdoc/>
        public override Modifier RollModifier(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => Context.GetTotalMod(RollAttr[0], new Check(Check.Roll.Ability), null),
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
