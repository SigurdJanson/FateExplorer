using NUnit.Framework;
using FateExplorer.Shared;
namespace UnitTests.Shared;

[TestFixture()]
public class DerivedValueTests
{
    [Test]
    public void Init_Test([Values(1, 2, 5, 10)] int value)
    {
        // Arrange
        var derivedValue = new DerivedValue(value);

        // Act

        // Assert
        Assert.That(derivedValue.Imported, Is.EqualTo(value));
        Assert.That(derivedValue.True, Is.EqualTo(value));
        Assert.That(derivedValue.Effective, Is.EqualTo(value));
        Assert.That((int)derivedValue, Is.EqualTo(value));
    }

    /// <summary>
    /// Test for modifiers added by activatables.
    /// </summary>
    [Test]
    public void AddOnSetup_Okay([Values(5, 10)] int Imported, [Values(-3, +3)] int Mod)
    {
        // Arrange
        var derivedValue = new DerivedValue(Imported);

        // Act
        derivedValue.AddOnSetup(Mod);

        // Assert
        Assert.That(derivedValue.True, Is.EqualTo(Imported + Mod));
        Assert.That(derivedValue.Effective, Is.EqualTo(Imported + Mod));
    }



    /// <summary>
    /// Test for true value. Happy case.
    /// </summary>
    [Test]
    public void True_Okay([Values(5, 10)] int Imported, 
        [Values(-3, +3)] int ActivatableMod, [Values(5, 10, 15)] int True)
    {
        // Arrange
        var derivedValue = new DerivedValue(Imported);

        // Act
        derivedValue.AddOnSetup(ActivatableMod);
        derivedValue.True = True;

        // Assert
        Assert.That(derivedValue.True, Is.EqualTo(True));
        Assert.That(derivedValue.Effective, Is.EqualTo(True));
    }


    /// <summary>
    /// Test for effective value. Happy case.
    /// </summary>
    [Test]
    public void Effective_Okay([Values(5, 10)] int Imported,
        [Values(-3, +3)] int ActivatableMod, [Values(5, 10, 15)] int True,
        [Values(5, 10, 15)] int Effective)
    {
        // Arrange
        var derivedValue = new DerivedValue(Imported);

        // Act
        derivedValue.AddOnSetup(ActivatableMod);
        derivedValue.True = True;
        derivedValue.Effective = Effective;

        // Assert
        Assert.That(derivedValue.True, Is.EqualTo(True));
        Assert.That(derivedValue.Effective, Is.EqualTo(Effective));
    }

}