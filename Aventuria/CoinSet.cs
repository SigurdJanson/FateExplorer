namespace Aventuria;

/// <summary>
/// A collection to count coins of a given currency.
/// </summary>
public class CoinSet : ICollection<int>, IEnumerable<int>
{
    private int[] Coin { get; set; }
    public required Currency Currency { get; init; }

    /// <summary>
    /// Returns the coins as monetary value it the currency of the CoinSet.
    /// </summary>
    public Money Value => new(JoinAmount(), Currency);

    /// <summary>
    /// Returns the coins as monetary value in the reference currency.
    /// </summary>
    public Money RefValue => new(JoinAmount() * Currency.Rate, Currency.ReferenceCurrency);


    public int Count => Coin.Length; // ICollection

    public bool IsReadOnly => false; // ICollection


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="currency">For a collection of coins the currency must be specified.</param>
    public CoinSet(Currency currency)
    {
        Currency = currency;
        Coin = new int[currency.CoinValue.Length];
    }


    /// <summary>
    /// Constructor that also initialises the number of coins in the set.
    /// </summary>
    /// <param name="currency">For a collection of coins the currency must be specified.</param>
    /// <param name="coins">The number of coins in the set. Starts with the coin with highest value 
    /// and goes down. There cannot be fewer coins specified by the currency.</param>
    public CoinSet(Currency currency, params int[] coins)
    {
        Currency = currency;
        Coin = new int[currency.CoinValue.Length];

        if (coins.Length != Coin.Length) 
            throw new ArgumentException("The number of coins given must match the different coins of `currency`");
        for (int i = 0; i < coins.Length; i++)
            Coin[i] = coins[i];
    }


    /// <summary>
    /// Get or set the number of coins for a specific coin identified by it's numeric index.
    /// </summary>
    /// <param name="index">Numeric index. Coins are sorted according the the </param>
    /// <returns></returns>
    public int this[int index]
    {
        get => Coin[index];
        set => Coin[index] = value;
    }

    /// <summary>
    /// Get or set the number of coins for a specific coin identified by it's name.
    /// </summary>
    /// <param name="coin"></param>
    /// <returns></returns>
    public int this[string coin]
    {
        get => Coin[FindCoinIndex(coin)];
        set => Coin[FindCoinIndex(coin)] = value;
    }

    /// <summary>
    /// Searches for the coin with the given name and and returns the zero-based 
    /// index of the occurrence within the range.
    /// </summary>
    /// <param name="coin">The english name of the coin as specified </param>
    /// <returns>The zero-based index of the first occurrence of an element that 
    /// matches the name, if found; otherwise, -1.</returns>
    public int FindCoinIndex(string coin)
    {
        for (int i = 0; i < Currency.CoinNames.Length; i++)
            if (Currency.CoinNames[i] == coin) return i;
        return -1;
    }

    /// <summary>
    /// Takes an amount, converts it into a set of coins.
    /// It creates the smallest set of coins possible to represent the given amount of money.
    /// </summary>
    /// <param name="value">An amount of money</param>
    /// <param name="currency">The currency of the amount</param>
    /// <returns>An array of <c>int</c> with one element for each coin of the given currency.</returns>
    public static int[] ParseAmount(decimal value, Currency currency)
    {
        var coins = new int[currency.CoinValue.Length];
        for (int c = 0; c < coins.Length; c++)
        {
            // Get coin value
            decimal CoinValue = currency.CoinValue[c];

            // Determine maximum amount
            int Count = (int)(value % CoinValue);
            c += Count;
            value -= CoinValue * Count;
            coins[c] = Count;
        }
        return coins;
    }

    /// <summary>
    /// Takes an amount, converts it into a set of coins, and replaces the current amount.
    /// It creates the smallest set of coins possible to represent the given amount of money.
    /// </summary>
    /// <param name="value">An amount of money of the same currency.</param>
    public void ParseAmount(decimal value)
    {
        Coin = ParseAmount(value, Currency);
    }


    /// <summary>
    /// Compute the total weight of the coins.
    /// </summary>
    /// <returns>Weight in Stone</returns>
    public decimal Weight()
    {
        decimal result = 0;
        for(var c = 0; c < Coin.Length; c++)
        {
            result += Coin[c] * Currency.CoinWeight[c];
        }
        return result;
    }


    /// <summary>
    /// Returns the total value of the coins.
    /// </summary>
    protected decimal JoinAmount()
    {
        decimal Result = 0;
        for (int c = 0; c < Coin.Length; c++)
            Result += Currency.CoinValue[c] * Coin[c];
        return Result;
    }

    #region implement ICollection<int>

    public void Add(int item)
    {
        throw new NotImplementedException();
    }


    public void Clear() => Array.Clear(Coin);


    public bool Contains(int item) => Coin[item] != 0;


    public void CopyTo(int[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }


    public bool Remove(int item)
    {
        Coin[item] = 0;
        return true;
    }



    public IEnumerator<int> GetEnumerator()
    {
        foreach(var coin in Coin) yield return coin;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}
