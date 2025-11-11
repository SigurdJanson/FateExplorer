namespace Aventuria;


/// <summary>
/// Represents a currency used in Aventuria. Specifies names, symbols, and value.<br/>
/// <list type="bullet">
/// <item><description>Coin value is specified relative to middenrealm Ducats.</description></item>
/// <item><description>Every currency has a key coin. Currencies are referenced by that key coin. Non-native names are used. Use the coin Aventurians would use.</description></item>
/// <item><c>KeyCoin * Rate</c> gives the value of 1 key coin in Ducat.</item>
/// </list>
/// </summary>
public class Currency : Enumeration
{
    /// <summary>
    /// Coin name (sorted by <see cref="CoinValue">coin value</see>)
    /// </summary>
    public required string[] CoinNames { get; init; }
    /// <summary>
    /// Coin abbreviations / symbols / codes (sorted by <see cref="CoinValue">coin value</see>)
    /// </summary>
    public required string[] CoinCodes { get; init; }

    /// <summary>
    /// Localised coin names in the language of the Aventurian people (sorted by <see cref="CoinValue">coin value</see>)
    /// </summary>
    public required string[] NativeCoinNames { get; init; }
    /// <summary>
    /// Localised coin abbreviations in the language of the Aventurian people (sorted by <see cref="CoinValue">coin value</see>)
    /// </summary>
    public required string[] NativeCoinCodes { get; init; }


    /// <summary>
    /// Par value (dt. Nennwert). The <see cref="KeyCoinIndex">key coin</see> must have a value 
    /// of 1. Other coin values are in relation to that. Coin values must be ordered by value 
    /// starting with the highest.
    /// </summary>
    public required decimal[] CoinValue { get; init; }

    /// <summary>
    /// Real value (dt. Realwert) i.e. the material value rather than the value appointed by the 
    /// minting authority.<br/>
    /// Values must be ordered by <see cref="CoinValue">par value</see>
    /// </summary>
    public required decimal[] CoinRealValue { get; init; }

    /// <summary>
    /// Each coin's weight in Stone.<br/>
    /// Values must be ordered by <see cref="CoinValue">par value</see>
    /// </summary>
    public required decimal[] CoinWeight { get; init; }

    /// <summary>
    /// The regions where the currency is commonly traded with
    /// </summary>
    public required Region[] Origin { get; init; }

    /// <summary>
    /// The coin to identify the currency with.
    /// </summary>
    public required int KeyCoinIndex { get; init; }

    /// <summary>
    /// The name of the key coin
    /// </summary>
    public string KeyCoin { get => CoinNames[KeyCoinIndex]; }

    /// <summary>
    /// The code of the key coin
    /// </summary>
    public string KeyCoinCode { get => CoinCodes[KeyCoinIndex]; }

    /// <summary>
    /// Retrieve the number of coins available in the currency system.
    /// </summary>
    public int CoinCount { get => CoinValue.Length; }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">User-defined name of the currency</param>
    /// <param name="value">The numeric index of the enumeration</param>
    public Currency(string name, int value) : base(name, value)
    {}

    /// <summary>
    /// The exchange rate defined by the rate between the two key (reference)
    /// coin of Middenrealm Ducat and the current currency.
    /// This rate is used to convert money amounts stored as decimals.
    /// </summary>
    public decimal Rate { get; init; }


    /// <summary>
    /// Create and return an instance of the reference currency.
    /// </summary>
    public static Currency ReferenceCurrency => MiddenrealmDucat;


