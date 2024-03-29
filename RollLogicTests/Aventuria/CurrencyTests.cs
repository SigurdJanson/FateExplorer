﻿using Aventuria;
using Moq;
using NUnit.Framework;

namespace UnitTests.Aventuria;

[TestFixture]
public class CurrencyTests
{
    private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        mockRepository = new MockRepository(MockBehavior.Strict);


    }

    //private Currency CreateCurrency()
    //{
    //    return new Currency(TODO, TODO);
    //}

    [Test]
    public void MiddenrealmThaler_CoinNames_ReturnsExpectedValue()
    {
        var currency = Currency.MiddenrealmThaler;
        var expected = new string[] { "Ducat", "Silverthaler", "Haler", "Kreutzer" };

        Assert.AreEqual(expected, currency.CoinNames);
    }

    [Test]
    public void DwarvenThaler_CoinValue_ReturnsExpectedValue()
    {
        var currency = Currency.DwarvenThaler;
        var expected = new decimal[] { 12, 2, 0.2m };

        Assert.AreEqual(expected, currency.CoinValue);
    }

    [Test]
    public void PaaviGuilder_CoinAbbr_ReturnsExpectedValue()
    {
        var currency = Currency.PaaviGuilder;
        var expected = new string[] { "R" };

        Assert.AreEqual(expected, currency.CoinAbbr);
    }

    [Test]
    public void NostrianCrown_Rate_ReturnsExpectedValue()
    {
        var currency = Currency.NostrianCrown;
        var expected = 5.0m;

        Assert.AreEqual(expected, currency.Rate);
    }

    [Test]
    public void Andrathaler_CoinWeight_ReturnsExpectedValue()
    {
        var currency = Currency.Andrathaler;
        var expected = new decimal[] { 0.025m };

        Assert.AreEqual(expected, currency.CoinWeight);
    }

    [Test]
    public void Horasdor_Origin_ReturnsExpectedValue()
    {
        var currency = Currency.Horasdor;
        var expected = new Region[] { Region.Fairfields };

        Assert.AreEqual(expected, currency.Origin);
    }

    [Test, Culture("de-DE")]
    public void AlanfaOreal_NativeCoinNames_ReturnsExpectedValue()
    {
        var currency = Currency.AlanfaOreal;
        var expected = new string[] { "Dublone", "Oreal", "Kleiner Oreal", "Dirham" };

        Assert.AreEqual(expected, currency.NativeCoinNames);
    }
    [Test, Culture("en-GB")]
    public void AlanfaOreal_NativeCoinNames_En_ReturnsExpectedValue()
    {
        var currency = Currency.AlanfaOreal;
        var expected = new string[] { "Dubloon", "Oreal", "Small Oreal", "Dirham" };

        Assert.AreEqual(expected, currency.NativeCoinNames);
    }

    [Test]
    public void BornlandPenny_NativeCoinSymbols_ReturnsExpectedValue()
    {
        var currency = Currency.BornlandPenny;
        var expected = new string[] { "BB", "BG", "BD" };

        Assert.AreEqual(expected, currency.NativeCoinSymbols);
    }



    [Test, Ignore("This is a template")]
    public void TestMethod1()
    {
        // Arrange
        var currency = Currency.BornlandPenny;

        // Act


        // Assert
        Assert.Fail();
        mockRepository.VerifyAll();
    }


}
