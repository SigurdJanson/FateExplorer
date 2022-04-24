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
    private readonly string[] MonthName = new[] { "Praios", "Rondra", "Efferd", "Travia", "Boron", "Hesinde", "Firun", "Tsa", "Phex", "Peraine", "Ingerimm", "Rahja", "Namenloser" };
    private readonly string[] MonthAbbr = new[] { "Pra", "Ron", "Eff", "Tra", "Bor", "Hes", "Fir", "Tsa", "Phx", "Per", "Ing", "Rah", "Nal" };
    private readonly string[] WeekDayName = new[] { "Praiostag", "Rondratag", "Feuertag", "Wassertag", "Windstag", "Erdtag", "Markttag" };
    private readonly string[] WeekDayAbbr = new[] { "Pr", "Ro", "Fe", "Wa", "Wi", "Er", "Ma" };
    private BosparanCalendar Calendar { get; set; }
    private DateTime CurrentDate { get; set; }
    private DateTime EffectiveDate { get; set; }

    public CalendarViMo()
    {
        CurrentDate = DateTime.Now;
        EffectiveDate = new DateTime(CurrentDate.Ticks);
        Calendar = new();
    }

    public const string DateRegex = @"^\s*(?<day>\d{1,2})\.?\s*(?<month>[A-Z]*)\s*(?<year>-?\d{1,4})\s*(?<reck>[A-Z]*)\s*$";


    public string WeekdayShort => WeekDayAbbr[(int)Calendar.GetDayOfWeek(EffectiveDate)];
    public string Weekday => WeekDayName[(int)Calendar.GetDayOfWeek(EffectiveDate)];

    public int DayOfMonth => Calendar.GetDayOfMonth(EffectiveDate);

    public string Month => MonthName[Calendar.GetMonth(EffectiveDate) - 1];
    public string MonthIcon => IconsFE.Concat(MonthName[Calendar.GetMonth(EffectiveDate) - 1]);
    public int MonthNr => Calendar.GetMonth(EffectiveDate);

    public int Year => Calendar.GetYear(EffectiveDate);

    public string Reckoning => "BF";

    public string Season => Calendar.GetMonth(EffectiveDate) switch
    {
        <= 2 => "Sommer",
        <= 5 => "Herbst",
        <= 8 => "Winter",
        <= 11 => "Frühjahr",
        <= 13 => "Sommer",
        _ => "Unbekannt"
    };


    public void GotoTomorrow() => EffectiveDate = EffectiveDate.AddDays(1);

    public void GotoYesterday() => EffectiveDate = EffectiveDate.AddDays(-1);

    public void GotoEarthDate() => EffectiveDate = DateTime.Now;

    public void GotoDate(DateTime date) => EffectiveDate = date;


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
            foreach (var m in this.MonthName)
                if (!m.StartsWith(MonthStr, StringComparison.InvariantCultureIgnoreCase))
                    ++Month;
                else
                    break;
            if (Month > MonthName.Length)
                throw new FormatException($"String {dateStr} could not be interpreted as date. Month could not be matched.");
        }

        return Calendar.ToDateTime(Year, Month, Day, 0, 0, 0, 0);
    }
    
}
