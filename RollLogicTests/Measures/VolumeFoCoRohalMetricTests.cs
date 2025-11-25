using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class VolumeFoCoRohalMetricTests
{

    private static readonly object[] MetricCases =
    [
        new object[] { 1.00 },   // 1 m²
        new object[] { 1000.00*1000 }, // 1 km²
        new object[] { 0.0001 },  // 1 cm²
    ];


    internal static VolumeFoCoRohalMetric CreateFoCo()
    {
        return new VolumeFoCoRohalMetric(new DereCultureInfo("MidRealm", "de"))
        {
            DereCulture = new DereCultureInfo("MidRealm", "de")
        };
    }


    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ConvertToBase(double inLiter)
    {
        // Arrange
        var squareFoCoRohalMetric = CreateFoCo();
        VolumeMeasure value = new(inLiter);

        // Act
        var result = squareFoCoRohalMetric.ConvertToBase(value);

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

        var squareFoCoRohalMetric = CreateFoCo();
        VolumeMeasure value = new(inAnglePaces);
        string Format = Purpose + size;
        double Expected = size switch
        {
            "L" => VolumeFoCoRohalMetric.ToCask(inAnglePaces),
            "M" => VolumeFoCoRohalMetric.ToQuart(inAnglePaces),
            "S" => VolumeFoCoRohalMetric.ToFlow(inAnglePaces),
            "" => VolumeFoCoRohalMetric.ToFlow(inAnglePaces),
            _ => throw new NotSupportedException()
        };

        // Act
        double result = squareFoCoRohalMetric.ConvertByPurpose(value, Format);

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
        var squareFoCoRohalMetric = CreateFoCo();
        VolumeMeasure value = new(inAnglePaces);

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
        [Values("a", "t", "b", "f")] string purpose)
    {
        // Arrange
        double ConversionFactor = size == "L" ? 1 : 100;

        var squareFoCoRohalMetric = CreateFoCo();
        VolumeMeasure value = new(inPaces);
        string Format = purpose + size;

        // Act
        double result;

        // Assert
        Assert.Throws<NotSupportedException>(() =>
        {
            result = squareFoCoRohalMetric.ConvertByPurpose(value, Format);
        });
    }




    [Test]
    public void ConvertBySize_CallsCorrectMethod([Random(-1000.0, 1000.0, 1)] double inPaces)
    {
        // Arrange
        StandardMeasureSize[] Format = [StandardMeasureSize.XS, StandardMeasureSize.S,
            StandardMeasureSize.M, StandardMeasureSize.L, StandardMeasureSize.XL];
        Func<double, double>[] ExpectedMethods = [
            VolumeFoCoRohalMetric.ToFlow,
            VolumeFoCoRohalMetric.ToDraught,
            VolumeFoCoRohalMetric.ToQuart,
            VolumeFoCoRohalMetric.ToCask,
            VolumeFoCoRohalMetric.ToRoomPace
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
    public void ToFlow(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 0.01; // 

        // Act
        var result = VolumeFoCoRohalMetric.ToFlow(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToDraught(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 4; // 

        // Act
        var result = VolumeFoCoRohalMetric.ToDraught(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToQuart(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0; // 

        // Act
        var result = VolumeFoCoRohalMetric.ToQuart(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToUrn(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 10; // 1/10th liter

        // Act
        var result = VolumeFoCoRohalMetric.ToUrn(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToCask(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 100; // 

        // Act
        var result = VolumeFoCoRohalMetric.ToCask(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor).Within(1).Ulps);
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToRoomPace(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 1000; // 

        // Act
        var result = VolumeFoCoRohalMetric.ToRoomPace(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToOx(double inLiter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 1200; // 

        // Act
        var result = VolumeFoCoRohalMetric.ToOx(inLiter);

        // Assert
        Assert.That(result, Is.EqualTo(inLiter * ConversionFactor));
    }
}
