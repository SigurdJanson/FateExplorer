using FateExplorer.RollLogic;
using System;

namespace FateExplorer.Shared
{
    public enum RollSuccessLevel
    {
        Botch = 1, PendingBotch = -1,
        Fail = 2,
        Success = 3,
        Critical = 4, PendingCritical = -4,
        na = 99
    }

    public static class SuccessHelpers
    {

        /// <summary>
        /// Determines the success of a d20 roll against a crition.
        /// </summary>
        /// <param name="Eyes">The result of the roll</param>
        /// <param name="Attribute">The criterion the roll goes against</param>
        /// <returns>The result; criticals/botches are given as *pending*</returns>
        public static RollSuccessLevel PrimaryD20Success(int Eyes, int Attribute)
        {
            if (Eyes > 20 || Eyes < 1) throw new ArgumentOutOfRangeException(nameof(Eyes));
            if (Eyes == 1)
                return RollSuccessLevel.PendingCritical;
            else if (Eyes == 20)
                return RollSuccessLevel.PendingBotch;
            else if (Eyes <= Attribute)
                return RollSuccessLevel.Success;
            else
                return RollSuccessLevel.Fail;
        }



        /// <summary>
        /// Determines the success of a d20 roll against a crition.
        /// </summary>
        /// <param name="Eyes">The result of the roll</param>
        /// <param name="Attribute">The criterion the roll goes against</param>
        /// <returns>The result</returns>
        public static RollSuccessLevel D20Success(int Eyes, int Attribute)
        {
            if (Eyes == 1)
                return RollSuccessLevel.Critical;
            else if (Eyes == 20)
                return RollSuccessLevel.Botch;
            else if (Eyes <= Attribute)
                return RollSuccessLevel.Success;
            else
                return RollSuccessLevel.Fail;
        }


        /// <summary>
        /// Combines two roll results and determines the final success level after the confirmation roll.
        /// </summary>
        /// <param name="Primary">Success of the primary roll</param>
        /// <param name="Confirm">Success of the confirmation roll</param>
        /// <returns></returns>
        public static RollSuccessLevel CheckSuccess(RollSuccessLevel Primary, RollSuccessLevel Confirm)
        {
            if (Primary == RollSuccessLevel.na) return RollSuccessLevel.na;

            if (Primary == RollSuccessLevel.Critical || Primary == RollSuccessLevel.PendingCritical)
            {
                if (Confirm == RollSuccessLevel.Botch ||
                    Confirm == RollSuccessLevel.Fail)
                    return RollSuccessLevel.Success;
                else if (Confirm == RollSuccessLevel.na)
                    return RollSuccessLevel.PendingCritical;
                else
                    return RollSuccessLevel.Critical;
            }
            else if (Primary == RollSuccessLevel.Botch || Primary == RollSuccessLevel.PendingBotch)
            {
                if (Confirm == RollSuccessLevel.Botch ||
                    Confirm == RollSuccessLevel.Fail)
                    return RollSuccessLevel.Botch;
                else if (Confirm == RollSuccessLevel.na)
                    return RollSuccessLevel.PendingBotch;
                else
                    return RollSuccessLevel.Fail;
            }
            else
                return Primary;
        }


        /// <summary>
        /// Combines two roll results and determines the final success level after the confirmation roll.
        /// Both are rolled against the same criterion.
        /// </summary>
        /// <param name="PrimaryEyes">The result of the primary roll</param>
        /// <param name="ConfirmEyes">The result of the confirmation roll</param>
        /// <param name="Attribute">The criterion the rolls go against</param>
        /// <returns></returns>
        public static RollSuccessLevel CheckSuccess(int PrimaryEyes, int ConfirmEyes, int Attribute)
        {
            RollSuccessLevel Confirm;
            RollSuccessLevel Primary = PrimaryD20Success(PrimaryEyes, Attribute);
            if (Primary == RollSuccessLevel.PendingCritical || Primary == RollSuccessLevel.PendingBotch)
            {
                Confirm = D20Success(ConfirmEyes, Attribute);
                return CheckSuccess(Primary, Confirm);
            } 
            else
            {
                return Primary;
            }
                

        }
    }
}
