using FateExplorer.RollLogic;
using Moq;
using NUnit.Framework;

namespace UnitTests.RollLogic
{
    
    [TestFixture]
    public class EnergyRollMTests
    {

        private MockRepository mockRepository;
        private Mock<IRandomNG> MoqRng;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new(MockBehavior.Strict);

            MoqRng = mockRepository.Create<IRandomNG>();
        }





        [Test, Repeat(10)]
        public void Roll_NoModifier_ExpectedRange1to6()
        {
            // Arrange
            var energyRollM = new EnergyRollM(RegenerationSite.Default, RegenerationDisturbance.None, false, 0);

            // Act
            var result = energyRollM.Roll();

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.GreaterOrEqual(result[0], 1);
            Assert.LessOrEqual(result[0], 6);
        }



        [Test]
        [TestCase(RegenerationSite.Good, 6, 7, Description = "+1")]
        [TestCase(RegenerationSite.Poor, 6, 5, Description = "-1")]
        [TestCase(RegenerationSite.Default, 6, 6, Description = "Unmodified")]
        [TestCase(RegenerationSite.Bad, 6, 3, Description = "Half")]
        [TestCase(RegenerationSite.Terrible, 6, 0, Description = "No regeneration at all")]
        [TestCase(RegenerationSite.Good, 3, 4, Description = "+1")]
        [TestCase(RegenerationSite.Poor, 3, 2, Description = "-1")]
        [TestCase(RegenerationSite.Default, 3, 3, Description = "Unmodified")]
        [TestCase(RegenerationSite.Bad, 3, 2, Description = "Half")]
        [TestCase(RegenerationSite.Terrible, 3, 0, Description = "No regeneration at all")]
        public void Roll_VariousRegenerationSites_ExpectedMod(RegenerationSite Site, int Roll, int ModifiedRoll)
        {
            const RegenerationDisturbance Disturb = RegenerationDisturbance.None; // no added modifier
            // Arrange
            var energyRollM = new EnergyRollM(Site, Disturb, false, 0);

            MoqRng.Setup(Rng => Rng.IRandom(It.Is<int>(i => i == 1), It.Is<int>(i => i == 6)))
                .Returns(Roll);
            energyRollM.RNG = MoqRng.Object;

            // Act
            var result = energyRollM.Roll();

            // Assert
            Assert.AreEqual(ModifiedRoll, result[0]);
            Assert.AreEqual(ModifiedRoll - Roll, energyRollM.ModifiedBy[0]);
            
            mockRepository.VerifyAll();
            MoqRng.Verify(Rng => Rng.IRandom(It.Is<int>(i => i == 1), It.Is<int>(i => i == 6)), Times.AtLeastOnce);
        }
    }
}
