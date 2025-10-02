using System;
using System.Numerics;

namespace Aventuria;

public readonly struct Weight : IFormattable, // IParsable<TSelf>, ISpanParsable<TSelf>, 
    IEquatable<Weight>, IEqualityOperators<Weight, Weight, bool>, 
    ISubtractionOperators<Weight, Weight, Weight>,
    IDecrementOperators<Weight>,
    IAdditionOperators<Weight, Weight, Weight>,
    IIncrementOperators<Weight>,
    IDivisionOperators<Weight, Weight, Weight>, IDivisionOperators<Weight, int, Weight>, IDivisionOperators<Weight, decimal, Weight>,
    IMultiplyOperators<Weight, int, Weight>, IMultiplyOperators<Weight, decimal, Weight>,
    IAdditiveIdentity<Weight, Weight>,
    IMultiplicativeIdentity<Weight, Weight>,
    IMinMaxValue<Weight>
{
    /// <summary>
    /// The weight internally represented in Stone (i.e. kg in Earthen terms).
    /// </summary>
    private decimal Value { get; init; }

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
    public Weight(decimal weight)
    {
        Value = weight;
    }

    /// <summary>
    /// Returns the value of the weight as decimal.
    /// </summary>
    /// <param name="w">A <see cref="Weight"/> object</param>
    public static explicit operator decimal(Weight w) => w.Value;


    /// <summary>
    /// Returns the reference unit 
    /// </summary>
    public static Weight RefValue => new(1m);

    public decimal ToGran() => ToGran(Value); // Rohal
    public decimal ToCarat() => ToCarat(Value); // Rohal
    public decimal ToScruple() => ToScruple(Value); // Rohal
    public decimal ToOunce() => ToOunce(Value); // 1 Stone = 40 Ounces; Rohal
    public decimal ToStone() => Value; // Rohal
    public decimal ToSack() => ToSack(Value);
    public decimal ToCuboids() => ToCuboids(Value); // Rohal

    public static decimal ToCuboids(decimal w) => w / 100 / 10;
    public static decimal ToSack(decimal w) => w / 100;
    public static decimal ToOunce(decimal w) => w * 40;
    public static decimal ToScruple(decimal w) => w * 40 * 25;
    public static decimal ToCarat(decimal w) => w * 40 * 25 * 5;
    public static decimal ToGran(decimal w) => w * 40 * 25 * 5 * 5;



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

    public static Weight One => new (1m);
    public static Weight Zero => new (0m);
    public static bool IsZero(Weight weight) => weight == Zero;
    public static Weight Abs(Weight w) => new(Math.Abs(w.Value));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsComplexNumber(Weight w) => false;
    public static bool IsInteger(Weight w) => decimal.IsInteger(w.Value);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsRealNumber(Weight w) => true;
    public static bool IsEvenInteger(Weight w) => decimal.IsEvenInteger(w.Value);
    public static bool IsOddInteger(Weight w) => decimal.IsOddInteger(w.Value);
    public static bool IsPositive(Weight w) => decimal.IsPositive(w.Value);
    public static bool IsNegative(Weight w) => decimal.IsNegative(w.Value);
    public static Weight Truncate(Weight w) => new(decimal.Truncate(w.Value));

    #endregion


    /*
     */
    #region OTHER MATH INTERFACES
    public static Weight operator +(Weight left, Weight right) => new(left.Value + right.Value);

    public static Weight operator ++(Weight value) => new(value.Value + 1m);
    public static Weight operator -(Weight left, Weight right) => new(left.Value - right.Value);
    public static Weight operator --(Weight value) => new(value.Value - 1m);



    public static Weight operator *(Weight left, int right) => new(left.Value * right); // note: types are not commutative
    public static Weight operator *(Weight left, decimal right) => new(left.Value * right); // note: types are not commutative

    public static Weight operator /(Weight left, int right) => new(left.Value / right);
    public static Weight operator /(Weight left, decimal right) => new(left.Value / right);
    public static Weight operator /(Weight left, Weight right) => new(left.Value / right.Value);


    public static Weight MaxValue => new(decimal.MaxValue);
    public static Weight MinValue => new(decimal.MinValue);


    public static Weight AdditiveIdentity => new(0);
    public static Weight MultiplicativeIdentity => new(1);

    #endregion
}
