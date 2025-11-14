using System.Numerics;

namespace Aventuria.Measures;

/// <summary>
/// Represents a two-dimensional area measurement using the unit "anglepace" (i.e. square meters). Provides arithmetic, comparison,
/// and formatting operations for square measures.
/// </summary>
/// <remarks>SquareMeasure supports standard arithmetic operations such as addition, subtraction, multiplication,
/// and division, as well as equality and comparison checks. The struct is immutable and can be used in calculations
/// involving area. Division by zero when using the division operators will result in a DivideByZeroException.
/// SquareMeasure implements several numeric interfaces, allowing it to be used in generic numeric algorithms and
/// collections.</remarks>
public readonly struct SquareMeasure : IMeasure, // IParsable<TSelf>, ISpanParsable<TSelf>, 
    IEquatable<SquareMeasure>, IEqualityOperators<SquareMeasure, SquareMeasure, bool>,
    ISubtractionOperators<SquareMeasure, SquareMeasure, SquareMeasure>,
    IDecrementOperators<SquareMeasure>,
    IAdditionOperators<SquareMeasure, SquareMeasure, SquareMeasure>,
    IIncrementOperators<SquareMeasure>,
    IDivisionOperators<SquareMeasure, SquareMeasure, double>, IDivisionOperators<SquareMeasure, LengthMeasure, LengthMeasure>,
    IDivisionOperators<SquareMeasure, int, SquareMeasure>, IDivisionOperators<SquareMeasure, double, SquareMeasure>,
    IMultiplyOperators<SquareMeasure, int, SquareMeasure>, IMultiplyOperators<SquareMeasure, double, SquareMeasure>,
    IMultiplyOperators<SquareMeasure, LengthMeasure, VolumeMeasure>,
    IAdditiveIdentity<SquareMeasure, SquareMeasure>,
    IMultiplicativeIdentity<SquareMeasure, SquareMeasure>,
    IMinMaxValue<SquareMeasure>
{
    /// <summary>
    /// The weight internally represented in Stone (i.e. kg in Earthen terms).
    /// </summary>
    private double Value { get; init; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="length">Sets the length in unit "anglepace"</param>
    public SquareMeasure(double length)
    {
        Value = length;
    }


    /// <summary>
    /// Returns the value as double in paces squared, i.e. anglepace.
    /// </summary>
    /// <param name="value">A <see cref="SquareMeasure"/> object</param>
    public static explicit operator double(SquareMeasure value) => value.Value;



    public override bool Equals(object? obj) // inherited from object
        => obj is not null && Equals((SquareMeasure)obj);

    public override int GetHashCode() // inherited from object
    => Value.GetHashCode();



    public override string? ToString() => Value.ToString();

    public string ToString(string? format, IFormatProvider? formatProvider) // IFormattable // TODO: implement LengthMeasure::toString()
    {
        return Value.ToString(format, formatProvider); // TODO: Implement using DereCultureInfo
    }


    public bool Equals(SquareMeasure other) // IEquatable
        => Value == other.Value;

    public static bool operator ==(SquareMeasure left, SquareMeasure right) // IEqualityOperators
        => left.Equals(right);

    public static bool operator !=(SquareMeasure left, SquareMeasure right) // IEqualityOperators
        => !left.Equals(right);

    public static SquareMeasure operator -(SquareMeasure left, SquareMeasure right) // ISubtractionOperators
        => new(left.Value - right.Value);

    public static SquareMeasure operator --(SquareMeasure value) // IDecrementOperators
        => new(value.Value - 1);

    public static SquareMeasure operator +(SquareMeasure left, SquareMeasure right) // IAdditionOperators
        => new(left.Value + right.Value);

    public static SquareMeasure operator ++(SquareMeasure value) // IIncrementOperators
        => new(value.Value + 1);

    public static double operator /(SquareMeasure left, SquareMeasure right) // IDivisionOperators
    {
        if (right.Value == 0)
            throw new DivideByZeroException("Division by zero.");
        return left.Value / right.Value;
    }

    public static LengthMeasure operator /(SquareMeasure left, LengthMeasure right) // IDivisionOperators
    {
        if ((double)right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / (double)right);
    }


    public static SquareMeasure operator /(SquareMeasure left, int right) // IDivisionOperators
    {
        if (right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / right);
    }   

    public static SquareMeasure operator /(SquareMeasure left, double right) // IDivisionOperators
    {
        if (right == 0)
            throw new DivideByZeroException("Division by zero.");
        return new(left.Value / right);
    }

    public static SquareMeasure operator *(SquareMeasure left, int right) // IMultiplyOperators
        => new(left.Value * right);

    public static SquareMeasure operator *(SquareMeasure left, double right) // IMultiplyOperators
        => new(left.Value * right);

    public static VolumeMeasure operator *(SquareMeasure area, LengthMeasure length) // IMultiplyOperators
        => new(area.Value * (double)length);

    public static SquareMeasure AdditiveIdentity => new(0); // IAdditiveIdentity

    public static SquareMeasure MultiplicativeIdentity => new(1); // IMultiplicativeIdentity

    public static SquareMeasure MinValue => new(double.MinValue); // IMinMaxValue

    public static SquareMeasure MaxValue => new(double.MaxValue); // IMinMaxValue



    #region IMeasure Members
    public double ToDouble() => Value;

    public decimal ToDecimal() => throw new NotImplementedException();

    #endregion
}
