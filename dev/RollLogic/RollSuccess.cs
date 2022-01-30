using System;

namespace FateExplorer.RollLogic
{

    public struct RollSuccess
    {
        const int Min = 1, Max = 20;
        const int DefaultBotchThreshold = Max;

        public int BotchThreshold;// { get; set; }


        public RollSuccess()
        {
            BotchThreshold = DefaultBotchThreshold;
            PrimaryLevel = Level.na;
            ConfirmationLevel = Level.na;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="botchThreshold">
        /// Some checks require a botch threshold aother that ">= 20". Set it here if needed.
        /// </param>
        public RollSuccess(Level Prim, Level Confirm, int botchThreshold = DefaultBotchThreshold)
        {
            PrimaryLevel = Prim;
            ConfirmationLevel = Confirm;
            BotchThreshold = botchThreshold;
        }

        public static implicit operator Level(RollSuccess s) 
            => CheckSuccess(s.PrimaryLevel, s.ConfirmationLevel);

        public enum Level
        {
            Botch = 20, 
            PendingBotch = -20,
            Fail = 11,
            Success = 9,
            Critical = 1, 
            PendingCritical = -1,
            na = 0
        }

        public Level PrimaryLevel, ConfirmationLevel; // { get; set; } = Level.na;
        //public Level ConfirmationLevel; // { get; set; } = Level.na;

        public Level CurrentLevel
        {
            get => CheckSuccess(PrimaryLevel, ConfirmationLevel);
        }

        /// <summary>
        /// Determines if the current state of the check is pending (botch or critical).
        /// </summary>
        public bool IsPending
        {
            get => CurrentLevel == Level.PendingBotch || CurrentLevel == Level.PendingCritical;
        }

        /// <summary>
        /// Update the status of the success evaluation after a new roll in the series.
        /// This method works for 1d20 roll checks. 
        /// </summary>
        /// <param name="Primary">Success level of the primary roll</param>
        /// <param name="Confirm"></param>
        public void Update(IRollM Primary, IRollM Confirm, int Attr)
        {
            if (Primary is null)
                PrimaryLevel = Level.na;
            else 
                PrimaryLevel = PrimaryD20Success(Primary.OpenRoll[0], Attr);

            if (Confirm is null)
                ConfirmationLevel = Level.na; 
            else
                ConfirmationLevel = D20Success(Confirm.OpenRoll[0], Attr);
        }



        public void Update(Level Primary, Level Confirm)
        {
            PrimaryLevel = Primary;
            ConfirmationLevel = Confirm;
        }




        /// <summary>
        /// Determines the success of a d20 roll against a crition.
        /// </summary>
        /// <param name="Eyes">The result of the roll</param>
        /// <param name="Attribute">The criterion the roll goes against</param>
        /// <returns>The result; criticals/botches are given as *pending*</returns>
        public Level PrimaryD20Success(int Eyes, int Attribute)
        {
            if (Eyes > Max || Eyes < Min) throw new ArgumentOutOfRangeException(nameof(Eyes));
            if (Eyes == 1)
                return Level.PendingCritical;
            else if (Eyes >= BotchThreshold)
                return Level.PendingBotch;
            else if (Eyes <= Attribute)
                return Level.Success;
            else
                return Level.Fail;
        }


        /// <summary>
        /// Determines the success of a d20 roll against a crition.
        /// </summary>
        /// <param name="Eyes">The result of the roll</param>
        /// <param name="Attribute">The criterion the roll goes against</param>
        /// <returns>The result</returns>
        public Level D20Success(int Eyes, int Attribute)
        {
            if (Eyes == 1)
                return Level.Critical;
            else if (Eyes >= BotchThreshold)
                return Level.Botch;
            else if (Eyes <= Attribute)
                return Level.Success;
            else
                return Level.Fail;
        }




        /// <summary>
        /// Combines two roll results and determines the final success level after the confirmation roll.
        /// </summary>
        /// <param name="Primary">Success of the primary roll</param>
        /// <param name="Confirm">Success of the confirmation roll</param>
        /// <returns></returns>
        public static Level CheckSuccess(Level Primary, Level Confirm)
        {
            if (Primary == Level.na) return Level.na;

            if (Primary == Level.Critical || Primary == Level.PendingCritical)
            {
                if (Confirm == Level.Botch ||
                    Confirm == Level.Fail)
                    return Level.Success;
                else if (Confirm == Level.na)
                    return Level.PendingCritical;
                else
                    return Level.Critical;
            }
            else if (Primary == Level.Botch || Primary == Level.PendingBotch)
            {
                if (Confirm == Level.Botch ||
                    Confirm == Level.Fail)
                    return Level.Botch;
                else if (Confirm == Level.na)
                    return Level.PendingBotch;
                else
                    return Level.Fail;
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
        public Level CheckSuccess(int PrimaryEyes, int ConfirmEyes, int Attribute)
        {
            Level Confirm;
            Level Primary = PrimaryD20Success(PrimaryEyes, Attribute);
            if (Primary == Level.PendingCritical || Primary == Level.PendingBotch)
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
