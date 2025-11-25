using System.Numerics;

namespace Aventuria.Measures;


public enum StandardMeasureSize
{
    XS, S, M, L, XL
}

/// <summary>
/// Base class for unit converters converting between the base units used by a specific measurement type
/// and the culture-dependent unit used by a culture on Dere. <br/>Used by <see cref="DereCultureInfo"/>.
/// </summary>
/// <typeparam name="TMeasure">The type of the target measurement, such as <see cref="LengthMeasure"/></typeparam>
/// <typeparam name="TUnit">The type of the target unit the measure is represented in.</typeparam>
public abstract class UnitConverterBase<TMeasure, TBaseType> 
    where TMeasure : IMeasure,
                     IMultiplyOperators<TMeasure, int, TMeasure>, 
                     IMultiplyOperators<TMeasure, TBaseType, TMeasure>
{

    /// <summary>
    /// Convert value into another unit using a purpose-based format definition.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="Format"></param>
    /// <returns></returns>
    public abstract TBaseType ConvertByPurpose(TMeasure value, string Format);

    /// <summary>
    /// Convert value into another unit using a format definition based on t-shirt size.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="Format">Either "XS", "S", "M", "L", "XL"</param>
    /// <returns></returns>
    public abstract TBaseType ConvertBySize(TMeasure value, StandardMeasureSize size);


    /// <summary>
    /// Convert (universal base) value into the base unit of the given culturen.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public abstract TBaseType ConvertToBase(TMeasure value);


    /// <summary>
    /// Helper method to resolve size-based purpose conversions.
    /// Converts to a size requested in the format string. Uses the smallest size as default when the format is missing or invalid.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <param name="Format">Designated format. This method uses the second place of the string. Supports S, M, L on this place.</param>
    /// <param name="small">Conversion function used for 'small' units ('S') and alternative conversion in case the format is missing or invalid.</param>
    /// <param name="medium">Conversion function used for 'medium' units ('M').</param>
    /// <param name="large">Conversion function used for 'large' units ('L').</param>
    /// <returns></returns>
    protected static double ResolvePurposeSize(TMeasure value, string Format, 
        Func<double, double> small, 
        Func<double, double> medium, 
        Func<double, double> large)
    {
        if (Format.Length > 1)
        {
            if (Format[1] == 'L')
                return large(value.ToDouble());
            if (Format[1] == 'M')
                return medium(value.ToDouble());
        }
        return small(value.ToDouble());
    }



    /// <inheritdoc cref="ResolvePurposeSize"/>
    protected static decimal ResolvePurposeSize(TMeasure value, string Format,
        Func<decimal, decimal> small,
        Func<decimal, decimal> medium,
        Func<decimal, decimal> large)
    {
        if (Format.Length > 1)
        {
            if (Format[1] == 'L')
                return large(value.ToDecimal());
            if (Format[1] == 'M')
                return medium(value.ToDecimal());
        }
        return small(value.ToDecimal());
    }
}



