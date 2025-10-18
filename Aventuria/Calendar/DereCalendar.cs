using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Aventuria.Calendar;



public abstract class DereCalendar : System.Globalization.Calendar
{
    #region Exceptions and Messages

    internal const string ArgumentOutOfRange_BadYearMonthDay = "The year, month, and day parameters describe an un-representable DateTime.";
    internal const string ArgumentOutOfRange_BadYear = "The year parameter must be between {0} and {1}";
    internal const string ArgumentOutOfRange_BadMonth = "The month parameter must be between {0} and {1}";
    internal const string ArgumentOutOfRange_BadDay = "The day of month parameter must be between {0} and {1}";
    internal const string ArgumentOutOfRange_InvalidEraValue = "The era parameter is not in the valid range.";
    internal const string ArgumentOutOfRange_HasNoYear0 = "The calendar does not support a year zero";

    internal static void CheckArgumentOutOfRange_BadDay(int value, int min, int max, [CallerArgumentExpression(nameof(value))] string? argument = null)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(argument, value, string.Format(ArgumentOutOfRange_BadDay, min, max));
    }
    internal static void CheckArgumentOutOfRange_BadMonth(int value, int min, int max, [CallerArgumentExpression(nameof(value))] string? argument = null)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(argument, value, string.Format(ArgumentOutOfRange_BadMonth, min, max));
    }
    internal static void CheckArgumentOutOfRange_BadYear(int value, int min, int max, [CallerArgumentExpression(nameof(value))] string? argument = null)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(argument, value, string.Format(ArgumentOutOfRange_BadYear, min, max));
    }
    
    [DoesNotReturn]
    internal static void ThrowYear0Exception() => throw new ArgumentOutOfRangeException("year", ArgumentOutOfRange_HasNoYear0);

    #endregion



    public virtual bool HasYear0 => true;
    protected const int DaysInDereYear = 365;



    /// <summary>
    /// Returns the number of full leap days (Feb 29) between two dates.
    /// </summary>
    /// <param name="start">Start date of time period</param>
    /// <param name="end">End date of time period</param>
    /// <returns><list type="bullet"><item>The number of leap days between start and end</item>
    /// 0 when start to end covers less than a whole day; 0 when </list></returns>
    public static int GetLeapDays(DateTime start, DateTime end)
    {
        if (end - start < TimeSpan.FromDays(1)) return 0; // No full day

        int LeapDays = 0;
        int startYear = start.Year; // search exclusive the start 
        int endYear = end.Year;     // ... and end year

        if (startYear == endYear)
        {
            if (DateTime.IsLeapYear(startYear))
                return (start.Month <= 2 && (end.Month >= 3 || (end.Day == 29 && end.Month == 2))) ? 1 : 0;
            else
                return 0; // same year but not a leap year
        }
        else
        {
            // Count leap day INside start/end year if applicable
            if (start.Month <= 2 && DateTime.IsLeapYear(startYear))
            {
                LeapDays++; // is leap year and month is Jan/Feb
            }
            if ((end.Month > 2 || (end.Month == 2 && end.Day == 29)) && DateTime.IsLeapYear(endYear))
            {
                LeapDays++; // is leap year and month is after Feb or exactly Feb 29
            }

            // Count leap days in BETWEEN start and end year
            startYear += 1; // search exclusive the start 
            endYear -= 1;   // ... and end year

            if (endYear > startYear) // start.Year <= end.Year - 3
            {
                LeapDays += (endYear / 4) - (endYear / 100) + (endYear / 400);
                LeapDays -= (startYear / 4) - (startYear / 100) + (startYear / 400);
            }

            return LeapDays;
        }
    }


    public static DateTime AddTicks(DateTime time, long ticks)
    {
        time = IgnoreLeapDay(time, -1);
        DateTime result = time.AddTicks(ticks);
        return result.AddDays(GetLeapDays(time, result) * -1); // remove leap days in between
    }

    /// <summary>
    /// Corrects a <paramref name="time"/> to compensate for leap days in the Gregorian calendar.
    /// When being converted to the Dere calendar, leap days (Feb 29) do not exist and must be skipped.
    /// Depending on the calculation direction, the date can be moved forward or backward.
    /// </summary>
    /// <param name="time">The point in time to correct for leap days.</param>
    /// <param name="direction">The direction to correct. Theoretically, any integer can be used but only +/- 1 makes kinda sense.</param>
    /// <returns>The time adjusted for leap days.</returns>
    public static DateTime IgnoreLeapDay(DateTime time, int direction)
    {
        var EarthCalendar = CultureInfo.InvariantCulture.Calendar; // Needed to determine leap years
        if (EarthCalendar.IsLeapDay(time.Year, time.Month, time.Day))
            return time.AddDays(direction); // skip leap day that does not exist in Aventuria
        return time;
    }


    /// <summary>
    /// Between check <paramref name="start"/> <![CDATA[<=]]> <paramref name="time"/> <![CDATA[<=]]> <paramref name="end"/>.
    /// If <paramref name="start"/> is after <paramref name="end"/> time, the values are swapped.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="start">DateTime object specifying the start time of the interval.</param>
    /// <param name="end">DateTime object specifying the start time of the interval.</param>
    /// <returns></returns>
    public static bool IsBetween(DateTime time, DateTime start, DateTime end)
    {
        if (start > end)
            (end, start) = (start, end);

        return time >= start && time <= end;
    }


    /// <summary>
    /// Determines the ABSOLUTE difference of FULL years between two dates.
    /// Leap days are ignored; the 29th of February is treated as if it was the 28th
    /// (conforming with Aventurian calendars).
    /// </summary>
    /// <param name="Early">The earlier date</param>
    /// <param name="Late">THe later date</param>
    /// <returns>Difference in full years (absolute value >= 0)</returns>
    public static int AbsDeltaInYears(DateTime Early, DateTime Late)
    {
        if (Early > Late) throw new ArgumentException("'Late' must be greater or equal to 'Early'");

        // Aventurian calendars do not have leap years: check for Feb, 29th
        const int Feb = 2;
        const int LeapDay = 29;
        if (Early.Day == LeapDay && Early.Month == Feb) Early = Early.AddDays(-1);
        if (Late.Day == LeapDay && Late.Month == Feb) Late = Late.AddDays(-1);

        int Delta = Late.Year - Early.Year;

        // correction if the one year is not a full year
        if (Late.Month < Early.Month || (Late.Month == Early.Month && Late.Day < Early.Day))
            Delta--;

        return Delta;
    }


    /// <summary>
    /// Calculate the difference in days between two dates. For this leap years are ignored.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="reference"></param>
    /// <returns></returns>
    public static int DeltaInDays(DateTime time, DateTime reference)
    {
        // Needed to determine leap years
        var EarthCalendar = CultureInfo.InvariantCulture.Calendar;

        DateTime Early = time <= reference ? time : reference;
        DateTime Late = time > reference ? time : reference;
        int Years = AbsDeltaInYears(Early, Late);

        // skip leap days by using whole years
        int Days = Years * DaysInDereYear;
        // add remaining interval
        DateTime EarlyPlusYears = Early.AddYears(Years);
        Days += (Late - EarlyPlusYears).Days;
        // One leap day may be left, remove if necessary
        if (EarthCalendar.IsLeapYear(EarlyPlusYears.Year) && EarlyPlusYears.DayOfYear < 31 + 29)
            Days--;
        else
        {
            if (EarthCalendar.IsLeapYear(Late.Year) && Late.DayOfYear > 31 + 28)
                Days--;
        }
        return Days;
    }



    // 0-11, 0 is new moon (dead mada), 3 is half, 6 is full moon (wheel),
    // 9 is half, 11 the phase before new moon
    /// <summary>
    /// Computes the position in the moon cycle given a date.
    /// </summary>
    /// <returns>0 is the first day of the moon cycle (new moon; dead Mada). 
    /// 14 is the middle of the cycle (full moon; wheel). 27 is the cycle's end.</returns>
    public static int GetMoonCycle(DateTime time)
    {
        // the 14.04.2022 on Earth; in Aventuria it's the 17th (16! with 1. index == 0) day of the moon phase
        DateTime Reference = new(2022, 4, 14);
        const int MoonPhaseRef = 16;
        const int MoonCycle = 28;

        int Days = DeltaInDays(time, Reference);
        int Offset = Days % MoonCycle;

        int Result;
        if (time >= Reference)
            Result = Offset + MoonPhaseRef;
        else
            Result = (Offset == 0 ? 0 : MoonCycle - Offset) + MoonPhaseRef;

        if (Result >= MoonCycle) Result -= MoonCycle;
        return Result;
    }


    #region Leap Year Methods are obsolete on Dere

    public override int GetLeapMonth(int year) => 0;
    public override int GetLeapMonth(int year, int era) => 0;
    public override bool IsLeapDay(int year, int month, int day) => false;
    public override bool IsLeapDay(int year, int month, int day, int era) => false;
    public override bool IsLeapMonth(int year, int month) => false;
    public override bool IsLeapMonth(int year, int month, int era) => false;
    public override bool IsLeapYear(int year) => false;
    public override bool IsLeapYear(int year, int era) => false;

    #endregion


}
