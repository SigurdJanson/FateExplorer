using System;
using System.Globalization;

namespace FateExplorer.Shared;


/// <summary>
/// Converts w <see cref="Weight"/> using w format string and <see cref="CultureInfo.CurrentUICulture"/>.
/// <list type="table">
///     <listheader>
///        <term>term</term>
///        <description>description</description>
///    </listheader>
///    <item>
///        <term>"g"</term>
///        <description>Weight in the language's base unit (Stone) without naming the unit.</description>
///    </item>
///    <item>
///        <term>"G"</term>
///        <description>Weight in the language's base unit (Stone) with a full unit name.</description>
///    </item>
///    <item>
///        <term>"r"</term>
///        <description>Short string the Rohal unit that represents the weight best.</description>
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
    const int Unspecified = 0;
    const int German = 1;
    const int English = 2;

    protected int Language { get; set; } = Unspecified;

    public WeightFormatter(CultureInfo cultureInfo)
    {
        Language = cultureInfo.ThreeLetterISOLanguageName switch
        {
            "deu" => German,
            "eng" => English,
            _ => Unspecified
        };
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
        if (formatProvider is null) return BaseStr().Trim();
        if (!formatProvider.Equals(this)) return BaseStr().Trim();

        string thisFmt;
        if (string.IsNullOrEmpty(format))
            thisFmt = "G"; // Handle null or empty format string, string with precision specifier.
        else
            thisFmt = format[..1]; // Extract first character of format string (precision specifiers are not supported).

        // Return w formatted string.
        Weight w = arg is not null ? (Weight)arg : Weight.Zero;
        string resultString = (thisFmt[0], char.IsUpper(thisFmt[0])) switch
        {
            ('g', false) => BaseStr(),
            ('G', true) => BaseUnitStr(),
            ('R', true) => Split(w),
            ('r', false) => Best(w),
            _ => throw new InvalidOperationException("Unknown format string")
        };

        return resultString.Trim();
    }


    /// <summary>
    /// Return a weight in Stones.
    /// </summary>
    /// <param name="w">Weight</param>
    /// <returns>Weight represented as string in unit 'Stone'</returns>
    protected static string BaseStr() => $"{{0:N4}}";


    /// <summary>
    /// Return a weight in Stones as with an abbreviated unit.
    /// </summary>
    /// <returns>Weight represented as string in unit 'Stone'</returns>
    protected string BaseUnitStr()
    {
        //decimal w = (decimal)W;
        return $"{{0:N4}} {StoneUnit}"; //$"{(decimal)w} {StoneUnit}";
    }

    
    protected string Split(Weight W)
    {
        decimal Ref;
        decimal w = (decimal)W;

        Ref = Weight.ToCuboids(1m);
        int Cubes = (int)Math.Floor(w * Ref);
        w -= Cubes / Ref;
        int Stones = (int)Math.Floor(w);
        w -= Stones;

        Ref = Weight.ToOunce(1m);
        int Ounce = (int)Math.Floor(w * Ref);
        w -= Ounce / Ref;

        Ref = Weight.ToScruple(1m);
        int Scruple = (int)Math.Floor(w * Weight.ToScruple(1m));
        w -= Scruple / Ref;

        Ref = Weight.ToCarat(1m);
        int Carat = (int)Math.Floor(w * Weight.ToCarat(1m));
        w -= Carat / Ref;

        decimal Gran = w * Weight.ToGran(1m);

        return $"{Cubes} {CuboidUnitAbbr} {Stones} {StoneUnitAbbr} {Ounce} {OunceUnitAbbr} {Scruple} {ScrupleUnitAbbr} {Carat} {CaratUnitAbbr} {Gran} {GranUnitAbbr}";
    }


    protected string Best(Weight W)
    {
        const int Threshold = 1;
        decimal w = (decimal)W;

        int Cubes = (int)Math.Floor(w * Weight.ToCuboids(1m));
        if (Cubes > Threshold) return $"{Weight.ToCuboids(w)} {CuboidUnitAbbr}";

        int Stones = (int)Math.Floor(w);
        if (Stones > Threshold) return $"{w} {StoneUnitAbbr}";

        int Ounce = (int)Math.Floor(w * Weight.ToOunce(1m));
        if (Ounce > Threshold) return $"{Weight.ToOunce(w)} {OunceUnitAbbr}";

        int Scruple = (int)Math.Floor(w * Weight.ToScruple(1m));
        if (Scruple > 4) return $"{Weight.ToScruple(w)} {ScrupleUnitAbbr}";

        int Carat = (int)Math.Floor(w * Weight.ToCarat(1m));
        if (Carat > Threshold) return $"{Weight.ToCarat(w)} {CaratUnitAbbr}";

        decimal Gran = w * Weight.ToGran(1m);
        return $"{Gran} {GranUnitAbbr}";
    }

    // TODO: hard-coded strings
    private string StoneUnit => Language == German ? "Stein" : "stone";
    private string CuboidUnitAbbr => Language == German ? "Q" : "C";
    private string StoneUnitAbbr => Language == German ? "St" : "st";
    private string OunceUnitAbbr => "oz"; // Language == German ? "oz" : "oz"; // ℥
    private string ScrupleUnitAbbr => "s"; // Language == German ? "s" : "s"; // ℈
    private string CaratUnitAbbr => Language == German ? "kt" : "ct";
    private string GranUnitAbbr => "gr"; // Language == German ? "gr" : "gr";

}
