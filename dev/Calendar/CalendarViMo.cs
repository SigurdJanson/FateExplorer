using FateExplorer.GameData;
using FateExplorer.Shared;
using MudBlazor;
using System;
using System.Text.RegularExpressions;

namespace FateExplorer.Calendar;


/// <summary>
/// 
/// </summary>
/// <remarks>In this early version the CalendarViMo can only be used with reckoning of Bosparan's Fall</remarks>
public class CalendarViMo
{
    private CalendarDB GameData { get; set; } // injected
    private IDateOfPlay DateOfPlay { get; set; } // injected



    private BosparanCalendar Calendar { get; set; }
    private DateTime CurrentDate { get; set; }
    private DateTime EffectiveDate { get => DateOfPlay.Date; set => DateOfPlay.Date = value; }

    /// <summary>Regular expression used to parse dates represented as strings (Bosparan calendar only)</summary>
    public const string DateRegex = @"^\s*(?<day>\d{1,2})\.?\s*(?<month>[A-Z]*)\s*(?<year>-?\d{1,4})\s*(?<reck>[A-Z]*)\s*$";


    public CalendarViMo(CalendarDB gameData, IDateOfPlay dateOfPlay)
    {
        GameData = gameData; // Inject
        DateOfPlay = dateOfPlay; // Inject

        CurrentDate = DateTime.Now;
        EffectiveDate = new DateTime(CurrentDate.Ticks);
        Calendar = new();
    }


    public DateTime GetDate() => EffectiveDate;

    public string GetDateAsString(bool Long, DateTime? date = null)
    {
        DateTime Value = date ?? EffectiveDate;
        if (Long)
        {
            string weekDay = GameData.GetWeekday(Calendar.GetDayOfWeek(Value));
            int dayOfMonth = Calendar.GetDayOfMonth(Value);
            string month = GameData.GetMonth(Calendar.GetMonth(Value));
            int year = Calendar.GetYear(Value);
            return $"{weekDay}, {dayOfMonth}. {month} {year}";
        }
        else
        {
            string weekDay = GameData.GetWeekday(Calendar.GetDayOfWeek(Value));
            int dayOfMonth = Calendar.GetDayOfMonth(Value);
            return $"{dayOfMonth}, {weekDay}";
        }
    }

    public string WeekdayShort => GameData.GetWeekdayAbbr(Calendar.GetDayOfWeek(EffectiveDate));
    public string Weekday => GameData.GetWeekday(Calendar.GetDayOfWeek(EffectiveDate));

    public int DayOfMonth => Calendar.GetDayOfMonth(EffectiveDate);

    public string Month => GameData.GetMonth(Calendar.GetMonth(EffectiveDate));

    public string MonthIcon => IconsFE.Concat(Month);

    public int MonthNr => Calendar.GetMonth(EffectiveDate);

    public int Year => Calendar.GetYear(EffectiveDate);

    public string Reckoning => "BF";

    public string Season => GameData.GetSeason(Calendar.GetMonth(EffectiveDate));
    public string SeasonIcon => GameData.GetSeasonId(Calendar.GetMonth(EffectiveDate)) switch
    {
        global::Season.Spring => Icons.Material.Sharp.LocalFlorist,
        global::Season.Summer => Icons.Material.Sharp.BrightnessHigh,
        global::Season.Autumn => Icons.Material.Sharp.Umbrella,
        global::Season.Winter => Icons.Material.Sharp.AcUnit,
        _ => throw new ArgumentOutOfRangeException($"Unknown season")
    };

    public string MoonPhaseName 
        => GameData.GetMoonPhaseName(GameData.GetMoonPhase(Calendar.GetMoonPhase(EffectiveDate)));

    public string MoonPhaseIcon => IconsFE.Moon(GameData.GetMoonPhase(Calendar.GetMoonPhase(EffectiveDate)));

    public void GotoTomorrow() => EffectiveDate = EffectiveDate.AddDays(1);

    public void GotoYesterday() => EffectiveDate = EffectiveDate.AddDays(-1);

    public void GotoEarthDate() => EffectiveDate = DateTime.Now;

    public void GotoDate(DateTime date) => EffectiveDate = date;


    public (string, string)[] GetHolidays() 
        => GameData.GetHolidays(Calendar.GetMonth(EffectiveDate), DayOfMonth);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateStr"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    // https://stackoverflow.com/questions/56065683/regex-for-matching-dates-month-day-year-or-m-d-yy?msclkid=f2b2cd08c3af11ec8681f35279f46fe3
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Inlinevariablendeklaration", Justification = "Readibility")]
    public DateTime Parse(string dateStr)
    {
        //string pattern = @"^\s * (? 'day'\d{ 1,2})\.?\s * (? 'month'[A - Za - z] *)\s * (? 'year'\d{ 1,4})\s * (? 'Reck'[A - Za - z] *)\s *$";
        const string DayName = "day";
        const string MonthName = "month";
        const string YearName = "year";
        const string ReckoningName = "reck";

        Regex rgx = new(DateRegex, RegexOptions.IgnoreCase);
        Match match = rgx.Match(dateStr);
        if (!match.Success)
            throw new FormatException($"String {dateStr} could not be interpreted as date");

        string Reckoning = match.Groups[ReckoningName].Value;
        if (!string.IsNullOrEmpty(Reckoning))
            if (!Reckoning.Equals("bf", StringComparison.InvariantCultureIgnoreCase))
                throw new FormatException($"String {dateStr} could not be interpreted as date. Reckoning not recognized.");

        int Day = int.Parse(match.Groups[DayName].Value);
        int Year = int.Parse(match.Groups[YearName].Value);

        int Month;
        string MonthStr = match.Groups[MonthName].Value;
        if (!int.TryParse(MonthStr, out Month))
        {
            Month = 1;
            foreach (var m in GameData.MonthNames)
                if (!m.StartsWith(MonthStr, StringComparison.InvariantCultureIgnoreCase))
                    ++Month;
                else
                    break;
            if (Month > GameData.MonthNames.Length)
                throw new FormatException($"String {dateStr} could not be interpreted as date. Month could not be matched.");
        }

        return Calendar.ToDateTime(Year, Month, Day, 0, 0, 0, 0);
    }
    
}
