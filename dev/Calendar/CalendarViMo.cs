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
  
    public DateTime EffectiveDate 
    { 
        get => DateOfPlay.Date;
        protected set => DateOfPlay.Date = value;
    }


    /// <summary>Regular expression used to parse dates represented as strings (Bosparan calendar only)</summary>
    protected const string DayRegex = @"(?<day>\d{1,2})\.?"; // interprets the day in month
    protected const string MonthRegex = @"(?<month>[A-Za-z]*)"; // interprets the month
    protected const string YearRegex = @"(?<year>-?\d{1,4})"; // interprets the day in month
    protected const string ReckoningRegex = @"(?<reck>[vVbB]?\.?\s*[A-Za-z]*)"; // interprets the calendar designation
    protected const string BeforeReckoningRegex = @"(?<reck>[vVbB][\.\s]?\s*[bfBF]{2})$"; // interprets the calendar designation
    public const string DateRegex = @"^\s*" + DayRegex + @"\s*" + MonthRegex + @"\s*" + YearRegex + @"\s*" + ReckoningRegex + @"\s*$";


    public CalendarViMo(CalendarDB gameData, IDateOfPlay dateOfPlay)
    {
        GameData = gameData; // Inject
        DateOfPlay = dateOfPlay; // Inject

        Calendar = new();
    }


    #region SPECIFIC DATE

    /// <summary>
    /// Converts the given date to a string with the desired standard format.
    /// If no date is given, the current effective date will be used.
    /// </summary>
    /// <param name="Format">
    /// A <see href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings">
    /// standard format specifier</see> or on of these: x = "dd, dddd" and p = "dd. MMMM yyyy"</param>
    /// <param name="date">A date to convert or null to use the current date</param>
    /// <returns>A formatted date as string</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="FormatException"></exception>
    public string DateToString(char Format, DateTime? date = null)
    {
        DateTime Value = date ?? EffectiveDate;

        string weekDay = GameData.GetWeekday(Calendar.GetDayOfWeek(Value));
        string dayOfMonth = Calendar.GetDayOfMonth(Value).ToString(); // `.ToString` to avoid boxing
        string month = GameData.GetMonth(Calendar.GetMonth(Value));
        int year = Calendar.GetYear(Value);

        return Format switch
        {
            'd' => $"{dayOfMonth}.{Calendar.GetMonth(Value)}.{year}",
            'D' => $"{weekDay}, {dayOfMonth}. {month} {Math.Abs(year)} {GameData.GetFBReckoning(year)}",
            'm' or 'M' => $"{dayOfMonth}. {month}",
            's' => $"{year}-{Calendar.GetMonth(Value)}-{dayOfMonth}",
            'y' or 'Y' => $"{month} {Math.Abs(year)}",
            'p' => $"{dayOfMonth}. {month} {year}",
            'x' => $"{dayOfMonth}, {weekDay}",
            'g' or 'G' or 'f' or 'F' or 'o' or 'O' or 'r' or 'R' or 't' or 'T' or 'u' or 'U' 
                => throw new ArgumentException("Time formats not available"),
            _ => throw new FormatException("Unknown format specifier. Method does not support custom specifiers."),
        };
    }

    public string WeekdayShort => GameData.GetWeekdayAbbr(Calendar.GetDayOfWeek(EffectiveDate));
    public string Weekday => GameData.GetWeekday(Calendar.GetDayOfWeek(EffectiveDate));

    /// <summary>
    /// Get the current (effective) day of the month
    /// </summary>
    public int DayOfMonth => Calendar.GetDayOfMonth(EffectiveDate);

    /// <summary>
    /// Get the day of the month of the give date
    /// </summary>
    /// <param name="Date">A date</param>
    public int GetDayOfMonth(DateTime Date) => Calendar.GetDayOfMonth(Date);


    /// <summary>
    /// Get current (effective) month as long name
    /// </summary>
    public string Month => GameData.GetMonth(Calendar.GetMonth(EffectiveDate));

    /// <summary>
    /// Get current (effective) month as deity icon
    /// </summary>
    public string MonthIcon => MonthNr != 13 ? IconsFE.Concat(Month) : IconsFE.NamelessOne;

    /// <summary>
    /// Get current (effective) month number
    /// </summary>
    public int MonthNr => Calendar.GetMonth(EffectiveDate);

    /// <summary>
    /// Get the month number of a give date
    /// </summary>
    /// <param name="Date">A date</param>
    public int GetMonthNr(DateTime Date) => Calendar.GetMonth(Date);

    /// <summary>
    /// Get the current (effective) year
    /// </summary>
    public int Year => Calendar.GetYear(EffectiveDate);

    /// <summary>
    /// Get the year of a given date
    /// </summary>
    /// <param name="Date">A date</param>
    public int GetYear(DateTime Date) => Calendar.GetYear(Date);

    public string Reckoning => GameData.GetFBReckoning(Calendar.GetYear(EffectiveDate));

    public string Season => GameData.GetSeason(Calendar.GetMonth(EffectiveDate));
    public string SeasonIcon => GameData.GetSeasonId(Calendar.GetMonth(EffectiveDate)) switch
    {
        Shared.Season.Spring => Icons.Material.Sharp.LocalFlorist,
        Shared.Season.Summer => Icons.Material.Sharp.BrightnessHigh,
        Shared.Season.Autumn => Icons.Material.Sharp.Umbrella,
        Shared.Season.Winter => Icons.Material.Sharp.AcUnit,
        _ => throw new ArgumentOutOfRangeException($"Unknown season")
    };

    public string MoonPhaseName 
        => GameData.GetMoonPhaseName(GameData.GetMoonPhase(Calendar.GetMoonCycle(EffectiveDate)));

    public string MoonPhaseIcon => IconsFE.Moon(GameData.GetMoonPhase(Calendar.GetMoonCycle(EffectiveDate)));

    public void GotoTomorrow() => EffectiveDate = EffectiveDate.AddDays(1);

    public void GotoYesterday() => EffectiveDate = EffectiveDate.AddDays(-1);

    public void GotoEarthDate() => EffectiveDate = DateTime.Now;

    public void GotoDate(DateTime date) => EffectiveDate = date;

    public void GotoDate(int Day, int Month, int Year) 
        => EffectiveDate = Calendar.ToDateTime(Year, Month, Day, 0, 0, 0, 0);

    public DateTime ToDateTime(int Day, int Month, int Year)
        => Calendar.ToDateTime(Year, Month, Day, 0, 0, 0, 0);


    public (string, string)[] GetHolidays()
    {
        int Month = Calendar.GetMonth(EffectiveDate);
        DayOfWeek DoW = Calendar.GetDayOfWeek(EffectiveDate);
        MoonPhase Phase = GameData.GetMoonPhase(Calendar.GetMoonCycle(EffectiveDate));
        return GameData.GetHolidays(Month, DayOfMonth, DoW, Phase);
    }



    /// <summary>
    /// Parse a string of an Aventurian date and translate it into DateTime.
    /// </summary>
    /// <param name="dateStr">A string that can be interpreted as date.</param>
    /// <returns>A DateTime representation of the given date.</returns>
    /// <exception cref="FormatException"></exception>
    // https://stackoverflow.com/questions/56065683/regex-for-matching-dates-month-day-year-or-m-d-yy?msclkid=f2b2cd08c3af11ec8681f35279f46fe3
    public DateTime Parse(string dateStr)
    {
        const string DayName = "day";
        const string MonthName = "month";
        const string YearName = "year";
        const string ReckoningName = "reck";

        Regex rgx = new(DateRegex, RegexOptions.IgnoreCase);
        Match match = rgx.Match(dateStr);
        if (!match.Success)
            throw new FormatException($"String {dateStr} could not be interpreted as date");

        string xReckoning = match.Groups[ReckoningName].Value; // the eXtracted reckoning
        if (!string.IsNullOrEmpty(xReckoning))
            if (!xReckoning.Contains(Reckoning, StringComparison.InvariantCultureIgnoreCase)) // TODO: handle "BF" and "v. BF"
                throw new FormatException($"String {dateStr} could not be interpreted as date. Reckoning not recognized.");

        int Day = int.Parse(match.Groups[DayName].Value);
        int Year = int.Parse(match.Groups[YearName].Value);
        if (Regex.Match(xReckoning, BeforeReckoningRegex).Success) // if calendar is "b.FB"
            Year *= -1;

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

        DateTime result;
        try { result = Calendar.ToDateTime(Year, Month, Day, 0, 0, 0, 0); } 
        catch { throw new FormatException($"String {dateStr} could not be interpreted as date"); }
        return result;
    }

    #endregion




    #region General Calendar Data

    public string[] ListOfMonths => GameData.MonthNames;

    public int DaysInMonth(int year, int month)
    {
        return Calendar.GetDaysInMonth(year, month);
    }

    #endregion
}
