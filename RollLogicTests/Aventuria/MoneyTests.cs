using Aventuria;
using FateExplorer.Pages;
using FateExplorer.Shop;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;

namespace UnitTests.Aventuria;

[TestFixture]
public class MoneyTests
{


    [SetUp]
    public void SetUp()
    {}


    public readonly static object[] OptimizeDenomination_Ducat_TestCase =
    [
        new object[] { 0m, (decimal[])[0m, 0m, 0m, 0m] },
        new object[] { 1m, (decimal[])[1m, 0m, 0m, 0m] },
        new object[] { 0.1m, (decimal[])[0m, 1m, 0m, 0m] },
        new object[] { 0.01m, (decimal[])[0m, 0m, 1m, 0m] },
        new object[] { 0.001m, (decimal[])[0m, 0m, 0m, 1m] },
        new object[] { 0.0001m, (decimal[])[0m, 0m, 0m, 0.1m] },
        new object[] { 12.341m, (decimal[])[12.0m, 3m, 4m, 1m] },
        new object[] { -12.341m, (decimal[])[12.0m, 3m, 4m, 1m] }
    ];
    public readonly static object[] OptimizeDenomination_Denominations_TestCase = 
    [
        new object[] { Currency.MiddenrealmDucat }, // reference currency
        new object[] { Currency.DwarvenThaler },    // non-decimal conversion
        new object[] { Currency.AlanfaDoubloon },    // non-decimal conversion
        new object[] { Currency.NostrianCrown }     // single coin currency
    ];

    /// <summary>
    /// Helper function to be able to specify currencies in test cases as string.
    /// </summary>
    /// <param name="currencyStr">One of the predefined strings</param>
    /// <returns>A currency object</returns>
    /// <exception cref="System.Exception">When given a string that is not in the list.</exception>
    protected static Currency String2Currency(string currencyStr)
    {
        return currencyStr switch
        {
            "MiddenrealmDucat" => Currency.MiddenrealmDucat,
            "Dwarventhaler" => Currency.DwarvenThaler,
            "NostrianCrown" => Currency.NostrianCrown,
            _ => throw new System.Exception()
        };
    }




    #region static properties

    [Test]
    public void MaxValue_ShouldHaveDecimalMaxAndReferenceCurrency()
    {
        var max = Money.MaxValue;

        Assert.That(max.ToDecimal(), Is.EqualTo(decimal.MaxValue));
        Assert.That(max.Currency, Is.EqualTo(Currency.ReferenceCurrency));
    }

    [Test]
    public void MinValue_ShouldHaveDecimalMinAndReferenceCurrency()
    {
        var min = Money.MinValue;

        Assert.That(min.ToDecimal(), Is.EqualTo(decimal.MinValue));
        Assert.That(min.Currency, Is.EqualTo(Currency.ReferenceCurrency));
    }

    [Test]
    public void AdditiveIdentity_ShouldBeZeroAndReferenceCurrency()
    {
        var zero = Money.AdditiveIdentity;

        Assert.That(zero.ToDecimal(), Is.EqualTo(0.0m));
        Assert.That(zero.Currency, Is.EqualTo(Currency.ReferenceCurrency));
    }

    [Test]
    public void MultiplicativeIdentity_ShouldBeOneAndReferenceCurrency()
    {
        var one = Money.MultiplicativeIdentity;

        Assert.That(one.ToDecimal(), Is.EqualTo(1.0m));
        Assert.That(one.Currency, Is.EqualTo(Currency.ReferenceCurrency));
    }

    [Test]
    public void AdditiveIdentity_WhenAddedToAnyMoney_ShouldNotChangeAmountOrCurrency()
    {
        // Arrange
        var original = new Money(123.45m, Currency.ReferenceCurrency);

        // Act: simulate adding the additive identity by using underlying amounts and currency
        var result = original + Money.AdditiveIdentity.ToDecimal();

        // Assert
        Assert.That(original, Is.EqualTo(original));
        //---Assert.That(original.Currency, result.Currency);
    }

