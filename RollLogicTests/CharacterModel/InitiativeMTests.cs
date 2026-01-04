using FateExplorer.CharacterModel;
using Moq;
using NUnit.Framework;

namespace UnitTests.CharacterModel;

[TestFixture]
public class InitiativeMTests
{
    #region Test Helpers

    private static Mock<ICharacterM> CreateHeroMock(int courage, int agility)
    {
        var heroMock = new Mock<ICharacterM>();

        heroMock
            .Setup(h => h.GetAbility(AbilityM.COU))
            .Returns(courage);

        heroMock
            .Setup(h => h.GetAbility(AbilityM.AGI))
            .Returns(agility);

        return heroMock;
    }

    #endregion Test Helpers



    #region Construction / Initial Value

    [TestCase(10, 10)] // sum is even
    [TestCase(5, 6)] // sum is odd
    [TestCase(1, 1)]
    public void Constructor_ComputesCorrectInitialEffectiveValue(int courage, int agility)
    {
        // Arrange
        var hero = CreateHeroMock(courage, agility);

        // Act
        var initiative = new InitiativeM(hero.Object);

        // Assert
        Assert.That(initiative.Effective, Is.EqualTo(InitiativeM.ComputeValue(courage, agility)));
    }

    #endregion


    #region Value Computation

    [TestCase(14, 14, ExpectedResult = 14)] // case Louisa, VR1 p. 57
    [TestCase(15, 12, ExpectedResult = 14)] // case Chris, VR1 p. 57
    [TestCase(12, 15, ExpectedResult = 14)] // case Sarah, VR1 p. 57 - order of values reversed
    [TestCase(10, 15, ExpectedResult = 13)] // 
    public int ComputeValue_ComputesCorrectValue(int courage, int agility)
    {
        // Arrange
        // Act & Assert
        return InitiativeM.ComputeValue(courage, agility);
    }

    #endregion


    #region Dependency Updates

    [TestCase(10, 10, 12)]
    [TestCase(3, 7, 1)]
    public void DependencyChange_AgilityOnly_RecomputesUsingCachedCourage(
        int initialCourage,
        int initialAgility,
        int newAgility)
    {
        // Arrange
        var hero = CreateHeroMock(initialCourage, initialAgility);
        var initiative = new InitiativeM(hero.Object);

        // Act
        initiative.DependencyHasChanged(AbilityM.AGI, newAgility);

        // Assert
        Assert.That(
            initiative.Effective,
            Is.EqualTo(InitiativeM.ComputeValue(initialCourage, newAgility)));
    }


    [TestCase(10, 10, 12)]
    [TestCase(8, 4, 2)]
    public void DependencyChange_CourageOnly_RecomputesUsingCachedAgility(
        int initialCourage,
        int initialAgility,
        int newCourage)
    {
        // Arrange
        var hero = CreateHeroMock(initialCourage, initialAgility);
        var initiative = new InitiativeM(hero.Object);

        // Act
        initiative.DependencyHasChanged(AbilityM.COU, newCourage);

        // Assert
        Assert.That(
            initiative.Effective,
            Is.EqualTo(InitiativeM.ComputeValue(newCourage, initialAgility)));
    }


    [Test]
    public void DependencyChange_AgilityThenCourage_UsesLatestValues()
    {
        // Arrange
        var hero = CreateHeroMock(5, 5);
        var initiative = new InitiativeM(hero.Object);

        // Act
        initiative.DependencyHasChanged(AbilityM.AGI, 10);
        initiative.DependencyHasChanged(AbilityM.COU, 7);

        // Assert
        Assert.That(
            initiative.Effective,
            Is.EqualTo(InitiativeM.ComputeValue(7, 10)));
    }


    [Test]
    public void DependencyChange_RepeatedCourage_UsesLatestValues()
    {
        // Arrange
        var hero = CreateHeroMock(10, 5);
        var initiative = new InitiativeM(hero.Object);

        // Act
        initiative.DependencyHasChanged(AbilityM.COU, 12);
        initiative.DependencyHasChanged(AbilityM.COU, 6);

        // Assert
        Assert.That(
            initiative.Effective,
            Is.EqualTo(InitiativeM.ComputeValue(6, 5)));
    }

    #endregion



    #region Ignored Dependencies

    [Test]
    public void DependencyChange_UnrelatedAttribute_DoesNotChangeEffectiveValue()
    {
        // Arrange
        var hero = CreateHeroMock(6, 8);
        var initiative = new InitiativeM(hero.Object);
        var originalValue = initiative.Effective;

        // Act
        initiative.DependencyHasChanged("UNRELATED", 100);

        // Assert
        Assert.That(initiative.Effective, Is.EqualTo(originalValue));
    }

    #endregion

}
