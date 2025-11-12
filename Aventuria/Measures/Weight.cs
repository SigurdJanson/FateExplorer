using System;
using System.Numerics;

namespace Aventuria.Measures;

public readonly struct Weight : IFormattable, // IParsable<TSelf>, ISpanParsable<TSelf>, 
    IEquatable<Weight>, IEqualityOperators<Weight, Weight, bool>, 
    ISubtractionOperators<Weight, Weight, Weight>,
    IDecrementOperators<Weight>,
    IAdditionOperators<Weight, Weight, Weight>,
    IIncrementOperators<Weight>,
    IDivisionOperators<Weight, Weight, Weight>, IDivisionOperators<Weight, int, Weight>, IDivisionOperators<Weight, double, Weight>,
    IMultiplyOperators<Weight, int, Weight>, IMultiplyOperators<Weight, double, Weight>,
    IAdditiveIdentity<Weight, Weight>,
    IMultiplicativeIdentity<Weight, Weight>,
    IMinMaxValue<Weight>
    // INumberBase<Weight> // interface is only partially implemented
{
    /// <summary>
    /// Represents the default number of significant digits used in numeric calculations.
    /// When printing weights, values below this will be counted as zero.
    /// </summary>
    public const int SignificantDigits = 10;

    /// <summary>
    /// The weight internally represented in Stone (i.e. kg in Earthen terms).
    /// </summary>
    private double Value { get; init; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="weight">Sets the weight in unit "Stone"</param>
    public Weight(int weight)
    {
        Value = weight;
    }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="weight">Sets the weight in unit "Stone"</param>
    public Weight(double weight)
    {
        Value = weight;
    }

    /// <summary>
    /// Returns the w of the weight as double.
    /// </summary>
    /// <param name="w">A <see cref="Weight"/> object</param>
    public static explicit operator double(Weight w) => w.Value;


    /// <summary>
    /// Returns the reference unit 
    /// </summary>
    public static Weight RefValue => new(1.00);

    public double ToGran() => ToGran(Value); // Rohal
    public double ToCarat() => ToCarat(Value); // Rohal
    public double ToScruple() => ToScruple(Value); // Rohal
    public double ToOunce() => ToOunce(Value); // 1 Stone = 40 Ounces; Rohal
    public double ToStone() => Value; // Rohal
    public double ToSack() => ToSack(Value);
    public double ToCuboids() => ToCuboids(Value); // Rohal

    public static double ToCuboids(double w) => w / 100 / 10;
    public static double ToSack(double w) => w / 100;
    public static double ToOunce(double w) => w * 40;
    public static double ToScruple(double w) => w * 40 * 25;
    public static double ToCarat(double w) => w * 40 * 25 * 5;
    public static double ToGran(double w) => w * 40 * 25 * 5 * 5;



    public override string ToString() => Value.ToString();

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        // Handle null or empty string.
        if (string.IsNullOrEmpty(format)) format = "G";
        // Remove spaces and convert to uppercase.
        format = format.Trim();

        WeightFormatter formatter = formatProvider?.GetFormat(GetType()) as WeightFormatter ?? 
            new WeightFormatter(System.Globalization.CultureInfo.CurrentUICulture);

        return string.Format(formatter.Format(format, this, formatter), Math.Abs(Value));
    }


    /*
     */
    #region COMPARISON INTERFACES
    public static bool operator ==(Weight left, Weight right) => left.Equals(right);
    public static bool operator !=(Weight left, Weight right) => !left.Equals(right);



    public override bool Equals(object? obj) => obj is not null && obj is Weight && Equals((Weight)obj); // IEquatable
    public bool Equals(Weight other) => Value == other.Value; // IEquatable
    public override int GetHashCode() => Value.GetHashCode(); // IEquatable
    #endregion


    /*
     */
    #region NUMBER BASE

    public static Weight One => new (1.00);
    public static Weight Zero => new (0.00);
    public static bool IsZero(Weight weight) => weight == Zero;
    public static Weight Abs(Weight w) => new(Math.Abs(w.Value));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsComplexNumber(Weight w) => false;
    public static bool IsInteger(Weight w) => w.Value == double.Truncate(w.Value);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsRealNumber(Weight w) => true;
    public static bool IsEvenInteger(Weight w) => double.IsEvenInteger(w.Value);
    public static bool IsOddInteger(Weight w) => double.IsOddInteger(w.Value);
    public static bool IsPositive(Weight w) => double.IsPositive(w.Value);
    public static bool IsNegative(Weight w) => double.IsNegative(w.Value);
    public static Weight Truncate(Weight w) => new(double.Truncate(w.Value));

    //public static bool IsCanonical(Weight w) => double.IsCanonical(w.Value);
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsFinite(Weight w) => true;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsInfinity(Weight w) => false;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsNaN(Weight w) => false;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsNegativeInfinity(Weight w) => false;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsPositiveInfinity(Weight w) => false;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsImaginaryNumber(Weight w) => false;
    public static bool IsNormal(Weight w) => w.Value != 0;
    public static bool IsSubnormal(Weight w) => w.Value == 0;
    public static Weight MaxMagnitude(Weight x, Weight y) => new(double.MaxMagnitude(x.Value, y.Value));
    public static Weight MaxMagnitudeNumber(Weight x, Weight y) => MaxMagnitude(x, y);
    public static Weight MinMagnitude(Weight x, Weight y) => new(double.MinMagnitude(x.Value, y.Value));
    public static Weight MinMagnitudeNumber(Weight x, Weight y) => MinMagnitude(x, y);
    public static int Radix => 10;
    public static Weight operator -(Weight w) => new(-w.Value);
    public static Weight operator +(Weight w) => w;
    public static Weight operator *(Weight left, Weight right) => throw new InvalidOperationException();

    #endregion


    /*
     */
    #region OTHER MATH INTERFACES
    public static Weight operator +(Weight left, Weight right) => new(left.Value + right.Value);

    public static Weight operator ++(Weight value) => new(value.Value + 1.0);
    public static Weight operator -(Weight left, Weight right) => new(left.Value - right.Value);
    public static Weight operator --(Weight value) => new(value.Value - 1.0);



    public static Weight operator *(Weight left, int right) => new(left.Value * right); // note: types are not commutative
    public static Weight operator *(Weight left, double right) => new(left.Value * right); // note: types are not commutative

    public static Weight operator /(Weight left, int right) => new(left.Value / right);
    public static Weight operator /(Weight left, double right) => new(left.Value / right);
    public static Weight operator /(Weight left, Weight right) => new(left.Value / right.Value);


    public static Weight MaxValue => new(double.MaxValue);
    public static Weight MinValue => new(double.MinValue);


    public static Weight AdditiveIdentity => new(0.0);
    public static Weight MultiplicativeIdentity => new(1.0);

    #endregion


}
