using FateExplorer.RollLogic;
using NUnit.Framework;

namespace RollLogicTests.RollLogic
{
    [TestFixture]
    public class EnergyRollMTests
    {
        [Test]
        public void Roll_NoModifier_ExpectedRange1to6()
        {
            // Arrange
            var energyRollM = new EnergyRollM(RegenerationSite.Default, RegenerationDisturbance.None);

            // Act
            var result = energyRollM.Roll();

            // Assert
            Assert.AreEqual(1, result.Length);
            Assert.GreaterOrEqual(result[0], 1);
            Assert.LessOrEqual(result[0], 6);
        }
    }
}
