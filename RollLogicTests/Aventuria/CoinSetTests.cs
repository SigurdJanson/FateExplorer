using Aventuria;
using Moq;
using NUnit.Framework;

namespace UnitTests.Aventuria;

[TestFixture]
public class CoinSetTests
{

    [SetUp]
    public void SetUp()
    {
    }



    [Test, Description("Newly created set contains given number of coin mintages")]
    public void Constructor_NoCoins_CountCorrectMintages()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        // Assert
        Assert.That(4, Is.EqualTo(coinSet.Count));
    }



    [Test, Description("Newly created set contains no coins")]
    public void Constructor_NoCoins_CountZeroCoins()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        // Assert
        Assert.That(0, Is.EqualTo(coinSet.CoinCount));
    }



    [Test]
    [TestCase("Ducat", ExpectedResult = 0)] // highest value must be first
    [TestCase("Kreutzer", ExpectedResult = 3)] // Lowest value must be last
    public int FindCoinIndex_ExistingName_ReturnCorrectIndex(string coinName)
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);

        // Act
        var result = coinSet.FindCoinIndex(coinName);

        // Assert
        return result;
    }

    [Test]
    [TestCase("Dukat", ExpectedResult = -1)] // German writing
    [TestCase("", ExpectedResult = -1)] // empty string
    [TestCase(null, ExpectedResult = -1)] // null string
    public int FindCoinIndex_InvalidName_ReturnMinus1(string coinName)
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);

        // Act
        var result = coinSet.FindCoinIndex(coinName);

        // Assert
        return result;
    }



    [Test]
    public void StaticParseAmount_Zero_ReturnArrayOfZeroes()
    {
        // Arrange
        decimal value = 0m;
        Currency currency = Currency.MiddenrealmThaler;
        int ExpectedLength = 4;
        int[] Expected = new int[] { 0, 0, 0, 0 };

        // Act
        var result = CoinSet.ParseAmount(value, currency);

        // Assert
        Assert.That(result.Length, Is.EqualTo(ExpectedLength));
        Assert.That(result, Is.EqualTo(Expected));
    }


    [Test]
    public void StaticParseAmount_NotZero_ReturnCorrectValues()
    {
        // Arrange
        decimal value = 12.32m;
        Currency currency = Currency.MiddenrealmThaler;
        int ExpectedLength = 4;
        int[] Expected = new int[] { 1, 2, 3, 2 };

        // Act
        var result = CoinSet.ParseAmount(value, currency);

        // Assert
        Assert.That(result.Length, Is.EqualTo(ExpectedLength));
        Assert.That(result, Is.EqualTo(Expected));
    }


    [Test]
    public void ParseAmount_ValidValue_SetsCoinsCorrectly()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        decimal value = 10.01m;

        // Act
        coinSet.ParseAmount(value);

        // Assert
        Assert.That(coinSet[0], Is.EqualTo(1));
        Assert.That(coinSet[1], Is.EqualTo(0));
        Assert.That(coinSet[2], Is.EqualTo(0));
        Assert.That(coinSet[3], Is.EqualTo(1));
    }



    [Test]
    public void Value_ReturnsCorrectMoney()
    {
        // Arrange
        CoinSet coinSet = new(Currency.MiddenrealmThaler)
        {
            [0] = 2,
            [1] = 3,
            [2] = 4,
            [3] = 5
        };
        decimal expected = 23.45m;

        // Act
        Money result = coinSet.Value;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(expected, Currency.MiddenrealmThaler)));
    }


    [Test]
    public void RefValue_OtherCurrencyThanRef_ReturnsCorrectMoney()
    {
        // Arrange
        CoinSet coinSet = new(Currency.DwarvenThaler)
        {
            [0] = 2,
            [1] = 3,
            [2] = 4
        };
        decimal expected = 30.8m; // 2 * 12  +  3 * 2  +   4 * 0.2

        // Act
        Money result = coinSet.RefValue;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(expected, Currency.MiddenrealmThaler)));
    }





    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(1, ExpectedResult = 0.005)]
    [TestCase(5, ExpectedResult = 0.005 * 5)]
    [TestCase(15.5, ExpectedResult = 0.025 + 5 * 0.005 + 5 * 0.0025)]
    public decimal Weight_StateUnderTest_ReturnCorrectWeight(decimal Amount)
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        coinSet.ParseAmount(Amount);

        // Act
        var result = coinSet.Weight();

        // Assert
        return result;
    }




    [Test]
    public void Clear_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        for (int i = 0; i < coinSet.Count; i++) 
        {
            coinSet[i] = 1;
        }
        Assume.That(coinSet.CoinCount, Is.GreaterThan(0));

        // Act
        coinSet.Clear();

        // Assert
        Assert.That(coinSet.CoinCount, Is.EqualTo(0));
        Assert.That(coinSet.Weight, Is.EqualTo(0));
    }



    [Test]
    public void Contains_ReturnsCorrectAnswer()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        for (int i = 0; i < coinSet.Count; i++)
            coinSet[i] = i + 1;

        // Act

        // Assert
        Assert.That(coinSet.Contains(1), Is.True);
        Assert.That(coinSet.Contains(4), Is.True);
        Assert.That(coinSet.Contains(0), Is.False);
        Assert.That(coinSet.Contains(5), Is.False);
    }


    [Test]
    public void CopyTo_AllCoins_SetsCoinsCorrectly()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        int[] array = new int[] { 1, 2, 0, 4 };
        int arrayIndex = 0;

        // Act
        coinSet.CopyTo(array, arrayIndex);

        // Assert
        for(int i  = 0; i < array.Length; i++)
            Assert.That(coinSet[i], Is.EqualTo(array[i]));
    }



    [Test]
    public void Add_NotImplemented()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler);
        int coin = 1;

        // Act
        // Assert
        Assert.That(() => coinSet.Add(coin), Throws.Exception);
    }

    [Test]
    public void Remove_NotImplemented()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmThaler)
        {
            [0] = 1, [1] = 1, [2] = 1, [3] = 1
        };
        int coin = 1;

        // Act
        // Assert
        Assert.That(() => coinSet.Remove(coin), Throws.Exception);
    }

}
