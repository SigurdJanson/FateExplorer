using System.Globalization;


namespace Aventuria.Calendar;


/// <summary>
/// Provides functions to convert an earthen DateTime into a date of an Aventurian calendar.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>The calendar has 12 months of 30 days each and a "month" of 5 nameless days at the end of the year.</item>
/// <item>Uses the New Year method to convert Earth dates to Dere dates: the year on Dere begins when the Earth year 
/// begins, so 1. Praios corresponds to 1. January.</item>
/// <item>Can handle dates starting with the 11th era after 977 b. FB.</item>
/// <item>11th era is fixed at the moment because we do not know, yet, when the 12th starts.</item>
/// <item>Some methods may work independent of those restrictions but that is guaranteed if those methods do not
/// need to use the DateTime struct to work.</item>
/// </list>
/// </remarks>
public class BosparanCalendar : DereCalendar
{
    //public override bool HasYear0 => true; // not required because inherited from DereCalendar

    public override int[] Eras => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];


    //protected const int DaysInYear = DereCalendar.DaysInDereYear; // inherited from DereCalendar
    protected const int DaysInMonth = 30;
	protected const int DaysInWeek = 7;
	protected const int NamelessDays = 5;
	protected const int MonthsInYear = 13;
	protected const int YearCorrectionFromGregorian = -977; // make this virtual to adapt all kinds of other calendars
															// TODO: Add public virtual bool HasYear0 => true/false; // to adapt all kinds of other calendars
	protected const int AssumedEra = 11;
    public new const int CurrentEra = AssumedEra; // override `Calendar.CurrentEra`




    /// <inheritdoc/>
    public override CalendarAlgorithmType AlgorithmType => CalendarAlgorithmType.SolarCalendar;


	public override int GetMonth(DateTime time)
	{
		// Needed to determine leap years
		var EarthCalendar = CultureInfo.InvariantCulture.Calendar;

		int Days = time.DayOfYear;
		if (EarthCalendar.IsLeapYear(time.Year) && Days > 31 + 28) Days--;
		return ((Days - 1) / DaysInMonth) + 1;
	}


	public override int GetYear(DateTime time) => time.Year + YearCorrectionFromGregorian; // TODO: use HasYear0

    /// <inheritdoc/>
    /// <remarks>The Karmakortheon is included in the previous era. At the moment 
    /// the class assumes that we always play in the 11th age because <see cref="DateTime"/> 
	/// can only go back until 977 b. FB.</remarks>
    public override int GetEra(DateTime time) => AssumedEra;


	public override int GetDayOfMonth(DateTime time)
			{
		// Needed to determine leap years
		var EarthCalendar = CultureInfo.InvariantCulture.Calendar;

		int Days = time.DayOfYear;
		if (EarthCalendar.IsLeapYear(time.Year) && Days > 31 + 28) Days--;
		return ((Days - 1) % 30) + 1;
	}


	/// <inheritdoc/>
	public override DayOfWeek GetDayOfWeek(DateTime time)
	{
		// the 14.04.2022 on Earth is a Thursday; in Aventuria it is a Day of Praios (Sunday)
		DateTime Reference = new(2022, 4, 14);
		int Days = DeltaInDays(time, Reference);
		int Offset = Days % DaysInWeek;

		DayOfWeek Result;
		if (time > Reference)
			Result = (DayOfWeek)Offset;
		else
			Result = (DayOfWeek)(Offset == 0 ? 0 : 7 - Offset);

		return Result;
	}


	/// <inheritdoc/>
	public override int GetDayOfYear(DateTime time)
	{
		var EarthCal = CultureInfo.InvariantCulture.Calendar;

		if (EarthCal.IsLeapYear(time.Year))
			if (time.DayOfYear > 31 + 28) // days Jan. + Feb.
				return time.DayOfYear - 1;
		return time.DayOfYear;
	}


	/// <inheritdoc/>
	/// <param name="rule">Is ignored in this reckoning.</param>
	/// <param name="firstDayOfWeek">Is ignored in this reckoning.</param>
	public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule = CalendarWeekRule.FirstDay, DayOfWeek firstDayOfWeek = DayOfWeek.Thursday)
    {
		return time.DayOfYear / DaysInWeek + 1;
    }


    #region Calendar Info

    public override int GetDaysInYear(int year) => DaysInDereYear;
	public override int GetDaysInYear(int year, int era) => DaysInDereYear;


    /// <inheritdoc/>
    /// <remarks>Treats the days of the Nameless One as 13th month</remarks>
    public override int GetMonthsInYear(int year) => MonthsInYear;

    /// <inheritdoc/>
    /// <remarks>Treats the days of the Nameless One as 13th month</remarks>
    public override int GetMonthsInYear(int year, int era) => MonthsInYear;


	public override int GetDaysInMonth(int year, int month) 
		=> GetDaysInMonth(year, month, AssumedEra);


	/// <inheritdoc/>
	/// <remarks>Accepts the days of the Nameless One as 13th month</remarks>
	public override int GetDaysInMonth(int year, int month, int era)
	{
		if (month < 1 || month > MonthsInYear) 
			throw new ArgumentOutOfRangeException(nameof(month), month, "Calendar knows 12 months and 1 pseudo-month of 5 days of the Nameless One");
		if (era < 1 || era > 12)
			throw new ArgumentOutOfRangeException(nameof(era), era, "Calendar knows only eras from 1 to 12");

		return (month <= 12 ? DaysInMonth : NamelessDays);
	}

    #endregion


    #region Add Methods

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
			leapDay= new(time.Year, 2, 29);
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


    /// <inheritdoc/>
    /// <remarks>Treats the days of the Nameless One as 13th month</remarks>
    public override DateTime AddMonths(DateTime time, int months)
	{
		// Needed to determine leap years
		var EarthCalendar = CultureInfo.InvariantCulture.Calendar;

		// 
		int Month = GetMonth(time);
		DateTime result = time.AddYears(months / MonthsInYear);
		int Leftover = months % MonthsInYear;
		if (Leftover == 0)
			return result;
		else
		{
			int Day = GetDayOfMonth(time);

			int DaysToAdd;
			if (Leftover > 0)
			{
				if (Month + Leftover < MonthsInYear)
					DaysToAdd = DaysInMonth * Leftover;
				else if (Month + Leftover == MonthsInYear) // new date lies in the days of the Nameless One
					DaysToAdd = (DaysInMonth * Leftover) - Math.Max(0, Day - NamelessDays);
				else // (Month + Leftover > 13)
					DaysToAdd = DaysInMonth * (Leftover - 1) + NamelessDays;
			}
			else
			{
				if (Month + Leftover > 0) // still same year
					DaysToAdd = DaysInMonth * Leftover;
				else if (Month + Leftover == 0) // the 13th month of the previous year
					DaysToAdd = (DaysInMonth * (Leftover+1)) - Math.Max(5, Day);
				else // (Month + Leftover < 0) - previous year
					DaysToAdd = DaysInMonth * (Leftover + 1) - NamelessDays;
			}

			if (EarthCalendar.IsLeapYear(result.Year))
			{
				if (result.DayOfYear < 31 + 29 && result.DayOfYear + DaysToAdd >= 31 + 29)
					DaysToAdd += Leftover > 0 ? 1 : -1;
				if (result.DayOfYear >= 31 + 29 && result.DayOfYear + DaysToAdd < 31 + 29)
					DaysToAdd += Leftover > 0 ? 1 : -1;
			}

			return result.AddDays(DaysToAdd);
		}
	}


    /// <inheritdoc/>
    public override DateTime AddYears(DateTime time, int years)
    {
        var EarthCalendar = CultureInfo.InvariantCulture.Calendar; // Needed to determine leap years
        DateTime result = time.AddYears(years);
        if (EarthCalendar.IsLeapDay(result.Year, result.Month, result.Day))
            result = result.AddDays(-1); // skip leap day that does not exist in Aventuria
        return result;
    }

    #endregion




    /// <inheritdoc/>
    /// <remarks>Assumes current 11th era</remarks>
    public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
    {
        return ToDateTime(year, month, day, hour, minute, second, millisecond, AssumedEra);
    }


    public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
	{
		CheckArgumentOutOfRange(day, 1, DaysInMonth);
        if (month == MonthsInYear)
            CheckArgumentOutOfRange(day, 1, NamelessDays);
        CheckArgumentOutOfRange(month, 1, MonthsInYear);
        CheckArgumentOutOfRange(year, 1 + YearCorrectionFromGregorian, 9999 - YearCorrectionFromGregorian);
        CheckArgumentOutOfRange(era, 1, 12);

        int EarthYear = year - YearCorrectionFromGregorian;

		DateTime result = new(EarthYear, 1, 1, hour, minute, second, millisecond, DateTimeKind.Local);
		int DaysToAdd = (day - 1) + (month - 1) * DaysInMonth; // correct day by 1 because we already have the 1. of Jan.
		// Add extra leap day that does not exist in Aventuria
		var EarthCalendar = CultureInfo.InvariantCulture.Calendar; // Needed to determine leap years
		if (EarthCalendar.IsLeapYear(EarthYear) && DaysToAdd >= 31+28) DaysToAdd++;

		return result.AddDays(DaysToAdd);
	}


}

