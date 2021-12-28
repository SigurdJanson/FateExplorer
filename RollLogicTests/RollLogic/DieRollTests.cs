using FateExplorer.WPA.RollLogic;
using Moq;
using NUnit.Framework;

namespace RollLogicTests.RollLogic
{
    [TestFixture]
    public class DieRollTests
    {
        private MockRepository mockRepository;
        private Mock<IRandomNG> MoqRng;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            MoqRng = mockRepository.Create<IRandomNG>();
        }

        private DieRoll CreateDieRoll(int Sides)
        {
            DieRoll ClassUnderTest = new DieRoll(Sides);
            return ClassUnderTest;
        }


        [Test, Repeat(100)]
        public void Roll_FirstRoll_WithinLimits([Values(2, 6, 12, 20)] int Sides)
        {
            // Arrange
            var dieRoll = this.CreateDieRoll(Sides);

            // Act
            var result = dieRoll.Roll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.GreaterOrEqual(result, 1);
                Assert.LessOrEqual(result, Sides);
                Assert.AreEqual(result, dieRoll.OpenRoll);
                Assert.AreEqual(dieRoll.PrevRoll, 0);
            });
            this.mockRepository.VerifyAll();
        }




        [Test, Repeat(100)]
        public void Roll_SecondRoll_WithinLimits([Values(2, 6, 12, 20)] int Sides)
        {
            // Arrange
            var dieRoll = this.CreateDieRoll(Sides);

            // Act
            var first = dieRoll.Roll();
            var second = dieRoll.Roll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.GreaterOrEqual(second, 1);
                Assert.LessOrEqual(second, Sides);
                Assert.AreEqual(second, dieRoll.OpenRoll);
                Assert.AreEqual(first, dieRoll.PrevRoll);
            });
            this.mockRepository.VerifyAll();
        }

    }
}