    public static Currency MiddenrealmDucat =>
        new(nameof(MiddenrealmDucat), 1)
        {
            CoinNames = [Properties.Resources.MiddenrealmDucatName, 
                Properties.Resources.MiddenrealmSilverthalerName,
                Properties.Resources.MiddenrealmHalerName, 
                Properties.Resources.MiddenrealmKreutzerName],
            CoinCodes = [Properties.Resources.MiddenrealmDucatAbbr,
                Properties.Resources.MiddenrealmSilverthalerAbbr,
                Properties.Resources.MiddenrealmHalerAbbr,
                Properties.Resources.MiddenrealmKreutzerAbbr],
            Rate = 1.0m,
            CoinValue = [1, 0.1m, 0.01m, 0.001m],
            CoinRealValue = [10, 1, 0.1m, 0.01m],
            CoinWeight = [0.025m, 0.005m, 0.0025m, 0.00125m],
            Origin = [Region.Middenrealm],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames =
            [
                Properties.Resources.MiddenrealmDucatName,
                Properties.Resources.MiddenrealmSilverthalerName,
                Properties.Resources.MiddenrealmHalerName,
                Properties.Resources.MiddenrealmKreutzerName
            ],
            NativeCoinCodes =
            [
                Properties.Resources.MiddenrealmDucatAbbr,
                Properties.Resources.MiddenrealmSilverthalerAbbr,
                Properties.Resources.MiddenrealmHalerAbbr,
                Properties.Resources.MiddenrealmKreutzerAbbr 
            ]
        };

    public static Currency DwarvenThaler => // Mountain Kingdoms
        new(nameof(DwarvenThaler), 2)
        {
            CoinNames = [Properties.Resources.DwarvenThalerName,
                Properties.Resources.DwarvenShillingName,
                Properties.Resources.DwarvenPennyName],
            CoinCodes = [Properties.Resources.DwarvenThalerAbbr,
                Properties.Resources.DwarvenShillingAbbr,
                Properties.Resources.DwarvenPennyAbbr],
            Rate = 1.20m,
            CoinValue = [1.0m, 2.0m/12m, 0.2m/12m],
            CoinRealValue = [12, 2, 0.2m],
            CoinWeight = [0.025m, 0.01m, 0.01m],
            Origin = [Region.CentralMountainKingdoms, Region.SouthernMountainKingdoms],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames =
            [
                Properties.Resources.DwarvenAuromoxName, 
                Properties.Resources.DwarvenArganbroxName, 
                Properties.Resources.DwarvenAtebroxName 
            ],
            NativeCoinCodes =
            [
                Properties.Resources.DwarvenAuromoxAbbr, 
                Properties.Resources.DwarvenArganbroxAbbr,
                Properties.Resources.DwarvenAtebroxAbbr 
            ]
        };

    public static Currency PaaviGuilder =>
        new(nameof(PaaviGuilder), 3)
        {
            CoinNames = [Properties.Resources.PaaviGuilderName],
            CoinCodes = [Properties.Resources.PaaviGuilderAbbr],
            Rate = 5.0m,
            CoinValue = [1],
            CoinRealValue = [1],
            CoinWeight = [0.0125m],
            Origin = [Region.PaaviRegion],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames = [Properties.Resources.PaaviGuilderName],
            NativeCoinCodes = [Properties.Resources.PaaviGuilderAbbr]
        };

    public static Currency NostrianCrown =>
        new(nameof(NostrianCrown), 4)
        {
            CoinNames = [Properties.Resources.NostrianCrownName],
            CoinCodes = [Properties.Resources.NostrianCrownAbbr],
            Rate = 5.0m,
            CoinValue = [1],
            CoinRealValue = [0.5m],
            CoinWeight = [0.025m], // 25
            Origin = [Region.Nostria],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames = [Properties.Resources.NostrianCrownName],
            NativeCoinCodes = [Properties.Resources.NostrianCrownAbbr]
        };

    public static Currency Andrathaler =>
        new(nameof(Andrathaler), 5)
        {
            CoinNames = [Properties.Resources.AndrathalerName],
            CoinCodes = [Properties.Resources.AndrathalerAbbr],
            Rate = 5.0m,
            CoinValue = [1],
            CoinRealValue = [0.5m],
            CoinWeight = [0.025m], // 25
            Origin = [Region.Andergast],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames = [Properties.Resources.AndrathalerName],
            NativeCoinCodes = [Properties.Resources.AndrathalerAbbr]
        };


