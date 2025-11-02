using System.Numerics;

namespace Aventuria.Measures;

public readonly struct LengthMeasure : IFormattable, // IParsable<TSelf>, ISpanParsable<TSelf>, 
    IEquatable<LengthMeasure>, IEqualityOperators<LengthMeasure, LengthMeasure, bool>,
    ISubtractionOperators<LengthMeasure, LengthMeasure, LengthMeasure>,
    IDecrementOperators<LengthMeasure>,
    IAdditionOperators<LengthMeasure, LengthMeasure, LengthMeasure>,
    IIncrementOperators<LengthMeasure>,
    IDivisionOperators<LengthMeasure, LengthMeasure, double>, IDivisionOperators<LengthMeasure, int, LengthMeasure>, IDivisionOperators<LengthMeasure, double, LengthMeasure>,
    IMultiplyOperators<LengthMeasure, int, LengthMeasure>, IMultiplyOperators<LengthMeasure, double, LengthMeasure>, // Todo: multiple 2 lengths gives an area
    IAdditiveIdentity<LengthMeasure, LengthMeasure>,
    IMultiplicativeIdentity<LengthMeasure, LengthMeasure>,
    IMinMaxValue<LengthMeasure>
{
    const double MeterPerYard = 0.9144;
    const double MeterPerDrumod = 1.68; // 


    /// <summary>
    /// The distance internally represented in paces (i.e. meter in Earthen terms).
    /// </summary>
    private double Value { get; init; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="length">Sets the length in unit "paces" ()</param>
    public LengthMeasure(double length)
    {
        Value = length;
    }


    /// <summary>
    /// Returns the value of the length as double in paces.
    /// </summary>
    /// <param name="value">A <see cref="LengthMeasure"/> object</param>
    public static explicit operator double(LengthMeasure value) => value.Value;


    public override bool Equals(object? obj) // inherited from object
        => obj is not null && Equals((LengthMeasure)obj);

    public override int GetHashCode() // inherited from object
    => Value.GetHashCode();


    public override string? ToString() => Value.ToString();

    public string ToString(string? format, IFormatProvider? formatProvider) // IFormattable // TODO: implement LengthMeasure::toString()
    {
        return Value.ToString(format, formatProvider); // TODO: Implement using DereCultureInfo
    }

    public bool Equals(LengthMeasure other) // IEquatable
        => Value == other.Value;

    public static bool operator ==(LengthMeasure left, LengthMeasure right) // IEqualityOperators
        => left.Equals(right);

    public static bool operator !=(LengthMeasure left, LengthMeasure right) // IEqualityOperators
        => !left.Equals(right);

    public static LengthMeasure operator -(LengthMeasure left, LengthMeasure right) // ISubtractionOperators
        => new(left.Value - right.Value);

    public static LengthMeasure operator --(LengthMeasure value) // IDecrementOperators
        => new(value.Value - 1);

    public static LengthMeasure operator +(LengthMeasure left, LengthMeasure right) // IAdditionOperators
        => new(left.Value + right.Value);

    public static LengthMeasure operator ++(LengthMeasure value) // IIncrementOperators
        => new(value.Value + 1);

    /// <exception cref="DivideByZeroException"></exception>
    public static double operator /(LengthMeasure left, LengthMeasure right) // IDivisionOperators
    {
        if (right.Value == 0) throw new DivideByZeroException();
        return left.Value / right.Value;
    }

    /// <exception cref="DivideByZeroException"></exception>
    public static LengthMeasure operator /(LengthMeasure left, int right) // IDivisionOperators
    {
        if (right == 0) throw new DivideByZeroException();
        return new(left.Value / right);
    }

    /// <exception cref="DivideByZeroException"></exception>
    public static LengthMeasure operator /(LengthMeasure left, double right) // IDivisionOperators
    {
        if (right == 0) throw new DivideByZeroException();
        return new(left.Value / right);
    }

    public static LengthMeasure operator *(LengthMeasure left, int right) // IMultiplyOperators
        => new(left.Value * right);

    public static LengthMeasure operator *(LengthMeasure left, double right) // IMultiplyOperators
        => new(left.Value * right);

    public static LengthMeasure AdditiveIdentity => new(0); // IAdditiveIdentity

    public static LengthMeasure MultiplicativeIdentity => new(1); // IMultiplicativeIdentity

    public static LengthMeasure MinValue => new(double.MinValue); // IMinMaxValue

    public static LengthMeasure MaxValue => new(double.MaxValue); // IMinMaxValue


    // Conversion
    #region Conversion methods

    // Imperial Middenrealm units
    public double ToYards() => Value / MeterPerYard;

    // Metric Middenrealm units
    public double ToPaces() => Value;

    // Naval Rohal Units
    //public double ToRohalFathom() => Value * CmPerYard / 500; // 1 Rohal Fathom
    //public double ToRohalLot() => Value * CmPerYard / 1000; // 1 Rohal Lot

    // Dwarven Units
    //public double ToRim() => Value * RimPerYard; // 1 notch = 2.25 inches
    //public double ToDrom() => Value * RimPerYard / 70; //
    public double ToDrumod() => Value / MeterPerDrumod; //
    //public double ToDrash() => Value * RimPerYard / 70 / 6 / 4; //
    //public double ToDumad() => Value * RimPerYard / 70 / 6 / 4 / 11; //

    //// Andergast Units
    //public double ToTeshkalerCubit() => Value * CmPerYard / 150;
    //public double ToThuranianCubit() => Value * CmPerYard / 50;

    //// Other units
    //public double ToEmperorsArm() => Value * CmPerYard / 5; // 1 Emperor's Arm = 4 span
    //public double ToNiveseDaysMarch() => Value * CmPerYard / 100000 / 12; // 1 Nivese Day's March = 12 km
    #endregion
}
