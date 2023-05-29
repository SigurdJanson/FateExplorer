using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Aventuria;




public readonly struct Money : IFormattable, // IParsable<TSelf>
    IEquatable<Money>, IEqualityOperators<Money, Money, bool>,
    ISubtractionOperators<Money, Money, Money>,
    IDecrementOperators<Money>,
    IAdditionOperators<Money, Money, Money>,
    IIncrementOperators<Money>,
    IDivisionOperators<Money, Money, decimal>, IDivisionOperators<Money, int, Money>, IDivisionOperators<Money, decimal, Money>,
    IMultiplyOperators<Money, int, Money>, IMultiplyOperators<Money, decimal, Money>,
    IAdditiveIdentity<Money, Money>,
    IMultiplicativeIdentity<Money, Money>, 
    IMinMaxValue<Money>
{
    public required Currency Currency { get; init; }

    public static Money MaxValue => new(decimal.MaxValue, Currency.Reference);

    public static Money MinValue => new(decimal.MinValue, Currency.Reference);

    public static Money MultiplicativeIdentity => new(1.0m, Currency.Reference);

    public static Money AdditiveIdentity => new(0.0m, Currency.Reference);



    private readonly decimal JointAmount;

    /// <summary>
    /// Initializes a new instance of Money with the specified amount and currency.
    /// </summary>
    /// <param name="amount">The amount of money.</param>
    /// <param name="currency">The currency to represent with the instance of Money.</param>
    [SetsRequiredMembers]
    public Money(decimal amount, Currency currency)
    {
        JointAmount = amount;
        Currency = currency;
    }


    /// <summary>
    /// Converts the amount of money into another currency.
    /// </summary>
    /// <param name="currency">Designated currency</param>
    /// <returns>New money in ew currency</returns>
    public Money ToCurrency(Currency currency)
    {
        decimal Value = JointAmount * Currency.Rate;
        return new(Value / currency.Rate, currency);
    }




    #region Math

    /// <summary>
    /// Adds a Decimal value to a Money value.
    /// </summary>
    /// <param name="d">The Decimal value to add.</param>
    /// <param name="m">The Money value to add.</param>
    /// <returns>A Money value with an amount equal to the d plus the amount of m.</returns>
    public static Money operator +(decimal d, Money m)
    {
        return new Money(m.JointAmount + d, m.Currency);
    }

    /// <summary>
    ///  Adds a Money value to a Decimal value.
    /// </summary>
    /// <param name="m">The Money value to add.</param>
    /// <param name="d">The Decimal value to add.</param>
    /// <returns>A Money value with an amount equal to the amount of m plus d.</returns>
    public static Money operator +(Money m, decimal d)
    {
        return d + m;
    }

    /// <summary>
    /// Adds two Money values of the same currency.
    /// </summary>
    /// <param name="m1">The first Money value to add.</param>
    /// <param name="m2">The second Money value to add.</param>
    /// <returns>A Money value equal to the sum of both Money values.</returns>
    /// <exception cref="CurrencyMismatchException">m1 and m2 represent different currencies.</exception>
    public static Money operator +(Money m1, Money m2)
    {
        RequireSameCurrency(m1, m2);
        return m1 + m2.JointAmount;
    }

    /// <summary>
    /// Subtracts a Decimal value from a Money value.
    /// </summary>
    /// <param name="m">The Money value from which to subtract.</param>
    /// <param name="d">The Decimal value to subtract.</param>
    /// <returns>A Money value with an amount equal to the amount of m minus d.</returns>
    public static Money operator -(Money m, decimal d)
    {
        return new Money(m.JointAmount - d, m.Currency);
    }

    /// <summary>
    /// Subtracts one Money value from another.
    /// </summary>
    /// <param name="m1">The Money value from which to subtract.</param>
    /// <param name="m2">The Money value to subtract.</param>
    /// <returns>A Money value with an amount equal to the amount of m1 minus the amount of m2.</returns>
    public static Money operator -(Money m1, Money m2)
    {
        RequireSameCurrency(m1, m2);
        return m1 - m2.JointAmount;
    }

    /// <inheritdoc/>
    public static Money operator *(Money m, decimal d)
    {
        return new Money(m.JointAmount * d, m.Currency);
    }

    /// <inheritdoc/>
    public static Money operator /(Money m1, decimal d)
    {
        return new Money(m1.JointAmount / d, m1.Currency);
    }

    /// <summary>
    /// Rounds a Money value to the nearest integer.
    /// </summary>
    /// <param name="m">The Money value to round.</param>
    /// <returns>The integer value of Money that is nearest to the d parameter. If d is halfway between two integers, one of which is even and the other odd, the even value is returned.</returns>
    /// <seealso cref="decimal.Round(Decimal)"/>
    public static Money Round(Money m)
    {
        return new Money(decimal.Round(m.JointAmount), m.Currency);
    }

    /// <summary>
    /// Rounds a Money value to a specified number of decimal places.
    /// </summary>
    /// <param name="m">The Money value to round.</param>
    /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
    /// <returns>The Money value equivalent to m rounded to decimals number of decimal places.</returns>
    /// <seealso cref="decimal.Round(Decimal, Int32)"/>
    public static Money Round(Money m, int decimals)
    {
        return new Money(decimal.Round(m.JointAmount, decimals), m.Currency);
    }

    /// <summary>
    /// Rounds a Money value to the nearest integer. A parameter specifies how to round the value if it is midway between two other numbers.
    /// </summary>
    /// <param name="m">The Money value to round.</param>
    /// <param name="mode">A value that specifies how to round the amount of m if it is midway between two other numbers.</param>
    /// <returns>The integer value of Money that is nearest to the d parameter. If the amount of m is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two values is returned.</returns>
    /// <seealso cref="decimal.Round(Decimal, MidpointRounding)"/>
    public static Money Round(Money m, MidpointRounding mode)
    {
        return new Money(decimal.Round(m.JointAmount, mode), m.Currency);
    }

    /// <summary>
    /// Rounds a Money value to a specified precision. A parameter specifies how to round the value if it is midway between two other numbers.
    /// </summary>
    /// <param name="m">The Money value to round.</param>
    /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
    /// <param name="mode">A value that specifies how to round the amount of m if it is midway between two other numbers.</param>
    /// <returns>The Money value that is nearest to the d parameter with a precision equal to the decimals parameter. If the amount of m is halfway between two numbers, one of which is even and the other odd, the mode parameter determines which of the two values is returned. If the precision of the amount of m is less than decimals, m is returned unchanged.</returns>
    /// <seealso cref="decimal.Round(Decimal, Int32, MidpointRounding)"/>
    public static Money Round(Money m, int decimals, MidpointRounding mode)
    {
        return new Money(decimal.Round(m.JointAmount, decimals, mode), m.Currency);
    }

 

    /// <inheritdoc/>
    public static Money operator --(Money value) =>
        new(value.JointAmount - 1, value.Currency);

    /// <inheritdoc/>
    public static Money operator ++(Money value) =>
        new(value.JointAmount + 1, value.Currency);

    /// <inheritdoc/>
    public static decimal operator /(Money left, Money right)
    {
        return left.JointAmount / right.JointAmount;
    }

    /// <inheritdoc/>
    public static Money operator /(Money left, int right)
    {
        return new(left.JointAmount / right, left.Currency);
    }

    /// <inheritdoc/>
    public static Money operator *(Money left, int right)
    {
        return new(left.JointAmount * right, left.Currency);
    }

    /// <summary>
    /// Compares this instance to a decimal value and returns an indication of their relative values. 
    /// </summary>
    /// <param name="value">The decimal value to compare to this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and value.</returns>
    public int CompareTo(decimal value)
    {
        return JointAmount.CompareTo(value);
    }

    #endregion // Math






    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Money money && Equals(money);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash *= 23 + JointAmount.GetHashCode();
            hash *= 23 + Currency.GetHashCode();
            return hash;
        }
    }

    #region Comparison

    /// <inheritdoc/>
    public bool Equals(Money other)
    {
        return (JointAmount == other.JointAmount && Currency == other.Currency);
    }


    /// <summary>
    /// Compares this instance to another <see cref="Money">Money</see> value of the same currency and returns an indication of their relative values. 
    /// </summary>
    /// <param name="value">The Money value to compare to this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and value.</returns>
    public int CompareTo(Money value)
    {
        //TODO: RequireSameCurrency(this, value);
        return JointAmount.CompareTo(value.JointAmount);
    }


    /// <summary>
    /// Compares this instance to another object and returns an indication of their relative values. 
    /// </summary>
    /// <param name="value">The object to compare to this instance, or <c>null</c>.</param>
    /// <returns>A signed number indicating the relative values of this instance and value.</returns>
    public int CompareTo(object value)
    {
        //-if (value == null) throw new ArgumentNullException(nameof(value));
        if (value is Money money) return CompareTo(money);
        throw new ArgumentException($"Cannot compare type {value.GetType()} to type Money.");
    }

    /// <inheritdoc/>
    public static bool operator == (Money m1, Money m2)
    {
        //TODO: RequireSameCurrency(m1, m2);
        return m1.JointAmount.Equals(m2.JointAmount);
    }

    /// <inheritdoc/>
    public static bool operator !=(Money m1, Money m2)
    {
        //TODO: RequireSameCurrency(m1, m2);
        return !m1.JointAmount.Equals(m2.JointAmount);
    }

    /// <inheritdoc/>
    public static bool operator >(Money m1, Money m2)
    {
        //TODO: RequireSameCurrency(m1, m2);
        return m1.JointAmount > m2.JointAmount;
    }

    /// <inheritdoc/>
    public static bool operator <(Money m1, Money m2)
    {
        //TODO: RequireSameCurrency(m1, m2);
        return m1.JointAmount < m2.JointAmount;
    }

    /// <inheritdoc/>
    public static bool operator >=(Money m1, Money m2)
    {
        //TODO: RequireSameCurrency(m1, m2);
        return m1.JointAmount >= m2.JointAmount;
    }

    /// <inheritdoc/>
    public static bool operator <=(Money m1, Money m2)
    {
        //TODO: RequireSameCurrency(m1, m2);
        return m1.JointAmount <= m2.JointAmount;
    }

    /// <inheritdoc/>
    public static bool operator >(Money m, decimal d)
    {
        return m.JointAmount > d;
    }

    /// <inheritdoc/>
    public static bool operator <(Money m, decimal d)
    {
        return m.JointAmount < d;
    }

    /// <inheritdoc/>
    public static bool operator >=(Money m, decimal d)
    {
        return m.JointAmount >= d;
    }

    /// <inheritdoc/>
    public static bool operator <=(Money m, decimal d)
    {
        return m.JointAmount <= d;
    }

    #endregion // Comparison





    #region NUMBER BASE

    public static Money One => new(1m, Currency.Reference);
    public static Money Zero => new(0m, Currency.Reference);
    public static bool IsZero(Money money) => money.JointAmount == 0.0m;
    public static Weight Abs(Money money) => new(Math.Abs(money.JointAmount));

    [SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsComplexNumber(Money money) => false;
    public static bool IsInteger(Money money) => decimal.IsInteger(money.JointAmount);

    [SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Parameter required for Interface")]
    public static bool IsRealNumber(Money money) => true;
    public static bool IsEvenInteger(Money money) => decimal.IsEvenInteger(money.JointAmount);
    public static bool IsOddInteger(Money money) => decimal.IsOddInteger(money.JointAmount);
    public static bool IsPositive(Money money) => decimal.IsPositive(money.JointAmount);
    public static bool IsNegative(Money money) => decimal.IsNegative(money.JointAmount);
    public static Weight Truncate(Money money) => new(decimal.Truncate(money.JointAmount));

    #endregion



    /// <summary>
    /// Returns a string representation of the Money value consisting of the Amount and the currency.
    /// </summary>
    /// <returns>A string representation of the Money value consisting of the Amount and the currency.</returns>
    /// <remarks>The string returned is not intended for UI display.</remarks>
    public override string ToString()
    {
        // TODO
        return string.Concat(JointAmount.ToString(CultureInfo.CurrentUICulture), " ", Currency.ToString());
    }

    /// <summary>
    /// Returns a string representation of the Money value consisting of the Amount and the currency.
    /// </summary>
    /// <returns>A string representation of the Money value consisting of the Amount and the currency.</returns>
    /// <remarks>The string returned is not intended for UI display.</remarks>
    public string ToString(string format)
    {
        // TODO
        return string.Concat(JointAmount.ToString(CultureInfo.CurrentUICulture), " ", Currency.ToString());
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return JointAmount.ToString(format, formatProvider);
    }





    public static void RequireSameCurrency(Money a, Money b)
    {
        if (a.Currency != b.Currency) throw new ArgumentException("Currency mismatch");
    }

}