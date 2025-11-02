using System.Numerics;

namespace Aventuria.Measures;

/// <summary>
/// Represents a measurement of volume in the custom unit "anglepace." Provides arithmetic, comparison, and formatting
/// operations for volume values.
/// </summary>
/// <remarks>This struct supports standard arithmetic operators, equality checks, and formatting via the
/// IFormattable interface. It is immutable and can be used in calculations involving volume quantities. The value is
/// internally stored as a double representing anglepaces. Use the provided operators and methods to perform arithmetic
/// and comparison operations. Division by zero will result in a DivideByZeroException.</remarks>
public readonly struct VolumeMeasure : IFormattable, // IParsable<TSelf>, ISpanParsable<TSelf>, 
    IEquatable<VolumeMeasure>, IEqualityOperators<VolumeMeasure, VolumeMeasure, bool>,
    ISubtractionOperators<VolumeMeasure, VolumeMeasure, VolumeMeasure>,
    IDecrementOperators<VolumeMeasure>,
    IAdditionOperators<VolumeMeasure, VolumeMeasure, VolumeMeasure>,
    IIncrementOperators<VolumeMeasure>,
    IDivisionOperators<VolumeMeasure, VolumeMeasure, double>, IDivisionOperators<VolumeMeasure, SquareMeasure, LengthMeasure>, IDivisionOperators<VolumeMeasure, LengthMeasure, SquareMeasure>,
    IDivisionOperators<VolumeMeasure, int, VolumeMeasure>, IDivisionOperators<VolumeMeasure, double, VolumeMeasure>,
    IMultiplyOperators<VolumeMeasure, int, VolumeMeasure>, IMultiplyOperators<VolumeMeasure, double, VolumeMeasure>,
    IAdditiveIdentity<VolumeMeasure, VolumeMeasure>,
    IMultiplicativeIdentity<VolumeMeasure, VolumeMeasure>,
    IMinMaxValue<VolumeMeasure>
{
    /// <summary>
    /// The weight internally represented in Stone (i.e. kg in Earthen terms).
    /// </summary>
    private double Value { get; init; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="volume">Sets the volumne in unit "anglepace" ()</param>
    public VolumeMeasure(double volume)
    {
        Value = volume;
    }


    /// <summary>
    /// Returns the value of the volume as double in anglepaces.
    /// </summary>
    /// <param name="value">A <see cref="VolumeMeasure"/> object</param>
    public static explicit operator double(VolumeMeasure value) => value.Value;



    public override bool Equals(object? obj) // inherited from object
        => obj is not null && Equals((VolumeMeasure)obj);

    public override int GetHashCode() // inherited from object
    => Value.GetHashCode();



    public override string? ToString() => Value.ToString();

    public string ToString(string? format, IFormatProvider? formatProvider) // IFormattable // TODO: implement LengthMeasure::toString()
    {
        return Value.ToString(format, formatProvider); // TODO: Implement using DereCultureInfo
    }




    public bool Equals(VolumeMeasure other) // IEquatable
        => Value == other.Value;

    public static bool operator ==(VolumeMeasure left, VolumeMeasure right) // IEqualityOperators
        => left.Equals(right);

    public static bool operator !=(VolumeMeasure left, VolumeMeasure right) // IEqualityOperators
        => !left.Equals(right);

    public static VolumeMeasure operator -(VolumeMeasure left, VolumeMeasure right) // ISubtractionOperators
        => new(left.Value - right.Value);

    public static VolumeMeasure operator --(VolumeMeasure value) // IDecrementOperators
        => new(value.Value - 1);

    public static VolumeMeasure operator +(VolumeMeasure left, VolumeMeasure right) // IAdditionOperators
        => new(left.Value + right.Value);

    public static VolumeMeasure operator ++(VolumeMeasure value) // IIncrementOperators
        => new(value.Value + 1);

    /// <exception cref="DivideByZeroException"></exception>
    public static double operator /(VolumeMeasure left, VolumeMeasure right) // IDivisionOperators
    {
        if (right.Value == 0)
            throw new DivideByZeroException("Division by zero.");
        return left.Value / right.Value;
    }

    /// <exception cref="DivideByZeroException"></exception>
    public static LengthMeasure operator /(VolumeMeasure left, SquareMeasure right) // IDivisionOperators
    {
        if ((double)right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / (double)right);
    }

    public static SquareMeasure operator /(VolumeMeasure left, LengthMeasure right) // IDivisionOperators
    {
        if ((double)right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / (double)right);
    }

    /// <exception cref="DivideByZeroException"></exception>
    public static VolumeMeasure operator /(VolumeMeasure left, int right) // IDivisionOperators
    {
        if (right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / right);
    }

    /// <exception cref="DivideByZeroException"></exception>
    public static VolumeMeasure operator /(VolumeMeasure left, double right) // IDivisionOperators
    {
        if (right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / right);
    }

    public static VolumeMeasure operator *(VolumeMeasure left, int right) // IMultiplyOperators
        => new(left.Value * right);

    public static VolumeMeasure operator *(VolumeMeasure left, double right) // IMultiplyOperators
        => new(left.Value * right);

    public static VolumeMeasure AdditiveIdentity => new(0); // IAdditiveIdentity

    public static VolumeMeasure MultiplicativeIdentity => new(1); // IMultiplicativeIdentity

    public static VolumeMeasure MinValue => new(double.MinValue); // IMinMaxValue

    public static VolumeMeasure MaxValue => new(double.MaxValue); // IMinMaxValue

}
