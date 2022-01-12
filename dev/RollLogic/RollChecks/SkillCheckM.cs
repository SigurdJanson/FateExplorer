using FateExplorer.Shared;
using FateExplorer.ViewModel;
using System;

namespace FateExplorer.RollLogic
{


    public class SkillCheckM : CheckBaseM
    {
        public new const string checkTypeId = "DSA5/0/skill";

        public override string CheckTypeId { get => checkTypeId + Domain; }

        public SkillDomain Domain { get; protected set; }

        // inherited Attribute

        public int Skill { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="ability"></param>
        public SkillCheckM(SkillsDTO skill, AbilityDTO[] ability)
        {
            // inherited
            Attribute = new int[3];
            Attribute[0] = ability[0].EffectiveValue;
            Attribute[1] = ability[1].EffectiveValue;
            Attribute[2] = ability[2].EffectiveValue;

            Skill = skill.EffectiveValue;

            AttributeId = skill.Id;
            Domain = skill.Domain;
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


        public static int ComputeSkillRemainder(int[] Eyes, int[] Attributes, int Skill, int Mod)
        {
            int Count20s = 0, Count1s = 0;
            foreach (int e in Eyes)
                if (e == 1) Count1s++;
                else if (e == 20) Count20s++;

            if (Count1s >= 2)
                return Skill;
            else if (Count20s >= 2)
                return 0;

            int[] EffectiveAttr = new int[3], Check = new int[3];
            for (int i = 0; i < Attributes.Length; i++)
                EffectiveAttr[i] = Attributes[i] + Mod;

            for (int i = 0; i < Attributes.Length; i++)
                Check[i] = Math.Max(Eyes[i] - EffectiveAttr[i], 0);

            int CheckSum = 0;
            Array.ForEach(Check, (int i) => CheckSum += i);
            return Skill - CheckSum;
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


        /// <inheritdoc />
        public override RollSuccessLevel Success
        {
            get => RollList.Count switch
            {
                0 => RollSuccessLevel.na,
                1 => ComputeSuccess(RollList[0].OpenRoll, Attribute, Skill, CheckModifier.Total),
                _ => RollSuccessLevel.na
            };
        }


        ///// <inheritdoc />
        //public override RollSuccessLevel RollSuccess(int Roll)
        //{
        //    if (Roll >= RollList.Count)
        //        throw new ArgumentOutOfRangeException(nameof(Roll));

        //    return RollList.Count switch
        //    {
        //        0 => RollSuccessLevel.na,
        //        1 => ComputeSuccess(RollList[0].OpenRoll, Attribute, Skill, CheckModifier.Total),
        //        _ => RollSuccessLevel.na
        //    };
        //}

        //public override IRollM GetConfirmationRoll()
        //{
        //    throw new NotImplementedException();
        //}

        public override RollSuccessLevel RollSuccess(RollType Which)
        {
            throw new NotImplementedException();
        }

        public override IRollM GetRoll(RollType Which, bool AutoRoll = false)
        {
            throw new NotImplementedException();
        }
    }
}
