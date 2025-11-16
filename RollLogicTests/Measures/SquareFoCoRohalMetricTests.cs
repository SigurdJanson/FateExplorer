using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class SquareFoCoRohalMetricTests
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


    internal static SquareFoCoRohalMetric CreateSquareFoCoRohalMetric()
    {
        return new SquareFoCoRohalMetric(new DereCultureInfo("MidRealm", "de"))
        {
            DereCulture = new DereCultureInfo("MidRealm", "de")
        };
    }


    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ConvertToBase(double inSqrMeter)
    {
        // Arrange
        var squareFoCoRohalMetric = CreateSquareFoCoRohalMetric();
        SquareMeasure value = new(inSqrMeter);

        // Act
        var result = squareFoCoRohalMetric.ConvertToBase(value);

        // Assert
        Assert.That((double)result, Is.EqualTo(inSqrMeter));
    }

    [Test]
    public void ConvertByPurpose_Agricultural_Miles(
        [Random(-1000.0, 1000.0, 1)] double inAnglePaces, 
        [Values("", "S", "M", "L")] string size)
    {
        // Arrange
        const string Purpose = "a";

        var squareFoCoRohalMetric = CreateSquareFoCoRohalMetric();
        SquareMeasure value = new(inAnglePaces);
        string Format = Purpose + size;
        double Expected = size switch
        {
            "L" => SquareFoCoRohalMetric.ToAcre(inAnglePaces),
            "M" => SquareFoCoRohalMetric.ToHectare(inAnglePaces),
            "S" => SquareFoCoRohalMetric.ToSquare(inAnglePaces),
            "" => SquareFoCoRohalMetric.ToSquare(inAnglePaces),
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
        [Values("a", "m", "c", "f")] string purpose)
    {
        const string Small = "S", Medium = "M", Large = "L";
        // Arrange
        var squareFoCoRohalMetric = CreateSquareFoCoRohalMetric();
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

        var squareFoCoRohalMetric = CreateSquareFoCoRohalMetric();
        SquareMeasure value = new(inPaces);
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
            SquareFoCoRohalMetric.ToAngleSpan,
            SquareFoCoRohalMetric.ToAngleSpan,
            SquareFoCoRohalMetric.ToAnglePace,
            SquareFoCoRohalMetric.ToAngleMile,
            SquareFoCoRohalMetric.ToAcre
        ];
        var squareFoCo = CreateSquareFoCoRohalMetric();
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
        var squareFoCo = CreateSquareFoCoRohalMetric();
        var lengthMeasure = new SquareMeasure(100.0);
        var unsupportedSize = (StandardMeasureSize)999; // invalid size

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => squareFoCo.ConvertBySize(lengthMeasure, unsupportedSize));
    }




    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToAngleSpan(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1 / 0.04; // 

        // Act
        var result = SquareFoCoRohalMetric.ToAngleSpan(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToAnglePace(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1; // 

        // Act
        var result = SquareFoCoRohalMetric.ToAnglePace(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToSquare(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 625; // 

        // Act
        var result = SquareFoCoRohalMetric.ToSquare(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToHectare(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 10000; // 100 x 100 meters

        // Act
        var result = SquareFoCoRohalMetric.ToHectare(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToAngleMile(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1.0 / 1E6; // 

        // Act
        var result = SquareFoCoRohalMetric.ToAngleMile(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToAcre(double inSqrMeter)
    {
        // Arrange
        const double ConversionFactor = 1 / 4E6; // 

        // Act
        var result = SquareFoCoRohalMetric.ToAcre(inSqrMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inSqrMeter * ConversionFactor));
    }

}
