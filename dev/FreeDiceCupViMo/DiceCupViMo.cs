using FateExplorer.WPA.RollLogic;
using System;

namespace FateExplorer.WPA.FreeDiceCupViMo
{
    public enum CupType
    {
        Single = 1, Multi = 2, MixedMulti = 3
    }

    public class DiceCupViMo
    {
        public DiceCupViMo(string name, string descr, int[] eyes, bool factoryDefault = false)
        {

            Name = name ?? "";
            Description = descr ?? "";
            FactoryDefault = factoryDefault;
Console.WriteLine($"{Name}: Props are done");

            // Set cup type and eyes
            if (eyes is null || eyes.Length == 0)
                throw new ArgumentNullException(nameof(eyes), "Internal error: empty dice cup defined");

            if (eyes.Length == 1)
            {
                Type = CupType.Single;
                Eyes = eyes[0];
            }
            else
            {
                bool AllTheSame = true;
                for(int i = 1; i < eyes.Length; i++)
                    if (eyes[0] != eyes[i])
                    {
                        AllTheSame = false;
                        break;
                    }
                Type = AllTheSame ? CupType.Multi : CupType.MixedMulti;
                Eyes = AllTheSame ? eyes[0] : 0;
            }
Console.WriteLine($"{Name}: Eyes are done");
            // Make a roller
            switch (Type)
            {
                case CupType.Single: Roller = new DieRoll(Eyes); break;
                case CupType.Multi: Roller = new MultiDieRoll(Eyes, eyes.Length); break;
                case CupType.MixedMulti: throw new NotImplementedException(); //TODO
            }
Console.WriteLine($"{Name}: Roller is done");
        }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Is this dice cup built in by FateExplorer or user-defined?
        /// </summary>
        public bool FactoryDefault { get; protected set; }

        /// <summary>
        /// The eyes of the dice in this dice cup. Returns 0 if the dice are mixed.
        /// </summary>
        public int Eyes { get; protected set; }

        public CupType Type { get; set; }

        protected IRoll Roller { get; set; }

        public int GetRollResult()
        {
            return Roller.Roll();
        }
    }
}
