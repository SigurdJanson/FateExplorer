namespace Aventuria;


// TODO: still a draft
public class Currency : Enumeration
{


    public string[] CoinNames;
    public string[] NativeCoinNames;

    public string[] CoinSymbols;
    public string[] NativeCoinSymbols;

    /// <summary>
    /// Par value (dt. Nennwert)
    /// </summary>
    public decimal[] CoinValue;
    /// <summary>
    /// Real value (dt. Realwert)
    /// </summary>
    public decimal[] CoinRealValue;
    /// <summary>
    /// Each coin's weight in gramm
    /// </summary>
    public decimal[] CoinWeight;

    public Currency(string name, int value) : base(name, value)
    {
    }

    /// <summary>
    /// The exchange rate defined by the rate between the two reference
    /// Coin of Middenrealm (Silver-) Thaler and the current currency.
    /// </summary>
    public decimal Rate { get; init; }




    public static Currency Reference => MiddenrealmThaler();

    public static Currency MiddenrealmThaler()
    {
        Currency Middenrealm = new(nameof(Middenrealm), 1)
        {
            CoinNames = new string[] { "Dukat", "Silbertaler", "Heller", "Kreutzer" },
            CoinSymbols = new string[] { "D", "S", "H", "K" },
            Rate = 1.0m,
            CoinValue = new decimal[] { 10, 1, 0.1m, 0.01m },
            CoinRealValue = new decimal[] { 10, 1, 0.1m, 0.01m },
            CoinWeight = new decimal[] { 25, 5, 2.5m, 1.25m },

            // set according to UI language
            NativeCoinNames = new string[] { "Dukat", "Silbertaler", "Heller", "Kreutzer" },
            NativeCoinSymbols = new string[] { "D", "S", "H", "K"}
        };

        return Middenrealm;
    }

    public static Currency MountainKingdomDwarvenThaler()
    {
        Currency DwarvenThaler = new(nameof(DwarvenThaler), 2)
        {
            CoinNames = new string[] { "Auromox", "Arganbrox", "Atebrox" },
            CoinSymbols = new string[] { "R", "G", "T" },
            Rate = 12.0m,
            CoinValue = new decimal[] { 1, 1 / 6, 1 / 60 },
            CoinRealValue = new decimal[] { 10, 1, 0.1m, 0.01m },
            CoinWeight = new decimal[] { 1, 1, 1 },

            // set according to UI language
            NativeCoinNames = new string[] { "Auromox", "Arganbrox", "Atebrox" },
            NativeCoinSymbols = new string[] { "R", "G", "T" }
        };

        return DwarvenThaler;
    }
}