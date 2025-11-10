using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTests")]

namespace Aventuria.Measures;


/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the Rohal standard measures and the metric system.
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class LengthFoCoRohalMetric : UnitConverterBase<LengthMeasure, double>
{
    //public DereCultureInfo DereCulture { get; init; }; // inherited

    public LengthFoCoRohalMetric(DereCultureInfo culture) => DereCulture = culture;

    public override double ConvertToBase(LengthMeasure value) => (double)value; // convert to meter


    public override double ConvertByPurpose(LengthMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            't' => ToMile((double)value), // travel distance in miles
            'b' => ResolvePurposeSize(value, Format, small: ToHalfFinger, medium: ToHalfFinger, large:ToPace), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToPace, medium: ToPlummet, large: ToPlummet), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToSpan, medium: ToPlummet, large: ToPace), // construction measures
            'f' => ResolvePurposeSize(value, Format, small: ToHalfFinger, medium: ToSpan, large: ToPace), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToFathom, medium: ToFathom, large: ToPlummet),
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported."),
        };
        return result;
    }



    public override double ConvertBySize(LengthMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToHalfFinger((double)value),
            StandardMeasureSize.S => ToHalfSpan((double)value),
            StandardMeasureSize.M => ToSpan((double)value),
            StandardMeasureSize.L => ToPace((double)value),
            StandardMeasureSize.XL => ToMile((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };


    /// <summary>
    /// Converts from the base unit to the respctive unit of this method.
    /// </summary>
    /// <param name="Value">A length measured in base units, i.e. the default unit used by <see cref="LengthMeasure"/>.</param>
    /// <returns>A double value</returns>
    public static double ToHalfFinger(double Value) => Value * 100; // 1 cm

    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToFinger(double Value) => Value * 50; // 2 cm

    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToHalfSpan(double Value) => Value * 10; // 10 cm

    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToSpan(double Value) => Value * 5; // 20 cm

    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToPace(double Value) => Value; // 100 cm = 1m

    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToMile(double Value) => Value / 1000; // 1000 m


    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToFathom(double Value) => Value / 2; // 2 m

    /// <inheritdoc cref="ToHalfFinger(double)"/>
    public static double ToPlummet(double Value) => Value / 10; // 10 m



}




/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the Rohal standard measures and the imperial system.
/// </summary>
internal class LengthFoCoRohalImperial : UnitConverterBase<LengthMeasure, double>
{
    protected const double CmPerYard = 91.44; //
    protected const double MeterPerYard = 0.9144; //

    //public DereCultureInfo DereCulture { get; init; } = culture; // inherited

    /// <param name="culture">The culture information used to format and interpret length measurements.</param>
    public LengthFoCoRohalImperial(DereCultureInfo culture) => DereCulture = culture;


    public override double ConvertByPurpose(LengthMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            't' => ToMiddenmile((double)value), // travel distance in miles
            'b' => ResolvePurposeSize(value, Format, small: ToHalfThumb, medium: ToHalfThumb, large: ToYard), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToYard, medium: ToPlummet, large: ToPlummet), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToFoot, medium: ToPlummet, large: ToYard), // construction measures
            'f' => ResolvePurposeSize(value, Format, small: ToHalfThumb, medium: ToFoot, large: ToYard), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToFathom, medium: ToFathom, large: ToPlummet),
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported."),
        };
        return result;
    }



    public override double ConvertBySize(LengthMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToHalfThumb((double)value),
            StandardMeasureSize.S => ToHand((double)value),
            StandardMeasureSize.M => ToFoot((double)value),
            StandardMeasureSize.L => ToYard((double)value),
            StandardMeasureSize.XL => ToMiddenmile((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };

    public override double ConvertToBase(LengthMeasure value) => (double)value / MeterPerYard; // yards become yards


    public static double ToHalfThumb(double Value) => Value * 36 * 2; // 1 half thumb = 2 thumb = 2 inches
    public static double ToInch(double Value) => Value / MeterPerYard * 36; // 1 yard = 36 inches
    public static double ToHand(double Value) => Value / MeterPerYard * 9; // 1 yard = 9 hands
    public static double ToFoot(double Value) => Value / MeterPerYard * 3;
    public static double ToYard(double Value) => Value / MeterPerYard;
    public static double ToMiddenmile(double Value) => Value / MeterPerYard / 1094; // 1 mile = 1094 yards

    /// <inheritdoc cref="LengthFoCoRohalMetric.ToHalfFinger(double)"/>
    public static double ToFathom(double Value) => Value / MeterPerYard / 2; // 2 yrd

    /// <inheritdoc cref="LengthFoCoRohalMetric.ToHalfFinger(double)"/>
    public static double ToPlummet(double Value) => Value / MeterPerYard / 10; // 10 yrd

}




/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the dwarven measures. Because dwarven units are
/// hard to interpred, anyway, this converter does not distinguish between metric and imperial systems.
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class LengthFoCoDwarven : UnitConverterBase<LengthMeasure, double>
{
    protected const double RimPerMeter = 250; // 

    //public DereCultureInfo DereCulture { get; init; } // inherited;

    public LengthFoCoDwarven(DereCultureInfo culture) => DereCulture = culture;

    public override double ConvertByPurpose(LengthMeasure value, string Format)
    {
        ArgumentException.ThrowIfNullOrEmpty(Format);

        var result = Format[0] switch
        {
            't' => ResolvePurposeSize(value, Format, small: ToDorgrosh, medium: ToDorgrosh, large: ToPakash), // travel distance in miles
            'b' => ResolvePurposeSize(value, Format, small: ToRim, medium: ToRim, large: ToDrom), // body measures
            'm' => ResolvePurposeSize(value, Format, small: ToDrumod, medium: ToDrash, large: ToDumad), // mining measures
            'c' => ResolvePurposeSize(value, Format, small: ToDrom, medium: ToDrom, large: ToDrumod), // construction measures
            'f' => ResolvePurposeSize(value, Format, small: ToRim, medium: ToRim, large: ToDrom), // fabric measures
            'd' => ResolvePurposeSize(value, Format, small: ToDrash, medium: ToDumad, large: ToDorgrosh),
            _ => throw new NotSupportedException($"Size format '{Format}' is not supported."),
        };
        return result;
    }



    public override double ConvertBySize(LengthMeasure value, StandardMeasureSize size)
        => size switch
        {
            StandardMeasureSize.XS => ToRim((double)value),
            StandardMeasureSize.S => ToRim((double)value),
            StandardMeasureSize.M => ToDrom((double)value),
            StandardMeasureSize.L => ToDrumod((double)value),
            StandardMeasureSize.XL => ToDorgrosh((double)value),
            _ => throw new NotSupportedException($"Size format '{size}' is not supported."),
        };

    public override double ConvertToBase(LengthMeasure value) => (double)value * RimPerMeter; //


    // Dwarven Units
    public static double ToRim(double Value) => Value * RimPerMeter; // 1 notch
    public static double ToDrom(double Value) => Value * RimPerMeter / 70; //
    public static double ToDrumod(double Value) => Value * RimPerMeter / 70 / 6; //
    public static double ToDrash(double Value) => Value * RimPerMeter / 70 / 6 / 4; //
    public static double ToDumad(double Value) => Value * RimPerMeter / 70 / 6 / 4 / 11; //
    public static double ToDorgrosh(double Value) => Value * RimPerMeter / 70 / 6 / 4 / 11 / 16; //
    public static double ToPakash(double Value) => Value * RimPerMeter / 70 / 6 / 4 / 11 / 16 / 21; //
}





/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the Rohal standard measures and the imperial system.
/// It adds the Novadi baryd unit (day's march).
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class LengthFoCoNovadiMetric(DereCultureInfo culture) : LengthFoCoRohalMetric(culture)
{
    public static double ToBaryd(double Value) => Value / 1000 / 15; // 1 baryd = 15 miles
}





/// <summary>
/// This format converter (FoCo) Provides length conversion operations using the Rohal standard measures and the imperial system.
/// It adds the Novadi baryd unit (day's march).
/// </summary>
/// <param name="culture">The culture information used to format and interpret length measurements.</param>
internal class LengthFoCoNovadiImperial(DereCultureInfo culture) : LengthFoCoRohalImperial(culture)
{
    public static double ToBaryd(double Value) => Value * MeterPerYard / 1000 / 15; // 1 baryd = 15 miles
}