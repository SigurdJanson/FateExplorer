using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData;

public class CalendarDB
{
    [JsonPropertyName("generic")]
    public Generic Generic { get; set; }


    private const string FBReckoningId = "FB";

    /// <summary>
    /// Reuturn the weekday name to a given day number of the week.
    /// </summary>
    /// <param name="day">The day's number in the week starting with 1 being a Windsday (1-7)</param>
    /// <returns>The localised week day name</returns>
    public string GetWeekday(DayOfWeek day) => Generic.WeekDays.Find(i => (int)day == i.Earthid).Name;

    /// <summary>
    /// Reuturn the abbreaviated weekday name to a given day number of the week.
    /// </summary>
    /// <param name="day">The day's number in the week starting with 1 being a Windsday (1-7)</param>
    /// <returns>The localised week day abbreviation</returns>
    public string GetWeekdayAbbr(DayOfWeek day) => Generic.WeekDays.Find(i => (int)day == i.Earthid).Abbr;

    /// <summary>
    /// Get all the week day names as array
    /// </summary>
    public string[] WeekdayNames => Generic.WeekDays.OrderBy(i => i.Earthid).Select(i => i.Name).ToArray();

    /// <summary>
    /// Get all the week day abbreviations as array
    /// </summary>
    public string[] WeekdayAbbrs => Generic.WeekDays.OrderBy(i => i.Earthid).Select(i => i.Abbr).ToArray();

    /// <summary>
    /// The the month's name tht fits the number in the year.
    /// </summary>
    /// <param name="month">The number of the month in the year (1-13)</param>
    /// <returns>The localised month name</returns>
    public string GetMonth(int month) => Generic.Month.Find(i => month == i.Iid).Name;

    /// <summary>
    /// Get all the month names as array
    /// </summary>
    public string[] MonthNames => Generic.Month.Select(i => i.Name).ToArray();

    /// <summary>
    /// Get all the month abbreviations as array
    /// </summary>
    public string[] MonthAbbr => Generic.Month.Select(i => i.Abbr).ToArray();

    /// <summary>
    /// Get the string identifying the Middenrealm calendar of the Fall Bosparans.
    /// </summary>
    /// <param name="year">The year</param>
    /// <returns>An abbreviated string that identifies a year of the Fall Bosparans</returns>
    public string GetFBReckoning(int year)
    {
        var Default = Generic.Reckoning.Find(i => i.Id == FBReckoningId);
        if (year < 0)
            return Default?.LabelNeg ?? string.Empty;
        else
            return Default?.LabelPos ?? string.Empty;
    }


    /// <summary>
    /// Returns the id of the season the given month lies in.
    /// </summary>
    /// <param name="MonthId">The number of the month in the year (1-13)</param>
    /// <returns>Id of the season</returns>
    public Season GetSeasonId(int MonthId) => (Season)Generic.Month.Find(i => i.Iid == MonthId).SeasonId;
   

    /// <summary>
    /// Returns the name of the season the given month lies in.
    /// </summary>
    /// <param name="MonthId">The number of the month in the year (1-13)</param>
    /// <returns>Localised name of the season</returns>
    public string GetSeason(int MonthId)
    {
        int SeasonId = Generic.Month.Find(i => i.Iid == MonthId).SeasonId;
        return Generic.Season.Find(i => i.Iid == SeasonId).Name;
    }




    /// <summary>
    /// Get the moon phase based on an offset of days
    /// </summary>
    /// <param name="DaysFromDeath">The distance from the last new moon (i.e. Mada's Death)</param>
    /// <returns></returns>
    public MoonPhase GetMoonPhase(int DaysFromDeath)
    {
        if (Math.Abs(DaysFromDeath) >= Generic.MoonphaseDays.Count)
            throw new ArgumentOutOfRangeException(nameof(DaysFromDeath));
        if (DaysFromDeath < 0) DaysFromDeath = Generic.MoonphaseDays.Count - DaysFromDeath + 1;

        return (MoonPhase)Generic.MoonphaseDays[DaysFromDeath];
    }


    /// <summary>
    /// Get the name that describes the given moon phase.
    /// </summary>
    /// <param name="phase">Moon phase</param>
    /// <returns>A string with the localized name</returns>
    public string GetMoonPhaseName(MoonPhase phase)
    {
        return Generic.Moonphase.Find(i => i.Iid == (int)phase).Name;
    }



    /// <summary>
    /// Get a list of fixed holidays held on a given day
    /// </summary>
    /// <param name="Month">Month according to FB reckoning</param>
    /// <param name="Day">Day in month according to FB reckoning</param>
    /// <returns>A list of tupels with name and description of the holidays</returns>
    public (string Name, string Descr)[] GetHolidays(int Month, int Day)
    {
        if (Month < 1 || Month > 13) 
            throw new ArgumentOutOfRangeException(nameof(Month), Month, "Months must be 1-13");

        int DayOfYear = Day + (Month - 1) * 30;

        List<(string Name, string Descr)> Holidays = new();
        foreach (var h in Generic.Holiday)
        {
            int DayStart = h.Day + (h.Month - 1) * 30;
            int DayEnd = DayStart + h.Duration - 1;
            if (DayOfYear >= DayStart && DayOfYear <= DayEnd)
                Holidays.Add((h.Name, h.Descr));
        }
        return Holidays.ToArray();
    }
}



public class Generic
{
    [JsonPropertyName("week")]
    public List<WeekdayEntry> WeekDays { get; set; }

    [JsonPropertyName("month")]
    public List<MonthEntry> Month { get; set; }

    [JsonPropertyName("reckoning")]
    public List<ReckoningEntry> Reckoning { get; set; }

    [JsonPropertyName("season")]
    public List<SeasonEntry> Season { get; set; }

    [JsonPropertyName("holiday")]
    public List<HolidayEntry> Holiday { get; set; }

    [JsonPropertyName("moonphasedays")]
    public List<int> MoonphaseDays { get; set; }

    [JsonPropertyName("moonphase")]
    public List<MoonphaseEntry> Moonphase { get; set; }
}


public class WeekdayEntry
{
    [JsonPropertyName("iid")]
    public int Iid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("abbr")]
    public string Abbr { get; set; }

    [JsonPropertyName("earthid")]
    public int Earthid { get; set; }

    [JsonPropertyName("earthday")]
    public string Earthday { get; set; }
}


public class MonthEntry
{
    [JsonPropertyName("iid")]
    public int Iid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("abbr")]
    public string Abbr { get; set; }

    [JsonPropertyName("seasoniid")]
    public int SeasonId { get; set; }

    [JsonPropertyName("days")]
    public int DaysInMonth { get; set; }

}


public class ReckoningEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("txt+")]
    public string LabelPos { get; set; }

    [JsonPropertyName("txt-")]
    public string LabelNeg { get; set; }

    [JsonPropertyName("correction")]
    public int Correction { get; set; }

    [JsonPropertyName("has0")]
    public bool HasYear0 { get; set; }
}


public class SeasonEntry
{
    [JsonPropertyName("iid")]
    public int Iid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}


public class HolidayEntry
{
    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("descr")]
    public string Descr { get; set; }
}


public class MoonphaseEntry
{
    [JsonPropertyName("iid")]
    public int Iid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}



