using FateExplorer.RollLogic;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTests.RollLogic
{
    [TestFixture]
    public class BestOf2d6Tests
    {
        private const int Sides = 6; // the BestOf2d6 has 6 sides, of course

        private MockRepository mockRepository;
        private Mock<IRandomNG> MoqRng;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            MoqRng = mockRepository.Create<IRandomNG>();
        }

        private BestOf2d6 CreateBestOf2d6()
        {
            return new BestOf2d6();
        }



        [Test]
        public void OpenRollCombined_DifferentRandomNumbers_ReturnsMax(
            [Values(6, 3, 1)] int RandomResult1, [Values(5, 4, 2)] int RandomResult2)
        {
            // Arrange
            var bestOf2d6 = this.CreateBestOf2d6();
            MoqRng.SetupSequence(r => r.IRandom(It.Is<int>(i => i == 1), It.Is<int>(i => i == Sides)))
                .Returns(RandomResult1).Returns(RandomResult2);
            bestOf2d6.RNG = MoqRng.Object;
            bestOf2d6.Roll();

            // Act
            var result = bestOf2d6.OpenRollCombined();

            // Assert
            Assert.AreEqual(Math.Max(RandomResult1, RandomResult2), result);
            this.mockRepository.VerifyAll();
        }


        [Test]
        public void OpenRollCombined_DoubletRandomNumbers_ReturnsSum([Values(6,3,1)]int RandomResult)
        {
            // Arrange
            var bestOf2d6 = this.CreateBestOf2d6();
            MoqRng.SetupSequence(r => r.IRandom(It.Is<int>(i => i == 1), It.Is<int>(i => i == Sides)))
                .Returns(RandomResult).Returns(RandomResult).Returns(RandomResult);
            bestOf2d6.RNG = MoqRng.Object;
            bestOf2d6.Roll();

            // Act
            var result = bestOf2d6.OpenRollCombined();

            // Assert
            Assert.AreEqual(2*RandomResult, result);
            this.mockRepository.VerifyAll();
        }


    }
}
