using MudBlazor;
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

    public decimal ToGran() => Value * 40 * 25 * 5 * 5; // Rohal
    public decimal ToCarat() => Value * 40 * 25 * 5; // Rohal
    public decimal ToScruple() => Value * 40 * 25; // Rohal
    public decimal ToOunce() => Value * 40; // 1 Stone = 40 Ounces; Rohal
    public decimal ToStone() => Value; // Rohal
    public decimal ToSack() => Value / 100;
    public decimal ToCuboids() => Value / 100 / 10; // Rohal

    public override bool Equals(object obj) => Equals((Weight)obj);
    public bool Equals(Weight other) => Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();


    public override string ToString() => Value.ToString();

    public string ToString(string format, IFormatProvider formatProvider)
    {
        // Handle null or empty string.
        if (string.IsNullOrEmpty(format)) format = "G";
        // Remove spaces and convert to uppercase.
        format = format.Trim().ToUpperInvariant();

        WeightFormatter formatter = formatProvider.GetFormat(GetType()) as WeightFormatter;

        return string.Format(formatter.Format(format, this, formatter), Math.Abs((int)this));
    }

}
