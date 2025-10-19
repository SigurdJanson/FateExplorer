using Aventuria.Calendar;
using NUnit.Framework;
using System;

namespace UnitTests.Aventuria.Calendar;


[TestFixture]
public class CalendarWeekTests
{
    [Test]
    [TestCase(DesignationOfWeek.Lizardian)]
    [TestCase(DesignationOfWeek.Nameless)]
    public void WeekLength(DesignationOfWeek culture)
    {
        // Arrange
        var week = Weekday.GetCalendarWeek((DesignationOfWeek)culture);

        // Act
        int Length = Enum.GetValues(typeof(DesignationOfWeek)).Length;

        // Assert
        Assert.That(week.Length, Is.EqualTo(Length), $"Week length for {culture} should be {Length}");
    }


    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(7)]
    public void Parse(int day)
    {
        // Arrange
        //BosparanWeek bosparanWeek = new();
        Weekday Expected = new(day, DesignationOfWeek.Bosparan);

        // Act
        var result = BosparanWeek.Parse(day);

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }



    [Test]
    public void Equals_Null_ReturnsFalse()
    {
        #nullable enable
        // Arrange
        var bosparanWeek = new BosparanWeek();
        CalendarWeek? other = null;

        // Act
        var result = bosparanWeek.Equals(other);

        // Assert
        Assert.That(result, Is.False);
        #nullable restore
    }


    [Test]
    public void Equals_SameType_ReturnsTrue()
    {
        #nullable enable
        // Arrange
        var bosparanWeek = new BosparanWeek();
        CalendarWeek? other = new BosparanWeek(); // create another instance;

        // Act
        var result = bosparanWeek.Equals(other);

        // Assert
        Assert.That(result, Is.True);
        #nullable restore
    }



    [Test]
    public void Equals_Same_True()
    {
#nullable enable
        // Arrange
        var bosparanWeek = new BosparanWeek();
        object? obj = new BosparanWeek(); // create another instance;;

        // Act
        var result = bosparanWeek.Equals(obj);

        // Assert
        Assert.That(result, Is.True);
#nullable restore
    }



    [Test]
    [TestCase(DesignationOfWeek.Bosparan)]
    [TestCase(DesignationOfWeek.Novadi)]
    [TestCase(DesignationOfWeek.Gjalsker)]
    [TestCase(DesignationOfWeek.Nameless)]
    [TestCase(DesignationOfWeek.Lizardian)]
    public void GetHashCode(DesignationOfWeek culture)
    {
        // Arrange
        CalendarWeek Week = Weekday.GetCalendarWeek(culture);

        // Act
        var result = Week.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(culture.GetHashCode()));
    }



    [Test]
    [TestCase(DesignationOfWeek.Bosparan)]
    [TestCase(DesignationOfWeek.Novadi)]
    [TestCase(DesignationOfWeek.Gjalsker)]
    public void ToString(DesignationOfWeek culture)
    {
        // Arrange
        var bosparanWeek = Weekday.GetCalendarWeek(culture);

        // Act
        var result = bosparanWeek.ToString();

        // Assert
        Assert.That(result, Is.EqualTo($"{culture}"));
    }
}
