using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class VolumeFocoDwarvenImperialTests
{

    private static readonly object[] MetricCases =
    [
        new object[] { 1.00 },   // 1 m²
        new object[] { 1000.00*1000 }, // 1 km²
        new object[] { 0.0001 },  // 1 cm²
    ];


    internal static VolumeFoCoDwarvenImperial CreateFoCo()
    {
        return new VolumeFoCoDwarvenImperial(new DereCultureInfo("MidRealm", "de"))
        {
            DereCulture = new DereCultureInfo("MidRealm", "de")
        };
    }


    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ConvertToBase(double inLiter)
    {
        // Arrange
        var volumeFoCoDwarvenImperial = CreateFoCo();
        VolumeMeasure value = new(inLiter);

        // Act
        var result = volumeFoCoDwarvenImperial.ConvertToBase(value);

        // Assert
        Assert.That((double)result, Is.EqualTo(inLiter));
    }

    [Test]
    public void ConvertByPurpose_Liquid(
        [Random(-1000.0, 1000.0, 1)] double inAnglePaces,
        [Values("", "S", "M", "L")] string size)
    {
        // Arrange
        const string Purpose = "l";

        var volumeFoCoDwarvenImperial = CreateFoCo();
        VolumeMeasure value = new(inAnglePaces);
        string Format = Purpose + size;
        double Expected = size switch
        {
            "L" => VolumeFoCoDwarvenImperial.ToBaroshtrom(inAnglePaces),
            "M" => VolumeFoCoDwarvenImperial.ToBarosht(inAnglePaces),
            "S" => VolumeFoCoDwarvenImperial.ToBarosht(inAnglePaces),
            "" => VolumeFoCoDwarvenImperial.ToBarosht(inAnglePaces),
            _ => throw new NotSupportedException()
        };

        // Act
        double result = volumeFoCoDwarvenImperial.ConvertByPurpose(value, Format);

        // Assert
        Assert.That(result, Is.EqualTo(Expected));
    }


    [Test]
    public void ConvertByPurpose_SupportedPurposes_ReturnsNumbersInCorrectOrder(
        [Random(-1000.0, 1000.0, 1)] double inAnglePaces,
        [Values("m", "c", "d", "l", "p")] string purpose)
    {
        const string Small = "S", Medium = "M", Large = "L";
        // Arrange
        var volumeFoCoDwarvenImperial = CreateFoCo();
        VolumeMeasure value = new(inAnglePaces);

        // Act
        double resultSmall = volumeFoCoDwarvenImperial.ConvertByPurpose(value, purpose + Small);
        double resultMedium = volumeFoCoDwarvenImperial.ConvertByPurpose(value, purpose + Medium);
        double resultLarge = volumeFoCoDwarvenImperial.ConvertByPurpose(value, purpose + Large);

        // Assert
        Assert.That(Math.Abs(resultSmall), Is.GreaterThanOrEqualTo(Math.Abs(resultMedium)));
        Assert.That(Math.Abs(resultMedium), Is.GreaterThanOrEqualTo(Math.Abs(resultLarge)));
    }


    [Test]
    public void ConvertByPurpose_UnsupportedPurpose(
        [Random(-1000.0, 1000.0, 1)] double inPaces,
        [Values("", "S", "M", "L")] string size,
        [Values("a", "t", "b", "f")] string purpose)
    {
        // Arrange
        double ConversionFactor = size == "L" ? 1 : 100;

        var volumeFoCoDwarvenImperial = CreateFoCo();
        VolumeMeasure value = new(inPaces);
        string Format = purpose + size;

        // Act
        double result;

        // Assert
        Assert.Throws<NotSupportedException>(() =>
        {
            result = volumeFoCoDwarvenImperial.ConvertByPurpose(value, Format);
        });
    }




    [Test]
    public void ConvertBySize_CallsCorrectMethod([Random(-1000.0, 1000.0, 1)] double inPaces)
    {
        // Arrange
        StandardMeasureSize[] Format = [StandardMeasureSize.XS, StandardMeasureSize.S,
            StandardMeasureSize.M, StandardMeasureSize.L, StandardMeasureSize.XL];
        Func<double, double>[] ExpectedMethods = [
            VolumeFoCoDwarvenImperial.ToBarosht,
            VolumeFoCoDwarvenImperial.ToBarosht,
            VolumeFoCoDwarvenImperial.ToBarosht,
            VolumeFoCoDwarvenImperial.ToBaroshtrom,
            VolumeFoCoDwarvenImperial.ToBaroshtrom
        ];
        var squareFoCo = CreateFoCo();
        VolumeMeasure value = new(inPaces);

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
        var squareFoCo = CreateFoCo();
        var lengthMeasure = new VolumeMeasure(100.0);
        var unsupportedSize = (StandardMeasureSize)999; // invalid size

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => squareFoCo.ConvertBySize(lengthMeasure, unsupportedSize));
    }




    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToBarosht(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (VolumeMeasure.LitersPerOunce * 32); // 

        // Act
        var result = VolumeFoCoDwarvenImperial.ToBarosht(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToBaroshtrom(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / (VolumeMeasure.LitersPerOunce * 32) / 76; // 

        // Act
        var result = VolumeFoCoDwarvenImperial.ToBaroshtrom(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

}

