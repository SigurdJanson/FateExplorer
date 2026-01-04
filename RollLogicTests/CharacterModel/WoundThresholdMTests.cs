using FateExplorer.CharacterModel;
using Moq;
using NUnit.Framework;

namespace UnitTests.CharacterModel;

[TestFixture]
public class WoundThresholdMTests
{
    #region Test Helpers

    private static Mock<ICharacterM> CreateHeroMock(int constitution)
    {
        var heroMock = new Mock<ICharacterM>();

        heroMock
            .Setup(h => h.GetAbility(AbilityM.CON))
            .Returns(constitution);

        return heroMock;
    }

    #endregion Test Helpers



    #region Construction / Initial Value

    [TestCase(10)] // sum is even
    [TestCase(5)] // sum is odd
    [TestCase(1)]
    public void Constructor_ComputesCorrectInitialEffectiveValue(int constitution)
    {
        // Arrange
        var hero = CreateHeroMock(constitution);

        // Act
        var woundthreshold = new WoundThresholdM(hero.Object);

        // Assert
        Assert.That(woundthreshold.Effective, Is.EqualTo(WoundThresholdM.ComputeValue(constitution)));
    }

    #endregion


    #region Value Computation

    [TestCase(14, ExpectedResult = 7)] //
    [TestCase(15, ExpectedResult = 8)] //
    [TestCase(12, ExpectedResult = 6)] // 
    [TestCase(10, ExpectedResult = 5)] // 
    public int ComputeValue_ComputesCorrectValue(int constitution)
    {
        // Arrange
        // Act & Assert
        return WoundThresholdM.ComputeValue(constitution);
    }

    #endregion


    #region Dependency Updates

    [TestCase(10, 12)]
    [TestCase(8, 2)]
    public void DependencyChange_CourageOnly_RecomputesUsingCachedAgility(
        int initialConstitution,
        int newConstitution)
    {
        // Arrange
        var hero = CreateHeroMock(initialConstitution);
        var woundthreshold = new WoundThresholdM(hero.Object);

        // Act
        woundthreshold.DependencyHasChanged(AbilityM.CON, newConstitution);

        // Assert
        Assert.That(
            woundthreshold.Effective,
            Is.EqualTo(WoundThresholdM.ComputeValue(newConstitution)));
    }

    #endregion



    #region Ignored Dependencies

    [Test]
    public void DependencyChange_UnrelatedAttribute_DoesNotChangeEffectiveValue()
    {
        // Arrange
        var hero = CreateHeroMock(6);
        var woundThreshold = new WoundThresholdM(hero.Object);
        var originalValue = woundThreshold.Effective;

        // Act
        woundThreshold.DependencyHasChanged("UNRELATED", 10);

        // Assert
        Assert.That(woundThreshold.Effective, Is.EqualTo(originalValue));
    }

    #endregion

}
