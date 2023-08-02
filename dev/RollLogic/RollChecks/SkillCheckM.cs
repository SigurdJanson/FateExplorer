using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{

    public class SkillCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/skill";

        /// <inheritdoc />
        public override Check WhichCheck => new(Domain);


        /// <summary>
        /// The domain of the skill
        /// </summary>
        public Check.Skill Domain { get; protected set; }


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
        /// <param name="skill">The skill to check</param>
        /// <param name="ability">The abilities needed for the check</param>
        /// <param name="modifier">An optional modifier (may be <c>null</c>).</param>
        /// <param name="gameData">Access to the game data base</param>
        public SkillCheckM(SkillsDTO skill, AbilityDTO[] ability, BaseContextM context, IGameDataService gameData)
            : base(context, gameData)
        {
            // inherited
            RollAttr = new int[3];
            RollAttrName = new string[3];
            for (int a = 0; a < 3; a++)
            {
                RollAttr[a] = ability[a].EffectiveValue;
                RollAttrName[a] = ability[a].ShortName;
            }
            //Context = context; //Already assigned through base
            Context.OnStateChanged += UpdateAfterModifierChange;

            TargetAttr = skill.EffectiveValue;
            TargetAttrName = skill.Name;

            Name = skill.Name;
            AttributeId = skill.Id;
            Domain = skill.Domain;

            RollList = new();
            ThrowCup(RollType.Primary); // directly roll first roll and add
        }





        /// <summary>
        /// Update the check assessment after a modifier update
        /// </summary>
        public override void UpdateAfterModifierChange()
        {
            // NOTE: skill rolls cannot use the logic of RollSuccess which is made for a simple 1d20
            //--- => Success.Update(RollList[RollType.Primary], RollList[RollType.Confirm], CheckModificator.Apply(RollAttr[0]));
            int[] EffectiveSkill = Context.ApplyTotalMod(RollAttr, new Check(Domain), null);
            var s = ComputeSuccess(RollList[RollType.Primary].OpenRoll, EffectiveSkill, TargetAttr ?? 0, 0);
            Success.Update(s, s);
        }


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




        /// <summary>
        /// Determines the quality level given a number of remaining skill points
        /// </summary>
        /// <param name="Remainder">The remaining skill points after the roll</param>
        /// <returns>A quality level (see <seealso href="https://ulisses-regelwiki.de/checks.html">
        /// Core Rules page 23</seealso>)</returns>
        public static int ComputeSkillQuality(int Remainder)
        {
            if (Remainder < 0) return 0;

            return Math.Max((Remainder + 2) / 3, 1);
        }


        /// <summary>
        /// Computes how many skill points remain after compensating all rolls that
        /// exceeded their according attribute.
        /// </summary>
        /// <param name="Eyes">Rolled die eyes</param>
        /// <param name="Attributes">Attribute values (abilities)</param>
        /// <param name="Skill">Skill value</param>
        /// <param name="Mod">An additive modifier</param>
        /// <returns></returns>
        public static int ComputeSkillRemainder(int[] Eyes, int[] Attributes, int Skill, int Mod)
        {
            if (Eyes.Length != Attributes.Length) 
                throw new ArgumentException("Skill checks need as many rolled dice as abilities");

            int Count20s = 0, Count1s = 0;
            foreach (int e in Eyes)
                if (e == 1) Count1s++;
                else if (e == 20) Count20s++;

            if (Count1s >= 2)
                return Skill;
            else if (Count20s >= 2)
                return -1;

            // Determine how many points must be compensated by the skill value
            int ToCompensate = 0;
            for (int i = 0; i < Attributes.Length; i++)
            {
                int EffectiveAttr = Attributes[i] + Mod;
                ToCompensate += Math.Max(Eyes[i] - EffectiveAttr, 0); //-Check;
            }

            // Subtract 
            return Skill - ToCompensate;
        }


        /// <inheritdoc />
        public override RollSuccess Success { get; protected set; }



        public static RollSuccess.Level ComputeSuccess(int[] Eyes, int[] Attributes, int Skill, int Mod)
        {
            int Count20s = 0, Count1s = 0;
            foreach (int e in Eyes)
                if (e == 1) Count1s++;
                else if (e == 20) Count20s++;

            if (Count1s >= 2)
                return RollSuccess.Level.Critical;
            else if (Count20s >= 2)
                return RollSuccess.Level.Botch;
            else
            {
                int Remainder = ComputeSkillRemainder(Eyes, Attributes, Skill, Mod);

                if (Remainder >= 0)
                    return RollSuccess.Level.Success;
                else
                    return RollSuccess.Level.Fail;
            }
        }




        public override RollSuccess.Level SuccessOfRoll(RollType Which)
        {
            int[] EffectiveSkills = Context.ApplyTotalMod(RollAttr, new Check(Domain), null);
            return ComputeSuccess(
                        RollList[Which].OpenRoll,
                        EffectiveSkills,
                        TargetAttr ?? 0,
                        0); // mod not required here because skill value is already modified
        }



        /// <inheritdoc/>
        public override int Remainder
        {
            get => ComputeSkillRemainder(
                RollList[RollType.Primary].OpenRoll,
                Context.ApplyTotalMod(RollAttr, new Check(Domain), null),
                TargetAttr ?? 0,
                0);// mod not required here because skill value is already modified
        }

        /// <inheritdoc/>
        public override int ModDelta
        {
            get
            {
                int[] EffectiveSkills = Context.ApplyTotalMod(RollAttr, new Check(Domain), null);
                int MaxMod = -1;
                for (int i = 0; i < EffectiveSkills.Length; i++)
                {
                    EffectiveSkills[i] = RollAttr[i] - EffectiveSkills[i];
                    if (Math.Abs(EffectiveSkills[i]) > MaxMod) MaxMod = EffectiveSkills[i];
                }
                return MaxMod;
            }
        }


        /// <inheritdoc />
        public override string ClassificationLabel
        {
            get
            {
                if (RollList[RollType.Botch] is not null)
                {
                    int Result = RollList[RollType.Botch].OpenRollCombined();
                    var Botch = GameData.GetSkillBotch(Domain, Result);

                    return Botch.Label;
                }
                else
                    return "QL";
            }
        }

        /// <inheritdoc />
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
                    return ComputeSkillQuality(Remainder).ToString();
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
                    var Botch = GameData.GetSkillBotch(Domain, (int)Result);

                    return Botch.Descr;
                }
                return null;
            }
        }


        // ROLLS  ////////////////////////

        public override bool NeedsBotchEffect
        {
            get => base.NeedsBotchEffect && Domain != Check.Skill.Skill;
        }

        /// <inheritdoc/>
        public override int[] RollRemainder(RollType Which)
        {
            if (Which != RollType.Primary && Which != RollType.Confirm)
                throw new ArgumentException("Skill rolls only support remainders for primary and confirmation rolls", nameof(Which));

            throw new NotImplementedException();
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
                RollType.Primary => new SkillRoll(),
                RollType.Botch => NeedsBotchEffect ? new BotchEffectRoll() : null,
                _ => throw new ArgumentException("Skill rolls only support primary and confirmation rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary)
            {
                int[] EffectiveSkills = Context.ApplyTotalMod(RollAttr, new Check(Domain), null);
                // NOTE: skill rolls cannot use the logic of RollSuccess which is made for a simple 1d20
                RollSuccess.Level s = ComputeSuccess(RollList[RollType.Primary].OpenRoll, EffectiveSkills, TargetAttr ?? 0, 0);
                Success.Update(s, s);
            }

            return roll;
        }


        /// <inheritdoc/>
        public override Modifier RollModifier(RollType Which)
        {
            return Which switch
            {
                RollType.Primary => Context.GetTotalMod(RollAttr[0], new Check(Domain), null),
                _ => throw new NotImplementedException()
            };
        }


        /// <inheritdoc />
        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            if (AutoRoll && RollList[Which] is null)
                RollList[Which] = ThrowCup(Which);

            return RollList[Which];
        }
    }
}
