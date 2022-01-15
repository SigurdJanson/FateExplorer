using FateExplorer.Shared;
using FateExplorer.ViewModel;
using System;

namespace FateExplorer.RollLogic
{

    public class SkillCheckM : CheckBaseM
    {
        /// <inheritdoc />
        public new const string checkTypeId = "DSA5/0/skill";



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
        public SkillCheckM(SkillsDTO skill, AbilityDTO[] ability, ICheckModifierM modifier)
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



        public static int[] ComputeAttributeRemainder(int[] Eyes, int[] Attributes, int Mod) //- is this needed???
        {
            int DieCount = Eyes.Length;

            int[] EffectiveAttr = new int[DieCount], Check = new int[DieCount];
            for (int i = 0; i < Attributes.Length; i++)
                EffectiveAttr[i] = Attributes[i] + Mod;

            for (int i = 0; i < Attributes.Length; i++)
                Check[i] = Math.Max(Eyes[i] - EffectiveAttr[i], 0);

            return Check;
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
            int DieCount = Eyes.Length;

            int Count20s = 0, Count1s = 0;
            foreach (int e in Eyes)
                if (e == 1) Count1s++;
                else if (e == 20) Count20s++;

            if (Count1s >= 2)
                return Skill;
            else if (Count20s >= 2)
                return 0;

            int[] EffectiveAttr = new int[DieCount], Check = new int[DieCount];
            for (int i = 0; i < Attributes.Length; i++)
                EffectiveAttr[i] = Attributes[i] + Mod;

            for (int i = 0; i < Attributes.Length; i++)
                Check[i] = Math.Max(Eyes[i] - EffectiveAttr[i], 0);

            int CheckSum = 0;
            Array.ForEach(Check, (int i) => CheckSum += i);
            return Skill - CheckSum;
        }


        /// <inheritdoc />
        public override RollSuccessLevel Success
        {
            get
            {
                if (RollList[RollType.Confirm] is not null)
                    return SuccessHelpers.CheckSuccess(
                        RollSuccess(RollType.Primary),
                        RollSuccess(RollType.Confirm));
                else if (RollList[RollType.Primary] is not null)
                    return RollSuccess(RollType.Primary);
                else
                    return RollSuccessLevel.na;
            }
        }



        public static RollSuccessLevel ComputeSuccess(int[] Eyes, int[] Attributes, int Skill, int Mod)
        {
            int Count20s = 0, Count1s = 0;
            foreach (int e in Eyes)
                if (e == 1) Count1s++;
                else if (e == 20) Count20s++;

            if (Count1s >= 2)
                return RollSuccessLevel.Critical;
            else if (Count20s >= 2)
                return RollSuccessLevel.Botch;
            else
            {
                int Remainder = ComputeSkillRemainder(Eyes, Attributes, Skill, Mod);

                if (Remainder >= 0)
                    return RollSuccessLevel.Success;
                else
                    return RollSuccessLevel.Fail;
            }
        }




        public override RollSuccessLevel RollSuccess(RollType Which)
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
                RollType.Primary => new SkillRoll(null),
                RollType.Confirm => NeedsConfirmation ? new SkillRoll(new D20ConfirmEntry()) : null,
                RollType.Botch => NeedsBotchEffect ? new BotchEffectRoll() : null,
                _ => throw new ArgumentException("Ability rolls only support primary and confirmation rolls")
            };
            RollList[Which] = roll;
            return roll;
        }



        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            if (AutoRoll && RollList[Which] is null)
                RollList[Which] = ThrowCup(Which);

            return RollList[Which];
        }
    }
}
