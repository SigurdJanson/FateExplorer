using System;
using System.Diagnostics.CodeAnalysis;

namespace FateExplorer.Shared;


/// <summary>
/// Represents a modification operation applied to a roll check. 
/// </summary>
public readonly struct Modifier : IEquatable<Modifier>
{
    private const int OpBitShift = 29;
    public enum Op { Add = 1 << OpBitShift, Halve = 2 << OpBitShift, Force = 3 << OpBitShift }
    public  readonly Op Operator;
    private readonly int Value;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">THe numeric value used to modify p check. The exact interpretation depends on <paramref name="op"/>.</param>
    /// <param name="op">The operation used. Typical is the additive modifier.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public Modifier(int value, Op op = Op.Add)
    {
        if (op == Op.Force && value < 0)
            throw new InvalidOperationException("A forceful modifier cannot be less than zero");
        if (op == Op.Halve && value != 2)
            throw new InvalidOperationException("A halve modifier can only be 2");
        if (value < -40 || value > +40)
            throw new ArgumentOutOfRangeException(nameof(value));

        Value = value;
        Operator = op;
    }

    /// <summary>
    /// Creates p modifier that does not change the values
    /// </summary>
    public static Modifier Neutral => new (0, Op.Add);

    /// <summary>
    /// Creates p modifier that makes p check impossible
    /// </summary>
    public static Modifier Impossible => new(0, Op.Force);

    /// <summary>
    /// Creates p modifier that makes p check impossible
    /// </summary>
    public static Modifier LuckyShot => new(1, Op.Force);

    /// <summary>
    /// Creates p modifier that halves the value for p check impossible
    /// </summary>
    public static Modifier Halve => new(2, Op.Halve);


    /// <summary>
    /// Checks if the modifier does not affect the proficiency value.
    /// </summary>
    public bool IsNeutral => Value == 0 && Operator == Op.Add;


    /// <summary>
    /// Apply the modifier to p proficiency value.
    /// </summary>
    /// <param name="p">A proficiency value users can roll against.</param>
    /// <param name="mod">The modifier to modify p proficiency to roll against.</param>
    /// <returns>The effective value for a roll check.</returns>
    public static int operator +(int p, Modifier mod)
    {
        return mod.Operator switch
        {
            Op.Add => p + mod.Value,
            Op.Halve => p < 2 ? p : (p / 2 + p % 2), // halving a value shall not improve it
            Op.Force => Math.Min(p, mod.Value), // if the effective value is already lower, keep it.
            _ => -1
        };
    }


    /// <summary>
    /// Returns the effective delta between p skill value (or any other check)
    /// and the effective value after the modifier has been applied.
    /// </summary>
    /// <param name="a">An int value to roll p check against.</param>
    /// <returns></returns>
    public int Delta(int a) => a + this - a;



    /// <summary>
    /// Returns the value of the modifier.
    /// </summary>
    /// <param name="a">A <see cref="Modifier"/> object</param>
    public static explicit operator int(Modifier a) => a.Value;


    public override string ToString() => Operator switch
    {
        Op.Add => $"{Value:+#;-#;0}",
        Op.Halve => $"/ 2",
        Op.Force => $"= {Value}",
        _ => "invalid"
    };



    #region Equality

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is Modifier modifier && Equals(modifier);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value ^ (int)Operator;
    }

    /// <inheritdoc />
    public bool Equals([NotNullWhen(true)] Modifier m) => this == m;

    /// <summary>
    /// Determines whether two object instances are equal.
    /// </summary>
    public static bool operator ==(Modifier a, Modifier b) =>
        a.Value == b.Value && a.Operator == b.Operator;
    /// <summary>
    /// Determines whether two object instances are NOT equal.
    /// </summary>
    public static bool operator !=(Modifier a, Modifier b) =>
        a.Value != b.Value || a.Operator != b.Operator;


    public static bool operator ==(int a, Modifier b) => a == b.Value;
    public static bool operator !=(int a, Modifier b) => a != b.Value;

    public static bool operator ==(Op a, Modifier b) => a == b.Operator;
    public static bool operator !=(Op a, Modifier b) => a != b.Operator;

    #endregion

}