    public static Currency Horasdor =>
        new (nameof(Horasdor), 6)
        {
            CoinNames = [
                Properties.Resources.HorasdorName,
                Properties.Resources.MiddenrealmDucatName,
                Properties.Resources.MiddenrealmSilverthalerName,
                Properties.Resources.MiddenrealmHalerName,
                Properties.Resources.MiddenrealmKreutzerName
            ],
            CoinCodes = [
                Properties.Resources.HorasdorAbbr,
                Properties.Resources.MiddenrealmDucatAbbr,
                Properties.Resources.MiddenrealmSilverthalerAbbr,
                Properties.Resources.MiddenrealmHalerAbbr,
                Properties.Resources.MiddenrealmKreutzerAbbr
            ],
            Rate = 20.0m,
            CoinValue = [1, 1/20m, 1/200m, 0.1m/200m, 0.01m/200m],
            CoinRealValue = [1.0m, 1.0m / 20m, 1.0m / 200m, 0.1m / 200m, 0.01m / 200m],
            CoinWeight = [0.5m, 0.025m, 0.005m, 0.0025m, 0.00125m], // 500, 25, 5, 2.5, 1.25
            Origin = [Region.Fairfields],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames =
            [
                Properties.Resources.HorasdorName,
                Properties.Resources.MiddenrealmDucatName,
                Properties.Resources.MiddenrealmSilverthalerName,
                Properties.Resources.MiddenrealmHalerName,
                Properties.Resources.MiddenrealmKreutzerName
            ],
            NativeCoinCodes =
            [
                Properties.Resources.HorasdorAbbr,
                Properties.Resources.MiddenrealmDucatAbbr,
                Properties.Resources.MiddenrealmSilverthalerAbbr,
                Properties.Resources.MiddenrealmHalerAbbr,
                Properties.Resources.MiddenrealmKreutzerAbbr
            ]
        };

    public static Currency AlanfaDoubloon =>
        new(nameof(AlanfaDoubloon), 7)
        {
            CoinNames = [Properties.Resources.AlanfaDoubloonName,
                Properties.Resources.AlanfaOrealName,
                Properties.Resources.AlanfaSmallOrealName,
                Properties.Resources.AlanfaDirhamName],
            CoinCodes = [Properties.Resources.AlanfaDoubloonAbbr,
                Properties.Resources.AlanfaOrealAbbr,
                Properties.Resources.AlanfaSmallOrealAbbr,
                Properties.Resources.AlanfaDirhamAbbr],
            Rate = 2.0m,
            CoinValue = [1, 0.05m, 0.025m, 0.0005m],
            CoinRealValue = [1, 0.05m, 0.025m, 0.0005m],
            CoinWeight = [0.050m, 0.005m, 0.003m, 0.003m], // 50, 5, 3, 3
            Origin = [Region.AlAnfaRegion],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames =
            [
                Properties.Resources.AlanfaDoubloonName,
                Properties.Resources.AlanfaOrealName,
                Properties.Resources.AlanfaSmallOrealName,
                Properties.Resources.AlanfaDirhamName
            ],
            NativeCoinCodes =
            [
                Properties.Resources.AlanfaDoubloonAbbr,
                Properties.Resources.AlanfaOrealAbbr,
                Properties.Resources.AlanfaSmallOrealAbbr,
                Properties.Resources.AlanfaDirhamAbbr
            ]
        };

