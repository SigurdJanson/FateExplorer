using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UnitTests.GameData;

#pragma warning disable IDE0017 // Initialisierung von Objekten vereinfachen


[TestFixture]
public class CalendarDBTests
{

    [SetUp]
    public void SetUp()
    {
    }

    private static CalendarDB CreateCalendarDB(bool Init = false)
    {
        if (!Init)
            return new CalendarDB();
        else
        {
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"calendar_de.json"));
            string jsonString = File.ReadAllText(fileName);
            CalendarDB Result = JsonSerializer.Deserialize<CalendarDB>(jsonString);
            return Result;
        }
    }

    private static List<WeekdayEntry> GetWeekdayDb()
    {
        List<WeekdayEntry> result = new()
        {
            new WeekdayEntry()
            {
                Iid = 1, Name = "Windstag", Abbr = "Wi", Earthday = "Donnerstag", Earthid = 4
            },
            new WeekdayEntry()
            {
                Iid = 2, Name = "Erdtag", Abbr = "Er", Earthday = "Freitag", Earthid = 5
            },
            new WeekdayEntry()
            {
                Iid = 3, Name = "Markttag", Abbr = "Ma", Earthday = "Samstag", Earthid = 6
            },
            new WeekdayEntry()
            {
                Iid= 4, Name = "Praiostag", Abbr = "Pr", Earthday = "Sonntag", Earthid = 0
            },
            new WeekdayEntry()
            {
                Iid= 5, Name = "Rohalstag", Abbr = "Ro", Earthday = "Montag", Earthid = 1
            },
            new WeekdayEntry()
            {
                Iid= 6, Name = "Feuertag", Abbr = "Fe", Earthday = "Dienstag", Earthid = 2
            },
            new WeekdayEntry()
            {
                Iid= 7, Name = "Wassertag", Abbr = "Wa", Earthday = "Mittwoch", Earthid = 3
            }
        };
        return result;
    }



    [Test]
    public void LoadFromFile_ParseSuccessful(
        [Values("de", "en")] string Language)
    {
        // Arrange
        string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
        string fileName = Path.GetFullPath(Path.Combine(BasePath, $"calendar_{Language}.json"));
        string jsonString = File.ReadAllText(fileName);

        // Act
        CalendarDB Result = JsonSerializer.Deserialize<CalendarDB>(jsonString);

        Assert.AreEqual(7, Result.Generic.WeekDays.Count);
        Assert.AreEqual(13, Result.Generic.Month.Count);
        Assert.AreEqual(4, Result.Generic.Season.Count);
    }



    /*
    * 
    */
    #region FIXED HOLIDAY

    [Test]
    [TestCase(4, 29)]
    [TestCase(6, 29)]
    [TestCase(5, 28)]
    [TestCase(5, 30)]
    public void Holiday_FixedSingleDay_NoHoliday(int Month, int Day)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.FixedHoliday = new() 
        { 
            new() { Day = 29, Duration = 1, Descr = "", Name = "Jan's Birthday", Month = 5 } 
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddFixedHolidays(Month, Day, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }


    [Test]
    [TestCase(5, 29)]
    public void Holiday_FixedSingleDay_Holiday(int Month, int Day)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.FixedHoliday = new()
        {
            new()
            {
                Day = 29,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddFixedHolidays(Month, Day, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }



    [Test]
    [TestCase(4, 15)]
    [TestCase(6, 15)]
    [TestCase(5, 14)]
    [TestCase(5, 22)]
    public void Holiday_FixedWeek_NoHoliday(int Month, int Day)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.FixedHoliday = new()
        {
            new()
            {
                Day = 15,
                Duration = 7,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddFixedHolidays(Month, Day, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }


    [Test]
    [TestCase(5, 15)]
    [TestCase(5, 16)]
    [TestCase(5, 17)]
    [TestCase(5, 18)]
    [TestCase(5, 19)]
    [TestCase(5, 20)]
    [TestCase(5, 21)]
    public void Holiday_FixedWeek_Holiday(int Month, int Day)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.FixedHoliday = new()
        {
            new()
            {
                Day = 15,
                Duration = 7,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddFixedHolidays(Month, Day, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }


    [Test]
    [TestCase(5, 29)]
    [TestCase(5, 30)]
    [TestCase(6, 1)]
    [TestCase(6, 2)]
    [TestCase(6, 3)]
    [TestCase(6, 4)]
    [TestCase(6, 5)]
    public void Holiday_FixedWeek_CarryOver_Holiday(int Month, int Day)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.FixedHoliday = new() 
        { 
            new() { Day = 29, Duration = 7, Descr = "", Name = "Jan's Birthday", Month = 5 }
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddFixedHolidays(Month, Day, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }

    #endregion


    /*
    * 
    */
    #region LUNAR HOLIDAY

    [Test]
    [TestCase(4, 15, MoonPhase.SmallChalice)]
    [TestCase(6, 15, MoonPhase.ThreeFifths)]
    [TestCase(5, 14, MoonPhase.Waning)]
    [TestCase(5, 22, MoonPhase.Helmet)]
    public void LunarHoliday_SingleDay_NoHoliday(int Month, int Day, MoonPhase Phase)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.LunarHoliday = new()
        {
            new()
            {
                MoonPhase = (int)MoonPhase.Wheel,
                Day = 1,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddLunarHolidays(Month, Day, Phase, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }


    [Test] // a first 
    [TestCase(5, 1, MoonPhase.Wheel)]
    [TestCase(5, 27, MoonPhase.Wheel)]
    public void LunarHoliday_FirstDay_Holiday(int Month, int Day, MoonPhase Phase)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.LunarHoliday = new()
        {
            new()
            {
                MoonPhase = (int)MoonPhase.Wheel,
                Day = 1,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();
        Cal.Generic.MoonphaseDays = new()
        {
            1, 2, 2, 2, 3, 3, 3,  4,  5,  5,  5,  6,  6,  6,
            7, 8, 8, 8, 9, 9, 9, 10, 11, 11, 11, 12, 12, 12
        };

        // Act
        Cal.AddLunarHolidays(Month, Day, Phase, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }


    [Test]
    [TestCase(5, 28, MoonPhase.Wheel)]
    [TestCase(5, 30, MoonPhase.Wheel)]
    public void LunardHoliday_FirstDay_SecondPhase_NoHoliday(int Month, int Day, MoonPhase Phase)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.LunarHoliday = new()
        {
            new()
            {
                MoonPhase = 7,
                Day = 1,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();
        Cal.Generic.MoonphaseDays = new()
        {
            1, 2, 2, 2, 3, 3, 3,  4,  5,  5,  5,  6,  6,  6,
            7, 8, 8, 8, 9, 9, 9, 10, 11, 11, 11, 12, 12, 12
        };

        // Act
        Cal.AddLunarHolidays(Month, Day, Phase, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }



    [Test]
    [TestCase(5, 1, MoonPhase.Wheel)]
    [TestCase(5, 27, MoonPhase.Wheel)]
    public void LunardHoliday_SecondDay_FirstPhase_NoHoliday(int Month, int Day, MoonPhase Phase)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.LunarHoliday = new()
        {
            new()
            {
                MoonPhase = 7,
                Day = 2,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();
        Cal.Generic.MoonphaseDays = new()
        {
            1, 2, 2, 2, 3, 3, 3,  4,  5,  5,  5,  6,  6,  6,
            7, 8, 8, 8, 9, 9, 9, 10, 11, 11, 11, 12, 12, 12
        };

        // Act
        Cal.AddLunarHolidays(Month, Day, Phase, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }


    #endregion


    /*
     * 
     */
    #region WEEKDAY BASED HOLIDAY

    //
    [Test]
    [TestCase(5, 1, DayOfWeek.Sunday)]
    [TestCase(5, 7, DayOfWeek.Monday)]
    [TestCase(5, 1, DayOfWeek.Tuesday)]
    [TestCase(5, 7, DayOfWeek.Thursday)]
    public void WeekHoliday_SingleDay_SameWeek_NoHoliday(int Month, int Day, DayOfWeek WeekDay)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = 1,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }


    [Test]
    [TestCase(5, 8, DayOfWeek.Wednesday)]
    [TestCase(5, 15, DayOfWeek.Wednesday)]
    [TestCase(5, 24, DayOfWeek.Wednesday)]
    public void WeekHoliday_SingleDay_OtherWeek_NoHoliday(int Month, int Day, DayOfWeek WeekDay)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = 1,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }



    [Test]
    [TestCase(5, 1, DayOfWeek.Wednesday)]
    [TestCase(5, 4, DayOfWeek.Wednesday)]
    [TestCase(5, 7, DayOfWeek.Wednesday)]
    public void WeekHoliday_SingleDay_Holiday(int Month, int Day, DayOfWeek WeekDay)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = 1,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }


    [Test]
    [TestCase(5, 30, DayOfWeek.Wednesday, -1)]
    [TestCase(5, 24, DayOfWeek.Wednesday, -1)]
    [TestCase(5, 23, DayOfWeek.Wednesday, -2)]
    [TestCase(5, 17, DayOfWeek.Wednesday, -2)]
    public void WeekHoliday_SingleDay_EndOfMonth_Holiday(int Month, int Day, DayOfWeek WeekDay, int CountingBack)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB(true);
        //--Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = CountingBack,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        //--Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }


    [Test]
    [TestCase(5, 30, DayOfWeek.Wednesday, -2)]
    [TestCase(5, 24, DayOfWeek.Wednesday, -2)]
    [TestCase(5, 23, DayOfWeek.Wednesday, -1)]
    [TestCase(5, 17, DayOfWeek.Wednesday, -1)]
    public void WeekHoliday_SingleDay_EndOfMonth_OtherWeek_NoHoliday(int Month, int Day, DayOfWeek WeekDay, int CountingBack)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB(true);
        //--Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = CountingBack,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        //--Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }



    [Test]
    [TestCase(4, 30, DayOfWeek.Wednesday, -1)]
    [TestCase(4, 24, DayOfWeek.Wednesday, -1)]
    [TestCase(6, 24, DayOfWeek.Wednesday, -1)]
    [TestCase(6, 30, DayOfWeek.Wednesday, -1)]
    public void WeekHoliday_SingleDay_EndOfMonth_OtherMonth_NoHoliday(int Month, int Day, DayOfWeek WeekDay, int CountingBack)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB(true);
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = CountingBack,
                Duration = 1,
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }



    [Test]
    [TestCase(5, 2, DayOfWeek.Thursday)]
    [TestCase(5, 3, DayOfWeek.Friday)]
    [TestCase(5, 7, DayOfWeek.Thursday)]
    [TestCase(5, 8, DayOfWeek.Thursday)] // is valid because 1st holiday is in week 1
    [TestCase(5, 9, DayOfWeek.Friday)] // is valid because 1st holiday is in week 1
    public void WeekHoliday_MultiDay_Holiday(int Month, int Day, DayOfWeek WeekDay)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = 1,
                Duration = 3, // !!!
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(1));
    }

    [Test]
    [TestCase(5, 1, DayOfWeek.Thursday)]
    [TestCase(5, 2, DayOfWeek.Friday)]
    public void WeekHoliday_MultiDay_CarryOver_NoHoliday(int Month, int Day, DayOfWeek WeekDay)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = 1,
                Duration = 3, // !!!
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }


    [Test]
    [TestCase(5, 9, DayOfWeek.Thursday)]
    [TestCase(5, 10, DayOfWeek.Friday)]
    public void WeekHoliday_MultiDay_WrongWeek_NoHoliday(int Month, int Day, DayOfWeek WeekDay)
    {
        // Arrange
        CalendarDB Cal = CreateCalendarDB();
        Cal.Generic = new();
        Cal.Generic.WeekHoliday = new()
        {
            new()
            {
                WeekDay = 7, // Wednesday = Waterday
                Day = 1,
                Duration = 3, // !!!
                Descr = "",
                Name = "Jan's Birthday",
                Month = 5
            }
        };
        Cal.Generic.WeekDays = GetWeekdayDb();
        List<(string Name, string Descr)> Holidays = new();

        // Act
        Cal.AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        // Assert
        Assert.That(Holidays.Count, Is.EqualTo(0));
    }
    #endregion

#pragma warning restore IDE0017 // Initialisierung von Objekten vereinfachen

}
