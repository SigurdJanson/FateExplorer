using Aventuria.Properties;
using System.Drawing;
using System.Globalization;
using System.Resources;

namespace Aventuria;

/// <summary>
/// <list type="bullet">
///     <item>
///         Coin representations (F, C, c) displays money as an amount of a specific coin. 
///         They support custom <see href="https://t1p.de/s2inp">numeric format strings</see>.
///     </item>
///     <item>
///         The coin set representations (L = long, S = short)
///     </item>
///     <item>The default representation is a coin set with short labels (S).</item>
/// </list>
/// 
/// 
/// </summary>
public class MoneyFormatter : IFormatProvider, ICustomFormatter
{
    private const string FormatCoinsLong = "L";
    private const string FormatCoinsShort = "S"; // 0D 2S 3H 0K

    private const string FormatAllCoins = "O";     // 0D 2S 3H 0K
    private const string FormatTrailingCoins = ">";   // 2S 3H 0K
    private const string FormatLeadingCoins = "<"; // 0D 2S 3H
    private const string FormatDropAll0 = "|";     //    2S 3H
    private const string FormatTrailWithFraction = "f";
    private const string FormatAllWithFraction = "F";

    private const string Convert2KeyCoin = "K";
    private const string Convert2HighestCoin = "C";
    private const string Convert2LowestCoin = "c";

    //private const string LongAllCoins = FormatCoinsLong + FormatAllCoins;    // 0D 2S 3H 0K
    //private const string LongTrail = FormatCoinsLong + FormatTrailingCoins;  //    2S 3H 0K
    //private const string LongLead = FormatCoinsLong + FormatLeadingCoins;    // 0D 2S 3H
    //private const string LongCompact = FormatCoinsLong + FormatDropAll0;     // 0D 2S 3H
    //private const string LongFraction = FormatCoinsLong + FormatWithFraction;//    2S 3H 0.3K
    //private const string LongAllFraction = FormatCoinsLong + FormatAllWithFraction; // 0D 2S 3H 0.3K

    //private const string ShortAllCoins = FormatCoinsShort + FormatAllCoins;    // 0D 2S 3H 0K
    //private const string ShortTrail = FormatCoinsShort + FormatTrailingCoins;  //    2S 3H 0K
    //private const string ShortLead = FormatCoinsShort + FormatLeadingCoins;    // 0D 2S 3H
    //private const string ShortCompact = FormatCoinsShort + FormatDropAll0;     // 0D 2S 3H
    //private const string ShortFraction = FormatCoinsShort + FormatWithFraction;//    2S 3H 0.3K
    //private const string ShortAllFraction = FormatCoinsShort + FormatAllWithFraction; // 0D 2S 3H 0.3K


    protected int DefaultPrecision { get; set; }


    public MoneyFormatter() : base()
    {
    }


    // Implements IFormatProvider.GetFormat(...)
    public object? GetFormat(Type? formatType)
    {
        // Determine whether custom formatting object is requested.
        if (formatType == typeof(MoneyFormatter))
            return this;
        else
            return null;
    }


    /// <summary>
    /// Format some <see cref="Money"/>.
    /// </summary>
    /// <param name="format">A format string containing formatting specifications.</param>
    /// <param name="arg">The money object to format</param>
    /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
    /// <returns>The string representation of the value of <paramref name="arg"/>, formatted as specified by 
    /// <paramref name="format"/> and <paramref name="formatProvider"/>.</returns>
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        Money amount = (Money)(arg ?? Money.Zero);
        string result = String.Empty;

        // Check whether this is an appropriate callback
        if (!Equals(formatProvider))
            return HandleOtherFormats(format ?? string.Empty, amount);


        // Set default format specifier
        if (string.IsNullOrEmpty(format))
            format = FormatCoinsShort;

