
namespace Aventuria;


public class DefaultFormatter : ICustomFormatter, IFormatProvider
{
    public object? GetFormat(Type? formatType)
    {
        // Return this instance if the requested format type is ICustomFormatter
        return formatType == typeof(DefaultFormatter) ? this : null;
    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        // If the argument is null, return an empty string
        if (arg == null) return string.Empty;

        // If no specific format is provided, use the default ToString()
        if (string.IsNullOrEmpty(format))
        {
            return arg?.ToString() ?? string.Empty;
        }

        // Otherwise, handle custom formatting (if needed)
        if (arg is IFormattable formattable)
        {
            return formattable.ToString(format, formatProvider);
        }

        // Fallback to default ToString() for non-IFormattable objects
        return arg?.ToString() ?? string.Empty;
    }
}
