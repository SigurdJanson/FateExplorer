namespace Aventuria;


// TODO: still a draft
public class Currency : Enumeration
{
    /// <summary>
    /// Internally used coin names in English
    /// </summary>
    public required string[] CoinNames { get; init; }
    /// <summary>
    /// Internally used coin abbreviations in English
    /// </summary>
    public required string[] CoinAbbr { get; init; }

    /// <summary>
    /// Localised coin names
    /// </summary>
    public required string[] NativeCoinNames { get; init; }
    /// <summary>
    /// Localised coin abbreviations
    /// </summary>
    public required string[] NativeCoinAbbr { get; init; }
    /// <summary>
    /// Localised coin symbols
    /// </summary>
    public required string[] NativeCoinSymbols { get; init; }

    /// <summary>
    /// Par value (dt. Nennwert)
    /// </summary>
    public required decimal[] CoinValue { get; init; }
    /// <summary>
    /// Real value (dt. Realwert)
    /// </summary>
    public required decimal[] CoinRealValue { get; init; }
    /// <summary>
    /// Each coin's weight in Stone
    /// </summary>
    public required decimal[] CoinWeight { get; init; }


    public Currency(string name, int value) : base(name, value)
    {}

    /// <summary>
    /// The exchange rate defined by the rate between the two reference
    /// Coin of Middenrealm (Silver-) Thaler and the current currency.
    /// This rate is used to convert money amounts stored as decimals.
    /// </summary>
    public decimal Rate { get; init; }


    public const int Reference = 1;

    public static Currency ReferenceCurrency => MiddenrealmThaler;

    public static Currency MiddenrealmThaler =>
        new(nameof(MiddenrealmThaler), Reference)
        {
            CoinNames = new string[] { "Ducat", "Silverthaler", "Haler", "Kreutzer" },
            CoinAbbr = new string[] { "D", "S", "H", "K" },
            Rate = 1.0m,
            CoinValue = new decimal[] { 10, 1, 0.1m, 0.01m },
            CoinRealValue = new decimal[] { 10, 1, 0.1m, 0.01m },
            CoinWeight = new decimal[] { 0.025m, 0.005m, 0.0025m, 0.00125m },

            // set according to UI language
            NativeCoinNames = new string[] 
            {
                Properties.Resources.MiddenrealmDucatName,
                Properties.Resources.MiddenrealmSilverthalerName,
                Properties.Resources.MiddenrealmHalerName,
                Properties.Resources.MiddenrealmKreutzerName
            },
            NativeCoinAbbr = new string[] 
            {
                Properties.Resources.MiddenrealmDucatAbbr,
                Properties.Resources.MiddenrealmSilverthalerAbbr,
                Properties.Resources.MiddenrealmHalerAbbr,
                Properties.Resources.MiddenrealmKreutzerAbbr 
            },
            NativeCoinSymbols = new string[]
            {
                Properties.Resources.MiddenrealmDucatSymbol,
                Properties.Resources.MiddenrealmSilverthalerSymbol,
                Properties.Resources.MiddenrealmHalerSymbol,
                Properties.Resources.MiddenrealmKreutzerSymbol
            }
        };

    public static Currency DwarvenThaler => // Mountain Kingdom
        new(nameof(DwarvenThaler), 2)
        {
            CoinNames = new string[] { "Auromox", "Arganbrox", "Atebrox" },
            CoinAbbr = new string[] { "R", "G", "T" },
            Rate = 12.0m, // TODO
            CoinValue = new decimal[] { 12, 2, 0.2m },
            CoinRealValue = new decimal[] { 12, 2, 0.2m },
            CoinWeight = new decimal[] { 0.025m, 0.01m, 0.01m },

            // set according to UI language
            NativeCoinNames = new string[] 
            { 
                Properties.Resources.DwarvenAuromoxName, 
                Properties.Resources.DwarvenArganbroxName, 
                Properties.Resources.DwarvenAtebroxName 
            },
            NativeCoinAbbr = new string[] 
            {
                Properties.Resources.DwarvenAuromoxAbbr, 
                Properties.Resources.DwarvenArganbroxAbbr,
                Properties.Resources.DwarvenAtebroxAbbr 
            },
            NativeCoinSymbols = new string[]
            {
                Properties.Resources.DwarvenAuromoxSymbol,
                Properties.Resources.DwarvenArganbroxSymbol,
                Properties.Resources.DwarvenAtebroxSymbol
            }
        };

    public static Currency PaaviGuilder =>
        new(nameof(PaaviGuilder), 3)
        {
            CoinNames = new string[] { "Guilder" },
            CoinAbbr = new string[] { "R" },
            Rate = 5.0m,
            CoinValue = new decimal[] { 5 },
            CoinRealValue = new decimal[] { 5 },
            CoinWeight = new decimal[] { 0.0125m  },

            // set according to UI language
            NativeCoinNames = new string[] { Properties.Resources.PaaviGuilderName },
            NativeCoinAbbr = new string[] { Properties.Resources.PaaviGuilderAbbr },
            NativeCoinSymbols = new string[] { Properties.Resources.PaaviGuilderSymbol }
        };

    public static Currency NostrianCrown =>
        new(nameof(NostrianCrown), 4)
        {
            CoinNames = new string[] { "Crown" },
            CoinAbbr = new string[] { "Cr" },
            Rate = 5.0m,
            CoinValue = new decimal[] { 5 },
            CoinRealValue = new decimal[] { 2.5m },
            CoinWeight = new decimal[] { 0.025m }, // 25

            // set according to UI language
            NativeCoinNames = new string[] { Properties.Resources.NostrianKroneName },
            NativeCoinAbbr = new string[] { Properties.Resources.NostrianKroneAbbr },
            NativeCoinSymbols = new string[] { Properties.Resources.NostrianKroneSymbol }
        };

