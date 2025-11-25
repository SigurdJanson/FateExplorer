using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class LengthFoCoRohalMetricTests
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
        new object[] { 1.00 },   // 1 meter
        new object[] { 1000.00 }, // 1 kilometer
        new object[] { 0.01 },  // 1 centimeter
    ];


    internal static LengthFoCoRohalMetric CreateLengthFoCo()
    {
        return new LengthFoCoRohalMetric();
    }


    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ConvertToBase(double inMeter)
    {
        // Arrange
        var lengthFoCoRohalMetric = CreateLengthFoCo();
        LengthMeasure value = new(inMeter);

        // Act
        var result = lengthFoCoRohalMetric.ConvertToBase(value);

        // Assert
        Assert.That((double)result, Is.EqualTo(inMeter));
    }

    [Test]
    public void ConvertByPurpose_Travelling_Miles([Random(-1000.0, 1000.0, 1)] double inPaces, [Values("", "S", "M", "L")] string size)
    {
        // Arrange
        const double ConversionFactor = 0.001;
        const string Purpose = "t";

        var lengthFoCoRohalMetric = CreateLengthFoCo();
        LengthMeasure value = new(inPaces);
        string Format = Purpose + size;

        // Act
        var result = lengthFoCoRohalMetric.ConvertByPurpose(value, Format);

        // Assert
        Assert.That(result, Is.EqualTo(inPaces * ConversionFactor));
    }

    [Test]
    public void ConvertByPurpose_Body([Random(-1000.0, 1000.0, 1)] double inPaces, [Values("", "S", "M", "L")] string size)
    {
        // Arrange
        double ConversionFactor = size == "L" ? 1 : 100;
        const string Purpose = "b";

        var lengthFoCoRohalMetric = CreateLengthFoCo();
        LengthMeasure value = new(inPaces);
        string Format = Purpose + size;

        // Act
        var result = lengthFoCoRohalMetric.ConvertByPurpose(value, Format);

        // Assert
        Assert.That(result, Is.EqualTo(inPaces * ConversionFactor));
    }

    [Test]
    public void ConvertByPurpose_SupportedPurposes_ReturnsNumbersInCorrectOrder(
        [Random(-1000.0, 1000.0, 1)] double inAnglePaces,
        [Values("t", "b", "m", "c", "f", "d")] string purpose)
    {
        const string Small = "S", Medium = "M", Large = "L";
        // Arrange
        var lengthFoCo = CreateLengthFoCo();
        LengthMeasure value = new(inAnglePaces);

        // Act
        double resultSmall = lengthFoCo.ConvertByPurpose(value, purpose + Small);
        double resultMedium = lengthFoCo.ConvertByPurpose(value, purpose + Medium);
        double resultLarge = lengthFoCo.ConvertByPurpose(value, purpose + Large);

        // Assert
        Assert.That(Math.Abs(resultSmall), Is.GreaterThanOrEqualTo(Math.Abs(resultMedium)));
        Assert.That(Math.Abs(resultMedium), Is.GreaterThanOrEqualTo(Math.Abs(resultLarge)));
    }


    [Test]
    public void ConvertBySize_CallsCorrectMethod([Random(-1000.0, 1000.0, 1)] double inPaces)
    {
        // Arrange
        StandardMeasureSize[] Format = [StandardMeasureSize.XS, StandardMeasureSize.S, 
            StandardMeasureSize.M, StandardMeasureSize.L, StandardMeasureSize.XL];
        Func<double, double>[] ExpectedMethods = [
            LengthFoCoRohalMetric.ToHalfFinger,
            LengthFoCoRohalMetric.ToHalfSpan,
            LengthFoCoRohalMetric.ToSpan,
            LengthFoCoRohalMetric.ToPace,
            LengthFoCoRohalMetric.ToMile
        ];
        var lengthFoCo = CreateLengthFoCo();
        LengthMeasure value = new(inPaces);

        for (int i = 0; i < Format.Length; i++)
        {
            // Act
            var result = lengthFoCo.ConvertBySize(value, Format[i]);
            // Assert
            var expected = ExpectedMethods[i](inPaces);
            Assert.That(result, Is.EqualTo(expected).Within(1).Ulps);
        }
    }

    [Test]
    public void ConvertBySize_UnsupportedSize_ThrowsNotSupportedException()
    {
        // Arrange
        var lengthFoCo = CreateLengthFoCo();
        var lengthMeasure = new LengthMeasure(100.0);
        var unsupportedSize = (StandardMeasureSize)999; // invalid size

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => lengthFoCo.ConvertBySize(lengthMeasure, unsupportedSize));
    }




    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToHalfFinger(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 100; // 1 half finger = 0.01 meters
        
        // Act
        var result = LengthFoCoRohalMetric.ToHalfFinger(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToFinger(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 50; // 1 finger = 0.02 meters = 2 cm

        // Act
        var result = LengthFoCoRohalMetric.ToFinger(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToHalfSpan(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 10; // 1 half span = 0.1 meters = 10 cm

        // Act
        var result = LengthFoCoRohalMetric.ToHalfSpan(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToSpan(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 5; // 1 span = 0.2 meters = 20 cm

        // Act
        var result = LengthFoCoRohalMetric.ToSpan(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToPace(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 1; // 

        // Act
        var result = LengthFoCoRohalMetric.ToPace(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToMile(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 0.001; // 1 mile = 1000 meters

        // Act
        var result = LengthFoCoRohalMetric.ToMile(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToFathom(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 0.5; // 1 fathom = 2 meters

        // Act
        var result = LengthFoCoRohalMetric.ToFathom(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor));
    }

    [Test]
    [TestCaseSource(nameof(MetricCases))]
    public void ToPlummet(double inMeter)
    {
        // Arrange
        const double ConversionFactor = 0.1; // 1 plummet = 10 meters

        // Act
        var result = LengthFoCoRohalMetric.ToPlummet(inMeter);

        // Assert
        Assert.That(result, Is.EqualTo(inMeter * ConversionFactor).Within(1).Ulps);
    }
}
