
namespace FateExplorer.RollLogic
{
    public class D20Roll : DieRollM
    {
        protected const int _Sides = 20;

        public D20Roll() : base(_Sides)
        {
        }

    }


    public class SkillRoll : MultiDieRoll
    {
        protected const int _Sides = 20;
        protected const int _DieCount = 3;

        public SkillRoll() : base(_Sides, _DieCount)
        {
        }

    }



    public class BotchEffectRoll : MultiDieRoll
    {
        protected const int _Sides = 6;
        protected const int _DieCount = 2;

        public BotchEffectRoll() : base(_Sides, _DieCount)
        {}
    }
}
