using FateExplorer.RollLogic;
//using Moq;
using NUnit.Framework;

namespace UnitTests.RollLogic;

[TestFixture]
public class EnergyPotionRollMTests
{
    //-private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        //-this.mockRepository = new MockRepository(MockBehavior.Strict);
    }

    private EnergyPotionRollM CreateEnergyPotionRollM(int QL)
    {
        return new EnergyPotionRollM(QL);
    }

    [Test, Repeat(25)]
    [TestCase(1, 1, 3)]
    [TestCase(2, 1, 6)]
    [TestCase(3, 1, 6 + 2)]
    [TestCase(4, 1, 6 + 4)]
    [TestCase(5, 1, 6 + 6)]
    [TestCase(6, 1, 6 + 8)]
    public void Roll_StateUnderTest_ExpectedBehavior(int QL, int ResultMin, int ResultMax)
    {
        // Arrange
        var energyPotionRollM = this.CreateEnergyPotionRollM(QL);

        // Act
        var result = energyPotionRollM.Roll();

        // Assert
        Assert.That(result[0], Is.InRange(ResultMin, ResultMax));
        //this.mockRepository.VerifyAll();
    }
}
