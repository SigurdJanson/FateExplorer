using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;

namespace UnitTests.Aventuria;

[TestFixture]
public class CoinSetTests
{

    [SetUp]
    public void SetUp()
    {
    }

    /// <summary>
    /// Helper function to be able to specify currencies in test cases as string.
    /// </summary>
    /// <param name="currencyStr">One of the predefined strings</param>
    /// <returns>A currency object</returns>
    /// <exception cref="System.Exception">When given a string that is not in the list.</exception>
    protected Currency String2Currency(string currencyStr)
    {
        return currencyStr switch
        {
            "MiddenrealmDucat" => Currency.MiddenrealmDucat,
            "Dwarventhaler" => Currency.DwarvenThaler,
            "NostrianCrown" => Currency.NostrianCrown,
            _ => throw new System.Exception()
        };
    }



    [Test, Description("Newly created set contains given number of coin mintages")]
    [TestCase("MiddenrealmDucat", 4)]
    [TestCase("Dwarventhaler", 3)]
    [TestCase("NostrianCrown", 1)]
    public void Constructor_NoCoins_CountCorrectMintages(string currency, int coinsInCurrency)
    {
        // Arrange
        Currency c = String2Currency(currency);
        // Act
        var coinSet = new CoinSet(c);
        // Assert
        Assert.That(coinsInCurrency, Is.EqualTo(coinSet.Count));
    }



    [Test, Description("Newly created set contains no coins")]
    [TestCase("MiddenrealmDucat")]
    [TestCase("Dwarventhaler")]
    [TestCase("NostrianCrown")]
    public void Constructor_NoCoins_CountZeroCoins(string currency)
    {
        Currency c = String2Currency(currency);

        // Arrange
        var coinSet = new CoinSet(c);
        // Assert
        Assert.That(0, Is.EqualTo(coinSet.CoinCount));
    }



