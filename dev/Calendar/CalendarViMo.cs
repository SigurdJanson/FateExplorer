using MudBlazor;
using System;

namespace FateExplorer.Calendar;


/// <summary>
/// 
/// </summary>
/// <remarks>In this early version the CalendarViMo can only be used with reckoning of Bosparan's Fall</remarks>
public class CalendarViMo
{
    private readonly string[] MonthAbbr = new[] { "Praios", "Rondra", "Efferd", "Travia", "Boron", "Hesinde", "Firun", "Tsa", "Phex", "Peraine", "Ingerimm", "Rahja", "Namenloser" };
    private readonly string[] WeekDay = new[] { "Praiostag", "Rondratag", "Feuertag", "Wassertag", "Windstag", "Erdtag", "Markttag" };
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

    public string WeekdayShort => WeekDayAbbr[(int)Calendar.GetDayOfWeek(EffectiveDate)];
    public string Weekday => WeekDay[(int)Calendar.GetDayOfWeek(EffectiveDate)];

    public int DayOfMonth => Calendar.GetDayOfMonth(EffectiveDate);

    public string Month => MonthAbbr[Calendar.GetMonth(EffectiveDate) - 1];
    public string MonthIcon => IconsFE.Concat(MonthAbbr[Calendar.GetMonth(EffectiveDate) - 1]);
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
}
