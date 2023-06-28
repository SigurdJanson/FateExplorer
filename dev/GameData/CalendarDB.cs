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
    /// <param name="DaysFromDeath">The distance from the last new moon (i.e. Mada's Death), 
    /// i.e. the position in the moon cycle.</param>
    /// <returns></returns>
    public MoonPhase GetMoonPhase(int DaysFromDeath)
    {
        if (Math.Abs(DaysFromDeath) >= Generic.MoonphaseDays.Count)
            throw new ArgumentOutOfRangeException(nameof(DaysFromDeath));
        if (DaysFromDeath < 0) DaysFromDeath = Generic.MoonphaseDays.Count - DaysFromDeath + 1;

        return (MoonPhase)Generic.MoonphaseDays[DaysFromDeath] - 1;
    }


    /// <summary>
    /// Get the name that describes the given moon phase.
    /// </summary>
    /// <param name="phase">Moon phase</param>
    /// <returns>A string with the localized name</returns>
    public string GetMoonPhaseName(MoonPhase phase)
    {
        return Generic.Moonphase.Find(i => i.Iid-1 == (int)phase).Name;
    }

    public (string Name, string Descr)[] GetHolidays(int Month, int Day, DayOfWeek WeekDay, MoonPhase Phase)
    {
        if (Month < 1 || Month > 13)
            throw new ArgumentOutOfRangeException(nameof(Month), Month, "Months must be 1-13");

        List<(string Name, string Descr)> Holidays = new();
        AddFixedHolidays(Month, Day, ref Holidays);
        AddLunarHolidays(Month, Day, Phase, ref Holidays);
        AddWeekHolidays(Month, Day, WeekDay, ref Holidays);

        return Holidays.ToArray();
    }


    /// <summary>
    /// Get a list of fixed holidays held on a given day
    /// </summary>
    /// <param name="Month">Month according to FB reckoning</param>
    /// <param name="Day">Day in month according to FB reckoning</param>
    /// <returns>A list of tupels with name and description of the holidays</returns>
    public void AddFixedHolidays(int Month, int Day, ref List<(string Name, string Descr)> Holidays)
    {
        if (Month < 1 || Month > 13) 
            throw new ArgumentOutOfRangeException(nameof(Month), Month, "Months must be 1-13");

        int DayOfYear = Day + (Month - 1) * 30;

        foreach (var h in Generic.FixedHoliday)
        {
            if (Month < h.Month || Month > h.Month + 1) continue; // only same or next month (because of carry over effect)

            int DayStart = h.Day + (h.Month - 1) * 30;
            int DayEnd = DayStart + h.Duration - 1;
            if (DayOfYear >= DayStart && DayOfYear <= DayEnd)
                Holidays.Add((h.Name, h.Descr));
        }
    }


    /// <summary>
    /// Find the lunar holidays for a given date.
    /// </summary>
    /// <param name="Month">Search for holidays in the month of this date</param>
    /// <param name="Day">Search for holidays in the day of this date</param>
    /// <param name="Phase"></param>
    /// <param name="Holidays"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <remarks>The method does not support lunar phases counting from the end of the month. 
    /// Holidays longer than a single day aren't supported either.</remarks>
    public void AddLunarHolidays(int Month, int Day, MoonPhase Phase, ref List<(string Name, string Descr)> Holidays)
    {
        if (Month < 1 || Month > 13)
            throw new ArgumentOutOfRangeException(nameof(Month), Month, "Months must be 1-13");

        // Lunar holidays
        foreach (var h in Generic.LunarHoliday)
        {
            if (Month != h.Month) continue;
            if (h.MoonPhase == (int)Phase)
                if (Day < Generic.MoonphaseDays.Count * h.Day && Day > Generic.MoonphaseDays.Count * (h.Day-1))
                    Holidays.Add((h.Name, h.Descr));
        }
    }


    /// <summary>
    /// Add holidays to th elist which are defined by the n-th week day of the month.
    /// </summary>
    /// <param name="Month"></param>
    /// <param name="Day"></param>
    /// <param name="WeekDay"></param>
    /// <param name="Holidays"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <remarks>The method does not support Weekdays counting from the end of the month. 
    /// Holidays longer than a single day aren't support either when counting backwards.</remarks>
    public void AddWeekHolidays(int Month, int Day, DayOfWeek WeekDay, ref List<(string Name, string Descr)> Holidays)
    {
        if (Month < 1 || Month > 13)
            throw new ArgumentOutOfRangeException(nameof(Month), Month, "Months must be 1-13");
        
        // `DayOfWeek` (0-6) is an Earth thing - the data base uses another notation (1-7)
        int DereWeekDay = Generic.WeekDays.Find(d => d.Earthid == (int)WeekDay).Iid;

        // Weekday-based movable holidays
        int WeekLen = Generic.WeekDays.Count;
        foreach (var h in Generic.WeekHoliday)
        {
            if (Month < h.Month) continue; // only same or next month (because of carry over effect)

            if (h.Day >= 0) // n-th week day of month
            {
                if (Month > h.Month + 1) continue; // only same or next month (because of carry over effect)
                if (h.WeekDay == DereWeekDay) // first day of interval
                {
                    if (((Day-1) / WeekLen) + 1 == h.Day)
                        Holidays.Add((h.Name, h.Descr));
                }
                else if (h.Duration > 1) // go backwards and try to find the required week day
                {
                    int TimeToBefore = DereWeekDay - h.WeekDay;
                    if (TimeToBefore < 1) TimeToBefore = WeekLen + TimeToBefore;
                    if (Day - TimeToBefore <= 0) continue; // skip if first holiday of this period is in previous month

                    if (TimeToBefore < h.Duration)
                    {
                        if ((Day - TimeToBefore - 1) / WeekLen + 1 == h.Day) // is the the n-th weekday of the month?
                            Holidays.Add((h.Name, h.Descr));
                    }
                }
            }
            else // Last n-th week day of month
            {
                if (Month != h.Month) continue; // only same or next month (because of carry over effect)

                if (h.WeekDay == DereWeekDay) // first day of interval
                {
                    int MonthLen = Generic.Month[Month].DaysInMonth;
                    if (Day > MonthLen + (WeekLen * h.Day) && Day <= MonthLen + (WeekLen * (h.Day + 1)))
                        Holidays.Add((h.Name, h.Descr));
                }
                // TODO ###################################################
            }
        }
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
    public List<HolidayEntry> FixedHoliday { get; set; }

    [JsonPropertyName("weekholiday")]
    public List<HolidayWeekdayEntry> WeekHoliday { get; set; }

    [JsonPropertyName("lunarholiday")]
    public List<HolidayLunarEntry> LunarHoliday { get; set; }


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

public class HolidayWeekdayEntry
{
    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("weekday")]
    public int WeekDay { get; set; }

    [JsonPropertyName("nthday")]
    public int Day { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("descr")]
    public string Descr { get; set; }
}

public class HolidayLunarEntry
{
    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("phase")]
    public int MoonPhase { get; set; }

    [JsonPropertyName("nthday")]
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



