using FateExplorer.RollLogic;
using System;

namespace FateExplorer.Shared
{


    public static class SuccessHelpers
    {


        /// <summary>
        /// Determines the success of a d20 roll against a crition.
        /// </summary>
        /// <param name="Eyes">The result of the roll</param>
        /// <param name="Attribute">The criterion the roll goes against</param>
        /// <returns>The result; criticals/botches are given as *pending*</returns>
        public static RollSuccess.Level PrimaryD20Success(int Eyes, int Attribute)
        {
            if (Eyes > 20 || Eyes < 1) throw new ArgumentOutOfRangeException(nameof(Eyes));
            if (Eyes == 1)
                return RollSuccess.Level.PendingCritical;
            else if (Eyes == 20)
                return RollSuccess.Level.PendingBotch;
            else if (Eyes <= Attribute)
                return RollSuccess.Level.Success;
            else
                return RollSuccess.Level.Fail;
        }



        /// <summary>
        /// Determines the success of a d20 roll against a crition.
        /// </summary>
        /// <param name="Eyes">The result of the roll</param>
        /// <param name="Attribute">The criterion the roll goes against</param>
        /// <returns>The result</returns>
        public static RollSuccess.Level D20Success(int Eyes, int Attribute)
        {
            if (Eyes == 1)
                return RollSuccess.Level.Critical;
            else if (Eyes == 20)
                return RollSuccess.Level.Botch;
            else if (Eyes <= Attribute)
                return RollSuccess.Level.Success;
            else
                return RollSuccess.Level.Fail;
        }


        /// <summary>
        /// Combines two roll results and determines the final success level after the confirmation roll.
        /// </summary>
        /// <param name="Primary">Success of the primary roll</param>
        /// <param name="Confirm">Success of the confirmation roll</param>
        /// <returns></returns>
        public static RollSuccess.Level CheckSuccess(RollSuccess.Level Primary, RollSuccess.Level Confirm)
        {
            if (Primary == RollSuccess.Level.na) return RollSuccess.Level.na;

            if (Primary == RollSuccess.Level.Critical || Primary == RollSuccess.Level.PendingCritical)
            {
                if (Confirm == RollSuccess.Level.Botch ||
                    Confirm == RollSuccess.Level.Fail)
                    return RollSuccess.Level.Success;
                else if (Confirm == RollSuccess.Level.na)
                    return RollSuccess.Level.PendingCritical;
                else
                    return RollSuccess.Level.Critical;
            }
            else if (Primary == RollSuccess.Level.Botch || Primary == RollSuccess.Level.PendingBotch)
            {
                if (Confirm == RollSuccess.Level.Botch ||
                    Confirm == RollSuccess.Level.Fail)
                    return RollSuccess.Level.Botch;
                else if (Confirm == RollSuccess.Level.na)
                    return RollSuccess.Level.PendingBotch;
                else
                    return RollSuccess.Level.Fail;
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
        public static RollSuccess.Level CheckSuccess(int PrimaryEyes, int ConfirmEyes, int Attribute)
        {
            RollSuccess.Level Confirm;
            RollSuccess.Level Primary = PrimaryD20Success(PrimaryEyes, Attribute);
            if (Primary == RollSuccess.Level.PendingCritical || Primary == RollSuccess.Level.PendingBotch)
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
