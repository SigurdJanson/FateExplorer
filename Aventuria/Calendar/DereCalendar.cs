using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Aventuria.Calendar;


/// <summary>
/// The base class for all Dere calendars. It provides methods for calendars without leap years that still match
/// earth years.
/// </summary>
public abstract class DereCalendar : System.Globalization.Calendar
{
    #region Exceptions and Messages

    internal const string ArgumentOutOfRange_BadYearMonthDay = "The year, month, and day parameters describe an un-representable DateTime.";
    internal const string ArgumentOutOfRange = "The '{0}' parameter must be between {1} and {2}";
    internal const string ArgumentOutOfRange_HasNoYear0 = "The calendar does not support a year zero";
    internal const string Argument_ResultCalendarRange = "The result is out of the supported range for this calendar. The result should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive";

    internal static void CheckArgumentOutOfRange(int value, int min, int max, [CallerArgumentExpression(nameof(value))] string? argument = null)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(argument, value, string.Format(ArgumentOutOfRange, argument, min, max));
    }
    internal static void CheckResultOutOfRange(long value, long min, long max, [CallerArgumentExpression(nameof(value))] string? argument = null)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(argument, value, string.Format(Argument_ResultCalendarRange, min, max));
    }


    #endregion

    // Number of milliseconds per time unit
    internal const int MillisPerSecond = 1000;
    internal const int MillisPerMinute = MillisPerSecond * 60;
    internal const int MillisPerHour = MillisPerMinute * 60;
    internal const int MillisPerDay = MillisPerHour * 24;

    // Number of days in a Dere year (there are no leap years on Dere!)
    protected const int DaysInDereYear = 365;

    public virtual bool HasYear0 => true;
    internal int _twoDigitYearMax = -1;


    /// <summary>
    /// Returns the number of <b>full</b> leap days (Feb 29) between two dates.
    /// </summary>
    /// <param name="start">Start date of time period</param>
    /// <param name="end">End date of time period</param>
    /// <returns><list type="bullet">
    /// <item>The number of leap days between start and end</item>
    /// <item>0 when start to end covers less than a whole day.</item>
    /// </list></returns>
    public static int GetLeapDays(DateTime start, DateTime end)
    {
        if (end < start) // make sure the direction is correct
            (start, end) = (end, start);
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
            else if (endYear == startYear)
            {
                if (DateTime.IsLeapYear(startYear)) LeapDays++;
            }

            return LeapDays;
        }
    }



    /// <inheritdoc/>
    /// <remarks>This method adds milliseconds but excludes leap days. 
    /// Hence, for every leap day in the given period it adds another day.
    /// What happens in this code, when the added days actually cross another 
    /// leap day?</remarks>
    public override DateTime AddMilliseconds(DateTime time, double milliseconds)
    {
        const long TicksPerMillisecond = 10000;
        const int DaysPer100Years = (365 * 4 + 1) * 25 - 1;
        const int DaysTo10000 = (DaysPer100Years * 4 + 1) * 25 - 366;
        const long MaxMillis = (long)DaysTo10000 * MillisPerDay;

        // If overflow occurs converting a floating-point type to an integer, or if the floating-point value
        // being converted to an integer is a NaN, the value returned is unspecified.
        //
        // Based upon this, this method should be performing the comparison against the double
        // before attempting a cast. Otherwise, the result is undefined.
        double tempMillis = milliseconds + (milliseconds >= 0 ? 0.5 : -0.5);
        if (!((tempMillis > -(double)MaxMillis) && (tempMillis < (double)MaxMillis)))
        {
            throw new ArgumentOutOfRangeException(
                nameof(milliseconds), milliseconds, 
                string.Format(ArgumentOutOfRange, nameof(milliseconds), -(double)MaxMillis, (double)MaxMillis));
        }

        // Convert to long after checking the range
        long millis = (long)tempMillis;
        long ticks = time.Ticks + millis * TicksPerMillisecond;
        CheckResultOutOfRange(ticks, DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks);
        DateTime result = new(ticks);

        // remove leap days in between
        int LeapDays = GetLeapDays(time, result); //GetLeapDays(time, result)
        result = result.AddDays(LeapDays * Math.Sign(milliseconds));

        return IgnoreLeapDay(result, Math.Sign(milliseconds));
    }


    public override DateTime AddSeconds(DateTime time, int seconds) 
        => AddMilliseconds(time, seconds * MillisPerSecond);
    

    public override DateTime AddMinutes(DateTime time, int minutes)
        => AddMilliseconds(time, minutes * MillisPerMinute);

    public override DateTime AddHours(DateTime time, int hours)
        => AddMilliseconds(time, hours * MillisPerHour);


    public override DateTime AddDays(DateTime time, int days)
    {
        time = IgnoreLeapDay(time, -1);
        int Years = days / DaysInDereYear;
        time = time.AddYears(Years);

        int Leftover = days % DaysInDereYear;
        if (Leftover == 0)
            return time;

        DateTime result = time.AddDays(Leftover);

        // The potential leap day is always Feb 29th
        DateTime leapDay;
        // Check if this specific leap day is within the inclusive range
        if (DateTime.IsLeapYear(time.Year))
        {
            leapDay = new(time.Year, 2, 29);
            if (IsBetween(leapDay, time, result))
                return result.AddDays(1 * Math.Sign(days)); // skip leap day that does not exist in Aventuria
        }
        if (DateTime.IsLeapYear(result.Year))
        {
            leapDay = new(result.Year, 2, 29);
            if (IsBetween(leapDay, time, result))
                return result.AddDays(1 * Math.Sign(days)); // skip leap day that does not exist in Aventuria
        }

        return result;
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


    #region Leap Year Methods are obsolete on Dere - DateTime inheritance

    public override int GetLeapMonth(int year) => 0;
    public override int GetLeapMonth(int year, int era) => 0;
    public override bool IsLeapDay(int year, int month, int day) => false;
    public override bool IsLeapDay(int year, int month, int day, int era) => false;
    public override bool IsLeapMonth(int year, int month) => false;
    public override bool IsLeapMonth(int year, int month, int era) => false;
    public override bool IsLeapYear(int year) => false;
    public override bool IsLeapYear(int year, int era) => false;

    #endregion


    #region DateTime Analysis - Calendar inheritance

    // inherited from Calendar
    //public virtual int GetHour(DateTime time);
    //public virtual int GetMinute(DateTime time);
    //public virtual int GetSecond(DateTime time);
    //public virtual double GetMilliseconds(DateTime time);

    #endregion

    /// <inheritdoc/>
    /// <remarks>
    /// Override this in every derived class to provide a proper default value. Here it uses FB reckoning.
    /// </remarks>
    public override int TwoDigitYearMax
    {
        get
        {
            if (_twoDigitYearMax == -1)
                _twoDigitYearMax = 1099; // Default to 1050 FB

            return _twoDigitYearMax;
        }
        set
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Calendar is read-only");
            _twoDigitYearMax = value;
        }
    }
}
