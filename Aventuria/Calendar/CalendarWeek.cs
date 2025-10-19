
namespace Aventuria.Calendar;


public enum DesignationOfWeek { Bosparan = 7, Novadi = 9, Gjalsker = 8, Lizardian = 5, Nameless = 13 }



public abstract class CalendarWeek : IEquatable<CalendarWeek>
{
    /// <summary>
    /// The number of days in the week.
    /// </summary>
    public virtual int Length { get; }

    /// <summary>
    /// Returns the first day of the week.
    /// </summary>
    public virtual Weekday First { get; }
    /// <summary>
    /// Returns the last day of the week.
    /// </summary>
    public virtual Weekday Last { get; }
    /// <summary>
    /// The index of the first day of the week.
    /// </summary>
    public virtual int Min => 1;
    /// <summary>
    /// The index of the last day of the week.
    /// </summary>
    public virtual int Max { get; }
    /// <summary>
    /// Identifies the type of week. 
    /// </summary>
    public virtual DesignationOfWeek Designation { get; }



    #region IEquatable Operators
    public bool Equals(CalendarWeek? other)
        => other is not null && Designation == other.Designation;
    #endregion

    #region Override object methods
    public override bool Equals(object? obj)
        => obj is not null && Designation == ((CalendarWeek)obj).Designation;

    public override int GetHashCode() => (int)Designation;

    public override string ToString() => $"{Designation}";
    #endregion

    public static bool operator ==(CalendarWeek left, CalendarWeek right)
    => left.Equals(right);
    public static bool operator !=(CalendarWeek left, CalendarWeek right)
    => !left.Equals(right);

}


/// <summary>
/// The week specification for the Bosparan calendar with 7 days a week. 
/// Used by several cultures in Aventuria using different names for the days,
/// such as Thorwaler or Dwarves
/// </summary>
public class BosparanWeek : CalendarWeek
{
    public enum BosparanDays
    {
        Windday = 1, Earthday, Marketday, Praiosday, Rohalsday, Fireday, Waterday
    }

    public override DesignationOfWeek Designation => DesignationOfWeek.Bosparan;
    public override int Length => 7;

    public static Weekday Windday => new((int)BosparanDays.Windday, DesignationOfWeek.Bosparan);
    public static Weekday Earthday => new((int)BosparanDays.Earthday, DesignationOfWeek.Bosparan);
    public static Weekday Marketday => new((int)BosparanDays.Marketday, DesignationOfWeek.Bosparan);
    public static Weekday Praiosday => new((int)BosparanDays.Praiosday, DesignationOfWeek.Bosparan);
    public static Weekday Rohalsday => new((int)BosparanDays.Rohalsday, DesignationOfWeek.Bosparan);
    public static Weekday Fireday => new((int)BosparanDays.Fireday, DesignationOfWeek.Bosparan);
    public static Weekday Waterday => new((int)BosparanDays.Waterday, DesignationOfWeek.Bosparan);

    public static Weekday Parse(int day) => new(day, DesignationOfWeek.Bosparan);

    public override Weekday First => Windday;
    public override Weekday Last => Waterday;

    public override int Max => (int)BosparanDays.Waterday;

    public BosparanWeek()
    { }

}



/// <summary>
/// Week specification for the Nameless days in the Bosparan calendar and others.
/// </summary>
public class NamelessWeek : CalendarWeek
{
    public enum NamelessDays
    {
        Isyahadin = 1, Aphestadil, Rahastes, Madaraestra, Shihayazad
    }

    public override DesignationOfWeek Designation => DesignationOfWeek.Nameless;
    public override int Length => 5;


    public static Weekday Isyahadin => new((int)NamelessDays.Isyahadin, DesignationOfWeek.Nameless);
    public static Weekday Aphestadil => new((int)NamelessDays.Aphestadil, DesignationOfWeek.Nameless);
    public static Weekday Rahastes => new((int)NamelessDays.Rahastes, DesignationOfWeek.Nameless);
    public static Weekday Madaraestra => new((int)NamelessDays.Madaraestra, DesignationOfWeek.Nameless);
    public static Weekday Shihayazad => new((int)NamelessDays.Shihayazad, DesignationOfWeek.Nameless);

    public static Weekday Parse(int day) => new(day, DesignationOfWeek.Nameless);


    public override Weekday First => Isyahadin;
    public override Weekday Last => Shihayazad;
    public override int Max => (int)NamelessDays.Shihayazad;

    public NamelessWeek()
    { }

}



