
namespace Aventuria.Measures;

internal interface IMeasure : IFormattable
{
    double ToDouble();
    decimal ToDecimal();
}
