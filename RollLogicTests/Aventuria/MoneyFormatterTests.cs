using Aventuria;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTests.Aventuria;

[TestFixture]
public class MoneyFormatterTests
{
    private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);


    }

    //[Test]
    //public void GetFormat_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange

    //    // Act
    //    var result = String.Format(new MoneyFormatter(), "{0:N}", 0);

    //    // Assert
    //    Assert.That(result, Is.EqualTo(""));
    //    this.mockRepository.VerifyAll();
    //}


    /*
    * SET OF COINS: BASIC FORMATTING in Middenrealm Ducats
    */
    [Test]
    [TestCase(0, ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 0 𝔨", Description = "All 0")]
    [TestCase(0.34, ExpectedResult = "0 𝔇 3 𝔖 4 𝔥 0 𝔨", Description = "Leading and trailing 0s")]
    [TestCase(0.347, ExpectedResult = "0 𝔇 3 𝔖 4 𝔥 7 𝔨", Description = "Only leading 0s")]
    [TestCase(1.1, ExpectedResult = "1 𝔇 1 𝔖 0 𝔥 0 𝔨", Description = "Only trailing 0s")]
    [TestCase(2.749, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨", Description = "All coins represented")]
    [TestCase(2.7493333, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨", Description = "Fraction is cut off")]
    public string Format_NoFormatSpec_ExpectedBehavior(decimal Amount)
    {
        // Arrange
        // Act
        var result = string.Format(new MoneyFormatter(), "{0}", new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }



    [Test(Description = "Middenrealm currency, German locale")]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("S", 0, ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 0 𝔨", Description = "All 0")]
    [TestCase("S", 0.34, ExpectedResult = "0 𝔇 3 𝔖 4 𝔥 0 𝔨", Description = "Leading and trailing 0s")]
    [TestCase("S", 0.347, ExpectedResult = "0 𝔇 3 𝔖 4 𝔥 7 𝔨", Description = "Only leading 0s")]
    [TestCase("S", 1.1, ExpectedResult = "1 𝔇 1 𝔖 0 𝔥 0 𝔨", Description = "Only trailing 0s")]
    [TestCase("S", 2.749, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨", Description = "All coins represented")]
    [TestCase("S", 2.7493333, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨", Description = "Fraction is cut off")]
    [TestCase("L", 0, ExpectedResult = "0 Dukat 0 Silbertaler 0 Heller 0 Kreutzer", Description = "All 0")]
    [TestCase("L", 0.34, ExpectedResult = "0 Dukat 3 Silbertaler 4 Heller 0 Kreutzer", Description = "Leading and trailing 0s")]
    [TestCase("L", 0.347, ExpectedResult = "0 Dukat 3 Silbertaler 4 Heller 7 Kreutzer", Description = "Only leading 0s")]
    [TestCase("L", 1.1, ExpectedResult = "1 Dukat 1 Silbertaler 0 Heller 0 Kreutzer", Description = "Only trailing 0s")]
    [TestCase("L", 2.749, ExpectedResult = "2 Dukat 7 Silbertaler 4 Heller 9 Kreutzer", Description = "All coins represented")]
    [TestCase("L", 2.7493333, ExpectedResult = "2 Dukat 7 Silbertaler 4 Heller 9 Kreutzer", Description = "Fraction is cut off")]
    public string Format_DefaultCoinSets_ExpectedBehavior(string FormatSpec, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }



    [Test]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("SF", 0, ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 0 𝔨", Description = "All 0")]
    [TestCase("SF", 0.876, ExpectedResult = "0 𝔇 8 𝔖 7 𝔥 6 𝔨", Description = "All coins represented")]
    [TestCase("SF", 0.87666666, ExpectedResult = "0 𝔇 8 𝔖 7 𝔥 6,67 𝔨", Description = "All coins represented")]
    [TestCase("SF", 2.749, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨", Description = "All coins represented")]
    [TestCase("SF", 2.7493333, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9,33 𝔨", Description = "Fraction is cut off")]
    [TestCase("LF", 2.749, ExpectedResult = "2 Dukat 7 Silbertaler 4 Heller 9 Kreutzer", Description = "All coins represented")]
    [TestCase("LF", 2.7493333, ExpectedResult = "2 Dukat 7 Silbertaler 4 Heller 9,33 Kreutzer", Description = "Fraction is cut off")]
    public string Format_AllCoinsWithFraction_ExpectedBehavior(string FormatSpec, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }



    [Test]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("Sf", 0, ExpectedResult = "0 𝔨", Description = "All 0")]
    [TestCase("Sf", 0.0063, ExpectedResult = "6,3 𝔨", Description = "All coins represented")]
    [TestCase("Sf", 0.876, ExpectedResult = "8 𝔖 7 𝔥 6 𝔨", Description = "All coins represented")]
    [TestCase("Sf", 0.87666666, ExpectedResult = "8 𝔖 7 𝔥 6,67 𝔨", Description = "All coins represented")]
    [TestCase("Sf", 2.749, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9 𝔨", Description = "All coins represented")]
    [TestCase("Sf", 2.7493333, ExpectedResult = "2 𝔇 7 𝔖 4 𝔥 9,33 𝔨", Description = "Fraction is cut off")]
    [TestCase("Lf", 2.749, ExpectedResult = "2 Dukat 7 Silbertaler 4 Heller 9 Kreutzer", Description = "All coins represented")]
    [TestCase("Lf", 2.7493333, ExpectedResult = "2 Dukat 7 Silbertaler 4 Heller 9,33 Kreutzer", Description = "Fraction is cut off")]
    public string Format_TrailWithFraction(string FormatSpec, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }


    [Test]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("S>", 0, ExpectedResult = "0 𝔇", Description = "")]
    [TestCase("S>", 0.100, ExpectedResult = "1 𝔖 0 𝔥 0 𝔨", Description = "")]
    [TestCase("S>", 0.0063, ExpectedResult = "6 𝔨", Description = "  ")]
    [TestCase("S>", 0.876, ExpectedResult = "8 𝔖 7 𝔥 6 𝔨", Description = " ")]
    [TestCase("S>", 0.87666666, ExpectedResult = "8 𝔖 7 𝔥 6 𝔨", Description = "")]
    [TestCase("Sf", 2.7, ExpectedResult = "2 𝔇 7 𝔖 0 𝔥 0 𝔨", Description = "")]
    public string Format_NoLeadingZeroes(string FormatSpec, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }


    [Test]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("S<", 0, ExpectedResult = "0 𝔇", Description = "")]
    [TestCase("S<", 0.100, ExpectedResult = "0 𝔇 1 𝔖", Description = "")]
    [TestCase("S<", 0.0063, ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 6 𝔨", Description = "Drops fractions")]
    [TestCase("S<", 0.876, ExpectedResult = "0 𝔇 8 𝔖 7 𝔥 6 𝔨", Description = " ")]
    [TestCase("S<", 0.87666666, ExpectedResult = "0 𝔇 8 𝔖 7 𝔥 6 𝔨", Description = "")]
    [TestCase("S<", 2.7, ExpectedResult = "2 𝔇 7 𝔖", Description = "")]
    public string Format_NoTrailingZeroes(string FormatSpec, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }


    /*
     * Reference coins
     */
    [Test]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("K", "Ducat", 0.0063, ExpectedResult = "0,0063 𝔇", Description = "Key denomination")]
    [TestCase("C", "Ducat", 0.0063, ExpectedResult = "0,0063 𝔇", Description = "High denomiation")]
    [TestCase("c", "Ducat", 0.0063, ExpectedResult = "6,3 𝔨", Description = "Low denomination")]
    [TestCase("K", "Dwarven", 0.87666666, ExpectedResult = "0,87666666 ZT", Description = "")]
    [TestCase("C", "Dwarven", 0.87666666, ExpectedResult = "0,876667 ZT", Description = "")]
    [TestCase("c", "Dwarven", 0.87666666, ExpectedResult = "52,6 ZG", Description = "")]
    [TestCase("K", "Nostrian", 2.7493333, ExpectedResult = "2,7493333 Kr", Description = "")]
    [TestCase("C", "Nostrian", 2.7493333, ExpectedResult = "2,7493 Kr", Description = "")]
    [TestCase("c", "Nostrian", 2.7493333, ExpectedResult = "2,7493 Kr", Description = "")]
    public string Format_Coins_DefaultFormat(string FormatSpec, string aCurrency, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";
        Currency currency = aCurrency switch
        {
            "Ducat" => Currency.MiddenrealmDucat,
            "Dwarven" => Currency.DwarvenThaler,
            "Nostrian" => Currency.NostrianCrown,
            _ => throw new ArgumentException("Unknown currency in unit test")
        };

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, currency));

        // Assert
        return result;
    }

    [Test]
    [SetCulture("de-DE")] //[SetUICulture("de-DE")]
    [TestCase("K000.##", "Ducat", 0.0063, ExpectedResult = "000,01 𝔇", Description = "Key denomination")]
    [TestCase("K0.##", "Dwarven", 0.24, ExpectedResult = "0,24 ZT", Description = "")]
    [TestCase("C0.###", "Dwarven", 0.87666666, ExpectedResult = "0,877 ZT", Description = "")]
    [TestCase("c0.000", "Dwarven", 0.24, ExpectedResult = "14,400 ZG", Description = "")]
    [TestCase("K#.##", "Nostrian", 2.7493333, ExpectedResult = "2,75 Kr", Description = "")]
    [TestCase("K#.##", "Nostrian", 0.27493333, ExpectedResult = ",27 Kr", Description = "")]
    public string Format_Coins_UserDefFormat(string FormatSpec, string aCurrency, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";
        Currency currency = aCurrency switch
        {
            "Ducat" => Currency.MiddenrealmDucat,
            "Dwarven" => Currency.DwarvenThaler,
            "Nostrian" => Currency.NostrianCrown,
            _ => throw new ArgumentException("Unknown currency in unit test")
        };

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, currency));

        // Assert
        return result;
    }



    /*
     * OTHER UI CULTURE
    */
    [Test]
    //[SetUICulture("en-US")]
    [SetCulture("en-US")]
    [TestCase("Sf", 0.0063, ExpectedResult = "6.3 𝔨")]
    [TestCase("Sf", 0.87666666, ExpectedResult = "8 𝔖 7 𝔥 6.67 𝔨")]
    public string Format_Fractions_CultureSpecificFormat(string FormatSpec, decimal Amount)
    {
        // Arrange
        string format = "{0:" + FormatSpec + "}";

        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, Currency.MiddenrealmDucat));

        // Assert
        return result;
    }


    [Test(Description = "Middenrealm currency, English locale")]
    [SetUICulture("en-US")]
    [SetCulture("en-US")]
    [TestCase("S", "Ducat", ExpectedResult = "0 𝔇 0 𝔖 0 𝔥 0 𝔨")]
    [TestCase("L", "Ducat", ExpectedResult = "0 Ducat 0 Silverthaler 0 Haler 0 Kreutzer")]
    [TestCase("S", "Dwarven", ExpectedResult = "0 DT 0 DS 0 DP")]
    [TestCase("L", "Dwarven", ExpectedResult = "0 Thaler 0 Shilling 0 Penny")]
    [TestCase("S", "Nostrian", ExpectedResult = "0 Cr")]
    [TestCase("L", "Nostrian", ExpectedResult = "0 Crown")]
    [TestCase("S", "Dinar", ExpectedResult = "0 aĐ 0 aS 0 aĦ 0 aK")]
    public string Format_CoinSets_CultureSpecificFormat(string FormatSpec, string aCurrency)
    {
        // Arrange
        decimal Amount = 0;
        string format = "{0:" + FormatSpec + "}";
        Currency currency = aCurrency switch
        {
            "Ducat" => Currency.MiddenrealmDucat,
            "Dwarven" => Currency.DwarvenThaler,
            "Nostrian" => Currency.NostrianCrown,
            "Dinar" => Currency.AranianDinar,
            _ => throw new ArgumentException("Unknown currency in unit test")
        };
        // Act
        var result = string.Format(new MoneyFormatter(), format, new Money(Amount, currency));

        // Assert
        return result;
    }
}
