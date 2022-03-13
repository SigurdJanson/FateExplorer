using FateExplorer.GameData;
using FateExplorer.Shared;
using FateExplorer.ViewModel;
using System;

namespace FateExplorer.RollLogic
{

    public class SkillCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/skill";


        /// <summary>
        /// The domain of the skill
        /// </summary>
        public SkillDomain Domain { get; protected set; }


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
        /// <param name="skill"></param>
        /// <param name="ability"></param>
        public SkillCheckM(SkillsDTO skill, AbilityDTO[] ability, ICheckModifierM modifier, IGameDataService gameData)
            : base(gameData)
        {
            // inherited
            RollAttr = new int[3];
            RollAttrName = new string[3];
            for (int a = 0; a < 3; a++)
            {
                RollAttr[a] = ability[a].EffectiveValue;
                RollAttrName[a] = ability[a].ShortName;
            }
            CheckModifier = modifier ?? new SimpleCheckModifierM(0);

            TargetAttr = skill.EffectiveValue;
            TargetAttrName = skill.Name;

            Name = skill.Name;
            AttributeId = skill.Id;
            Domain = skill.Domain;

            RollList = new();
            ThrowCup(RollType.Primary); // directly roll first roll and add
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

            return Math.Max(((Remainder - 1) / 3) + 1, 1);
        }



        //public static int[] ComputeAttributeRemainder(int[] Eyes, int[] Attributes, int Mod) //- is this needed???
        //{
        //    int DieCount = Eyes.Length;

        //    int[] EffectiveAttr = new int[DieCount], Check = new int[DieCount];
        //    for (int i = 0; i < Attributes.Length; i++)
        //        EffectiveAttr[i] = Attributes[i] + Mod;

        //    for (int i = 0; i < Attributes.Length; i++)
        //        Check[i] = Math.Max(Eyes[i] - EffectiveAttr[i], 0);

        //    return Check;
        //}


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
            //-int DieCount = Eyes.Length;
            if (Eyes.Length != Attributes.Length) 
                throw new ArgumentException("Skill checks need as many rolled dice as abilities");

            int Count20s = 0, Count1s = 0;
            foreach (int e in Eyes)
                if (e == 1) Count1s++;
                else if (e == 20) Count20s++;

            if (Count1s >= 2)
                return Skill;
            else if (Count20s >= 2)
                return 0;

            //-int EffectiveAttr; // = new int[DieCount];
            //-int[] Check = new int[DieCount];
            // Determine how many points must be compensated by the skill value
            int ToCompensate = 0;
            for (int i = 0; i < Attributes.Length; i++)
            {
                int EffectiveAttr = Attributes[i] + Mod;
                //-int Check = Math.Max(Eyes[i] - EffectiveAttr, 0);
                ToCompensate += Math.Max(Eyes[i] - EffectiveAttr, 0); //-Check;
            }
            //-for (int i = 0; i < Attributes.Length; i++)
            //-    Check[i] = Math.Max(Eyes[i] - EffectiveAttr[i], 0);

            // Subtract 
            //-Array.ForEach(Check, (int i) => ToCompensate += i);
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
            return ComputeSuccess(
                        RollList[Which].OpenRoll,
                        CheckModifier.Apply(RollAttr),
                        TargetAttr ?? 0,
                        0); // mod not required here because skill value is already modified
        }



        /// <inheritdoc/>
        public override int Remainder
        {
            get => ComputeSkillRemainder(
                RollList[RollType.Primary].OpenRoll,
                CheckModifier.Apply(RollAttr),
                TargetAttr ?? 0,
                0);// mod not required here because skill value is already modified
        }


        /// <inheritdoc />
        public override string ClassificationLabel => "QL";

        /// <inheritdoc />
        public override string Classification => ComputeSkillQuality(Remainder).ToString();

        /// <inheritdoc />
        public override string ClassificationDescr => null;


        // ROLLS  ////////////////////////

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
                //RollType.Confirm => NeedsConfirmation ? new SkillRoll() : null,
                RollType.Botch => NeedsBotchEffect ? new BotchEffectRoll() : null,
                _ => throw new ArgumentException("Skill rolls only support primary and confirmation rolls")
            };
            RollList[Which] = roll;

            if (Which == RollType.Primary)
            {
                RollSuccess.Level s = ComputeSuccess(RollList[RollType.Primary].OpenRoll, CheckModifier.Apply(RollAttr), TargetAttr ?? 0, 0);
                Success.Update(s, s);
            }

            return roll;
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
