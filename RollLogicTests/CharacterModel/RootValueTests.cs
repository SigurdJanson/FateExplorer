using FateExplorer.CharacterModel;
using NUnit.Framework;

namespace UnitTests.CharacterModel;


[TestFixture]
public class RootValueTests
{
    #region Test Helpers

    private static RootValue CreateRootValue(int imported, string id = "attr.test")
    {
        return new RootValue(imported)
        {
            Id = id
        };
    }

    #endregion



    #region Construction

    [Test]
    public void Constructor_InitializesImportedValue()
    {
        // Arrange
        var root = CreateRootValue(10);

        // Act
        var value = root.Imported;

        // Assert
        Assert.That(value, Is.EqualTo(10));
    }

    [Test]
    public void Constructor_InitializesTrueAndEffectiveEqualToImported()
    {
        // Arrange
        var root = CreateRootValue(7);

        // Act & Assert
        Assert.That(root.True, Is.EqualTo(7));
        Assert.That(root.Effective, Is.EqualTo(7));
    }

    #endregion




    #region Imported Property
    [TestCase(5, 8)]
    [TestCase(0, -3)]
    public void Imported_SetWithinRange_UpdatesTrueValue(int imported, int newImported)
    {
        // Arrange
        var root = CreateRootValue(imported);

        // Act
        root.Imported = newImported;

        // Assert
        Assert.That(root.Imported, Is.EqualTo(newImported));
        Assert.That(root.True, Is.EqualTo(newImported)); // True value should change, too
        Assert.That(root.Effective, Is.EqualTo(newImported)); // effective value should change, too
    }
    #endregion




    #region True Property

    [TestCase(5, 8)]
    [TestCase(0, -3)]
    public void True_SetWithinRange_UpdatesTrueValue(int imported, int newTrue)
    {
        // Arrange
        var root = CreateRootValue(imported);

        // Act
        root.True = newTrue;

        // Assert
        Assume.That(root.Imported, Is.EqualTo(imported)); // Just to be sure
        Assert.That(root.True, Is.EqualTo(newTrue));
    }



    [Test]
    public void True_ChangeTriggersStateChangedEvent()
    {
        // Arrange
        var root = CreateRootValue(10);
        string receivedId = null;
        int receivedValue = 0;

        root.OnStateChanged += (id, value) =>
        {
            receivedId = id;
            receivedValue = value;
        };

        // Act
        root.True = 12;

        // Assert
        Assert.That(receivedId, Is.EqualTo(root.Id));
        Assert.That(receivedValue, Is.EqualTo(root.Effective));
    }

    [Test]
    public void True_SetToSameEffectiveValue_DoesNotTriggerStateChangedEvent()
    {
        // Arrange
        var root = CreateRootValue(10);
        var callCount = 0;

        root.OnStateChanged += (_, _) => callCount++;

        // Act
        root.True = 10;

        // Assert
        Assert.That(callCount, Is.EqualTo(0));
    }

    #endregion




    #region Effective Property

    [TestCase(10, 15)]
    [TestCase(5, 2)]
    public void Effective_SetWithinRange_UpdatesEffectiveValue(int imported, int newEffective)
    {
        // Arrange
        var root = CreateRootValue(imported);

        // Act
        root.Effective = newEffective;

        // Assert
        Assert.That(root.Effective, Is.EqualTo(newEffective));
    }


    [Test]
    public void Effective_ChangeTriggersStateChangedEvent()
    {
        // Arrange
        var root = CreateRootValue(10);
        string receivedId = null;
        int receivedValue = 0;

        root.OnStateChanged += (id, value) =>
        {
            receivedId = id;
            receivedValue = value;
        };

        // Act
        root.Effective = 14;

        // Assert
        Assert.That(receivedId, Is.EqualTo(root.Id));
        Assert.That(receivedValue, Is.EqualTo(14));
    }

    [Test]
    public void Effective_SetToSameValue_DoesNotTriggerStateChangedEvent()
    {
        // Arrange
        var root = CreateRootValue(10);
        var callCount = 0;

        root.OnStateChanged += (_, _) => callCount++;

        // Act
        root.Effective = 10;

        // Assert
        Assert.That(callCount, Is.EqualTo(0));
    }

    #endregion




    #region Interaction Between Imported, True and Effective

    [Test]
    public void True_ChangeAffectsEffectiveAndTriggersEventOnce()
    {
        // Arrange
        var root = CreateRootValue(10);
        var callCount = 0;

        root.OnStateChanged += (_, _) => callCount++;

        // Act
        root.True = 13;

        // Assert
        Assert.That(root.Effective, Is.EqualTo(13));
        Assert.That(callCount, Is.EqualTo(1));
    }

    [Test]
    public void Effective_ChangeDoesNotAffectTrue()
    {
        // Arrange
        var root = CreateRootValue(10);

        // Act
        root.Effective = 15;

        // Assert
        Assert.That(root.True, Is.EqualTo(10));
        Assert.That(root.Effective, Is.EqualTo(15));
    }


    [Test]
    public void Effective_ChangesWithImported()
    {
        const int Value = 10, TrueDelta = 1, EffDelta = 2;
        const int newImported = 15;
        // Arrange
        var root = CreateRootValue(Value);
        root.True = Value + 1;
        root.Effective = Value + TrueDelta + EffDelta;

        // Act
        root.Imported = newImported;

        // Assert
        Assert.That(root.True, Is.EqualTo(newImported + TrueDelta));
        Assert.That(root.Effective, Is.EqualTo(newImported + TrueDelta + EffDelta));
    }
    #endregion




    #region Implicit Int Conversion

    [Test]
    public void ImplicitConversion_ReturnsEffectiveValue()
    {
        // Arrange
        var root = CreateRootValue(10);
        root.Effective = 18;

        // Act
        int value = root;

        // Assert
        Assert.That(value, Is.EqualTo(18));
    }

    #endregion
}

