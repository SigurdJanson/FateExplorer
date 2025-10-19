using Aventuria.Calendar;
using NUnit.Framework;
using System;

namespace UnitTests.Aventuria.Calendar;

[TestFixture]
public class WeekdayTests
{

    [Test]
    public void GetCalendarWeek()
    {
        // Arrange
        var weekday = new Weekday(2, DesignationOfWeek.Novadi);
        Type ExpectedType = typeof(NovadiWeek);
        // Act
        CalendarWeek result = weekday.GetCalendarWeek();

        // Assert
        Assert.That(result, Is.TypeOf(ExpectedType));
    }



    [Test]
    [TestCase(DesignationOfWeek.Bosparan, typeof(BosparanWeek))]
    [TestCase(DesignationOfWeek.Nameless, typeof(NamelessWeek))]
    public void GetCalendarWeek_Static(DesignationOfWeek designation, Type expectedType)
    {
        // Arrange
        // Act
        var result = Weekday.GetCalendarWeek(designation);

        // Assert
        Assert.That(result, Is.TypeOf(expectedType));
    }



    [Test]
    [TestCase(7, 7, DesignationOfWeek.Bosparan, ExpectedResult = 0, Category = "Culture is different")]
    [TestCase(1, 1, DesignationOfWeek.Novadi, ExpectedResult = 0, Category = "Culture is different")]
    [TestCase(1, 2, DesignationOfWeek.Novadi, ExpectedResult = -1, Category = "Day is different")]
    [TestCase(2, 1, DesignationOfWeek.Bosparan, ExpectedResult = 1, Category = "Day is different")]
    [TestCase(9, 9, DesignationOfWeek.Novadi, ExpectedResult = 0, Category = "Same")]
    public int CompareTo_SameCulture(int day1, int day2, DesignationOfWeek cal1)
    {
        // Arrange
        Weekday weekday = new (day1, cal1);
        Weekday other = new(day2, cal1);

        // Act
        var result = weekday.CompareTo(other);

        // Assert
        return result;
    }

    //[Test]
    //[TestCase(7, 7, DesignationOfWeek.Bosparan, ExpectedResult = 0, Category = "Culture is different")]
    //[TestCase(1, 1, DesignationOfWeek.Novadi, ExpectedResult = 0, Category = "Culture is different")]
    //[TestCase(1, 2, DesignationOfWeek.Novadi, ExpectedResult = 1, Category = "Day is different")]
    //[TestCase(2, 1, DesignationOfWeek.Bosparan, ExpectedResult = -1, Category = "Day is different")]
    //[TestCase(9, 9, DesignationOfWeek.Novadi, ExpectedResult = 0, Category = "Same")]
    //public int CompareTo_DifferentCulture(int day1, int day2, DesignationOfWeek cal1, DesignationOfWeek cal2)
    //{
    //    // Arrange
    //    Weekday weekday = new(day1, cal1);
    //    Weekday other = new(day2, cal1);

    //    // Act
    //    var result = weekday.CompareTo(other);

    //    // Assert
    //    return result;
    //}





    [Test]
    [TestCase(7, 7, DesignationOfWeek.Bosparan, DesignationOfWeek.Novadi, ExpectedResult = false, Category = "Culture is different")]
    [TestCase(1, 1, DesignationOfWeek.Novadi, DesignationOfWeek.Bosparan, ExpectedResult = false, Category = "Culture is different")]
    [TestCase(3, 7, DesignationOfWeek.Novadi, DesignationOfWeek.Novadi, ExpectedResult = false, Category = "Day is different")]
    [TestCase(2, 1, DesignationOfWeek.Novadi, DesignationOfWeek.Novadi, ExpectedResult = false, Category = "Day is different")]
    [TestCase(9, 9, DesignationOfWeek.Novadi, DesignationOfWeek.Novadi, ExpectedResult = true, Category = "Same")]
    public bool Equatable_Equals(int day1, int day2, DesignationOfWeek cal1, DesignationOfWeek cal2)
    {
        // Arrange
        Weekday weekday = new(day1, cal1);
        Weekday other = new(day2, cal2);

        // Act
        var result = weekday.Equals(other);

        // Assert
        return result;
    }


    [Test]
    [TestCase(7, DesignationOfWeek.Bosparan, DesignationOfWeek.Novadi, ExpectedResult = false, Category = "Different")]
    [TestCase(1, DesignationOfWeek.Novadi, DesignationOfWeek.Bosparan, ExpectedResult = false, Category = "Different")]
    [TestCase(3, DesignationOfWeek.Novadi, DesignationOfWeek.Novadi, ExpectedResult = true, Category = "Same")]
    public bool Equals_SameDay(int day, DesignationOfWeek cal1, DesignationOfWeek cal2)
    {
        // Arrange
        Weekday weekday = new(day, cal1);
        Weekday other = new(day, cal2);

        // Act
        var result = weekday.Equals(other);

        // Assert
        return result;
    }


    [Test]
    [TestCase(7, 6, DesignationOfWeek.Bosparan, ExpectedResult = false, Category = "Different")]
    [TestCase(1, 2, DesignationOfWeek.Novadi, ExpectedResult = false, Category = "Different")]
    [TestCase(3, 3, DesignationOfWeek.Novadi, ExpectedResult = true, Category = "Same")]
    public bool Equals_SameDesignation(int day1, int day2, DesignationOfWeek cal)
    {
        // Arrange
        Weekday weekday = new(day1, cal);
        Weekday other = new(day2, cal);

        // Act
        var result = weekday.Equals(other);

        // Assert
        return result;
    }



    [Test]
    [TestCase(7, 6, DesignationOfWeek.Bosparan, ExpectedResult = false, Category = "Different")]
    [TestCase(1, 2, DesignationOfWeek.Novadi, ExpectedResult = false, Category = "Different")]
    [TestCase(3, 3, DesignationOfWeek.Novadi, ExpectedResult = true, Category = "Same")]
    public bool EqualsAsObject_SameDesignation(int day1, int day2, DesignationOfWeek cal)
    {
        // Arrange
        Weekday weekday = new(day1, cal);
        object other = new Weekday(day2, cal);

        // Act
        var result = weekday.Equals(other);

        // Assert
        return result;
    }





    [Test]
    [TestCase(7, DesignationOfWeek.Bosparan, ExpectedResult = 458759)]
    [TestCase(1, DesignationOfWeek.Novadi, ExpectedResult = 65545)]
    public int GetHashCode(int day, DesignationOfWeek calendar)
    {
        // Arrange
        var weekday = new Weekday(day, calendar);

        // Act
        var result = weekday.GetHashCode();

        // Assert
        return result;
    }



    [Test]
    [TestCase(7, DesignationOfWeek.Bosparan, ExpectedResult = "7")]
    [TestCase(1, DesignationOfWeek.Novadi, ExpectedResult = "1")]
    public string ToString(int day, DesignationOfWeek calendar)
    {
        // Arrange
        Weekday weekday = new(day, calendar);

        // Act
        var result = weekday.ToString();

        // Assert
        return result;
    }
}
