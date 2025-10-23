using Aventuria.Calendar;
using NUnit.Framework;
using System;

namespace UnitTests.Aventuria.Calendar;


#pragma warning disable IDE0018 // Inlinevariablendeklaration


[TestFixture]
public class BosparanCalendarTests
{
    internal const int MillisPerSecond = 1000;
    internal const int MillisPerMinute = MillisPerSecond * 60;
    internal const int MillisPerHour = MillisPerMinute * 60;
    internal const long MillisPerDay = MillisPerHour * 24;


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
    [TestCase("30.1.2000", 11, ExpectedResult = "30.1.2011")] // Leap year
    [TestCase("31.1.2000", 12, ExpectedResult = "31.1.2012")] // Leap year
    [TestCase("18.4.2022", 0, ExpectedResult = "18.4.2022")]
    [TestCase("26.12.2005", -1, ExpectedResult = "26.12.2004")]
    [TestCase("27.12.2005", 3, ExpectedResult = "27.12.2008")]
    [TestCase("30.06.1988", 11, ExpectedResult = "30.6.1999")] // Leap year
    [TestCase("29.02.1996", 4, ExpectedResult = "28.2.2000")] // Leap year to no leap year
    public string AddYears(string Date, int Years)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddYears(time, Years);

        // Assert
        return result.ToString("dd.M.yyyy");
    }



    [Test]
    [TestCase("30.1.2000", 1, ExpectedResult = "01.3.2000")] // Leap year before Feb 29
    [TestCase("30.1.2000", 2, ExpectedResult = "31.3.2000")] // Leap year incl. Feb 29
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
    [TestCase("02.1.1998", 30, ExpectedResult = "02.5.2000")] // Add over a year incl. lap year
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



    [Test]
    [TestCase("01.01.2000", -1, ExpectedResult = "31.12.1999")] // between years
    [TestCase("31.12.1999", +1, ExpectedResult = "01.01.2000")] // between years
    [TestCase("01.03.2004", -1, ExpectedResult = "28.02.2004")] // Skip the leap day that does not exist in Aventuria
    [TestCase("28.02.2004", +1, ExpectedResult = "01.03.2004")] // Skip the leap day that does not exist in Aventuria
    [TestCase("01.01.2000", 365, ExpectedResult = "01.01.2001")]  // exactly 365 days, between years, regular year
    [TestCase("01.01.2001", -365, ExpectedResult = "01.01.2000")] // exactly -365 days, between years, regular year
    [TestCase("01.01.2004", 365, ExpectedResult = "01.01.2005")]  // exactly 365 days, between years, leap year
    [TestCase("01.01.2005", -365, ExpectedResult = "01.01.2004")] // exactly -365 days, between years, leap year
    [TestCase("01.01.2100", 390, ExpectedResult = "26.01.2101")]  // > 365 days, between years, regular year
    [TestCase("01.01.2101", -390, ExpectedResult = "07.12.2099")] // > 365 days, between years, regular year
    [TestCase("01.01.2004", 390, ExpectedResult = "26.01.2005")]  // > 365 days, between years, leap year
    [TestCase("01.01.2005", -390, ExpectedResult = "07.12.2003")] // > 365 days, between years, leap year
    public string AddDays(string Date, int Days)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddDays(time, Days);

        // Assert
        return result.ToString("dd.MM.yyyy");
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
    [TestCase(45, ExpectedResult = 1045)]
    [TestCase(0,  ExpectedResult = 1000)]
    [TestCase(99, ExpectedResult = 1099)]
    public int ToD4DigitYear(int Year)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();

        // Act
        var result = bosparanCalendar.ToFourDigitYear(Year);

        // Assert
        return result;
    }



    #region Dere Calendar Method

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
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = BosparanCalendar.GetMoonCycle(time);

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
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = BosparanCalendar.GetMoonCycle(time);

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
        var result = BosparanCalendar.GetMoonCycle(time);

        // Assert
        return result;
    }



    [Test]
    // Same day
    [TestCase("29.2.2000", "29.2.2000", ExpectedResult = 0)] //
    // Same year
    [TestCase("30.1.1796", "11.11.1796", ExpectedResult = 1)]  // Leap day in start & end year
    [TestCase("1.3.1796", "11.11.1796", ExpectedResult = 0)]   // Leap year but start & end after leap day
    [TestCase("31.1.3896", "28.2.3896", ExpectedResult = 0)]   // Leap year but start & end before leap day
    // Next Year
    [TestCase("1.1.2001", "1.1.2002", ExpectedResult = 0)]  // 
    [TestCase("1.1.2000", "1.1.2001", ExpectedResult = 1)]  // 1 leap day in 2000
    [TestCase("1.1.2000", "1.3.2001", ExpectedResult = 1)]  // 1 leap day in 2000
    [TestCase("1.3.2000", "1.3.2001", ExpectedResult = 0)]  // 
    [TestCase("29.11.1995", "28.11.1996", ExpectedResult = 1, Category = "Yr+1", Description = "Leap year, jump over leap day")] // Leap year in 1996
    // Multiple Years
    [TestCase("1.3.2000", "28.2.2004", ExpectedResult = 0)]  // No leap days, exactly between leap days
    [TestCase("29.2.2000", "29.2.2004", ExpectedResult = 2)] // exactly both leap days in 2 years
    [TestCase("1.3.0004", "1.3.0008", ExpectedResult = 1)]   // exactly one leap day in 4 years
    [TestCase("1.3.1096", "28.2.1104", ExpectedResult = 0)]  // no leap day in 8 years
    [TestCase("01.01.2005 00:00:00", "08.12.2003 00:00:00", ExpectedResult = 1)] // reversed direction
    public int GetLeapDays(string StartDate, string EndDate)
    {
        // Arrange
        DateTime start, end;
        Assume.That(DateTime.TryParse(StartDate, out start));
        Assume.That(DateTime.TryParse(EndDate, out end));
        // Act
        var result = BosparanCalendar.GetLeapDays(start, end);
        // Assert
        return result;
    }



    [Test]
    [TestCase("01.01.2000", 1 * MillisPerDay - 1, ExpectedResult = "01.01.2000")] // 1 ms less than a day
    [TestCase("01.01.2000", -1, ExpectedResult = "31.12.1999")] // 1 ms, previous year
    [TestCase("28.02.1704 23:59:59.999", 1, ExpectedResult = "01.03.1704")] // 1 ms, jump leap day
    [TestCase("01.03.1704", -1, ExpectedResult = "28.02.1704")] // 1 ms, jump leap day
    public string AddMilliseconds_LessThanDay_BetweenDaysNYears(string Date, long millisecondsToAdd)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddMilliseconds(time, millisecondsToAdd);

        // Assert
        //Assert.That((result - time).TotalMilliseconds, Is.EqualTo(millisecondsToAdd));
        return result.ToString("dd.MM.yyyy"); // HH:mm:ss.fff");
    }


    [Test] // same as AddDays tests, but in milliseconds
    [TestCase("01.01.2000", 1 * MillisPerDay - 1, ExpectedResult = "01.01.2000")] // 1 ms less than a day
    [TestCase("01.01.2000", -1 * MillisPerDay, ExpectedResult = "31.12.1999", Category = "-1")] // 1 day, between years
    [TestCase("31.12.1999", +1 * MillisPerDay, ExpectedResult = "01.01.2000", Category = "+1")] // 1 day, between years
    [TestCase("01.03.2004", -1 * MillisPerDay, ExpectedResult = "28.02.2004", Category = "-1, Leap")]  // Skip the leap day that does not exist in Aventuria
    [TestCase("28.02.2004", +1 * MillisPerDay, ExpectedResult = "01.03.2004", Category = "+1, Leap")] // Skip the leap day that does not exist in Aventuria

    [TestCase("01.01.1994", 365 * MillisPerDay, ExpectedResult = "01.01.1995", Category = "+365")]  // exactly 365 days, between years, leap year
    [TestCase("01.01.1995", -365 * MillisPerDay, ExpectedResult = "01.01.1994", Category = "-365")] // exactly -365 days, between years, leap year
    [TestCase("01.01.2004", 365 * MillisPerDay, ExpectedResult = "01.01.2005", Category = "+365, Leap")]  // exactly 365 days, between years, leap year
    [TestCase("01.01.2005", -365 * MillisPerDay, ExpectedResult = "01.01.2004", Category = "-365, Leap")] // exactly -365 days, between years, leap year

    [TestCase("01.01.2100", 390 * MillisPerDay, ExpectedResult = "26.01.2101", Category = "+390")]  // > 365 days, between years, regular year
    [TestCase("01.01.2101", -390 * MillisPerDay, ExpectedResult = "07.12.2099", Category = "-390")] // > 365 days, between years, regular year
    [TestCase("01.01.2004", 390 * MillisPerDay, ExpectedResult = "26.01.2005", Category = "+390, Leap")]  // > 365 days, between years, leap year
    [TestCase("01.01.2005", -390 * MillisPerDay, ExpectedResult = "07.12.2003", Category = "-390, Leap")] // > 365 days, between years, leap year
    public string AddMilliseconds_LeapDayTests(string Date, long millisecondsToAdd)
    {
        // Arrange
        var bosparanCalendar = new BosparanCalendar();
        DateTime time;
        Assume.That(DateTime.TryParse(Date, out time));

        // Act
        var result = bosparanCalendar.AddMilliseconds(time, millisecondsToAdd);

        // Assert
        //Assert.That((result - time).TotalMilliseconds, Is.EqualTo(millisecondsToAdd));
        return result.ToString("dd.MM.yyyy");
    }
    #endregion
}

#pragma warning restore IDE0018 // Inlinevariablendeklaration

