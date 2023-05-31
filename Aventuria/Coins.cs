namespace Aventuria;

// TODO: still a draft
public class Coins
{
    public int[] Coin { get; private set; }
    public Currency Currency { get; private set; }

    /// <summary>
    /// Returns the coins as monetary value.
    /// </summary>
    public Money Value => new(JoinAmount(), Currency);

    /// <summary>
    /// Returns the coins as monetary value in the reference currency.
    /// </summary>
    public Money RefValue => new(JoinAmount() * Currency.Rate, Currency.ReferenceCurrency);


    public Coins(Currency currency)
    {
        Currency = currency;
        Coin = new int[currency.CoinValue.Length];
    }

    /// <summary>
    /// Takes an amount, converts it into coins, and 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public void ParseAmount(decimal value)
    {
        for (int c = 0; c < Coin.Length; c++)
        {
            // Get coin value
            decimal CoinValue = Currency.CoinValue[c];

            // Determine maximum amount
            int Count = (int)(value % CoinValue);
            c += Count;
            value -= CoinValue * Count;
            Coin[c] = Count;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    protected decimal JoinAmount()
    {
        decimal Result = 0;
        for (int c = 0; c < Coin.Length; c++)
            Result += Currency.CoinValue[c] * Coin[c];
        return Result;
    }

}