/// <summary>
/// Week specification for the Novadi calendar with 9 days a week.
/// </summary>
public class NovadiWeek : CalendarWeek
{
    public enum NovadiDays
    {
        AlKira = 1, AlKadir, AlIqbal, ArRaad, ArRashid, AshSharisa, AlMhanash, AsSefraiz, AlHafla
    }

    public override DesignationOfWeek Designation => DesignationOfWeek.Novadi;
    public override int Length => 9;

    public static Weekday AlKira => new((int)NovadiDays.AlKira, DesignationOfWeek.Novadi);
    public static Weekday AlKadir => new((int)NovadiDays.AlKadir, DesignationOfWeek.Novadi);
    public static Weekday AlIqbal => new((int)NovadiDays.AlIqbal, DesignationOfWeek.Novadi);
    public static Weekday ArRaad => new((int)NovadiDays.ArRaad, DesignationOfWeek.Novadi);
    public static Weekday ArRashid => new((int)NovadiDays.ArRashid, DesignationOfWeek.Novadi);
    public static Weekday AshSharisa => new((int)NovadiDays.AshSharisa, DesignationOfWeek.Novadi);
    public static Weekday AlMhanash => new((int)NovadiDays.AlMhanash, DesignationOfWeek.Novadi);
    public static Weekday AsSefraiz => new((int)NovadiDays.AsSefraiz, DesignationOfWeek.Novadi);
    public static Weekday AlHafla => new((int)NovadiDays.AlHafla, DesignationOfWeek.Novadi);

    public static Weekday Parse(int day) => new(day, DesignationOfWeek.Novadi);

    public override Weekday First => AlKira;
    public override Weekday Last => AlHafla;
    public override int Max => (int)NovadiDays.AlHafla;

    public NovadiWeek()
    { }
}




/// <summary>
/// Week specification for the Lizard Folk calendar with 5 days a week.
/// </summary>
public class LizardianWeek : CalendarWeek
{
    public enum LizardianDays
    {
        Day0 = 1, Day1, Day2, Day3, Day4
    }

    public override DesignationOfWeek Designation => DesignationOfWeek.Lizardian;
    public override int Length => 5;

    public static Weekday Day0 => new((int)LizardianDays.Day0, DesignationOfWeek.Lizardian);
    public static Weekday Day1 => new((int)LizardianDays.Day1, DesignationOfWeek.Lizardian);
    public static Weekday Day2 => new((int)LizardianDays.Day2, DesignationOfWeek.Lizardian);
    public static Weekday Day3 => new((int)LizardianDays.Day3, DesignationOfWeek.Lizardian);
    public static Weekday Day4 => new((int)LizardianDays.Day4, DesignationOfWeek.Lizardian);

    public static Weekday Parse(int day) => new(day, DesignationOfWeek.Bosparan);

    public override Weekday First => Day0;
    public override Weekday Last => Day4;
    public override int Max => (int)LizardianDays.Day4;

    public LizardianWeek()
    { }

}



/// <summary>
/// Week specification for the Gjalsker calendar with 7 days a week.
/// </summary>
public class GjalskerWeek : CalendarWeek
{
    public enum GjalskerDays
    {
        Day1 = 1, Day2, Day3, Day4, Day5, Day6, Day7
    }

    public override DesignationOfWeek Designation => DesignationOfWeek.Gjalsker;
    public override int Length => 7;

    public static Weekday Day1 => new((int)GjalskerDays.Day1, DesignationOfWeek.Gjalsker);
    public static Weekday Day2 => new((int)GjalskerDays.Day2, DesignationOfWeek.Gjalsker);
    public static Weekday Day3 => new((int)GjalskerDays.Day3, DesignationOfWeek.Gjalsker);
    public static Weekday Day4 => new((int)GjalskerDays.Day4, DesignationOfWeek.Gjalsker);
    public static Weekday Day5 => new((int)GjalskerDays.Day5, DesignationOfWeek.Gjalsker);
    public static Weekday Day6 => new((int)GjalskerDays.Day6, DesignationOfWeek.Gjalsker);
    public static Weekday Day7 => new((int)GjalskerDays.Day7, DesignationOfWeek.Gjalsker);

    public static Weekday Parse(int day) => new(day, DesignationOfWeek.Gjalsker);

    public override Weekday First => Day1;
    public override Weekday Last => Day7;

    public override int Max => (int)GjalskerDays.Day1;

    public GjalskerWeek()
    { }

}