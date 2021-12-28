using FateExplorer.WPA.RollLogic;
using NUnit.Framework;
using Moq;
using Moq.Protected;

namespace RollLogicTests.RollLogic
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



    [TestFixture]
    public class MultiDieRollTests
    {
        private MockRepository mockRepository;
        private Mock<IRandomNG> MoqRng;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }



        [Test]
        [TestCase(2, 20)]
        public void Roll_StateUnderTest_ExpectedBehavior(int DieCount, int Sides)
        {
            // Arrange
            var mockRng = mockRepository.Create<IRandomNG>();
            mockRng.SetupSequence(rng => rng.IRandom(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1)
                .Returns(1);

            var mockRolls = mockRepository.Create<MultiDieRoll>(DieCount, Sides);
            mockRolls.Protected()
                    .SetupGet<IRandomNG>("RNG")
                    .Returns(mockRng.Object);
            var multiDieRoll = mockRolls.Object;

            // Act
            var result = multiDieRoll.Roll();

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}
