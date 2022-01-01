using FateExplorer.RollLogic;
using System;

namespace FateExplorer.FreeDiceCupViMo
{
    public enum CupType
    {
        Single = 1, Multi = 2, MixedMulti = 3
    }

    public class DiceCupViMo
    {
        public DiceCupViMo(string name, string descr, int[] sides, bool factoryDefault = false)
        {

            Name = name ?? "";
            Description = descr ?? "";
            FactoryDefault = factoryDefault;

            // Set cup type and eyes
            if (sides is null || sides.Length == 0)
                throw new ArgumentNullException(nameof(sides), "Internal error: empty dice cup defined");

            if (sides.Length == 1)
            {
                Type = CupType.Single;
                Sides = new int[1] { sides[0] };
            }
            else
            {
                bool AllTheSame = true;
                for(int i = 1; i < sides.Length; i++)
                    if (sides[0] != sides[i])
                    {
                        AllTheSame = false;
                        break;
                    }
                if (AllTheSame)
                {
                    Type = CupType.Multi;
                    Sides = new int[1] { sides[0] };
                }
                else
                {
                    Type = CupType.MixedMulti;
                    Sides = sides.Clone() as int[]; // new int[eyes.Length] {}
                }
                //Type = AllTheSame ? CupType.Multi : CupType.MixedMulti;
                //Eyes = AllTheSame ? eyes[0] : 0;
            }

            // Make a roller
            switch (Type)
            {
                case CupType.Single: Roller = new DieRoll(Sides[0]); break;
                case CupType.Multi: Roller = new MultiDieRoll(Sides[0], sides.Length); break;
                case CupType.MixedMulti: throw new NotImplementedException(); //TODO
            }
        }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Is this dice cup built in by FateExplorer or user-defined?
        /// </summary>
        public bool FactoryDefault { get; protected set; }

        /// <summary>
        /// The eyes of the dice in this dice cup. Returns an array of 1
        /// when all dice are the same. Otherways an array of length of
        /// the number of dice.
        /// </summary>
        public int[] Sides { get; protected set; }

        public CupType Type { get; set; }

        protected IRoll Roller { get; set; }

        public void Roll() => Roller.Roll();

        public int[] GetRollResult() => Roller.OpenRoll;

        public int GetCombinedRollResult() => Roller.OpenRollCombined();
    }
}
