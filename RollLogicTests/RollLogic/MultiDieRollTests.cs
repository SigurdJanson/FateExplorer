using FateExplorer.WPA.RollLogic;
using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Linq;

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
            this.mockRepository = new(MockBehavior.Strict);

            MoqRng = mockRepository.Create<IRandomNG>();
        }



        [Test(Description = "Test correct default aggregation")]
        [TestCase(2, 20)]
        [TestCase(10, 20)]
        [TestCase(2, 6)]
        [TestCase(10, 6)]
        public void Roll_DefaultAggregate_Sums(int DieCount, int Sides)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount);

            // Act
            var result = multiDieRoll.Roll();

            // Assert
            Assert.AreEqual(multiDieRoll.OpenRollVals.Sum(), result);
        }


        [Test(Description = "Test non-default aggregation")]
        [TestCase(2, 20)]
        [TestCase(10, 20)]
        [TestCase(2, 6)]
        [TestCase(10, 6)]
        public void Roll_DefaultAggregate_Products(int DieCount, int Sides)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount, (int a, int b) => a*b );

            // Act
            var result = multiDieRoll.Roll();

            // Assert
            int product = 1;
            foreach (int i in multiDieRoll.OpenRollVals)
                product *= i;
            Assert.AreEqual(product, result);
        }


        [Test(Description = "Default aggregator, 2 dice")]
        [TestCase(6, 2)]
        [TestCase(20, 2)]
        public void Roll_FakeRandoms_Sums(int Sides, int DieCount)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount);

            MoqRng.SetupSequence(Rng => Rng.IRandom(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1)
                .Returns(Sides);
            multiDieRoll.RNG = MoqRng.Object;

            // Act
            var result = multiDieRoll.Roll();
            Assume.That(multiDieRoll.OpenRollVals[0] == 1 && multiDieRoll.OpenRollVals[1] == Sides);

            // Assert
            Assert.AreEqual(Sides+1, result);
        }


        [Test(Description = "Multiplication aggregator, 2 dice")]
        [TestCase(6, 2)]
        [TestCase(20, 2)]
        public void Roll_FakeRandoms_Product(int Sides, int DieCount)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount, (int a, int b) => a * b);

            MoqRng.SetupSequence(Rng => Rng.IRandom(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1)
                .Returns(Sides);
            multiDieRoll.RNG = MoqRng.Object;

            // Act
            var result = multiDieRoll.Roll();
            Assume.That(multiDieRoll.OpenRollVals[0] == 1 && multiDieRoll.OpenRollVals[1] == Sides);

            // Assert
            Assert.AreEqual(Sides * 1, result);
        }
    }
}
