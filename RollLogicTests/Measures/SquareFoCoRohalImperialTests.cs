using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class SquareFoCoRohalImperialTests
{
    //private static readonly object[] MetricCases =
    //[
    //    new object[] { 1 / 0.9144, 1.000 },   // 1 meter = 1.09361 yards
    //    new object[] { 1000 / 0.9144, 1000.00 }, // 1 kilometer = 1093.6132983377 yards
    //    new object[] { 0.01 / 0.9144, 0.01 },  // 1 centimeter = 0.010936132983377 yards
    //    new object[] { 1, 0.9144 },   // 
    //];
    private static readonly object[] MetricCases =
    [
        new object[] { 1.00 },   // 1 m²
        new object[] { 1000.00*1000 }, // 1 km²
        new object[] { 0.0001 },  // 1 cm²
    ];


    internal static SquareFoCoRohalImperial CreateSquareFoCoRohalImperial()
    {
        return new SquareFoCoRohalImperial();
    }


    /// <summary>
    /// This test ensures only that ConvertToBase uses Imperial Yard. The correctness of the conversion is tested in <see cref="ToImperialYard"/> test.
    /// </summary>
    /// <param name="inSqrMeter"></param>
    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ConvertToBase_ResultIdenticalToImperialYards(double inSqrMeter)
    {
        // Arrange
        double InSquareYard = SquareFoCoRohalImperial.ToImperialYard(inSqrMeter); // dependency on method
        var squareFoCoRohalImperial = CreateSquareFoCoRohalImperial();
        SquareMeasure value = new(inSqrMeter);

        // Act
        var result = squareFoCoRohalImperial.ConvertToBase(value);

        // Assert
        Assert.That((double)result, Is.EqualTo(InSquareYard));
    }

    [Test]
    public void ConvertByPurpose_Agricultural_Miles(
        [Random(-1000.0, 1000.0, 1)] double inAnglePaces, 
        [Values("", "S", "M", "L")] string size)
    {
        // Arrange
        const string Purpose = "a";

        var squareFoCoRohalImperial = CreateSquareFoCoRohalImperial();
        SquareMeasure value = new(inAnglePaces);
        string Format = Purpose + size;
        double Expected = size switch
        {
            "L" => SquareFoCoRohalImperial.ToLand(inAnglePaces),
            "M" => SquareFoCoRohalImperial.ToImperialMile(inAnglePaces),
            "S" => SquareFoCoRohalImperial.ToMorgen(inAnglePaces),
            "" => SquareFoCoRohalImperial.ToMorgen(inAnglePaces),
            _ => throw new NotSupportedException()
        };

        // Act
        double result = squareFoCoRohalImperial.ConvertByPurpose(value, Format);

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }

    [Test]
    public void ConvertByPurpose_SupportedPurposes_ReturnsNumbersInCorrectOrder(
    [Random(-1000.0, 1000.0, 1)] double inAnglePaces,
    [Values("a", "m", "c", "f")] string purpose)
    {
        const string Small = "S", Medium = "M", Large = "L";
        // Arrange
        var squareFoCoRohalMetric = CreateSquareFoCoRohalImperial();
        SquareMeasure value = new(inAnglePaces);

        // Act
        double resultSmall = squareFoCoRohalMetric.ConvertByPurpose(value, purpose + Small);
        double resultMedium = squareFoCoRohalMetric.ConvertByPurpose(value, purpose + Medium);
        double resultLarge = squareFoCoRohalMetric.ConvertByPurpose(value, purpose + Large);

        // Assert
        Assert.That(Math.Abs(resultSmall), Is.GreaterThanOrEqualTo(Math.Abs(resultMedium)));
        Assert.That(Math.Abs(resultMedium), Is.GreaterThanOrEqualTo(Math.Abs(resultLarge)));
    }

    [Test]
    public void ConvertByPurpose_UnsupportedPurpose(
        [Random(-1000.0, 1000.0, 1)] double inPaces, 
        [Values("", "S", "M", "L")] string size,
        [Values("t", "b", "d")] string purpose)
    {
        // Arrange
        double ConversionFactor = size == "L" ? 1 : 100;

        var squareFoCoRohalImperial = CreateSquareFoCoRohalImperial();
        SquareMeasure value = new(inPaces);
        string Format = purpose + size;

        // Act
        double result;

        // Assert
        Assert.Throws<NotSupportedException>(() =>
        {
            result = squareFoCoRohalImperial.ConvertByPurpose(value, Format);
        });
    }




    [Test]
    public void ConvertBySize_CallsCorrectMethod([Random(-1000.0, 1000.0, 1)] double inPaces)
    {
        // Arrange
        StandardMeasureSize[] Format = [StandardMeasureSize.XS, StandardMeasureSize.S,
            StandardMeasureSize.M, StandardMeasureSize.L, StandardMeasureSize.XL];
        Func<double, double>[] ExpectedMethods = [
            SquareFoCoRohalImperial.ToImperialInch,
            SquareFoCoRohalImperial.ToImperialFoot,
            SquareFoCoRohalImperial.ToImperialYard,
            SquareFoCoRohalImperial.ToImperialMile,
            SquareFoCoRohalImperial.ToLand
        ];
        var squareFoCo = CreateSquareFoCoRohalImperial();
        SquareMeasure value = new(inPaces);

        for (int i = 0; i < Format.Length; i++)
        {
            // Act
            var result = squareFoCo.ConvertBySize(value, Format[i]);
            // Assert
            var expected = ExpectedMethods[i](inPaces);
            Assert.That(result, Is.EqualTo(expected).Within(1).Ulps);
        }
    }

    [Test]
    public void ConvertBySize_InvalidSize_ThrowsNotSupportedException()
    {
        // Arrange
        var squareFoCo = CreateSquareFoCoRohalImperial();
        var lengthMeasure = new SquareMeasure(100.0);
        var unsupportedSize = (StandardMeasureSize)999; // invalid size

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => squareFoCo.ConvertBySize(lengthMeasure, unsupportedSize));
    }




    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToImperialInch(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard) * 36 * 36; // 

        // Act
        var result = SquareFoCoRohalImperial.ToImperialInch(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToImperialFoot(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard) * 9; // 

        // Act
        var result = SquareFoCoRohalImperial.ToImperialFoot(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToImperialYard(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard); // 

        // Act
        var result = SquareFoCoRohalImperial.ToImperialYard(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToMorgen(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard) / 100 / 100; // 

        // Act
        var result = SquareFoCoRohalImperial.ToMorgen(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToImperialMile(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard * 1094 * 1094); // 

        // Act
        var result = SquareFoCoRohalImperial.ToImperialMile(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToLand(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard * 1094 * 1094 * 2400 * 2400); // 

        // Act
        var result = SquareFoCoRohalImperial.ToLand(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor).Within(1).Ulps);
    }

}
