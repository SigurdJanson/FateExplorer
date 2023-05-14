using System;

namespace FateExplorer.Shared;

public readonly struct Weight : IEquatable<Weight>, IFormattable
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

    public static Weight operator +(Weight left, Weight right) => new(left.Value + right.Value);
    public static Weight operator -(Weight left, Weight right) => new(left.Value - right.Value);

    public static Weight operator *(Weight left, int right) => new(left.Value * right); //TODO: can I do 2 * Weight?
    public static Weight operator *(Weight left, double right) => new(left.Value * (decimal)right); //TODO: can I do 2.0 * Weight? In this order?
    public static Weight operator /(Weight left, int right) => new(left.Value * right); //TODO: can I do 2 * Weight?
    public static Weight operator /(Weight left, double right) => new(left.Value * (decimal)right); //TODO: can I do 2.0 * Weight? In this order?

    public static bool operator ==(Weight left, Weight right) => left.Equals(right);
    public static bool operator !=(Weight left, Weight right) => !left.Equals(right);

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

    public override bool Equals(object obj) => Equals((Weight)obj);
    public bool Equals(Weight other) => Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();


    public override string ToString() => Value.ToString();

    public string ToString(string format, IFormatProvider formatProvider)
    {
        // Handle null or empty string.
        if (string.IsNullOrEmpty(format)) format = "G";
        // Remove spaces and convert to uppercase.
        format = format.Trim();

        WeightFormatter formatter = formatProvider.GetFormat(GetType()) as WeightFormatter;

        return string.Format(formatter.Format(format, this, formatter), Math.Abs((int)this));
    }

}
