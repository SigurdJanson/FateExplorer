using FateExplorer.RollLogic;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.RollLogic
{



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


        [Test(Description = "Test if output has `DieCount` places and all rolls are within allowed range"), Repeat(50)]
        [TestCase(6, 1)] // single die edge case
        [TestCase(6, 2)]
        [TestCase(6, 10)]
        [TestCase(20, 2)]
        [TestCase(20, 10)]
        public void Roll__CorrectArrayLengthNContent(int Sides, int DieCount)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount);

            // Act
            var result = multiDieRoll.Roll();

            // Assert
            Assert.That(DieCount, Is.EqualTo(result.Length));
            Assert.That(result, Is.All.Matches<int>(x => x >= 1 && x <= Sides));
        }


        [Test(Description = "Test correct default aggregation")]
        [TestCase(6, 1)] // single die edge case
        [TestCase(6, 2)]
        [TestCase(6, 10)]
        [TestCase(20, 2)]
        [TestCase(20, 10)]
        public void Roll_DefaultAggregate_Sums(int Sides, int DieCount)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount);
            multiDieRoll.Roll();

            // Act
            var result = multiDieRoll.OpenRollCombined();

            // Assert
            Assert.That(multiDieRoll.OpenRoll.Sum(), Is.EqualTo(result));
        }


        [Test(Description = "Test non-default aggregation")]
        [TestCase(2, 20)]
        [TestCase(10, 20)]
        [TestCase(2, 6)]
        [TestCase(10, 6)]
        public void Roll_DefaultAggregate_Products(int DieCount, int Sides)
        {
            // Arrange
            MultiDieRoll multiDieRoll = new(Sides, DieCount, (int a, int b) => a * b);
            multiDieRoll.Roll();

            // Act
            var result = multiDieRoll.OpenRollCombined();


            // Assert
            int product = 1;
            foreach (int i in multiDieRoll.OpenRoll)
                product *= i;
            Assert.That(product, Is.EqualTo(result));
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
            multiDieRoll.Roll();

            // Act
            var result = multiDieRoll.OpenRollCombined();
            Assume.That(multiDieRoll.OpenRoll[0] == 1 && multiDieRoll.OpenRoll[1] == Sides);

            // Assert
            Assert.That(Sides + 1, Is.EqualTo(result));
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
            multiDieRoll.Roll();

            // Act
            var result = multiDieRoll.OpenRollCombined();

            Assume.That(multiDieRoll.OpenRoll[0] == 1 && multiDieRoll.OpenRoll[1] == Sides);

            // Assert
            Assert.That(Sides * 1, Is.EqualTo(result));
        }
    }
}