    public static Currency Andrathaler =>
        new(nameof(Andrathaler), 5)
        {
            CoinNames = new string[] { "Andrathaler" },
            CoinAbbr = new string[] { "A" },
            Rate = 5.0m,
            CoinValue = new decimal[] { 5 },
            CoinRealValue = new decimal[] { 2.5m },
            CoinWeight = new decimal[] { 0.025m }, // 25

            // set according to UI language
            NativeCoinNames = new string[] { Properties.Resources.AndrathalerName },
            NativeCoinAbbr = new string[] { Properties.Resources.AndrathalerAbbr },
            NativeCoinSymbols = new string[] { Properties.Resources.AndrathalerSymbol }
        };


    public static Currency Horasdor =>
        new (nameof(Horasdor), 6)
        {
            CoinNames = new string[] { "Horasdor", "Dukat", "Silbertaler", "Heller", "Kreutzer" },
            CoinAbbr = new string[] { "H", "D", "S", "H", "K" },
            Rate = 1.0m,
            CoinValue = new decimal[] { 200, 10, 1, 0.1m, 0.01m },
            CoinRealValue = new decimal[] { 200, 10, 1, 0.1m, 0.01m },
            CoinWeight = new decimal[] { 0.5m, 0.025m, 0.005m, 0.0025m, 0.00125m }, // 500, 25, 5, 2.5, 1.25

            // set according to UI language
            NativeCoinNames = new string[]
            {
                Properties.Resources.HorasdorName,
                Properties.Resources.MiddenrealmDucatName,
                Properties.Resources.MiddenrealmSilverthalerName,
                Properties.Resources.MiddenrealmHalerName,
                Properties.Resources.MiddenrealmKreutzerName
            },
            NativeCoinAbbr = new string[]
            {
                Properties.Resources.HorasdorAbbr,
                Properties.Resources.MiddenrealmDucatAbbr,
                Properties.Resources.MiddenrealmSilverthalerAbbr,
                Properties.Resources.MiddenrealmHalerAbbr,
                Properties.Resources.MiddenrealmKreutzerAbbr
            },
            NativeCoinSymbols = new string[]
            {
                Properties.Resources.HorasdorSymbol,
                Properties.Resources.MiddenrealmDucatSymbol,
                Properties.Resources.MiddenrealmSilverthalerSymbol,
                Properties.Resources.MiddenrealmHalerSymbol,
                Properties.Resources.MiddenrealmKreutzerSymbol
            }
        };

    public static Currency AlanfaOreal =>
        new(nameof(AlanfaOreal), 7)
        {
            CoinNames = new string[] { "Doubloon", "Oreal", "Small Oreal", "Dirham" },
            CoinAbbr = new string[] { "Do", "Or", "so", "d" },
            Rate = 1.0m,
            CoinValue = new decimal[] { 20, 1, 0.5m, 0.01m },
            CoinRealValue = new decimal[] { 20, 1, 0.5m, 0.01m },
            CoinWeight = new decimal[] { 0.050m, 0.005m, 0.003m, 0.003m }, // 50, 5, 3, 3

            // set according to UI language
            NativeCoinNames = new string[]
            {
                Properties.Resources.AlanfaDoubloonName,
                Properties.Resources.AlanfaOrealName,
                Properties.Resources.AlanfaSmallOrealName,
                Properties.Resources.AlanfaDirhamName
            },
            NativeCoinAbbr = new string[]
            {
                Properties.Resources.AlanfaDoubloonAbbr,
                Properties.Resources.AlanfaOrealAbbr,
                Properties.Resources.AlanfaSmallOrealAbbr,
                Properties.Resources.AlanfaDirhamAbbr
            },
            NativeCoinSymbols = new string[]
            {
                Properties.Resources.AlanfaDoubloonSymbol,
                Properties.Resources.AlanfaOrealSymbol,
                Properties.Resources.AlanfaSmallOrealSymbol,
                Properties.Resources.AlanfaDirhamSymbol
            }
        };

    public static Currency BornlandPenny =>
        new(nameof(BornlandPenny), 8)
        {
            CoinNames = new string[] { "Lump", "Penny", "Slightling" },
            CoinAbbr = new string[] { "BB", "BG", "BD" },
            Rate = 1.0m,
            CoinValue = new decimal[] { 10, 1, 0.1m },
            CoinRealValue = new decimal[] { 10, 1, 0.1m },
            CoinWeight = new decimal[] { 0.025m, 0.005m, 0.005m }, // 25, 5, 5

            // set according to UI language
            NativeCoinNames = new string[]
            {
                Properties.Resources.BornlandLumpName,
                Properties.Resources.BornlandPennyName,
                Properties.Resources.BornlandSlightlingName
            },
            NativeCoinAbbr = new string[]
            {
                Properties.Resources.BornlandLumpAbbr,
                Properties.Resources.BornlandPennyAbbr,
                Properties.Resources.BornlandSlightlingAbbr
            },
            NativeCoinSymbols = new string[]
            {
                Properties.Resources.BornlandLumpSymbol,
                Properties.Resources.BornlandPennySymbol,
                Properties.Resources.BornlandSlightlingSymbol
            }
        };


    //public static Currency PaaviGuilder =>
    //    new(nameof(PaaviGuilder), 11)
    //    {

    //    };
}