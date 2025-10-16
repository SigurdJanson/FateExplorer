using System.Globalization;

namespace Aventuria.Calendar;



public abstract class DereCalendar : System.Globalization.Calendar
{
    public virtual bool HasYear0 => true;
    protected const int DaysInDereYear = 365;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static DateTime IgnoreLeapDay(DateTime time, int direction)
    {
        var EarthCalendar = CultureInfo.InvariantCulture.Calendar; // Needed to determine leap years
        if (EarthCalendar.IsLeapDay(time.Year, time.Month, time.Day))
            return time.AddDays(direction); // skip leap day that does not exist in Aventuria
        return time;
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
