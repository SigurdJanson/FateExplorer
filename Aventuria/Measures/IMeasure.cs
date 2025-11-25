namespace Aventuria.Measures;

public interface IMeasure : IFormattable
{
    double ToDouble();
    decimal ToDecimal();
}
