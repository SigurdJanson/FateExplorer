using Aventuria.Measures;
using System.Diagnostics.CodeAnalysis;

namespace Aventuria;

/// <summary>
/// A collection to count coins of a given currency.
/// </summary>
public class CoinSet : ICollection<int>, IEnumerable<int>
{
    private int[] Coin { get; set; }
    public required Currency Currency { get; init; }

    /// <summary>
    /// Returns the coins as monetary value of the currency of the CoinSet.
    /// </summary>
    public Money Value => new(JoinAmount(), Currency);

    /// <summary>
    /// Returns the coins as monetary value in the reference currency.
    /// </summary>
    public Money RefValue => new Money(JoinAmount(), Currency).ToCurrency(Currency.ReferenceCurrency); // * Currency.Rate, Currency.ReferenceCurrency);


    public int Count => Coin.Length; // ICollection

    /// <summary>
    /// Gets the total number of coins
    /// </summary>
    public int CoinCount => Coin.Sum(x => x);

    public bool IsReadOnly => false; // ICollection


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="currency">For a collection of coins the currency must be specified.</param>
    [SetsRequiredMembers]
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
    [SetsRequiredMembers]
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
            int Count = (int)(value / CoinValue);
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
    /// Compute the total <see cref="Measures.Weight">weight</see> of the coins in the default unit.
    /// </summary>
    /// <returns>Weight in Stone</returns>
    public Weight Weight()
    {
        double result = 0;
        for(var c = 0; c < Coin.Length; c++)
        {
            result += Coin[c] * Currency.CoinWeight[c];
        }
        return new Weight(result);
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

    /// <summary>
    /// Not supported by <c>CoinSet</c>
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Add(int item)
    {
        throw new NotImplementedException();
    }


    public void Clear() => Array.Clear(Coin);


    public bool Contains(int item)
    {
        foreach (var coin in Coin)
            if (coin == item) return true;
        return false;
    }


    public void CopyTo(int[] array, int arrayIndex)
    {
        for (int i = arrayIndex; i < Coin.Length; i++)
            Coin[i] = array[i-arrayIndex];
    }


    public bool Remove(int item)
    {
        throw new NotImplementedException();
    }



    public IEnumerator<int> GetEnumerator()
    {
        foreach(var coin in Coin) yield return coin;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion


    #region Math
    /// <summary>
    /// Add another set of coins of the same currency.
    /// </summary>
    /// <param name="coins">A coin set to add</param>
    /// <exception cref="ArgumentException">When the currencies do not match.</exception>
    public void Add(CoinSet coins)
    {
        if (coins.Currency != Currency)
            throw new ArgumentException("Trying to add a currency that does not match this coin set");
        for (int i = 0; i < Coin.Length; i++)
        {
            if (coins[i] < 0) throw new ArgumentOutOfRangeException($"{nameof(coins)}[{i}] was negative");
            Coin[i] += coins[i];
        }
    }
    #endregion
}
