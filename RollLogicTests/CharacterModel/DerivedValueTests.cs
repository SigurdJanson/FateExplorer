using FateExplorer.CharacterModel;
using NUnit.Framework;
using System;

namespace UnitTests.CharacterModel;

[TestFixture()]
public class DerivedValueTests
{
    const int DefaultMinValue = -100, DefaultMaxValue = 100;


    #region Test Helpers

    private sealed class TestDerivedValue : DerivedValue
    {
        public const int MinValue = 2, MaxValue = 18;

        public string LastAttrId { get; private set; }
        public int LastNewValue { get; private set; }
        public int CallCount { get; private set; }

        public TestDerivedValue(int value, string[] dependencies)
            : base(value)
        {
            DependencyId = dependencies;
            Min = MinValue;
            Max = MaxValue;
        }

        protected override void UpdateOnDependencyChange(string attrId, int newValue)
        {
            CallCount++;
            LastAttrId = attrId;
            LastNewValue = newValue;
        }
    }

    private static DerivedValue CreateDerivedValue(int value)
    {
        return new DerivedValue(value)
        {
            Id = "attr.derived"
        };
    }

    #endregion Test Helpers



    #region Construction

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



    [Test]
    public void Constructor_InitializesMinAndMaxDefaults()
    {
        // Arrange
        var derived = CreateDerivedValue(0);

        // Act & Assert
        Assert.That(derived.Min, Is.EqualTo(DefaultMinValue));
        Assert.That(derived.Max, Is.EqualTo(DefaultMaxValue));
    }


    [Test]
    public void Constructor_InitializesEmptyDependencies()
    {
        // Arrange
        var derived = CreateDerivedValue(5);

        // Act
        var deps = derived.GetDependencies();

        // Assert
        Assert.That(deps, Is.Not.Null);
        Assert.That(deps, Is.Empty);
    }


    #endregion Construction



    #region Dependency Inspection

    [Test]
    public void GetDependencies_ReturnsSameInstance()
    {
        // Arrange
        var deps = new[] { "attr.a", "attr.b" };
        var derived = new TestDerivedValue(0, deps);

        // Act
        var result = derived.GetDependencies();

        // Assert
        Assert.That(result, Is.SameAs(deps));
    }


    [TestCase("attr.a", true)]
    [TestCase("attr.b", true)]
    [TestCase("attr.c", false)]
    public void DependsOn_ReturnsExpectedResult(string dependency, bool expected)
    {
        // Arrange
        var derived = new TestDerivedValue(0, ["attr.a", "attr.b"]);

        // Act
        var result = derived.DependsOn(dependency);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void DependsOn_NoDependencies_ReturnsFalse()
    {
        // Arrange
        var derived = new TestDerivedValue(0, []);

        // Act
        var result = derived.DependsOn("attr.any");

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion Dependency Inspection




    #region Dependency Change Handling

    [Test]
    public void DependencyHasChanged_InvokesUpdateOnDependencyChange()
    {
        // Arrange
        var derived = new TestDerivedValue(0, new[] { "attr.a" });

        // Act
        derived.DependencyHasChanged("attr.a", 42);

        // Assert
        Assert.That(derived.CallCount, Is.EqualTo(1));
        Assert.That(derived.LastAttrId, Is.EqualTo("attr.a"));
        Assert.That(derived.LastNewValue, Is.EqualTo(42));
    }

    [Test]
    public void DependencyHasChanged_MultipleCalls_AreAllForwarded()
    {
        // Arrange
        var derived = new TestDerivedValue(0, ["attr.a"]);

        // Act
        derived.DependencyHasChanged("attr.a", 1);
        derived.DependencyHasChanged("attr.a", 2);

        // Assert
        Assert.That(derived.CallCount, Is.EqualTo(2));
        Assert.That(derived.LastNewValue, Is.EqualTo(2));
    }

    #endregion Dependency Change Handling



    #region Range Enforcement

    [Test]
    public void True_SetOutsideDefaultRange_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var derived = CreateDerivedValue(5);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => derived.True = DefaultMaxValue + 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => derived.True = DefaultMinValue - 1);
    }

    [Test]
    public void Effective_SetOutsideDefaultRange_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var derived = CreateDerivedValue(5);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => derived.Effective = DefaultMaxValue + 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => derived.Effective = DefaultMinValue - 1);
    }

    #endregion



    #region Value Setting
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
    #endregion Value Setting
}