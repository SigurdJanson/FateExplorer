using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData;

public class CalendarDB
{
    [JsonPropertyName("generic")]
    public Generic Generic { get; set; }

    public string GetWeekday(DayOfWeek day) => Generic.WeekDays.Find(i => (int)day == i.Earthid).Name;
    public string[] WeekdayNames => Generic.WeekDays.OrderBy(i => i.Earthid).Select(i => i.Name).ToArray();
    public string[] WeekdayAbbrs => Generic.WeekDays.OrderBy(i => i.Earthid).Select(i => i.Abbr).ToArray();

    public string[] MonthNames => Generic.Month.Select(i => i.Name).ToArray();
    public string[] MonthAbbr => Generic.Month.Select(i => i.Abbr).ToArray();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="MonthId"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    public string GetMoonPhaseName(MoonPhase phase)
    {
        return Generic.Moonphase[(int)phase].Name;
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
}


public class ReckoningEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

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



