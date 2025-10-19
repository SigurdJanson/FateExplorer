using System.Numerics;

namespace Aventuria.Calendar;




public readonly struct Weekday : IComparable,
    IEquatable<Weekday>, IEqualityOperators<Weekday, Weekday, bool>, IEqualityOperators<Weekday, int, bool>
{
    public readonly int Day { get; }
    public CalendarWeek Designation { get; init; }

    public Weekday(int dayNumber, DesignationOfWeek designation)
    {
        Day = dayNumber;
        Designation = GetCalendarWeek(designation);
    }

    public Weekday(int dayNumber, CalendarWeek designation)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(dayNumber, designation.Min);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(dayNumber, designation.Max);
        Day = dayNumber;
        Designation = designation;
    }


    public CalendarWeek GetCalendarWeek() => Designation;

    public static CalendarWeek GetCalendarWeek(DesignationOfWeek designation) => designation switch
    {
        DesignationOfWeek.Bosparan => new BosparanWeek(),
        DesignationOfWeek.Nameless => new NamelessWeek(),
        DesignationOfWeek.Novadi => new NovadiWeek(),
        DesignationOfWeek.Gjalsker => new GjalskerWeek(),
        DesignationOfWeek.Lizardian => new LizardianWeek(),
        _ => throw new NotImplementedException($"Calendar week for designation {designation} is not implemented.")
    };

    #region Implement IComparable
    public readonly int CompareTo(object? other)
        => other is not null ? Day.CompareTo(((Weekday)other).Day) : throw new ArgumentNullException(nameof(other));
    #endregion


    #region Implement IEquatable<Weekday>
    public readonly bool Equals(Weekday other)
        => Day == other.Day && Designation == other.Designation;
    #endregion


    #region Implement IEqualityOperators
    static bool IEqualityOperators<Weekday, Weekday, bool>.operator ==(Weekday left, Weekday right)
        => left.Equals(right);

    static bool IEqualityOperators<Weekday, Weekday, bool>.operator !=(Weekday left, Weekday right)
        => !left.Equals(right);


    static bool IEqualityOperators<Weekday, int, bool>.operator ==(Weekday left, int right)
        => left.Day == right;

    static bool IEqualityOperators<Weekday, int, bool>.operator !=(Weekday left, int right)
        => left.Day != right;
    #endregion


    #region Override Object Methods
    public override readonly bool Equals(object? obj)
        => obj is not null && Day == ((Weekday)obj).Day && Designation == ((Weekday)obj).Designation;

    public override readonly int GetHashCode() => Designation.GetHashCode() + (Day.GetHashCode() << 16);

    public override readonly string ToString() => $"{Day}";
    #endregion


    #region Comparison Operators
    public static bool operator ==(Weekday left, Weekday right)
        => left.Equals(right);

    public static bool operator !=(Weekday left, Weekday right)
        => !left.Equals(right);

    public static bool operator <(Weekday left, Weekday right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Weekday left, Weekday right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Weekday left, Weekday right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Weekday left, Weekday right)
    {
        return left.CompareTo(right) >= 0;
    }
    #endregion

    #region Casting Operators
    public static implicit operator int(Weekday d) => d.Day;
    //public static explicit operator Weekday(byte b) => new(b, new BosparanWeek());
    #endregion
}




