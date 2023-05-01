using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    /// <summary>
    /// INItiative rolls
    /// </summary>
    /// <remarks>
    /// Does not support a modifier. Instead the modifier is 
    /// "abused" to add the initiative base value.
    /// </remarks>
    public class InitiativeCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/initiative";

        /// <inheritdoc />
        /// <remarks>Not used in this context</remarks>
        public override int? TargetAttr { get; protected set; }

        /// <inheritdoc />
        /// <remarks>Not used in this context</remarks>
        public override string TargetAttrName { get; protected set; }


        /// <inheritdoc />
        /// <remarks>In this context the initiative value</remarks>
        public override int[] RollAttr { get; protected set; }

        /// <inheritdoc />
        /// <remarks>In this context the initiative value</remarks>
        public override string[] RollAttrName { get; protected set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InitiativeVal"></param>
        /// <param name="gameData"></param>
        public InitiativeCheckM(CharacterAttrDTO Initiative, BattlegroundM context, IGameDataService gameData)
            : base(context, gameData)
        {
            // inherited properties
            AttributeId = Initiative.Id;
            RollAttr = new int[1];
            RollAttrName = new string[1];
            //Context = context; //Already assigned through base
            Context.OnStateChanged += UpdateAfterModifierChange;

            RollAttr[0] = Initiative.EffectiveValue;
            RollAttrName[0] = Initiative.Name;
            Name = Initiative.Name;

            RollList = new();
            NextStep(RollType.Primary); // directly roll first roll and add
        }


        /// <inheritdoc />
        /// <remarkes>This check does not use modifiers</remarkes>
        public override void UpdateAfterModifierChange()
        { }

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
        public override string Classification 
            => $"{RollList[RollType.Primary].OpenRoll[0] + RollAttr[0] + RollModifier(RollType.Primary)}";

        /// <inheritdoc />
        public override string ClassificationLabel => RollAttrName[0];

        /// <inheritdoc />
        /// <remarks>Initiative rolls do not provide a classification description</remarks>
        public override string ClassificationDescr => null;


        /// <inheritdoc/>
        /// <remarks>Not needed at the moment</remarks>
        public override int Remainder
        {
            get => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int ModDelta => Context.ModDelta(RollAttr[0], new Check(Check.Roll.Initiative), null);


        /// <inheritdoc/>
        public override bool NeedsBotchEffect
        { get => false; }



        // ROLL /////////////////////////////////

        /// <inheritdoc />
        public override RollSuccess.Level SuccessOfRoll(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => RollSuccess.Level.na,
                _ => RollSuccess.Level.na
            };
        }




        private IRollM NextStep(RollType Which)
        {
            IRollM roll = Which switch
            {
                RollType.Primary => new DieRollM(6),
                RollType.Confirm => NeedsConfirmation ? new DieRollM(6) : null,
                _ => throw new ArgumentException("Initiative rolls only support primary and tie rolls")
            };
            RollList[Which] = roll;

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
                RollType.Primary => Context.GetTotalMod(RollAttr[0], new Check(Check.Roll.Initiative), null),
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