    [Test]
    public void MultiplicativeIdentity_WhenAppliedToAnyMoney_ShouldNotChangeAmountOrCurrency()
    {
        // Arrange
        var original = new Money(9876.54321m, Currency.ReferenceCurrency);

        // Act: simulate multiplication by the multiplicative identity
        var result = original * Money.MultiplicativeIdentity.ToDecimal();

        // Assert
        Assert.That(original, Is.EqualTo(result));
        //Assert.That(original.Currency, result.Currency);
    }

    #endregion



    [Test]
    [TestCaseSource(nameof(OptimizeDenomination_Ducat_TestCase))]
    public void OptimizeDenomination_Ducat_HappyCase(decimal Amount, params decimal[] Expected)
    {
        // Arrange
        Money m = new (Amount, Currency.MiddenrealmDucat);

        // Act
        var result = m.OptimizeDenomination();

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }

    [Test]
    [TestCaseSource(nameof(OptimizeDenomination_Denominations_TestCase))]
    public void OptimizeDenomination_ZeroAmount_VariousCurrencies(Currency currency)
    {
        // Arrange
        decimal[] Expected = new decimal[currency.CoinValue.Length];
        Money m = new(0.0m, currency);

        // Act
        var result = m.OptimizeDenomination();

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }

    [Test]
    [TestCaseSource(nameof(OptimizeDenomination_Denominations_TestCase))]
    public void OptimizeDenomination_LessThanSmallestDonmination_VariousCurrencies(Currency currency)
    {
        // Arrange
        decimal Fraction = 0.1m;
        decimal Amount = currency.CoinValue.Min() * Fraction;
        decimal[] Expected = new decimal[currency.CoinValue.Length];
        Expected[^1] = Fraction;
        Money m = new(Amount, currency);

        // Act
        var result = m.OptimizeDenomination();

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }



    [Test]
    [TestCase(19.675, "Ducat", 0, ExpectedResult = 19.675)] // ducat
    [TestCase(19.675, "Ducat", 1, ExpectedResult = 196.75)] // silverthaler
    [TestCase(19.675, "Ducat", 3, ExpectedResult = 19675)] // Kreutzer
    [TestCase(5.12, "Dwarves", 2, ExpectedResult = 5.120 * 60)]
    [TestCase(1.0, "Al'Anfa", 1, ExpectedResult = 1/0.05)]
    [TestCase(1.0, "Al'Anfa", 3, ExpectedResult = 1/0.0005)]
    [TestCase(5.12, "BornlandLump", -1, ExpectedResult = 5.12)]
    [TestCase(5.12, "BornlandLump", 1, ExpectedResult = 51.2)]
    public decimal ToDecimal(decimal Amount, string aCurrency, int Coin)
    {
        // Arrange
        Currency currency = aCurrency switch
        {
            "Ducat" => Currency.MiddenrealmDucat,
            "Dwarves" => Currency.DwarvenThaler,
            "Al'Anfa" => Currency.AlanfaDoubloon,
            "BornlandLump" => Currency.BornlandLump,
            _ => throw new ArgumentException("Unknown currency", nameof(aCurrency))
        };
        Money m = new(Amount, currency);

        // Act
        decimal result = m.ToDecimal(Coin);

        // Assert
        return result;
    }


    [Test]
    [TestCase(0.006666666, "Ducat", ExpectedResult = 0.0066667)]
    [TestCase(0.006666666, "Horasdor", ExpectedResult = 0.006666666)] // digits still fit within precision range
    [TestCase(0.0133333333, "Al'Anfa", ExpectedResult = 0.01333333)]
    [TestCase(0.0133333333, "Dwarves", ExpectedResult = 0.013333)]
    public decimal Round_Success(decimal value, string aCurrency)
    {
        // Arrange
        Currency currency = aCurrency switch
        {
            "Ducat" => Currency.MiddenrealmDucat,
            "Horasdor" => Currency.Horasdor,
            "Dwarves" => Currency.DwarvenThaler,
            "Al'Anfa" => Currency.AlanfaDoubloon,
            _ => throw new ArgumentException("Unknown currency", nameof(aCurrency))
        };
        Money m = new(value, currency);

        // Act
        decimal result = m.Round(value);

        // Assert
        return result;
    }