        // "Set of Coins" block
        if (format.StartsWith(FormatCoinsShort) || format.StartsWith(FormatCoinsLong))
        {
            bool LongForm = (format[..1] == FormatCoinsLong);

            if (format.Length == 1)
            {
                result = FormatAsSet(amount, LongForm, DropLead0: false, DropTrail0: false, true);
            }
            else
            {
                string SecondChar = format.Substring(1, 1);
                bool KeepLead0, KeepTrail0, KeepFraction;
                KeepLead0 = (SecondChar == FormatAllCoins || SecondChar == FormatLeadingCoins || SecondChar == FormatAllWithFraction);
                KeepFraction = (SecondChar == FormatAllWithFraction || SecondChar == FormatTrailWithFraction);
                KeepTrail0 = KeepFraction || SecondChar == FormatAllCoins || SecondChar == FormatTrailingCoins;
                result = FormatAsSet(amount, LongForm, !KeepLead0, !KeepTrail0, !KeepFraction);
            }
        }
        else if (format.StartsWith(Convert2KeyCoin))
        {
            format = format[1..];
            decimal value = amount.ToDecimal();
            result = value.ToString(format) + " " + amount.Currency.CoinCodes[amount.Currency.KeyCoinIndex];
        }
        if (format.StartsWith(Convert2HighestCoin))
        {
            format = format[1..];
            int MaxCoin = Array.IndexOf(amount.Currency.CoinValue, amount.Currency.CoinValue.Max());
            decimal value = amount.ToDecimal(MaxCoin);
            result = value.ToString(format) + " " + amount.Currency.CoinCodes[MaxCoin];
        }
        if (format.StartsWith(Convert2LowestCoin))
        {
            format = format[1..];
            int MinCoin = Array.IndexOf(amount.Currency.CoinValue, amount.Currency.CoinValue.Min());
            decimal value = amount.ToDecimal(MinCoin);
            result = value.ToString(format) + " " + amount.Currency.CoinCodes[MinCoin];
        }

        return result;
    }


    /// <summary>
    /// Formats an amount of money by finding the best split into the coins of the currency.
    /// </summary>
    /// <param name="money">The amount of money</param>
    /// <param name="longform">Abbreviate coins (short form) or use full names (long form)</param>
    /// <param name="DropLead0">Ignore high value coins when they do not add value.</param>
    /// <param name="DropTrail0">Ignore low value coins when their value is 0.</param>
    /// <param name="DropFraction">Ignore hack-silver values that are represented as fractions of the lowest coin.</param>
    /// <returns>A string representing the amount of money</returns>
    protected static string FormatAsSet(Money money, bool longform, bool DropLead0, bool DropTrail0, bool DropFraction)
    {
        decimal[] coins = money.OptimizeDenomination();
        bool[] Keep0 = new bool[coins.Length];
        Array.Fill<bool>(Keep0, true);

        // If zeroes shall be dropped, determine which coins are affected
        int droppedCoins = 0;
        if (DropLead0)
        {
            int i = 0;
            while (i < Keep0.Length && coins[i] == 0m)
            {
                Keep0[i] = false;
                i++;
            }
            droppedCoins += i;
        }
        if (DropTrail0)
        {
            int i = 1;
            while (i <= Keep0.Length && coins[^i] == 0m)
            {
                Keep0[^i] = false;
                i++;
            }
            droppedCoins += i - 1;
        }

        // When all coins are to be dropped, restore the key coin
        if (droppedCoins >= coins.Length)
        {
            if (DropFraction)
                Keep0[money.Currency.KeyCoinIndex] = true;
            else
                Keep0[^1] = true;
        }

        // Drop fraction if specified
        if (DropFraction)
                coins[^1] = Math.Floor(coins[^1]);

        // Convert to string
        string[] Denomination = longform switch
        {
            true => money.Currency.CoinNames,
            false => money.Currency.CoinCodes
        };
        string ValueCoinSep = longform switch
        {
            true => " ",
            false => "\u2009" // thin space
        };

        string result = "";
        bool FirstCoin = true;
        for (int i = 0; i < coins.Length; i++)
        {
            if (Keep0[i])
            {
                if (!FirstCoin) 
                    result += " ";
                else
                    FirstCoin = false;

                result += coins[i].ToString("0.##") + ValueCoinSep + Denomination[i];
            }
        }
        return result;
    }



    private string HandleOtherFormats(string format, object arg)
    {
        if (arg is IFormattable formattable)
            return formattable.ToString(format, CultureInfo.CurrentCulture);
        else 
            return arg?.ToString() ?? string.Empty;
    }
}
