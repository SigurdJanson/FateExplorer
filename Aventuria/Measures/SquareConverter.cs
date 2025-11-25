using System.Diagnostics;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTests")]

namespace Aventuria.Measures;


/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the Rohal standard measures and the metric system.
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class SquareFoCoRohalMetric : UnitConverterBase<SquareMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; }; // inherited

    public SquareFoCoRohalMetric() { }

    public override double ConvertToBase(SquareMeasure value) => (double)value; // convert to square meter


    public override double ConvertByPurpose(SquareMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            'a' => ResolvePurposeSize(value, Format, small: ToSquare, medium: ToHectare, large: ToAcre), // agricultural measures
            't' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // travel distance in miles
            'b' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToAnglePace, medium: ToSquare, large: ToHectare), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToAngleSpan, medium: ToAnglePace, large: ToSquare), // construction measures
            'f' => ResolvePurposeSize(value, Format, small: ToAngleSpan, medium: ToAngleSpan, large: ToAnglePace), // fabric measures
            'd' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // areas do not measure depth
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported.")
        };
        return result;
    }



    public override double ConvertBySize(SquareMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToAngleSpan((double)value),
            StandardMeasureSize.S => ToAngleSpan((double)value),
            StandardMeasureSize.M => ToAnglePace((double)value),
            StandardMeasureSize.L => ToAngleMile((double)value),
            StandardMeasureSize.XL => ToAcre((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };


    /// <summary>
    /// Converts from the base unit to the respective unit of this method.
    /// </summary>
    /// <param name="Value">A square measured in base units, i.e. the default unit used by <see cref="SquareMeasure"/>.</param>
    /// <returns>A double value</returns>
    public static double ToAngleSpan(double Value) => Value * 25; // 5 * 5


    /// <inheritdoc cref="ToAngleSpan(double)"/>
    public static double ToAnglePace(double Value) => Value; // base unit


    /// <inheritdoc cref="ToAngleSpan(double)"/>
    public static double ToSquare(double Value) => Value / 25 / 25; // 1 sqr = 625 m²


    /// <inheritdoc cref="ToAngleSpan(double)"/>
    public static double ToHectare(double Value) => Value / 100 / 100; // 


    /// <inheritdoc cref="ToAngleSpan(double)"/>
    public static double ToAngleMile(double Value) => Value / 1000 / 1000; // 1 am = 1,000,000 m²


    /// <inheritdoc cref="ToAngleSpan(double)"/>
    public static double ToAcre(double Value) => Value / 2000 / 2000; // 

}



internal class SquareFoCoRohalImperial : UnitConverterBase<SquareMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; }; // inherited

    public SquareFoCoRohalImperial() { }

    public override double ConvertToBase(SquareMeasure value) => ToImperialYard((double)value) ; // convert to square yard


    public override double ConvertByPurpose(SquareMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            'a' => ResolvePurposeSize(value, Format, small: ToMorgen, medium: ToImperialMile, large: ToLand), // agricultural measures
            't' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // travel distance in miles
            'b' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToImperialYard, medium: ToMorgen, large: ToImperialMile), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToImperialFoot, medium: ToImperialYard, large: ToMorgen), // construction measures
            'f' => ResolvePurposeSize(value, Format, small: ToImperialInch, medium: ToImperialFoot, large: ToImperialYard), // fabric measures
            'd' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // areas do not measure depth
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported.")
        };
        return result;
    }



    public override double ConvertBySize(SquareMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToImperialInch((double)value),
            StandardMeasureSize.S => ToImperialFoot((double)value),
            StandardMeasureSize.M => ToImperialYard((double)value),
            StandardMeasureSize.L => ToImperialMile((double)value),
            StandardMeasureSize.XL => ToLand((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };


    /// <summary>
    /// Converts from the base unit to the respective unit of this method.
    /// </summary>
    /// <param name="Value">A length measured in base units, i.e. the default unit used by <see cref="SquareMeasure"/>.</param>
    /// <returns>A double value</returns>
    public static double ToImperialInch(double Value) => Value / LengthMeasure.MeterPerYard / LengthMeasure.MeterPerYard * 36 * 36; // 36 x 36 in² per yrd²


    /// <inheritdoc cref="ToImperialInch(double)"/>
    public static double ToImperialFoot(double Value) => Value / LengthMeasure.MeterPerYard / LengthMeasure.MeterPerYard * 9; // 3 x 3 ft² per yrd²


    /// <inheritdoc cref="ToImperialInch(double)"/>
    public static double ToImperialYard(double Value) => Value / LengthMeasure.MeterPerYard / LengthMeasure.MeterPerYard; // 


    /// <inheritdoc cref="ToImperialInch(double)"/>
    public static double ToMorgen(double Value) => Value / (LengthMeasure.MeterPerYard * 100) / (LengthMeasure.MeterPerYard * 100); // 


    /// <inheritdoc cref="ToImperialInch(double)"/>
    public static double ToImperialMile(double Value) => Value / (LengthMeasure.MeterPerYard * 1094) / (LengthMeasure.MeterPerYard * 1094); //


    /// <inheritdoc cref="ToImperialInch(double)"/>
    public static double ToLand(double Value) => Value / (LengthMeasure.MeterPerYard * 1094 * 2400) / (LengthMeasure.MeterPerYard * 1094 * 2400); //

}