    [Test]
    [TestCase("Ducat")]
    [TestCase("Dwarves")]
    [TestCase("Al'Anfa")]
    public void OptimizeDenomination_ReturnsCorrectDenomination_VariousCurrencies(string aCurrency)
    {
        // Arrange
        Currency currency = aCurrency switch
        {
            "Ducat" => Currency.MiddenrealmDucat,
            "Dwarves" => Currency.DwarvenThaler,
            "Al'Anfa" => Currency.AlanfaDoubloon,
            _ => throw new ArgumentException("Unknown currency", nameof(aCurrency))
        };

        decimal Fraction = 0.5m;
        decimal Amount = currency.CoinValue.Min() * Fraction;
        Money m = new(Amount, currency);

        decimal[] Expected = new decimal[currency.CoinValue.Length];
        Expected[^1] = Fraction;

        // Act
        var result = m.OptimizeDenomination();

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }




    public readonly static object[] ToCurrencyTestCase =
    [
        new object[] { 13.2m,  Currency.DwarvenThaler,   11m },
        new object[] { -12.0m, Currency.DwarvenThaler,  -10m },
        new object[] { 0.0m,   Currency.DwarvenThaler,   0.0m },
        new object[] { 13.2m,  Currency.NostrianCrown,   13.2m/5 }
    ];

    /// <summary>
    /// Converts an amount of money to the middenrealmian reference currency
    /// to check if the conversion works.
    /// </summary>
    [Test, TestCaseSource(nameof(ToCurrencyTestCase))]
    public void ToCurrency_IdenticalCurrency_IdenticalValue(decimal m, Currency rc, decimal r)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = money.ToCurrency(rc);

