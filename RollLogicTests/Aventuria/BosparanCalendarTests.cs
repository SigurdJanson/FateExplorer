using Aventuria.Calendar;
using NUnit.Framework;
using System;

namespace UnitTests.Aventuria;


#pragma warning disable IDE0018 // Inlinevariablendeklaration


[TestFixture]
public class BosparanCalendarTests
{

    #region Get Functions

    [Test]
    [TestCase("18.4.2022", ExpectedResult = 4)]
    [TestCase("26.12.2005", ExpectedResult = 12)]
    [TestCase("27.12.2005", ExpectedResult = 13)]
    [TestCase("29.06.1987", ExpectedResult = 6)]
    [TestCase("30.06.1987", ExpectedResult = 7)]
    [TestCase("29.06.1988", ExpectedResult = 6)] // Leap year
    [TestCase("30.06.1988", ExpectedResult = 7)] // Leap year
    public int GetMonth(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetMonth(time);

        // Assert
        return result;
    }


    [Test]
    [TestCase("18.4.2022", ExpectedResult = 1045)]
    [TestCase("26.12.2005", ExpectedResult = 1028)]
    [TestCase("27.12.2005", ExpectedResult = 1028)]
    [TestCase("30.06.1987", ExpectedResult = 1010)]
    public int GetYear(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetYear(time);

        // Assert
        return result;
    }



    [Test]
    [TestCase("30.1.2000", ExpectedResult = 11)] // Leap year
    [TestCase("31.1.2000", ExpectedResult = 11)] // Leap year
    [TestCase("18.4.2022", ExpectedResult = 11)]
    [TestCase("26.12.2005", ExpectedResult = 11)]
    [TestCase("27.12.2005", ExpectedResult = 11)]
    [TestCase("30.06.1988", ExpectedResult = 11)] // Leap year
    public int GetEra_Always11(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetEra(time);

        // Assert
        return result;
    }



    [Test]
    [TestCase("30.1.2000", ExpectedResult = 30)]
    [TestCase("31.1.2000", ExpectedResult = 1)]
    [TestCase("18.4.2022", ExpectedResult = 18)]
    [TestCase("26.12.2005", ExpectedResult = 30)]
    [TestCase("27.12.2005", ExpectedResult = 1)]
    [TestCase("29.06.1987", ExpectedResult = 30)]
    [TestCase("30.06.1987", ExpectedResult = 1)]
    [TestCase("29.06.1988", ExpectedResult = 30)] // Leap year
    [TestCase("30.06.1988", ExpectedResult = 1)] // Leap year
    public int GetDayOfMonth(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetDayOfMonth(time);

        // Assert
        return result;
    }


    [Test]
    [TestCase("13.4.2022", ExpectedResult = DayOfWeek.Saturday)] // Reference -1
    [TestCase("14.4.2022", ExpectedResult = DayOfWeek.Sunday)] // Reference date
    [TestCase("15.4.2022", ExpectedResult = DayOfWeek.Monday)] // Reference +1
    [TestCase("30.1.2000", ExpectedResult = DayOfWeek.Tuesday)]
    [TestCase("31.1.2000", ExpectedResult = DayOfWeek.Wednesday)]
    [TestCase("26.12.2005", ExpectedResult = DayOfWeek.Monday)]
    [TestCase("27.12.2005", ExpectedResult = DayOfWeek.Tuesday)]
    [TestCase("30.06.1987", ExpectedResult = DayOfWeek.Sunday)]
    [TestCase("18.4.2022", ExpectedResult = DayOfWeek.Thursday)]
    [TestCase("28.9.2025", ExpectedResult = DayOfWeek.Tuesday)]
    public DayOfWeek GetDayOfWeek(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetDayOfWeek(time);

        // Assert
        return result;
    }


    [Test]
    [TestCase("30.1.2000", ExpectedResult = 30)]
    [TestCase("31.1.2000", ExpectedResult = 31)]
    [TestCase("18.4.2022", ExpectedResult = 3 * 30 + 18)]
    [TestCase("26.12.2005", ExpectedResult = 12 * 30)]
    [TestCase("27.12.2005", ExpectedResult = 12 * 30 + 1)]
    [TestCase("30.06.1987", ExpectedResult = 31 + 28 + 31 + 30 + 31 + 30)]
    public int GetDayOfYear(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetDayOfYear(time);

        // Assert
        return result;
    }


