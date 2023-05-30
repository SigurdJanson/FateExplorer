using System.Globalization;

namespace Aventuria;

class MoneyFormatter : IFormatProvider, ICustomFormatter
{
    const string Unspecified = "";
    const string German = "deu";
    const string English = "eng";
    protected string Language { get; set; }
    protected int DefaultPrecision { get; set; }


    public MoneyFormatter(CultureInfo cultureInfo)
    {
        Language = cultureInfo.ThreeLetterISOLanguageName switch
        {
            "deu" => German,
            "eng" => English,
            _ => Unspecified
        };
        DefaultPrecision = Language switch
        {
            German => cultureInfo.NumberFormat.NumberDecimalDigits,
            English => cultureInfo.NumberFormat.NumberDecimalDigits,
            _ => CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits
        };
        Properties.Resources.Culture = cultureInfo;
    }


    // IFormatProvider.GetFormat implementation.
    public object? GetFormat(Type? formatType)
    {
        // Determine whether custom formatting object is requested.
        if (formatType == typeof(Money))
            return this;
        else
            return null;
    }


    /// <summary>
    /// Format a <see cref="Weight"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        Money money = (Money)(arg ?? Money.Zero);
        formatProvider ??= new NumberFormatInfo()
        {
            CurrencySymbol = money.CurrencySymbol
        };
        format ??= "C";
        return money.ToString(format, formatProvider);
    }

}
