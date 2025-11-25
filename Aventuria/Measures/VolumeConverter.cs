using System.Runtime.CompilerServices;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("UnitTests")]

namespace Aventuria.Measures;


/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the Rohal standard measures and the metric system.
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class VolumeFoCoRohalMetric : UnitConverterBase<VolumeMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; }; // inherited

    public VolumeFoCoRohalMetric() { }

    public override double ConvertToBase(VolumeMeasure value) => (double)value; // convert to meter


    public override double ConvertByPurpose(VolumeMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            'a' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // agricultural measures
            't' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // travel distance in miles
            'b' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToUrn, medium: ToCask, large: ToRoomPace), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToQuart, medium: ToCask, large: ToRoomPace), // construction measures
            'f' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToQuart, medium: ToUrn, large: ToRoomPace), // dry measure ('d'epth for length measures; re-used here as 'd'ry)
            'l' => ResolvePurposeSize(value, Format, small: ToFlow, medium: ToQuart, large: ToCask), // liquid measures
            'p' => ResolvePurposeSize(value, Format, small: ToFlow, medium: ToDraught, large: ToQuart), // precision measures, e.g.alchemy
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported.")
        };
        return result;
    }



    public override double ConvertBySize(VolumeMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToFlow((double)value),
            StandardMeasureSize.S => ToDraught((double)value),
            StandardMeasureSize.M => ToQuart((double)value),
            StandardMeasureSize.L => ToCask((double)value),
            StandardMeasureSize.XL => ToRoomPace((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported.")
        };


    /// <summary>
    /// Converts from the base unit to the respective unit of this method.
    /// </summary>
    /// <param name="Value">A volume measured in base units, i.e. the default unit used by <see cref="VolumeMeasure"/>.</param>
    /// <returns>A double value</returns>
    /// <remarks>Flux</remarks>
    public static double ToFlow(double Value) => Value * 100; //


    /// <inheritdoc cref="ToFlow(double)"/>
    /// <remarks>Schank</remarks>
    public static double ToDraught(double Value) => Value * 4; //


    /// <inheritdoc cref="ToFlow(double)"/>
    /// <remarks>Maß</remarks>
    public static double ToQuart(double Value) => Value; // base unit


    /// <inheritdoc cref="ToFlow(double)"/>
    /// <remarks>Urn</remarks>
    public static double ToUrn(double Value) => Value / 10; // 


    /// <inheritdoc cref="ToFlow(double)"/>
    /// <remarks>Fass</remarks>
    public static double ToCask(double Value) => Value / 100; // 


    /// <inheritdoc cref="ToFlow(double)"/>
    /// <remarks>Raumschritt</remarks>
    public static double ToRoomPace(double Value) => Value / 1000; //

    /// <inheritdoc cref="ToFlow(double)"/>
    /// <remarks>Ox</remarks>
    public static double ToOx(double Value) => Value / 1200; // 

}



internal class VolumeFoCoRohalImperial : UnitConverterBase<VolumeMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; }; // inherited

    public VolumeFoCoRohalImperial() { }

    public override double ConvertToBase(VolumeMeasure value) => (double)value; // convert to meter


    public override double ConvertByPurpose(VolumeMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            'a' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // agricultural measures
            't' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // travel distance in miles
            'b' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToUrn, medium: ToBarrel, large: ToRoomYard), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToMeasure, medium: ToBarrel, large: ToRoomYard), // construction measures
            'f' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToMeasure, medium: ToUrn, large: ToRoomYard), // dry measure ('d'epth for length measures; re-used here as 'd'ry)
            'l' => ResolvePurposeSize(value, Format, small: ToOunce, medium: ToMeasure, large: ToBarrel), // liquid measures
            'p' => ResolvePurposeSize(value, Format, small: ToOunce, medium: ToPint, large: ToMeasure), // precision measures, e.g.alchemy
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported.")
        };
        return result;
    }



    public override double ConvertBySize(VolumeMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToOunce((double)value),
            StandardMeasureSize.S => ToPint((double)value),
            StandardMeasureSize.M => ToMeasure((double)value),
            StandardMeasureSize.L => ToBarrel((double)value),
            StandardMeasureSize.XL => ToRoomYard((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported.")
        };


    /// <summary>
    /// Converts from the base unit to the respective unit of this method.
    /// </summary>
    /// <param name="Value">A length measured in base units, i.e. the default unit used by <see cref="VolumeMeasure"/>.</param>
    /// <returns>A double value</returns>
    public static double ToOunce(double Value) => Value / VolumeMeasure.LitersPerOunce; //


    /// <inheritdoc cref="ToSquareInch(double)"/>
    public static double ToPint(double Value) => Value / VolumeMeasure.LitersPerOunce / 16; 


    /// <inheritdoc cref="ToSquareInch(double)"/>
    public static double ToMeasure(double Value) => Value / VolumeMeasure.LitersPerOunce / 32; // 


    /// <inheritdoc cref="ToSquareInch(double)"/>
    public static double ToUrn(double Value) => Value / VolumeMeasure.LitersPerOunce / 21 / 16; // 


    /// <inheritdoc cref="ToSquareInch(double)"/>
    public static double ToBarrel(double Value) => Value / VolumeMeasure.LitersPerOunce / 211 / 16; //


    /// <inheritdoc cref="ToSquareInch(double)"/>
    public static double ToRoomFoot(double Value) => Value / VolumeMeasure.LitersPerRoomYard * 27; // (3^3) room feet in a room yard

    /// <inheritdoc cref="ToSquareInch(double)"/>
    public static double ToRoomYard(double Value) => Value / VolumeMeasure.LitersPerRoomYard; //

}



/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the dwarven measures and the metric system..
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class VolumeFoCoDwarvenMetric : UnitConverterBase<VolumeMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; } // inherited;

    public VolumeFoCoDwarvenMetric() { }

    public override double ConvertByPurpose(VolumeMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            'a' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // agricultural measures
            't' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // travel distance in miles
            'b' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToBaroshtrom, medium: ToBaroshtrom, large: ToBaroshtrom), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBaroshtrom, large: ToBaroshtrom), // construction measures
            'f' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBarosht, large: ToBaroshtrom), // dry measure ('d'epth for length measures; re-used here as 'd'ry)
            'l' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBarosht, large: ToBaroshtrom), // liquid measures
            'p' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBarosht, large: ToBarosht), // precision measures, e.g.alchemy
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported.")
        };
        return result;
    }



    public override double ConvertBySize(VolumeMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToBarosht((double)value),
            StandardMeasureSize.S => ToBarosht((double)value),
            StandardMeasureSize.M => ToBarosht((double)value),
            StandardMeasureSize.L => ToBaroshtrom((double)value),
            StandardMeasureSize.XL => ToBaroshtrom((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };

    public override double ConvertToBase(VolumeMeasure value) => (double)value; //


    // Dwarven Units
    public static double ToBarosht(double Value) => Value; // 
    public static double ToBaroshtrom(double Value) => Value / 76; //
}



/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the dwarven measures and the imperial system.
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class VolumeFoCoDwarvenImperial : UnitConverterBase<VolumeMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; } // inherited;

    public VolumeFoCoDwarvenImperial() { }

    public override double ConvertByPurpose(VolumeMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            'a' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // agricultural measures
            't' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // travel distance in miles
            'b' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToBaroshtrom, medium: ToBaroshtrom, large: ToBaroshtrom), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBaroshtrom, large: ToBaroshtrom), // construction measures
            'f' => throw new NotSupportedException($"Size format '{Format}' is not supported."), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBarosht, large: ToBaroshtrom), // dry measure ('d'epth for length measures; re-used here as 'd'ry)
            'l' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBarosht, large: ToBaroshtrom), // liquid measures
            'p' => ResolvePurposeSize(value, Format, small: ToBarosht, medium: ToBarosht, large: ToBarosht), // precision measures, e.g.alchemy
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported.")
        };
        return result;
    }



    public override double ConvertBySize(VolumeMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToBarosht((double)value),
            StandardMeasureSize.S => ToBarosht((double)value),
            StandardMeasureSize.M => ToBarosht((double)value),
            StandardMeasureSize.L => ToBaroshtrom((double)value),
            StandardMeasureSize.XL => ToBaroshtrom((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };

    public override double ConvertToBase(VolumeMeasure value) => (double)value; //


    // Dwarven Units
    public static double ToBarosht(double Value) => Value / VolumeMeasure.LitersPerOunce / 32; //
    public static double ToBaroshtrom(double Value) => Value / VolumeMeasure.LitersPerOunce / 32 / 76; //
}


