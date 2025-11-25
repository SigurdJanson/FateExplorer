using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class VolumeFoCoRohalImperialTests
{

    private static readonly object[] MetricCases =
    [
        new object[] { 1.00 },   // 1 m²
        new object[] { 1000.00*1000 }, // 1 km²
        new object[] { 0.0001 },  // 1 cm²
    ];


    internal static VolumeFoCoRohalImperial CreateFoCo()
    {
        return new VolumeFoCoRohalImperial();
    }


    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ConvertToBase(double inLiter)
    {
        // Arrange
        var squareFoCoRohalImperial = CreateFoCo();
        VolumeMeasure value = new(inLiter);

        // Act
        var result = squareFoCoRohalImperial.ConvertToBase(value);

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

        var squareFoCoRohalImperial = CreateFoCo();
        VolumeMeasure value = new(inAnglePaces);
        string Format = Purpose + size;
        double Expected = size switch
        {
            "L" => VolumeFoCoRohalImperial.ToBarrel(inAnglePaces),
            "M" => VolumeFoCoRohalImperial.ToMeasure(inAnglePaces),
            "S" => VolumeFoCoRohalImperial.ToOunce(inAnglePaces),
            "" => VolumeFoCoRohalImperial.ToOunce(inAnglePaces),
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
        [Values("m", "c", "d", "l", "p")] string purpose)
    {
        const string Small = "S", Medium = "M", Large = "L";
        // Arrange
        var squareFoCoRohalImperial = CreateFoCo();
        VolumeMeasure value = new(inAnglePaces);

        // Act
        double resultSmall = squareFoCoRohalImperial.ConvertByPurpose(value, purpose + Small);
        double resultMedium = squareFoCoRohalImperial.ConvertByPurpose(value, purpose + Medium);
        double resultLarge = squareFoCoRohalImperial.ConvertByPurpose(value, purpose + Large);

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

        var squareFoCoRohalImperial = CreateFoCo();
        VolumeMeasure value = new(inPaces);
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
            VolumeFoCoRohalImperial.ToOunce,
            VolumeFoCoRohalImperial.ToPint,
            VolumeFoCoRohalImperial.ToMeasure,
            VolumeFoCoRohalImperial.ToBarrel,
            VolumeFoCoRohalImperial.ToRoomYard
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
    public void ToOunce(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / VolumeMeasure.LitersPerOunce; // 

        // Act
        var result = VolumeFoCoRohalImperial.ToOunce(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToPint(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / VolumeMeasure.LitersPerOunce / 16; // 

        // Act
        var result = VolumeFoCoRohalImperial.ToPint(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToMeasure(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / VolumeMeasure.LitersPerOunce / 32; // 

        // Act
        var result = VolumeFoCoRohalImperial.ToMeasure(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToUrn(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / VolumeMeasure.LitersPerOunce / 336; // 

        // Act
        var result = VolumeFoCoRohalImperial.ToUrn(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToBarrel(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / VolumeMeasure.LitersPerOunce / 3376; // 3376 ounces in a barrel

        // Act
        var result = VolumeFoCoRohalImperial.ToBarrel(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToRoomFoot(double inLiter)
    {
        // Arrange
        // step 1: from liters to m³; step 2; from m³ to yd³; step 3: from yd³ to ft³
        const double ConversionFactor = 0.001 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard) * (3 * 3 * 3); // 

        // Act
        var result = VolumeFoCoRohalImperial.ToRoomFoot(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToRoomYard(double inLiter)
    {
        // Arrange
        // step 1: from liters to m³; step 2; from m³ to yd³
        const double ConversionFactor = 0.001 / (LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard * LengthMeasure.MeterPerYard); // 

        // Act
        var result = VolumeFoCoRohalImperial.ToRoomYard(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }
}

