﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace FateExplorer.Shared;

public readonly struct Modifier : IEquatable<Modifier>
{
    private const int OpBitShift = 29;
    public enum Op { Add = 1 << OpBitShift, Halve = 2 << OpBitShift, Force = 3 << OpBitShift }
    private readonly Op Operator;
    private readonly int Value;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">THe numeric value used to modify a check. The exact interpretation depends on <paramref name="op"/>.</param>
    /// <param name="op">The operation used. Typical is the additive modifier.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public Modifier(int value, Op op = Op.Add)
    {
        if (op == Op.Force && value < 0)
            throw new InvalidOperationException("A forceful modifier cannot be less than zero");
        if (op == Op.Halve && value != 2)
            throw new InvalidOperationException("A halve modifier can only be 2");

        Value = value;
        Operator = op;
    }

    /// <summary>
    /// Apply the modifier to a value
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int operator +(int a, Modifier b)
    {
        return b.Operator switch
        {
            Op.Add => a + b.Value,
            Op.Halve => a / 2 + a % 2,
            Op.Force => b.Value,
            _ => -1
        };
    }



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
