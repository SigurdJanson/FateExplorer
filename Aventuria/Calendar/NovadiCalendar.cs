using System.Globalization;
using System.Runtime.CompilerServices;

namespace Aventuria.Calendar;

/// <summary>
/// Novadi divide the year into 40 weeks called god names,
/// each consisting of nine days. The remaining five days
/// of the year are holy holidays called Rastullahellah,
/// <list type="bullet">
/// <item>The calendar describes a month as the time AFTER the latest Rastullahellah.
/// However, to make sure that months do not cross the end of the year, technically a month
/// has include the upcoming Rastullahellah.</item>
/// </list>
/// </summary>
// TODO:Add Year 0 leap to add methods
public class NovadiCalendar : DereCalendar
{
    public override bool HasYear0 => false;

    public override int[] Eras => [0, -1]; // remember, eras are listed in reverse order

    //protected const int DaysInYear = DereCalendar.DaysInDereYear; // inherited from DereCalendar
    protected const int DaysInMonth = 73;
    protected const int DaysInWeek = 9;
    protected const int MonthsInYear = 5;
    protected const int WeeksInMonth = 8;
    protected const int WeeksInYear = 40;

    //
    private const int MillisPerSecond = 1000;
    private const int MillisPerMinute = MillisPerSecond * 60;
    private const int MillisPerHour = MillisPerMinute * 60;

    //
    protected const int RastullahellahDays = 5; // Novadi holidays
    protected const int YearCorrectionFromGregorian = -977 - 760; // make this virtual to adapt all kinds of other calendars

    protected const int NewYearsDeltaDays = 143; // days from 1st Praios to 23. Boron (which is the day after the 5th Rastullahellah)
    protected const int NewYearsDeltaMonths = 1; // Novadi months 
    protected const int NewYearsDeltaDaysInMonth = 70;

    public readonly DateTime RastullahsAppearance = new(1737, 5, 23); // 23. Boron 1


    /// <inheritdoc/>
    public override CalendarAlgorithmType AlgorithmType => CalendarAlgorithmType.Unknown;



    /*
public override DateTime AddMilliseconds(DateTime time, double milliseconds);
public override DateTime AddSeconds(DateTime time, int seconds);
public virtual DateTime AddMinutes(DateTime time, int minutes);
public override DateTime AddHours(DateTime time, int hours);


*/

