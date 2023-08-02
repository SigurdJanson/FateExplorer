using System;
using System.Globalization;
using System.Numerics;



namespace FateExplorer.Shared;



/// <summary>
/// Converts a <see cref="Modifier"/> using a format string and <see cref="CultureInfo.CurrentUICulture"/>.
/// Handles also "C", "D", "E", "F", and "G", so that it can be used with a modifier cast to <c>int</c>.
/// Also offers "L" for a longer explanatory string.
/// </summary>
public class ModifierFormatter : IFormatProvider, ICustomFormatter
{
    public ModifierFormatter()
    {}


    // IFormatProvider.GetFormat implementation.
    public object GetFormat(Type formatType)
    {
        // Determine whether custom formatting object is requested.
        if (formatType == typeof(Modifier))
            return this;
        else
            return null;
    }



    /// <summary>
    /// Format a <see cref="Modifier"/>.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="arg"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (!formatProvider.Equals(this)) return null;

        string thisFmt;
        if (string.IsNullOrEmpty(format))
            thisFmt = string.Empty; // Handle null or empty format string, string with precision specifier.
        else
            thisFmt = format[..1]; // Extract first character of format string (precision specifiers are not supported).

        // Return a formatted string.
        string resultString = thisFmt switch
        {
            "C" or "D" or "G" or "E" or "F" => ToString_Short((Modifier)arg),
            "L" => ToString_Long((Modifier)arg, CultureInfo.CurrentUICulture),
            _ => throw new NotImplementedException()
        };

        return resultString.Trim();
    }


    private static string GetStr(string id, CultureInfo culture)
    {
        return culture.Name switch
        {
            "de" or "de-DE" => id switch
            {
                "nameNeutral"   => "ohne Modifikation",
                "nameAdd"       => "um {0} erleichtert",
                "nameSubtract"  => "um {0} erschwert",
                "nameHalve"     => "halbiert",
                "nameForceToX"  => "auf {0} fixiert",
                "nameLuckyRoll" => "nur mit einer 1 nach Modifikation",
                "nameImpossible" => "unmöglich nach Modifikation",
                _ => throw new NotImplementedException()
            },
            _ => id switch
            {
                "nameNeutral"   => "without modification",
                "nameAdd"       => "with a penalty of {0}",
                "nameSubtract"  => "with a bonus of {0}",
                "nameHalve"     => "halved",
                "nameForceToX"  => "forced to {0}",
                "nameLuckyRoll" => "only a 1 after modification",
                "nameImpossible" => "impossible after modification",
                _ => throw new NotImplementedException()
            }
        };
    }

    private static string ToString_Long(Modifier Value, CultureInfo culture) => Value.Operator switch
    {
        Modifier.Op.Add => Math.Max((int)Value, -1) switch 
        { 
            -1 => GetStr("nameSubtract", culture), 
            0 => GetStr("nameNeutral", culture), 
            _ => GetStr("nameAdd", culture)
        },
        Modifier.Op.Halve => GetStr("nameHalve", culture),
        Modifier.Op.Force => Math.Max((int)Value, 0) switch
        {
            0 => GetStr("nameImpossible", culture),
            1 => GetStr("nameLuckyRoll", culture),
            _ => GetStr("nameForceToX", culture),
        },
        _ => throw new InvalidOperationException()
    };

    private static string ToString_Short(Modifier Value) => Value.Operator switch
    {
        Modifier.Op.Add => $"{(int)Value:+#;-#;0}",
        Modifier.Op.Halve => $"/ 2",
        Modifier.Op.Force => $"= {(int)Value}",
        _ => "invalid"
    };

}