    [Test, Description("Newly created set contains the given coins")]
    [TestCase("MiddenrealmDucat", new int[] {1, 2, 3, 4})]
    [TestCase("Dwarventhaler", new int[] {91, 92, 93})]
    public void Constructor_Coins_(string aCurrency, params int[] coins)
    {
        Currency currency = String2Currency(aCurrency);
        int sum = 0; foreach(int c in coins) sum += c;

        // Arrange
        var coinSet = new CoinSet(currency, coins); //CoinSet(Currency currency, params int[] coins)
        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(coinSet.Count, Is.EqualTo(coins.Length));
            Assert.That(coinSet.CoinCount, Is.EqualTo(sum));
            for (int i = 0; i < coins.Length; i++)
                Assert.That(coinSet[i], Is.EqualTo(coins[i]));
        }
    }



    [TestCase("0", new int[] { 5, 6, 942135153 }, "2")]
    public void DummyTestArray(string a, int[] values, string b)
    {
        Assert.That(values != null);
        Assert.That(a == "0");
        Assert.That(values[0] == 5);
        Assert.That(values[1] == 6);
        Assert.That(values[2] == 942135153);
        Assert.That(b == "2");
    }

    [TestCase("0", new int[] { 5, 6, 942135153 })]
    public void DummyTestParamsNArray(string a, params int[] values)
    {
        Assert.That(values != null);
        Assert.That(a == "0");
        Assert.That(values[0] == 5);
        Assert.That(values[1] == 6);
        Assert.That(values[2] == 942135153);
    }

    [TestCase("0", 5, 6, 942135153)]
    [TestCase("0", 5, 6, 942135153, 99)]
    public void DummyTestJustParams(string a, params int[] values)
    {
        Assert.That(values != null);
        Assert.That(a == "0");
        Assert.That(values[0] == 5);
        Assert.That(values[1] == 6);
        Assert.That(values[2] == 942135153);
    }


    // Finding coins by name does not seem necessary at the moment
    //[Test]
    //[TestCase("Ducat", ExpectedResult = 0)] // highest value must be first
    //[TestCase("Kreutzer", ExpectedResult = 3)] // Lowest value must be last
    //public int FindCoinIndex_ExistingName_ReturnCorrectIndex(string coinName)
    //{
    //    // Arrange
    //    var coinSet = new CoinSet(Currency.MiddenrealmDucat);

    //    // Act
    //    var result = coinSet.FindCoinIndex(coinName);

    //    // Assert
    //    return result;
    //}

    //[Test]
    //[TestCase("Dukat", ExpectedResult = -1)] // German writing
    //[TestCase("", ExpectedResult = -1)] // empty string
    //[TestCase(null, ExpectedResult = -1)] // null string
    //public int FindCoinIndex_InvalidName_ReturnMinus1(string coinName)
    //{
    //    // Arrange
    //    var coinSet = new CoinSet(Currency.MiddenrealmDucat);

    //    // Act
    //    var result = coinSet.FindCoinIndex(coinName);

    //    // Assert
    //    return result;
    //}



    [Test]
    public void StaticParseAmount_Zero_ReturnArrayOfZeroes()
    {
        // Arrange
        decimal value = 0m;
        Currency currency = Currency.MiddenrealmDucat;
        int ExpectedLength = 4;
        int[] Expected = [0, 0, 0, 0];

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
        decimal value = 1.232m;
        Currency currency = Currency.MiddenrealmDucat;
        int ExpectedLength = 4;
        int[] Expected = [1, 2, 3, 2];

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
        var coinSet = new CoinSet(Currency.MiddenrealmDucat);
        decimal value = 10.01m;

        // Act
        coinSet.ParseAmount(value);

        // Assert
        Assert.That(coinSet[0], Is.EqualTo(10));
        Assert.That(coinSet[1], Is.EqualTo(0));
        Assert.That(coinSet[2], Is.EqualTo(1));
        Assert.That(coinSet[3], Is.EqualTo(0));
    }



    [Test]
    public void Value_ReturnsCorrectMoney()
    {
        // Arrange
        CoinSet coinSet = new(Currency.MiddenrealmDucat)
        {
            [0] = 2,
            [1] = 3,
            [2] = 4,
            [3] = 5
        };
        decimal expected = 2.345m;

        // Act
        Money result = coinSet.Value;

        // Assert
        Assert.That(result, Is.EqualTo(new Money(expected, Currency.MiddenrealmDucat)));
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
        decimal expected = 1.2m * (2m + 3m / 6m + 4m * 2m / 120m)  / 1.000000000000000000000000000000000m; // 1.2 * (2 +  3/6  +  4 * 2/120) = 3.08m

        // Act
        Money result = coinSet.RefValue;

        // Assert
        Assert.That(result.ToDecimal(), Is.EqualTo(expected));
        //Assert.That(result, Is.EqualTo(new Money(expected, Currency.DwarvenThaler)));
    }





    [Test]
    [TestCase(0.0, ExpectedResult = 0.0)]
    [TestCase(1.0, ExpectedResult = 0.025)]
    [TestCase(5.0, ExpectedResult = 0.025 * 5)]
    [TestCase(15.531, ExpectedResult = 15 * 0.025 + 5 * 0.005 + 3 * 0.0025 + 1 * 0.00125)]
    public decimal Weight_StateUnderTest_ReturnCorrectWeight(decimal Amount)
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmDucat);
        coinSet.ParseAmount(Amount);

        // Act
        var result = coinSet.Weight();

        // Assert
        return result.ToStone();
    }




    [Test]
    public void Clear_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmDucat);
        for (int i = 0; i < coinSet.Count; i++) 
        {
            coinSet[i] = 1;
        }
        Assume.That(coinSet.CoinCount, Is.GreaterThan(0));

        // Act
        coinSet.Clear();

        // Assert
        Assert.That(coinSet.CoinCount, Is.EqualTo(0));
        Assert.That(coinSet.Weight, Is.EqualTo(Weight.Zero));
    }



    [Test]
    public void Contains_ReturnsCorrectAnswer()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmDucat);
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
        var coinSet = new CoinSet(Currency.MiddenrealmDucat);
        int[] array = [1, 2, 0, 4];
        int arrayIndex = 0;

        // Act
        coinSet.CopyTo(array, arrayIndex);

        // Assert
        for(int i  = 0; i < array.Length; i++)
            Assert.That(coinSet[i], Is.EqualTo(array[i]));
    }



    [Test]
    [TestCase("MiddenrealmDucat", new int[] { 1, 2, 3, 4 }, new int[] { 0, 0, 0, 0 })] // all zeroes
    [TestCase("MiddenrealmDucat", new int[] { 1, 2, 3, 4 }, new int[] { 4, 3, 2, 1 })]
    //[TestCase("MiddenrealmDucat", new int[] { 1, 2, 3, 4 }, new int[] { 4, 3, 2, 1 })]
    [TestCase("Dwarventhaler", new int[] { 91, 92, 93 }, new int[] { 1, 0, 3 })]
    public void Add_GetCorrectSums(string aCurrency, int[] coins, int[] moreCoins)
    {
        // Arrange
        Currency currency = String2Currency(aCurrency);
        int sum = 0; foreach (int c in coins) sum += c;
        var coinSet = new CoinSet(currency, coins);
        var coinSetToAdd = new CoinSet(currency, moreCoins);

        // Act
        coinSet.Add(coinSetToAdd);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            for (int i = 0; i < coins.Length; i++)
                Assert.That(coinSet[i], Is.EqualTo(coins[i] + moreCoins[i]));
        }
    }


    [Test]
    public void Add_WrongCurrency()
    {
        // Arrange

        var coinSet = new CoinSet(Currency.MiddenrealmDucat, [1, 2, 3, 4]);
        var coinSetToAdd = new CoinSet(Currency.AlanfaDoubloon, [3, 4, 5, 6]);

        // Act
        // Assert
        Assert.That(() => coinSet.Add(coinSetToAdd), Throws.ArgumentException);
    }




    [Test]
    public void Remove_NotImplemented()
    {
        // Arrange
        var coinSet = new CoinSet(Currency.MiddenrealmDucat)
        {
            [0] = 1, [1] = 1, [2] = 1, [3] = 1
        };
        int coin = 1;

        // Act
        // Assert
        Assert.That(() => coinSet.Remove(coin), Throws.Exception);
    }

}
