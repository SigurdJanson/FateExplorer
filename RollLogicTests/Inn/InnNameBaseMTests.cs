using FateExplorer.Inn;
using NUnit.Framework;

namespace UnitTests.Inn;

[TestFixture]
public class InnNameBaseMTests
{
    private InnNameBaseM _innNameBaseM;

    [SetUp]
    public void Setup()
    {
        _innNameBaseM = new InnNameBaseM();
    }

    [Test]
    public void PluralIsSuffix_PluralNotNullAndSuffix_ReturnsTrue()
    {
        _innNameBaseM.Plural = "-s";
        Assert.IsTrue(_innNameBaseM.PluralIsSuffix);
    }

    [Test]
    public void PluralIsSuffix_PluralNotNullAndNotSuffix_ReturnFalse()
    {
        _innNameBaseM.Plural = "Häuser";
        Assert.IsFalse(_innNameBaseM.PluralIsSuffix);
    }

    [Test]
    public void PluralIsSuffix_PluralIsNull_ReturnsFalse()
    {
        _innNameBaseM.Plural = null;
        Assert.IsFalse(_innNameBaseM.PluralIsSuffix);
    }



    [Test]
    public void Plural_PluralIsSuffix_ReturnsConcat()
    {
        _innNameBaseM.Singular = "Inn";
        _innNameBaseM.Plural = "-s";
        Assert.AreEqual("Inns", _innNameBaseM.Plural);
    }

    [Test]
    public void Plural_WhenPluralIsNotSuffix_ReturnsPlural()
    {
        _innNameBaseM.Singular = "Mouse";
        _innNameBaseM.Plural = "Mice";
        Assert.AreEqual("Mice", _innNameBaseM.Plural);
    }

    [Test]
    public void Plural_WhenPluralIsNull_ReturnsSingular()
    {
        _innNameBaseM.Singular = "Keiler";
        _innNameBaseM.Plural = null;
        Assert.AreEqual("Keiler", _innNameBaseM.Plural);
    }
}