    public override int TwoDigitYearMax
    {
        get
        {
            if (_twoDigitYearMax == -1)
                _twoDigitYearMax = 359; // Default to 359 after Rastullahs appearance

            return _twoDigitYearMax;
        }
        set
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Calendar is read-only");
            _twoDigitYearMax = value;
        }
    }


    #region Add Methods

    public override DateTime AddDays(DateTime time, int days)
    {
        DateTime result = time.AddDays(days);
        result = result.AddDays(GetLeapDays(time, result)); // add leap days in between
        return IgnoreLeapDay(result, +1);
    }


    public override DateTime AddWeeks(DateTime time, int weeks)
    {
        int CurrentWeekOfYear = GetWeekOfYear(time) - 1; // Convert 1-based index to zero-based index by subtracting 1
        int Rashtullahellahs = (weeks + CurrentWeekOfYear) / WeeksInMonth - CurrentWeekOfYear / WeeksInMonth; // Rastullahellahs

        int DaysToAdd = weeks * DaysInWeek + Rashtullahellahs;
        DateTime result = time.AddDays(DaysToAdd * DaysInWeek);

        return IgnoreLeapDay(result, -1);
    }


    public override DateTime AddMonths(DateTime time, int months)
    {
        GregorianCalendar EarthCalendar = new(); // Needed to determine leap years

        int years = Math.DivRem(months, MonthsInYear, out int Leftover);
        DateTime result = time.AddYears(years);

        if (Leftover != 0) // now between 0 and 4 (i.e. MonthsInYear-1)
        {
            time = result; // store intermediate result
            result = time.AddDays(Leftover * DaysInMonth);
            // handle leap day if there is one in BETWEEN current date and target date
            if (time.Month <= EarthCalendar.GetLeapMonth(result.Year) && result.Month > EarthCalendar.GetLeapMonth(result.Year))
            {
                result = result.AddDays(1); // add leap day that does not exist in Aventuria
            }
        }

        return IgnoreLeapDay(result, -1);
    }


    public override DateTime AddYears(DateTime time, int years)
    {
        DateTime result = time.AddYears(years);
        return IgnoreLeapDay(result, -1);
    }

    #endregion


    #region Date Analysis
    // 
    public override int GetDayOfMonth(DateTime time) => (GetDayOfYear(time) - 1) % DaysInMonth + 1; // since last holiday


    public override DayOfWeek GetDayOfWeek(DateTime time) // TODO: Big problem becuase the DayOfWeek enum has only 7 days
    {
        int DayOfMonth = GetDayOfMonth(time) - 1;
        return DayOfMonth == 0 ? (DayOfWeek)1 : (DayOfWeek)((DayOfMonth % DaysInWeek) + 1);
    }


    /// <inheritdoc/>
    public override int GetDayOfYear(DateTime time)
    {
        var EarthCal = CultureInfo.InvariantCulture.Calendar;

        int Day = time.DayOfYear - NewYearsDeltaDays + 1;
        if (EarthCal.IsLeapYear(time.Year))
            if (time.DayOfYear > 31 + 28) // days Jan. + Feb.
                Day -= 1;
        // move New Years to 23. Boron
        return Day > 0 ? Day : DaysInDereYear + Day;
    }

    /// <inheritdoc/>
    /// <param name="rule">Is ignored in this reckoning.</param>
    /// <param name="firstDayOfWeek">Is ignored in this reckoning.</param>
    public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule = CalendarWeekRule.FirstDay, DayOfWeek firstDayOfWeek = DayOfWeek.Thursday)
    {
        int DayOfYear = GetDayOfYear(time);
        int HolidaysSinceNewYear = DayOfYear / DaysInMonth;
        return ((DayOfYear - 1) - HolidaysSinceNewYear) / DaysInWeek + 1; // use zero-based index for divisions
    }


    public override int GetMonth(DateTime time) => (GetDayOfYear(time)-1) / DaysInMonth + 1; // use zero-based index for divisions


    public override int GetYear(DateTime time)
    {
        int Year = time.Year + YearCorrectionFromGregorian;
        if (time.DayOfYear >= NewYearsDeltaDays)
            Year++;
        return Year > 0 ? Year : Year -1;
    }
         

    public override int GetEra(DateTime time)
    { 
        return GetYear(time) > 0 ? Eras[0] : Eras[1];
    }

    /// <summary>
    /// Determines if the given date is a Rastullahellah, i.e. one of the 5 holidays between the months
    /// </summary>
    /// <param name="time">The date to check</param>
    public bool IsRastullahellah(DateTime time)
    {
        int DayOfYear = GetDayOfYear(time);
        return DayOfYear % DaysInMonth == 0;
    }

    #endregion



    #region Static Calendar Info
    public override int GetDaysInMonth(int year, int month) => DaysInMonth;
    public override int GetDaysInMonth(int year, int month, int era) => DaysInMonth;

    public override int GetDaysInYear(int year) => DaysInDereYear;
    public override int GetDaysInYear(int year, int era) => DaysInDereYear;

    public override int GetMonthsInYear(int year) => MonthsInYear;
    public override int GetMonthsInYear(int year, int era) => MonthsInYear;

    #endregion



    public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        => ToDateTime(year, month, day, hour, minute, second, millisecond, Eras[CurrentEra]);
    

    public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
    {
        CheckArgumentOutOfRange(day, 1, DaysInMonth);
        CheckArgumentOutOfRange(month, 1, MonthsInYear);
        CheckArgumentOutOfRange(era, -1, 0);
        if (era == 0 && year < 0 || era == -1 && year > 0)
            throw new ArgumentOutOfRangeException(nameof(year), year, "The year does not fit in that era");
        ArgumentOutOfRangeException.ThrowIfZero(year, nameof(year));

        int EarthYear = year - YearCorrectionFromGregorian;
        if (month * DaysInMonth + day < NewYearsDeltaDays) EarthYear--;
        CheckArgumentOutOfRange(EarthYear, 1 + YearCorrectionFromGregorian, 9999 - YearCorrectionFromGregorian);


        DateTime time = AddMonths(RastullahsAppearance, (year - 1) * MonthsInYear + month - 1);
        DateTime result = time.AddDays(day - 1);

        // handle leap day if there is one in BETWEEN current date and target date
        var EarthCalendar = CultureInfo.InvariantCulture.Calendar;
        if (time.Month <= EarthCalendar.GetLeapMonth(result.Year) && result.Month > EarthCalendar.GetLeapMonth(result.Year))
            result = result.AddDays(1); // add leap day that does not exist in Aventuria
        result = IgnoreLeapDay(result, -1);
        result = AddMilliseconds(result, hour * MillisPerHour + minute * MillisPerMinute + second * MillisPerSecond + millisecond);
        return result;
    }


}