        // Assert
        Assert.That(result, Is.EqualTo(new Money(r, rc) ));
    }

    [Test]
    [TestCase("13.2", ExpectedResult = "11")]
    [TestCase("0.0", ExpectedResult = "0.0")]
    [TestCase("-12.0", ExpectedResult = "-10")]
    public decimal ToCurrencyValue(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = money.ToCurrencyValue(Currency.DwarvenThaler);

        // Assert
        return result;
    }



    #region Comparisons

    [Test]
    [TestCase("1.0", "1.0", ExpectedResult = 0)]
    [TestCase("0.0", "1.0", ExpectedResult = -1)]
    [TestCase("1.0", "0.0", ExpectedResult = 1)]
    [TestCase("0.0", "-0.0", ExpectedResult = 0)]
    public int CompareTo(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);
        decimal value = v;

        // Act
        var result = money.CompareTo(value);

        // Assert
        return result;
    }



    [Test]
    [TestCase("0.0", "1.0", ExpectedResult = false)]
    [TestCase("1.0", "0.0", ExpectedResult = false)]
    [TestCase("1.0", "1.0", ExpectedResult = true)]
    [TestCase("-1.0", "-1.0", ExpectedResult = true)]
    [TestCase("-0.0", "-0.0", ExpectedResult = true)]
    [TestCase("-1.0", "0.0", ExpectedResult = false)]
    public bool Equals_MoneyAsObject_SameCurrency_TrueOrFalse(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);
        Money val = new(v, Currency.ReferenceCurrency);

        // Act
        var result = money.Equals((object)val);

        // Assert
        return result;
    }
    [Test]
    [TestCase("0.0", "1.0", ExpectedResult = false)]
    [TestCase("1.0", "1.0", ExpectedResult = false)]
    public bool Equals_NoMoneyObject_False(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);
        
        // Act
        var result = money.Equals(v);

        // Assert
        return result;
    }



    [Test]
    [TestCase("0.0", "1.0", ExpectedResult = false)]
    [TestCase("1.0", "0.0", ExpectedResult = false)]
    [TestCase("1.0", "1.0", ExpectedResult = true)]
    [TestCase("-1.0", "-1.0", ExpectedResult = true)]
    [TestCase("-0.0", "-0.0", ExpectedResult = true)]
    [TestCase("-1.0", "0.0", ExpectedResult = false)]
    public bool Equals_Money_SameCurrency_TrueOrFalse(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);
        Money val = new(v, Currency.ReferenceCurrency);

        // Act
        var result = money.Equals(val);

        // Assert
        return result;
    }

    [Test]
    [TestCase("0.0", "1.0", ExpectedResult = false)]
    [TestCase("1.0", "0.0", ExpectedResult = false)]
    [TestCase("1.0", "1.0", ExpectedResult = false)]
    [TestCase("-1.0", "-1.0", ExpectedResult = false)]
    [TestCase("-0.0", "-0.0", ExpectedResult = false)]
    [TestCase("-1.0", "0.0", ExpectedResult = false)]
    public bool Equals_OtherCurrency_False(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.MiddenrealmDucat);
        Money val = new(v, Currency.DwarvenThaler);

        // Act
        var result = money.Equals(val);

        // Assert
        return result;
    }




    [Test]
    [TestCase("0.0", "1.0", ExpectedResult = false)]
    [TestCase("1.0", "0.0", ExpectedResult = false)]
    [TestCase("1.0", "1.0", ExpectedResult = true)]
    [TestCase("-1.0", "-1.0", ExpectedResult = true)]
    [TestCase("-0.0", "-0.0", ExpectedResult = true)]
    [TestCase("-1.0", "0.0", ExpectedResult = false)]
    public bool GetHashCode_SameCurrency_TrueOrFalse(decimal m1, decimal m2)
    {
        // Arrange
        Money money1 = new(m1, Currency.ReferenceCurrency);
        Money money2 = new(m2, Currency.ReferenceCurrency);

        // Act
        var result = money1.GetHashCode() == money2.GetHashCode();

        // Assert
        return result;
    }

    [Test]
    [TestCase("0.0", "1.0", ExpectedResult = false)]
    [TestCase("1.0", "0.0", ExpectedResult = false)]
    [TestCase("1.0", "1.0", ExpectedResult = false)]
    [TestCase("-1.0", "-1.0", ExpectedResult = false)]
    [TestCase("-0.0", "-0.0", ExpectedResult = false)]
    [TestCase("-1.0", "0.0", ExpectedResult = false)]
    public bool GetHashCode_OtherCurrency_False(decimal m1, decimal m2)
    {
        // Arrange
        Money money1 = new(m1, Currency.PaaviGuilder);
        Money money2 = new(m2, Currency.AlanfaDoubloon);

        // Act
        var result = money1.GetHashCode() == money2.GetHashCode();

        // Assert
        return result;
    }





    [Test]
    [TestCase("1.0", "1.0", ExpectedResult = 0)]
    [TestCase("0.0", "1.0", ExpectedResult = -1)]
    [TestCase("1.0", "0.0", ExpectedResult = 1)]
    [TestCase("0.0", "-0.0", ExpectedResult = 0)]
    public int CompareTo_Money(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);
        Money value = new(v, Currency.ReferenceCurrency);

        // Act
        var result = money.CompareTo(value);

        // Assert
        return result;
    }

    [Test]
    [TestCase("1.0", "1.0", ExpectedResult = 0)]
    [TestCase("0.0", "1.0", ExpectedResult = -1)]
    [TestCase("1.0", "0.0", ExpectedResult = 1)]
    [TestCase("0.0", "-0.0", ExpectedResult = 0)]
    public int CompareTo_MoneyAsObject(decimal m, decimal v)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);
        Money value = new(v, Currency.ReferenceCurrency);

        // Act
        var result = money.CompareTo((object)value);

        // Assert
        return result;
    }

    #endregion



    #region Math Operations

    [Test]
    [TestCase("1.0", "1.0", ExpectedResult = 1.0)]
    [TestCase("1.0", "-1.0", ExpectedResult = -1.0)]
    [TestCase("-1.0", "1.0", ExpectedResult = 1.0)]
    [TestCase("-1.0", "-1.0", ExpectedResult = -1.0)]
    [TestCase("-0.0", "-0.0", ExpectedResult = 0.0)]
    [TestCase("-0.0", "1.0", ExpectedResult = 0.0)]
    [TestCase("0.0", "-1.0", ExpectedResult = -0.0)]
    [TestCase("-1.0", "0.0", ExpectedResult = 1.0)]
    public decimal CopySign(decimal m1, decimal m2)
    {
        // Arrange
        Money money1 = new(m1, Currency.ReferenceCurrency);
        Money money2 = new(m2, Currency.ReferenceCurrency);

        // Act
        var result = Money.CopySign(money1, money2);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }



    [Test]
    [TestCase("1.0", ExpectedResult = 1.0)]
    [TestCase("11.1", ExpectedResult = 1.0)]
    [TestCase("-1.0", ExpectedResult = -1.0)]
    [TestCase("-11.1", ExpectedResult = -1.0)]
    [TestCase("0.0", ExpectedResult = 0.0)]
    [TestCase("-0.0", ExpectedResult = 0.0)]
    public int Sign(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.Sign(money);

        // Assert
        return result;
    }



    [Test]
    [TestCase("1.0", "1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("0.5", "-1.0", "1.0", ExpectedResult = "0.5")]
    [TestCase("-0.5", "-1.0", "1.0", ExpectedResult = "-0.5")]
    [TestCase("1.0", "-1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("-1.0", "-1.0", "1.0", ExpectedResult = "-1.0")]
    [TestCase("-2.0", "-1.0", "1.0", ExpectedResult = "-1.0")]
    [TestCase("2.0", "-1.0", "1.0", ExpectedResult = "1.0")]
    public decimal Clamp_StateUnderTest_ExpectedBehavior(decimal v, decimal _min, decimal _max)
    {
        // Arrange
        Money value = new(v, Currency.ReferenceCurrency);
        Money min = new(_min, Currency.ReferenceCurrency);
        Money max = new(_max, Currency.ReferenceCurrency);

        // Act
        var result = Money.Clamp(value, min, max);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }


    [Test]
    [TestCase("1.0", ExpectedResult = false)]
    [TestCase("-1.0", ExpectedResult = false)]
    [TestCase("0.0", ExpectedResult = true)]
    [TestCase("-0.0", ExpectedResult = true)]
    public bool IsZero(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsZero(money);

        // Assert
        return result;
    }


    [Test]
    [TestCase("1.0", "1.0")]
    [TestCase("-1.0", "1.0")]
    [TestCase("0.0", "0.0")]
    [TestCase("-0.0", "0.0")]
    [TestCase("2.0", "2.0")]
    [TestCase("-2.0", "2.0")]
    public void Abs_StateUnderTest_ExpectedBehavior(decimal m, decimal abs)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.Abs(money);

        // Assert
        Assert.That(result, Is.EqualTo(new Money(abs, Currency.ReferenceCurrency)));
    }

    [Test]
    public void IsComplexNumber()
    {
        // Arrange
        Money money = new(1.0m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsComplexNumber(money);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("1.0")]
    [TestCase("-1.0")]
    [TestCase("0.0")]
    [TestCase("-0.0")]
    [TestCase("2.0")]
    [TestCase("-2.0")]
    public void IsInteger_Integer_True(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsInteger(money);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase("1.0000001")]
    [TestCase("-1.0000001")]
    [TestCase("0.9999999")]
    [TestCase("-0.9999999")]
    public void IsInteger_NoInteger_False(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsInteger(money);

        // Assert
        Assert.That(result, Is.False);
    }



    [Test]
    public void IsRealNumber__False()
    {
        // Arrange
        Money money = new(1.0m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsRealNumber(money);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsImaginaryNumber__False()
    {
        // Arrange
        Money money = new(1.0m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsImaginaryNumber(money);

        // Assert
        Assert.That(result, Is.False);
    }


    [Test]
    [TestCase("0.0")]
    [TestCase("-0.0")]
    [TestCase("2.0")]
    [TestCase("-2.0")]
    public void IsEvenInteger_IntegerEven_True(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsEvenInteger(money);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCase("1.0")]
    [TestCase("-1.0")]
    public void IsEvenInteger_IntegerOdd_False(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsEvenInteger(money);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test] // Ducats have 7 significant digits, 4 HackSilverDigits and 3 for the Kreutzer
    [TestCase("0.0000001")]
    [TestCase("-0.0000001")]
    [TestCase("2.0000001")]
    [TestCase("-2.0000001")]
    public void IsEvenInteger_NoIntegerEven_True(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsEvenInteger(money);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("1.0")]
    [TestCase("-1.0")]
    public void IsOddInteger_IntegerOdd_true(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsOddInteger(money);

        // Assert
        Assert.That(result, Is.True);
    }
    [Test]
    [TestCase("0.0")]
    [TestCase("-0.0")]
    [TestCase("2.0")]
    [TestCase("-2.0")]
    public void IsOddInteger_IntegerEven_False(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsOddInteger(money);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("1.0", ExpectedResult = true)]
    [TestCase("-1.0", ExpectedResult = false)]
    [TestCase("0.0", ExpectedResult = true)]
    [TestCase("-0.0", ExpectedResult = false)]
    public bool IsPositive(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsPositive(money);

        // Assert
        return result;
    }

    [TestCase(+1, ExpectedResult = true)]
    [TestCase(-1, ExpectedResult = false)]
    public bool IsPositive_MinMaxValue(int sign)
    {
        // Arrange
        decimal m;
        if (sign > 0) m = decimal.MaxValue; 
        else m = decimal.MinValue;
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsPositive(money);

        // Assert
        return result;
    }



    [Test]
    public void IsPositiveInfinity()
    {
        // Arrange
        Money money = new(Decimal.MaxValue, Currency.ReferenceCurrency);

        // Act
        var result = Money.IsPositiveInfinity(money);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    [TestCase("0", ExpectedResult = false)]
    [TestCase("-0", ExpectedResult = true)]
    [TestCase("1", ExpectedResult = false)]
    [TestCase("-1", ExpectedResult = true)]
    public bool IsNegative(decimal Value)
    {
        // Arrange
        Money money = new(Value, Currency.Andrathaler);

        // Act
        var result = Money.IsNegative(money);

        // Assert
        return result;
    }

    [Test]
    public void IsNegativeInfinity()
    {
        // Arrange
        Money money = new(Decimal.MinValue, Currency.Andrathaler);

        // Act
        var result = Money.IsNegativeInfinity(money);

        // Assert
        Assert.That(result, Is.False);

    }

    [Test]
    public void IsFinite()
    {
        // Arrange
        Money money = new(decimal.MaxValue, Currency.Andrathaler);

        // Act
        var result = Money.IsFinite(money);

        // Assert
        Assert.That(result, Is.True);

    }

    [Test] // consider the 7 significant digits of Ducats (4 hacksilver digits + 3 for Kreutzers)
    [TestCase("+1.0000001", ExpectedResult = 1)]
    [TestCase("-1.0000001", ExpectedResult = -1)]
    [TestCase("+0.9999999", ExpectedResult = 0)]
    [TestCase("-0.9999999", ExpectedResult = 0)]
    public decimal Truncate(decimal m)
    {
        // Arrange
        Money money = new(m, Currency.ReferenceCurrency);

        // Act
        var result = Money.Truncate(money);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }

    [Test]
    [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("1.0", "-1.0", ExpectedResult = "1.0")]
    [TestCase("-1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
    [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
    public decimal MaxMagnitude(decimal m1, decimal m2)
    {
        // Arrange
        Money x = new(m1, Currency.ReferenceCurrency);
        Money y = new(m2, Currency.ReferenceCurrency);

        // Act
        var result = Money.MaxMagnitude(x, y);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }

    [Test] // same as `MaxMagnitude`
    [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("1.0", "-1.0", ExpectedResult = "1.0")]
    [TestCase("-1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
    [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
    public decimal MaxMagnitudeNumber(decimal m1, decimal m2)
    {
        // Arrange
        Money x = new(m1, Currency.ReferenceCurrency);
        Money y = new(m2, Currency.ReferenceCurrency);

        // Act
        var result = Money.MaxMagnitudeNumber(x, y);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }

    [Test]
    [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("1.0", "-1.0", ExpectedResult = "-1.0")]
    [TestCase("-1.0", "1.0", ExpectedResult = "-1.0")]
    [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
    [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
    public decimal MinMagnitude(decimal m1, decimal m2)
    {
        // Arrange
        Money x = new(m1, Currency.ReferenceCurrency);
        Money y = new(m2, Currency.ReferenceCurrency);

        // Act
        var result = Money.MinMagnitude(x, y);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }

    [Test] // same as `MinMagnitude`
    [TestCase("1.0", "1.0", ExpectedResult = "1.0")]
    [TestCase("1.0", "-1.0", ExpectedResult = "-1.0")]
    [TestCase("-1.0", "1.0", ExpectedResult = "-1.0")]
    [TestCase("-1.0", "-1.0", ExpectedResult = "-1.0")]
    [TestCase("-0.0", "0.0", ExpectedResult = "0.0")]
    public decimal MinMagnitudeNumber(decimal m1, decimal m2)
    {
        // Arrange
        Money x = new(m1, Currency.ReferenceCurrency);
        Money y = new(m2, Currency.ReferenceCurrency);

        // Act
        var result = Money.MinMagnitudeNumber(x, y);

        // Assert
        return result.ToCurrencyValue(Currency.ReferenceCurrency);
    }

    [Test]
    public void IsNaN_Always_False()
    {
        // Arrange
        Money money = new(83736363, Currency.Andrathaler);

        // Act
        var result = Money.IsNaN(money);

        // Assert
        Assert.That(result, Is.False);
    }



    [Test]
    public void Add_SameCurrency_ReturnsSum(
        [Values("MiddenrealmDucat", "Dwarventhaler", "NostrianCrown")] string currencyStr,
        [Values(0, -17.971, 23.765)] decimal amount1,
        [Values(0, -17.971, 23.765)] decimal amount2)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency); 
        Money money2 = new(amount2, currency);

        // Act
        var result = money1 + money2;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(amount1+amount2, currency)));
    }


    [Test]
    public void Subtract_SameCurrency_ReturnsDiff(
    [Values("MiddenrealmDucat", "Dwarventhaler", "NostrianCrown")] string currencyStr,
    [Values(0, -17.971, 23.765)] decimal amount1,
    [Values(0, -17.971, 23.765)] decimal amount2)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);
        Money money2 = new(amount2, currency);

        // Act
        var result = money1 - money2;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(amount1 - amount2, currency)));
    }

    [Test]
    public void Multiply_SameCurrency_ReturnsProduct(
    [Values("MiddenrealmDucat", "Dwarventhaler", "NostrianCrown")] string currencyStr,
    [Values(0, 1, -17.971, 23.765)] decimal amount1,
    [Values(0, 1, -17.971, 23.765)] decimal amount2)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = money1 * amount2;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(amount1 * amount2, currency)));
    }

    [Test]
    public void Divide_SameCurrency_ReturnsCorrect(
    [Values("MiddenrealmDucat", "Dwarventhaler", "NostrianCrown")] string currencyStr,
    [Values(0, 1, -17.971, 23.765)] decimal amount1,
    [Values(-1, 1, -17.971, 23.765)] decimal amount2)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = money1 / amount2;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(amount1 / amount2, currency)));
    }

    [Test]
    public void Inc_SameCurrency_ReturnsSum(
    [Values("MiddenrealmDucat", "Dwarventhaler", "NostrianCrown")] string currencyStr,
    [Values(0, -1, 1, -17.971, 23.765)] decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var preresult = money1++;
        var postresult = money1;

        // Assert
        Assert.That(preresult, Is.EqualTo(new Money(amount1, currency)));
        Assert.That(postresult, Is.EqualTo(new Money(amount1 + 1, currency)));
    }

    [Test]
    public void Dec_SameCurrency_ReturnsDiff(
    [Values("MiddenrealmDucat", "Dwarventhaler", "NostrianCrown")] string currencyStr,
    [Values(0, -1, 1, -17.971, 23.765)] decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var preresult = money1--;
        var postresult = money1;

        // Assert
        Assert.That(preresult, Is.EqualTo(new Money(amount1, currency)));
        Assert.That(postresult, Is.EqualTo(new Money(amount1 - 1, currency)));
    }



    [Test]
    [TestCase("MiddenrealmDucat", 0, ExpectedResult = 0)]
    [TestCase("MiddenrealmDucat", -1.9999, ExpectedResult = -2)]
    [TestCase("Dwarventhaler", 1.5, ExpectedResult = 2)]
    [TestCase("Dwarventhaler", 2.5, ExpectedResult = 2)]
    [TestCase("NostrianCrown", -17.271, ExpectedResult = -17)]
    [TestCase("NostrianCrown", 24.765, ExpectedResult = 25)]
    public decimal Round_NoSpec_ReturnsCorrect(string currencyStr, decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = Money.Round(money1);

        // Assert
        return result.ToDecimal();
    }

    [Test]
    [TestCase("MiddenrealmDucat", 0, ExpectedResult = 0)]
    [TestCase("MiddenrealmDucat", -1.9999, ExpectedResult = -2.0)]
    [TestCase("Dwarventhaler", 1.5, ExpectedResult = 1.5)]
    [TestCase("Dwarventhaler", 2.5, ExpectedResult = 2.5)]
    [TestCase("NostrianCrown", -17.271, ExpectedResult = -17.3)]
    [TestCase("MiddenrealmDucat", -17.25, ExpectedResult = -17.2)]
    [TestCase("NostrianCrown", 24.765, ExpectedResult = 24.8)]
    [TestCase("NostrianCrown", 24.65, ExpectedResult = 24.6)]
    public decimal Round_1Decimal_ReturnsCorrect(string currencyStr, decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = Money.Round(money1, 1);

        // Assert
        return result.ToDecimal();
    }
    #endregion



    [Test, Culture("de-DE")]
    [TestCase("MiddenrealmDucat", 0, ExpectedResult = "0 MiddenrealmDucat")]
    [TestCase("MiddenrealmDucat", -1.9999, ExpectedResult = "-1,9999 MiddenrealmDucat")]
    [TestCase("Dwarventhaler", 1.5, ExpectedResult = "1,5 DwarvenThaler")]
    [TestCase("Dwarventhaler", 2.5, ExpectedResult = "2,5 DwarvenThaler")]
    [TestCase("NostrianCrown", -17.271, ExpectedResult = "-17,271 NostrianCrown")]
    public string ToString_NoFormat_ReturnsCorrect(string currencyStr, decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = money1.ToString();

        // Assert
        return result;
    }



    [Test, Culture("de-DE")]
    [TestCase("MiddenrealmDucat", "S", 0, ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 0 𝔨 MiddenrealmDucat")]
    [TestCase("MiddenrealmDucat", "S", -2.749, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨 MiddenrealmDucat")]
    [TestCase("Dwarventhaler", "K", 2.5, ExpectedResult = "2,5 ZT DwarvenThaler")]
    [TestCase("NostrianCrown", "K", -17.271, ExpectedResult = "-17,271 Kr NostrianCrown")]
    public string ToString_FormatString_ReturnsCorrect(string currencyStr, string format, decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = money1.ToString(format);

        // Assert
        return result;
    }



    [Test, Culture("de-DE")]
    [TestCase("MiddenrealmDucat", "S", 0, ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 0 𝔨")]
    [TestCase("MiddenrealmDucat", "S", -2.749, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨")] // coin set drops the sign
    [TestCase("Dwarventhaler", "K", 2.5, ExpectedResult = "2,5 ZT")]
    [TestCase("NostrianCrown", "K", -17.271, ExpectedResult = "-17,271 Kr")]
    public string ToString_MoneyFormatter_ReturnsCorrect(string currencyStr, string format, decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);

        // Act
        var result = money1.ToString(format, new MoneyFormatter());
        var resultWithNull = money1.ToString(format, null); // `null` assumes MoneyFormatter

        // Assert
        Assert.That(resultWithNull, Is.EqualTo(result));
        return result;
    }

    [Test, Culture("de-DE")]
    [TestCase("MiddenrealmDucat", "", 0, ExpectedResult = "0")]
    [TestCase("MiddenrealmDucat", "F2", -2.749, ExpectedResult = "-2,75")] // coin set drops the sign
    [TestCase("Dwarventhaler", "F3", 2.5, ExpectedResult = "2,500")]
    [TestCase("NostrianCrown", "0,0.00", -17.271, ExpectedResult = "-17,27")]
    public string ToString_DecimalFormatter_ReturnsKeyCoins(string currencyStr, string format, decimal amount1)
    {
        // Arrange
        Currency currency = String2Currency(currencyStr);
        Money money1 = new(amount1, currency);
        IFormatProvider formatProvider = (IFormatProvider)NumberFormatInfo.CurrentInfo; //.GetFormat(Type.GetType(nameof(Decimal)));

        // Act
        var result = money1.ToString(format, formatProvider);

        // Assert
        return result;
    }
}
