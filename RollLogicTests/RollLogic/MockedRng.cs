using FateExplorer.RollLogic;

namespace UnitTests.RollLogic
{
    public class MockedRng : IRandomNG
    {
        public int ForcedIValue { get; set; }

        public uint BRandom()
        {
            throw new System.NotImplementedException();
        }

        public int IRandom(int min, int max)
        {
            return ForcedIValue;
        }

        public double Random()
        {
            throw new System.NotImplementedException();
        }

        public void RandomInit(uint seed)
        {
            ForcedIValue = (int)seed;
        }
    }
}
