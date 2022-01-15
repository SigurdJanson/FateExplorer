using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic
{
    public class D20Roll : DieRollM
    {
        public const int _Sides = 20;
        public RollSuccessLevel Meaning { get; protected set; }

        public D20Roll(IEntryCondition entryCondition) : base(_Sides)
        {
            EntryCondition = entryCondition;
        }


        public RollSuccessLevel Verify(int Threshold)
        {
            if (Threshold < 1 || Threshold > Sides[0])
                throw new ArgumentOutOfRangeException(nameof(Threshold));

            RollSuccessLevel Success;
            if (OpenRoll[0] == 20)
                Success = RollSuccessLevel.PendingBotch;
            else if (OpenRoll[0] == 1)
                Success = RollSuccessLevel.PendingCritical;
            else
                Success = OpenRoll[0] <= Threshold ? RollSuccessLevel.Success : RollSuccessLevel.Fail;

            Meaning = Success;
            return Success;
        }
    }

    // Probably obsolete
    public class D20ConfirmRoll : DieRollM
    {
        public const int _Sides = 20;

        public RollSuccessLevel Meaning { get; protected set; }

        public D20ConfirmRoll() : base(_Sides)
        {
        }


        public RollSuccessLevel Verify(D20Roll Roll, int Threshold)
        {
            if (Threshold < 1 || Threshold > Sides[0])
                throw new ArgumentOutOfRangeException(nameof(Threshold));

            RollSuccessLevel Success;
            if (Roll.Meaning == RollSuccessLevel.PendingBotch)
            {
                if (OpenRoll[0] <= Threshold)
                    Success = RollSuccessLevel.Fail;
                else
                    Success = RollSuccessLevel.Botch;
            }
            else if (Roll.Meaning == RollSuccessLevel.PendingCritical)
            {
                if (OpenRoll[0] <= Threshold)
                    Success = RollSuccessLevel.Critical;
                else
                    Success = RollSuccessLevel.Success;
            }
            else
            {
                Success = Roll.Meaning;
            }

            Meaning = Success;
            return Success;
        }
    }


    public class SkillRoll : MultiDieRoll
    {
        public const int _Sides = 20;
        public const int _DieCount = 3;
        public RollSuccessLevel Meaning { get; protected set; }

        public SkillRoll(IEntryCondition entryCondition) : base(_Sides, _DieCount)
        {
            //EntryCondition = entryCondition;
        }


        public RollSuccessLevel Verify(int Threshold)
        {
            if (Threshold < 1 || Threshold > Sides[0])
                throw new ArgumentOutOfRangeException(nameof(Threshold));

            RollSuccessLevel Success;
            if (OpenRoll[0] == 20)
                Success = RollSuccessLevel.PendingBotch;
            else if (OpenRoll[0] == 1)
                Success = RollSuccessLevel.PendingCritical;
            else
                Success = OpenRoll[0] <= Threshold ? RollSuccessLevel.Success : RollSuccessLevel.Fail;

            Meaning = Success;
            return Success;
        }
    }



    public class BotchEffectRoll : MultiDieRoll
    {
        public const int _Sides = 6;
        public const int _DieCount = 2;

        public BotchEffectRoll() : base(_Sides, _DieCount)
        {
        }
    }
}