    public static Currency BornlandLump =>
        new(nameof(BornlandLump), 8)
        {
            CoinNames = [Properties.Resources.BornlandLumpName,
                Properties.Resources.BornlandPennyName,
                Properties.Resources.BornlandSlightlingName],
            CoinCodes = [Properties.Resources.BornlandLumpAbbr,
                Properties.Resources.BornlandPennyAbbr,
                Properties.Resources.BornlandSlightlingAbbr],
            Rate = 1.0m,
            CoinValue = [1, 0.1m, 0.01m],
            CoinRealValue = [1, 0.1m, 0.01m],
            CoinWeight = [0.025m, 0.005m, 0.005m], // 25, 5, 5
            Origin = [Region.Bornland],
            KeyCoinIndex = 0,

            // set according to UI language
            NativeCoinNames =
            [
                Properties.Resources.BornlandLumpName,
                Properties.Resources.BornlandPennyName,
                Properties.Resources.BornlandSlightlingName
            ],
            NativeCoinCodes =
            [
                Properties.Resources.BornlandLumpAbbr,
                Properties.Resources.BornlandPennyAbbr,
                Properties.Resources.BornlandSlightlingAbbr
            ]
        };


    public static Currency AranianDinar =>
    new(nameof(AranianDinar), 9)
    {
        CoinNames = [
            Properties.Resources.AranianDinarName,
            Properties.Resources.AranianShekelName,
            Properties.Resources.AranianHallahName,
            Properties.Resources.AranianKurushName
        ],
        CoinCodes = [
            Properties.Resources.AranianDinarAbbr,
            Properties.Resources.AranianShekelAbbr,
            Properties.Resources.AranianHallahAbbr,
            Properties.Resources.AranianKurushAbbr
        ],
        Rate = 1.0m,
        CoinValue = [1, 0.1m, 0.01m, 0.001m],
        CoinRealValue = [1, 0.1m, 0.01m, 0.001m],
        CoinWeight = [0.025m, 0.005m, 0.0025m, 0.00125m],
        Origin = [Region.Arania, Region.Tulamidya, Region.ThalusaRegion],
        KeyCoinIndex = 0,

        // set according to current culture
        NativeCoinNames =
        [
            Properties.Resources.AranianDinarName,
            Properties.Resources.AranianShekelName,
            Properties.Resources.AranianHallahName,
            Properties.Resources.AranianKurushName
        ],
        NativeCoinCodes =
        [
            Properties.Resources.AranianDinarAbbr,
            Properties.Resources.AranianShekelAbbr,
            Properties.Resources.AranianHallahAbbr,
            Properties.Resources.AranianKurushAbbr
        ]
    };


    public static Currency CaliphateMaravedi =>
    new(nameof(CaliphateMaravedi), 10)
    {
        CoinNames = [
            Properties.Resources.CaliphateMaravediName,
            Properties.Resources.CaliphateZechineName,
            Properties.Resources.CaliphateMuwlatName
        ],
        CoinCodes = [
            Properties.Resources.CaliphateMaravediAbbr,
            Properties.Resources.CaliphateZechineAbbr,
            Properties.Resources.CaliphateMuwlatAbbr
        ],
        Rate = 2.0m,
        CoinValue = [1, 0.1m, 0.0025m],
        CoinRealValue = [1, 0.1m, 0.0025m],
        CoinWeight = [0.05m, 0.01m, 0.0025m], // 50, 10, 2.5
        Origin = [Region.Khom, Region.SzintoRegion],
        KeyCoinIndex = 0,

        // set according to current culture
        NativeCoinNames =
        [
            Properties.Resources.CaliphateMaravediName,
            Properties.Resources.CaliphateZechineName,
            Properties.Resources.CaliphateMuwlatName
        ],
        NativeCoinCodes =
        [
            Properties.Resources.CaliphateMaravediAbbr,
            Properties.Resources.CaliphateZechineAbbr,
            Properties.Resources.CaliphateMuwlatAbbr
        ]
    };
    /*
     * TEMPLATE
     */
    //public static Currency PaaviGuilder =>
    //    new(nameof(PaaviGuilder), 11)
    //    {

    //    };
}