    [Test]
    [TestCase("30.1.2000", ExpectedResult = 365)] // Leap year
    [TestCase("31.1.2000", ExpectedResult = 365)] // Leap year
    [TestCase("18.4.2022", ExpectedResult = 365)]
    [TestCase("26.12.2005", ExpectedResult = 365)]
    [TestCase("27.12.2005", ExpectedResult = 365)]
    [TestCase("30.06.1988", ExpectedResult = 365)] // Leap year
    public int GetDaysInYear(string Date)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetDaysInYear(time.Year);

        // Assert
        return result;
    }


    [Test]
    [TestCase("30.1.2000", 11, ExpectedResult = 365)] // Leap year
    [TestCase("31.1.2000", 12, ExpectedResult = 365)] // Leap year
    [TestCase("18.4.2022", 11, ExpectedResult = 365)]
    [TestCase("26.12.2005", 10, ExpectedResult = 365)]
    [TestCase("27.12.2005", 11, ExpectedResult = 365)]
    [TestCase("30.06.1988", 11, ExpectedResult = 365)] // Leap year
    public int GetDaysInYear_Era_NoDifference(string Date, int Era)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetDaysInYear(time.Year, Era);

        // Assert
        return result;
    }



    [Test]
    [TestCase("30.1.2000", 11, ExpectedResult = 13)] // Leap year
    [TestCase("31.1.2000", 12, ExpectedResult = 13)] // Leap year
    [TestCase("18.4.2022", 11, ExpectedResult = 13)]
    [TestCase("26.12.2005", 10, ExpectedResult = 13)]
    [TestCase("27.12.2005", 11, ExpectedResult = 13)]
    [TestCase("30.06.1988", 11, ExpectedResult = 13)] // Leap year
    public int GetMonthsInYear_Era_NoDifference(string Date, int Era)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetMonthsInYear(time.Year, Era);

        // Assert
        return result;
    }



    [Test]
    [TestCase(2000, 1, 11, ExpectedResult = 30)]
    [TestCase(2022, 4, 11, ExpectedResult = 30)]
    [TestCase(2005, 12, 10, ExpectedResult = 30)]
    [TestCase(2005, 13, 11, ExpectedResult = 5)] // days of the Nameless One
    [TestCase(1987, 6, 11, ExpectedResult = 30)]
    public int GetDaysInMonth(int Year, int Month, int Era)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.GetDaysInMonth(Year, Month, Era);

        // Assert
        return result;
    }

    #endregion




    #region Add Functions

    [Test]
    [TestCase("30.1.2000", 11, ExpectedResult = 2011)] // Leap year
    [TestCase("31.1.2000", 12, ExpectedResult = 2012)] // Leap year
    [TestCase("18.4.2022", 0, ExpectedResult = 2022)]
    [TestCase("26.12.2005", -1, ExpectedResult = 2004)]
    [TestCase("27.12.2005", 3, ExpectedResult = 2008)]
    [TestCase("30.06.1988", 11, ExpectedResult = 1999)] // Leap year
    public int AddYears(string Date, int Years)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddYears(time, Years);

        // Assert
        return result.Year;
    }



    [Test]
    [TestCase("30.1.2000", 1, ExpectedResult = "01.3.2000")] // Leap year
    [TestCase("30.1.2000", 2, ExpectedResult = "31.3.2000")] // Leap year
    [TestCase("30.1.1999", 1, ExpectedResult = "01.3.1999")]
    [TestCase("30.1.1999", 2, ExpectedResult = "31.3.1999")]
    [TestCase("28.11.2005", 1, ExpectedResult = "28.12.2005")] // 2. Rahja -> 2. Namenlosen
    [TestCase("26.12.2005", 1, ExpectedResult = "31.12.2005")] // 30. Rahja -> 5. Namelosen
    [TestCase("26.12.2005", 2, ExpectedResult = "30.1.2006")]
    [TestCase("27.12.2005", 1, ExpectedResult = "01.1.2006")]
    [TestCase("27.12.2005", 2, ExpectedResult = "31.1.2006")]
    [TestCase("30.06.1988", 2, ExpectedResult = "29.8.1988")] // Leap year
    [TestCase("18.4.2022", 6, ExpectedResult = "15.10.2022")]
    [TestCase("18.4.2022", 10, ExpectedResult = "18.1.2023")] // between years
    public string AddMonths_LessThanYear(string Date, int Months)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddMonths(time, Months);

        // Assert
        return result.ToString("dd.M.yyyy");
    }


    [Test]
    [TestCase("30.1.2000", 13, ExpectedResult = "30.1.2001")] // Leap year
    [TestCase("31.1.2000", 13, ExpectedResult = "31.1.2001")] // Leap year
    [TestCase("18.4.2022", 2 * 13, ExpectedResult = "18.4.2024")]
    [TestCase("26.12.2005", 2 * 13, ExpectedResult = "26.12.2007")]
    [TestCase("27.12.2005", 2 * 13, ExpectedResult = "27.12.2007")]
    [TestCase("30.06.1988", 3 * 13, ExpectedResult = "30.6.1991")] // Leap year
    public string AddMonths_13Months(string Date, int Months)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddMonths(time, Months);

        // Assert
        return result.ToString("dd.M.yyyy");
    }


    [Test]
    [TestCase("1.1.2000", -1, ExpectedResult = "27.12.1999")] // between years, (Leap year)
    [TestCase("2.1.2000", -1, ExpectedResult = "28.12.1999")] // between years, (Leap year)
    [TestCase("31.1.2000", -2, ExpectedResult = "27.12.1999")] // between years, (Leap year)
    [TestCase("30.1.2000", -2, ExpectedResult = "26.12.1999")] // between years, (Leap year)
    [TestCase("26.12.2005", -1, ExpectedResult = "26.11.2005")]
    [TestCase("26.12.2005", -2, ExpectedResult = "27.10.2005")] // 30. Rahja -> 30. Ingerimm
    [TestCase("27.12.2005", -1, ExpectedResult = "27.11.2005")] // 1. Namenlos -> 1. Rahja
    [TestCase("27.12.2005", -2, ExpectedResult = "28.10.2005")]
    [TestCase("30.06.1988", -2, ExpectedResult = "01.5.1988")] // (Leap year)
    [TestCase("18.4.2022", -6, ExpectedResult = "14.11.2021")]
    [TestCase("18.4.2022", -10, ExpectedResult = "17.7.2021")] // between years
    public string AddMonths_LessThanYearNegative(string Date, int Months)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddMonths(time, Months);

        // Assert
        return result.ToString("dd.M.yyyy");
    }

    #endregion



    #region Leaping

    [Test]
    [TestCase(2000, 10, ExpectedResult = 0)] // Leap year
    [TestCase(2000, 12, ExpectedResult = 0)] // Leap year
    [TestCase(2022, 10, ExpectedResult = 0)]
    [TestCase(2005, 12, ExpectedResult = 0)]
    [TestCase(2005, 11, ExpectedResult = 0)]
    [TestCase(1988, 11, ExpectedResult = 0)] // Leap year
    public int GetLeapMonth_Always0(int Year, int Era)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.GetLeapMonth(Year, Era);

        // Assert
        return result;
    }


    [Test]
    [TestCase(1932, ExpectedResult = false)]
    [TestCase(2016, ExpectedResult = false)]
    [TestCase(2028, ExpectedResult = false)]
    [TestCase(2132, ExpectedResult = false)]
    public bool IsLeapDay_EarthLeapDay_False(int Year, int Month = 2, int Day = 29)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.IsLeapDay(Year, Month, Day);

        // Assert
        return result;
    }

    [Test]
    [TestCase(1900, ExpectedResult = false)]
    [TestCase(2017, ExpectedResult = false)]
    [TestCase(2018, ExpectedResult = false)]
    [TestCase(2019, ExpectedResult = false)]
    public bool IsLeapDay_NoEarthLeapDay_False(int Year, int Month = 2, int Day = 29)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.IsLeapDay(Year, Month, Day);

        // Assert
        return result;
    }


    [Test]
    public void IsLeapDay_VaryingEras_False(
        [Values(10, 11, 12)] int Era, [Values(1900, 2016, 2019)] int Year)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        int Month = 2;
        int Day = 29;

        // Act
        var result = bosparanCalendar.IsLeapDay(Year, Month, Day, Era);

        // Assert
        Assert.That(result, Is.False);
    }



    [Test]
    public void IsLeapMonth(
        [Values(10, 11, 12)] int Era,
        [Values(1900, 2016, 2019)] int Year,
        [Values(1, 2, 12)] int Month)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.IsLeapMonth(Year, Month, Era);

        // Assert
        Assert.That(result, Is.False);
    }



    [Test]
    public void IsLeapYear(
        [Values(10, 11, 12)] int Era,
        [Values(1900, 2016, 2019)] int Year)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.IsLeapYear(Year, Era);

        // Assert
        Assert.That(result, Is.False);
    }

    #endregion


    [Test]
    [TestCase(1045, 2, 1, ExpectedResult = "31.01.2022")]
    [TestCase(1045, 2, 30, ExpectedResult = "01.03.2022")]
    [TestCase(1047, 2, 1, ExpectedResult = "31.01.2024")] // earthen leap year
    [TestCase(1047, 2, 30, ExpectedResult = "01.03.2024")] // earthen leap year
    public string ToDateTime(int Year, int Month, int Day)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        int hour = 0;
        int minute = 0;
        int second = 0;
        int millisecond = 0;
        int era = 11;

        // Act
        var result = bosparanCalendar.ToDateTime(Year, Month, Day,
            hour, minute, second, millisecond, era);

        // Assert
        return result.ToShortDateString();
    }




    [Test]
    [TestCase(2000, 2, 29, 2009, 2, 27, ExpectedResult = 8)]
    [TestCase(2000, 2, 29, 2009, 2, 28, ExpectedResult = 9)]
    [TestCase(2000, 2, 29, 2009, 3, 1, ExpectedResult = 9)]

    [TestCase(2000, 2, 28, 2011, 2, 28, ExpectedResult = 11)]
    [TestCase(2000, 2, 28, 2011, 3, 1, ExpectedResult = 11)]

    [TestCase(2000, 2, 29, 2011, 2, 27, ExpectedResult = 10)]
    [TestCase(2000, 2, 29, 2011, 2, 28, ExpectedResult = 11)]
    [TestCase(2000, 2, 29, 2011, 3, 1, ExpectedResult = 11)]

    [TestCase(2000, 3, 1, 2011, 2, 27, ExpectedResult = 10)]
    [TestCase(2000, 3, 1, 2011, 2, 28, ExpectedResult = 10)]
    [TestCase(2000, 3, 1, 2011, 3, 1, ExpectedResult = 11)]
    [TestCase(2022, 4, 14, 2025, 9, 28, ExpectedResult = 3)]//"28.9.2025"
    public int AbsDeltaInYears(int Year1, int Month1, int Day1, int Year2, int Month2, int Day2)
    {
        // Arrange
        // Act
        var result = BosparanCalendar.AbsDeltaInYears(
            new DateTime(Year1, Month1, Day1),
            new DateTime(Year2, Month2, Day2));

        // Assert
        return result;
    }


    [TestCase("14.4.2022", ExpectedResult = 16)] // 14. Travia 1045; same day as reference date
    [TestCase("14.5.2022", ExpectedResult = 18)] // 14. Boron
    [TestCase("14.6.2022", ExpectedResult = 21)] // 
    [TestCase("14.4.2023", ExpectedResult = 17)] // 14. Travia 1046
    [TestCase("29.9.2123", ExpectedResult = 5)] // 2. Peraine 1146
    [TestCase("8.11.2022", ExpectedResult = 0)] // 12. Ingerimm 1045, new moon
    [TestCase("5.12.2022", ExpectedResult = 27)] // 9. Rahja 1045
    public int GetMoonCycle_FutureToRef(string Date)
    {
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetMoonCycle(time);

        // Assert
        return result;
    }





    [TestCase("13.4.2022", ExpectedResult = 15)] // 13. Travia 1045; day prior to reference date
    [TestCase("15.3.2022", ExpectedResult = 14)] // 14. Efferd = Month-1
    [TestCase("14.1.2022", ExpectedResult = 10)] // 14. Praios = Month-3
    [TestCase("14.4.2021", ExpectedResult = 15)] // 14. Travia 1044; Year-1
    [TestCase("1.1.1997", ExpectedResult = 0)] // 1. Praios 1020, new moon
    [TestCase("31.12.1996", ExpectedResult = 27)] // 1. Praios 1020, new moon
    public int GetMoonCycle_PastToRef(string Date)
    {
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.GetMoonCycle(time);

        // Assert
        return result;
    }




    [TestCase("4.8.2005", ExpectedResult = 27)] // 5.12.2022 = 9. Rahja 1045 ======> 6. Tsa (8) 1028 = 6.8.2005
    public int GetMoonCycle_No28(string Date)
    {
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));
        Assume.That(bosparanCalendar.GetYear(time), Is.EqualTo(1028));
        Assume.That(bosparanCalendar.GetMonth(time), Is.EqualTo(8));
        Assume.That(bosparanCalendar.GetDayOfMonth(time), Is.EqualTo(6));

        // Act
        var result = bosparanCalendar.GetMoonCycle(time);

        // Assert
        return result;
    }
}

#pragma warning restore IDE0018 // Inlinevariablendeklaration

