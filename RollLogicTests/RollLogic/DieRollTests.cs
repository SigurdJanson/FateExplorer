using FateExplorer.RollLogic;
using Moq;
using NUnit.Framework;

namespace vmCode_UnitTests.RollLogic
{
    [TestFixture]
    public class DieRollTests
    {
        private MockRepository mockRepository;
        private Mock<IRandomNG> MoqRng;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new(MockBehavior.Strict);

            MoqRng = mockRepository.Create<IRandomNG>();
        }

        private DieRollM CreateDieRoll(int Sides)
        {
            DieRollM ClassUnderTest = new(Sides);
            return ClassUnderTest;
        }


        [Test]
        public void Roll_FakeRoll([Values(2, 6, 12, 20)] int Sides)
        {
            // Arrange
            var dieRoll = this.CreateDieRoll(Sides);

            int MoqRandomInt = Sides - 1;
            MoqRng.Setup(r => r.IRandom(It.Is<int>(i => i == 1), It.Is<int>(i => i == Sides)))
                .Returns(Sides - 1);
            dieRoll.RNG = MoqRng.Object;

            // Act
            var result = dieRoll.Roll();

            // Assert
            Assert.AreEqual(MoqRandomInt, result[0]);
            Assert.AreEqual(1, result.Length);
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
                Assert.AreEqual(1, dieRoll.OpenRoll.Length);
                Assert.GreaterOrEqual(result[0], 1);
                Assert.LessOrEqual(result[0], Sides);
                Assert.AreEqual(dieRoll.OpenRoll, result);
                //Assert.AreEqual(0, dieRoll.PrevRoll[0]);
            });
            this.mockRepository.VerifyAll();
        }




        [Test, Repeat(100)]
        public void Roll_SecondRoll_WithinLimits([Values(2, 6, 12, 20)] int Sides)
        {
            // Arrange
            var dieRoll = this.CreateDieRoll(Sides);

            // Act
            var first = dieRoll.Roll()[0];
            var second = dieRoll.Roll()[0];

            // Assert
            Assert.Multiple(() =>
            {
                Assert.GreaterOrEqual(second, 1);
                Assert.LessOrEqual(second, Sides);
            });
            Assert.AreEqual(dieRoll.OpenRoll[0], second);
            Assert.AreEqual(dieRoll.PrevRoll[0], first);
            this.mockRepository.VerifyAll();
        }

    }
}
