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
        Assert.That(_innNameBaseM.PluralIsSuffix, Is.True);
    }

    [Test]
    public void PluralIsSuffix_PluralNotNullAndNotSuffix_ReturnFalse()
    {
        _innNameBaseM.Plural = "Häuser";
        Assert.That(_innNameBaseM.PluralIsSuffix, Is.False);
    }

    [Test]
    public void PluralIsSuffix_PluralIsNull_ReturnsFalse()
    {
        _innNameBaseM.Plural = null;
        Assert.That(_innNameBaseM.PluralIsSuffix, Is.False);
    }



    [Test]
    public void Plural_PluralIsSuffix_ReturnsConcat()
    {
        _innNameBaseM.Singular = "Inn";
        _innNameBaseM.Plural = "-s";
        Assert.That("Inns", Is.EqualTo(_innNameBaseM.Plural));
    }

    [Test]
    public void Plural_WhenPluralIsNotSuffix_ReturnsPlural()
    {
        _innNameBaseM.Singular = "Mouse";
        _innNameBaseM.Plural = "Mice";
        Assert.That("Mice", Is.EqualTo(_innNameBaseM.Plural));
    }

    [Test]
    public void Plural_WhenPluralIsNull_ReturnsSingular()
    {
        _innNameBaseM.Singular = "Keiler";
        _innNameBaseM.Plural = null;
        Assert.That("Keiler", Is.EqualTo(_innNameBaseM.Plural));
    }
}
