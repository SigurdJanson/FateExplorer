using FateExplorer.CharacterModel;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTests.CharacterModel;



[TestFixture]
public class DodgeMTests
{
    private MockRepository mockRepository;

    private Mock<ICharacterM> mockCharacterM;

    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);

        this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
    }

    #region Test Helpers

    private DodgeM CreateDodgeM()
    {
        return new DodgeM(this.mockCharacterM.Object);
    }


    private static Mock<ICharacterM> CreateHeroMock(int agility)
    {
        var heroMock = new Mock<ICharacterM>();
        heroMock
            .Setup(h => h.GetAbility(AbilityM.AGI))
            .Returns(agility);

        return heroMock;
    }

    #endregion Test Helpers



    #region Construction

    [Test]
    public void Constructor_InitializesMinAndMax()
    {
        // Arrange
        var hero = CreateHeroMock(10);

        // Act
        var dodge = new DodgeM(hero.Object);

        // Assert
        Assert.That(dodge.Min, Is.EqualTo(0));
        Assert.That(dodge.Max, Is.EqualTo(20));
    }


    [Test]
    public void Constructor_SetsDependencyToAgility()
    {
        // Arrange
        var hero = CreateHeroMock(10);

        // Act
        var dodge = new DodgeM(hero.Object);
        var deps = dodge.GetDependencies();

        // Assert
        Assert.That(deps, Is.EquivalentTo(new[] { AbilityM.AGI }));
    }

    #endregion Construction




    #region Dependency Handling

    [Test]
    public void DependsOn_Agility_ReturnsTrue()
    {
        // Arrange
        var hero = CreateHeroMock(10);
        var dodge = new DodgeM(hero.Object);

        // Act
        var result = dodge.DependsOn(AbilityM.AGI);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void DependsOn_OtherAttribute_ReturnsFalse()
    {
        // Arrange
        var hero = CreateHeroMock(10);
        var dodge = new DodgeM(hero.Object);

        // Act
        var result = dodge.DependsOn("OTHER");

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion Dependency Handling




    #region Dependency Change Propagation

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(9)]
    [TestCase(20)]
    public void DependencyHasChanged_UpdatesEffectiveValue(int newAgility)
    {
        // Arrange
        var hero = CreateHeroMock(0);
        var dodge = new DodgeM(hero.Object);

        // Act
        dodge.DependencyHasChanged(AbilityM.AGI, newAgility);

        // Assert
        Assert.That(dodge.Effective, Is.EqualTo(DodgeM.ComputeDodge(newAgility)));
    }

    #endregion Dependency Change Propagation




    #region Range Enforcement

    [Test]
    public void DependencyChange_ResultAboveMax_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hero = CreateHeroMock(0);
        var dodge = new DodgeM(hero.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => dodge.DependencyHasChanged(AbilityM.AGI, 50));
    }

    [Test]
    public void DependencyChange_ResultBelowMin_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var hero = CreateHeroMock(0);
        var dodge = new DodgeM(hero.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => dodge.DependencyHasChanged(AbilityM.AGI, -1));
    }

    #endregion



    #region Static ComputeDodge Method

    [Test, Sequential]
    public void Dodge([Values(4,8,16,19)] int doVal, [Values(2,4,8,10)] int Expected)
    {
        // Arrange
        mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == AbilityM.AGI)))
            .Returns(doVal);

        // Act
        var dodgeM = this.CreateDodgeM();

        // Assert
        Assert.That(Expected, Is.EqualTo(dodgeM.Effective));
        mockCharacterM.Verify(c => c.GetAbility(It.Is<string>(s => s == AbilityM.AGI)), Times.Once);
    }


    [Test]
    public void ComputeDodge_Layariel()
    {
        // Arrange
        int EffectiveAgility = HeroWipfelglanz.AbilityValues[AbilityM.AGI];

        // Act
        var result = DodgeM.ComputeDodge(EffectiveAgility);

        // Assert
        Assert.That(HeroWipfelglanz.Dodge, Is.EqualTo(result));
        this.mockRepository.VerifyAll();
    }



    [Test]
    public void ComputeDodge_Arbosch()
    {
        // Arrange
        int EffectiveAgility = HeroArbosch.AbilityValues[AbilityM.AGI];

        // Act
        var result = DodgeM.ComputeDodge(EffectiveAgility);

        // Assert
        Assert.That(HeroArbosch.Dodge, Is.EqualTo(result));
        this.mockRepository.VerifyAll();
    }


    [Test]
    public void ComputeDodge_Grassberger()
    {
        // Arrange
        int EffectiveAgility = HeroGrassberger.AbilityValues[AbilityM.AGI];

        // Act
        var result = DodgeM.ComputeDodge(EffectiveAgility);

        // Assert
        Assert.That(HeroGrassberger.Dodge, Is.EqualTo(result));
        this.mockRepository.VerifyAll();
    }

    #endregion Static ComputeDodge Method
}
