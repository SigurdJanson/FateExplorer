using System;
using System.Globalization;

namespace Aventuria.Measures;


/// <summary>
/// Converts w <see cref="Weight"/> using w format string and <see cref="CultureInfo.CurrentUICulture"/>.
/// <list type="table">
///     <listheader>
///        <term>term</term>
///        <description>description</description>
///    </listheader>
///    <item>
///        <term>"g"</term>
///        <description>Weight in the language's reference unit (Stone) without naming the unit. 
///        Default precision is given by the locale. It can be changed using a trailing number (e.g. "g12").</description>
///    </item>
///    <item>
///        <term>"G"</term>
///        <description>Weight in the language's reference unit (Stone) with a full unit name. 
///        Default precision is given by the locale. It can be changed using a trailing number (e.g. "G12").</description>
///    </item>
///    <item>
///        <term>"r"</term>
///        <description>Short string the Rohal unit that represents the weight best. 
///        Default precision is given by the locale. It can be changed using a trailing number (e.g. "r12").</description>
///    </item>
///    <item>
///        <term>"R"</term>
///        <description>Long string dividing the weight into any number of Rohal units needed to represent it.</description>
///    </item>
/// </list>
/// </summary>
/// <remarks>Supports only German and English</remarks>
public class WeightFormatter : IFormatProvider, ICustomFormatter
{
    private const char FormatGeneralSimple = 'g';
    private const char FormatGeneral = 'G';
    private const char FormatRohalBest = 'r';
    private const char FormatRohalAll = 'R';

    const int Unspecified = 0;
    const int German = 1;
    const int English = 2;

    protected int Language { get; set; } = Unspecified;
    protected int DefaultPrecision { get; set; }


    public WeightFormatter(CultureInfo cultureInfo)
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
        if (formatType == typeof(Weight))
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
        if (formatProvider is null) return BaseStr(DefaultPrecision).Trim();
        if (!formatProvider.Equals(this)) return BaseStr(DefaultPrecision).Trim();

        string thisFmt;
        if (string.IsNullOrEmpty(format))
            thisFmt = FormatGeneral.ToString(); // Handle null or empty format string, string with precision specifier.
        else
            thisFmt = format[..1]; // Extract first character of format string (precision specifiers are not supported).

        int Precision;
        if (!int.TryParse(format.AsSpan(1), out Precision))
            Precision = DefaultPrecision;


        // Return w formatted string.
        Weight w = arg is not null ? (Weight)arg : Weight.Zero;
        string resultString = (thisFmt[0], char.IsUpper(thisFmt[0])) switch
        {
            (FormatGeneralSimple, false) => BaseStr(Precision),
            (FormatGeneral, true) => BaseUnitStr(Precision),
            (FormatRohalAll, true) => Split(w),
            (FormatRohalBest, false) => Best(w, Precision),
            _ => throw new InvalidOperationException("Unknown format string")
        };

        return resultString.Trim();
    }


    /// <summary>
    /// Return a strng to format the weight in Stones.
    /// </summary>
    /// <param name="Precision">Defines the number of decimal digits.</param>
    /// <returns>Weight represented as string in unit 'Stone'</returns>
    protected static string BaseStr(int Precision) => 
        $"{{0:N{Precision}}}";


    /// <summary>
    /// Return a string to format in Stones with an abbreviated unit.
    /// </summary>
    /// <param name="Precision">Defines the number of decimal digits.</param>
    /// <returns>Weight represented as string in unit 'Stone'</returns>
    protected static string BaseUnitStr(int Precision) => 
        $"{{0:N{Precision}}} {StoneUnit}";


    /// <summary>
    /// Provides a full representation of a weight by splitting it into all Rohal units.
    /// </summary>
    /// <param name="W">A weight</param>
    /// <returns>A formatted string</returns>
    protected static string Split(Weight W)
    {
        double Ref;
        double w = (double)W;

        Ref = Weight.ToCuboids(1);
        int Cubes = (int)Math.Floor(w * Ref);
        w -= Cubes / Ref;
        int Stones = (int)Math.Floor(w);
        w -= Stones;

        Ref = Weight.ToOunce(1);
        int Ounce = (int)Math.Floor(w * Ref);
        w -= Ounce / Ref;

        Ref = Weight.ToScruple(1);
        int Scruple = (int)Math.Floor(w * Weight.ToScruple(1));
        w -= Scruple / Ref;

        Ref = Weight.ToCarat(1);
        int Carat = (int)Math.Floor(w * Weight.ToCarat(1));
        w -= Carat / Ref;

        double Gran = double.Round(w, Weight.SignificantDigits, MidpointRounding.AwayFromZero) * Weight.ToGran(1);

        return $"{Cubes} {CuboidUnitAbbr} {Stones} {StoneUnitAbbr} {Ounce} {OunceUnitAbbr} {Scruple} {ScrupleUnitAbbr} {Carat} {CaratUnitAbbr} {Gran} {GranUnitAbbr}";
    }

    /// <summary>
    /// Formats a given weight finding it's "best" representation. I.e. a representation with
    /// more at least 1 integral digit if possible.
    /// </summary>
    /// <param name="W">A weight</param>
    /// <param name="Precision">Defines the number of decimal digits.</param>
    /// <returns>A formatted string</returns>
    protected static string Best(Weight W, int Precision)
    {
        const int Threshold = 1;
        double w = (double)W;
        string Format = $"{{0:F{Precision}}} {{1}}";

        int Cubes = (int)Math.Floor(w * Weight.ToCuboids(1));
        if (Cubes > Threshold) return string.Format(Format, Weight.ToCuboids(w), CuboidUnitAbbr);

        int Stones = (int)Math.Floor(w);
        if (Stones > Threshold) return string.Format(Format, w, StoneUnitAbbr);

        int Ounce = (int)Math.Floor(w * Weight.ToOunce(1));
        if (Ounce > Threshold) return string.Format(Format, Weight.ToOunce(w), OunceUnitAbbr);

        int Scruple = (int)Math.Floor(w * Weight.ToScruple(1));
        if (Scruple > 4) return string.Format(Format, Weight.ToScruple(w), ScrupleUnitAbbr);

        int Carat = (int)Math.Floor(w * Weight.ToCarat(1));
        if (Carat > Threshold) return string.Format(Format, Weight.ToCarat(w), CaratUnitAbbr);

        double Gran = w * Weight.ToGran(1);
        return string.Format(Format, Gran, GranUnitAbbr);
    }

    private static string StoneUnit => Properties.Resources.WeightStone; // Language == German ? "Stein" : "stone";
    private static string CuboidUnitAbbr => Properties.Resources.WeightCuboidAbbr; // Language == German ? "Q" : "C";
    private static string StoneUnitAbbr => Properties.Resources.WeightStoneAbbr; // Language == German ? "St" : "st";
    private static string OunceUnitAbbr => Properties.Resources.WeightOunceAbbr; // "oz"; // Language == German ? "oz" : "oz"; // ℥
    private static string ScrupleUnitAbbr => Properties.Resources.WeightScrupleAbbr; // "s"; // Language == German ? "s" : "s"; // ℈
    private static string CaratUnitAbbr => Properties.Resources.WeightCaratAbbr; // Language == German ? "kt" : "ct";
    private static string GranUnitAbbr => Properties.Resources.WeightGranAbbr; // "gr"; // Language == German ? "gr" : "gr";

}
