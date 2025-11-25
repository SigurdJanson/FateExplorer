using NUnit.Framework;
using System;
using Aventuria;
using Aventuria.Measures;

namespace UnitTests.Measures;

[TestFixture]
public class LengthFoCoDwarvenTests
{
    private LengthFoCoDwarven _converter;

    [SetUp]
    public void SetUp()
    {
        _converter = new LengthFoCoDwarven();
    }

    internal static LengthFoCoDwarven CreateLengthFoCo()
    {
        return new LengthFoCoDwarven();
    }


    #region ConvertByPurpose Tests

    [TestCase("t", 10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4 / 11 / 16)]
    [TestCase("b", 10.0, ExpectedResult = 10.0 * 250)]
    [TestCase("m", 10.0, ExpectedResult = 10.0 * 250 / 70 / 6)]
    [TestCase("c", 10.0, ExpectedResult = 10.0 * 250 / 70)]
    [TestCase("f", 10.0, ExpectedResult = 10.0 * 250)]
    [TestCase("d", 10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4)]
    public double ConvertByPurpose_ReturnsExpectedValue(string format, double value)
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(value);

        // Act
        var result = _converter.ConvertByPurpose(lengthMeasure, format);

        // Assert
        return result;
    }

    [Test]
    public void ConvertByPurpose_SupportedPurposes_ReturnsNumbersInCorrectOrder(
    [Random(-1000.0, 1000.0, 1)] double inAnglePaces,
    [Values("t", "b", "m", "c", "f", "d")] string purpose)
    {
        const string Small = "S", Medium = "M", Large = "L";
        // Arrange
        var lengthFoCoDwarven = CreateLengthFoCo();
        LengthMeasure value = new(inAnglePaces);

        // Act
        double resultSmall = lengthFoCoDwarven.ConvertByPurpose(value, purpose + Small);
        double resultMedium = lengthFoCoDwarven.ConvertByPurpose(value, purpose + Medium);
        double resultLarge = lengthFoCoDwarven.ConvertByPurpose(value, purpose + Large);

        // Assert
        Assert.That(Math.Abs(resultSmall), Is.GreaterThanOrEqualTo(Math.Abs(resultMedium)));
        Assert.That(Math.Abs(resultMedium), Is.GreaterThanOrEqualTo(Math.Abs(resultLarge)));
    }

    [Test]
    public void ConvertByPurpose_InvalidFormat_ThrowsNotSupportedException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(10.0);
        var invalidFormat = "x";

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => _converter.ConvertByPurpose(lengthMeasure, invalidFormat));
    }

    [Test]
    public void ConvertByPurpose_NullFormat_ThrowsArgumentException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(10.0);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _converter.ConvertByPurpose(lengthMeasure, null));
    }

    [Test]
    public void ConvertByPurpose_EmptyFormat_ThrowsArgumentException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(10.0);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _converter.ConvertByPurpose(lengthMeasure, string.Empty));
    }

    #endregion



    #region ConvertBySize Tests

    [Test]
    public void ConvertBySize_CallsCorrectMethod([Random(-1000.0, 1000.0, 1)] double inPaces)
    {
        // Arrange
        StandardMeasureSize[] Format = [StandardMeasureSize.XS, StandardMeasureSize.S,
            StandardMeasureSize.M, StandardMeasureSize.L, StandardMeasureSize.XL];
        Func<double, double>[] ExpectedMethods = [
            LengthFoCoDwarven.ToRim,
            LengthFoCoDwarven.ToRim,
            LengthFoCoDwarven.ToDrom,
            LengthFoCoDwarven.ToDrumod,
            LengthFoCoDwarven.ToDorgrosh
        ];
        LengthMeasure value = new(inPaces);

        for (int i = 0; i < Format.Length; i++)
        {
            // Act
            var result = _converter.ConvertBySize(value, Format[i]);
            // Assert
            var expected = ExpectedMethods[i](inPaces);
            Assert.That(result, Is.EqualTo(expected).Within(1).Ulps);
        }
    }

    [TestCase(StandardMeasureSize.XS, 10.0, ExpectedResult = 10.0 * 250)]
    [TestCase(StandardMeasureSize.S, 10.0, ExpectedResult = 10.0 * 250)]
    [TestCase(StandardMeasureSize.M, 10.0, ExpectedResult = 10.0 * 250 / 70)]
    [TestCase(StandardMeasureSize.L, 10.0, ExpectedResult = 10.0 * 250 / 70 / 6)]
    [TestCase(StandardMeasureSize.XL, 10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4 / 11 / 16)]
    public double ConvertBySize_ReturnsExpectedValue(StandardMeasureSize size, double value)
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(value);

        // Act
        var result = _converter.ConvertBySize(lengthMeasure, size);

        // Assert
        return result;
    }

    [Test]
    public void ConvertBySize_UnsupportedSize_ThrowsNotSupportedException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(10.0);
        var unsupportedSize = (StandardMeasureSize)999; // Assuming this is an invalid size

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => _converter.ConvertBySize(lengthMeasure, unsupportedSize));
    }

    #endregion

    #region ConvertToBase Tests

    [TestCase(10.0, ExpectedResult = 10.0 * 250, TestName = "ConvertToBase_ReturnsExpectedValue")]
    public double ConvertToBase_ReturnsExpectedValue(double value)
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(value);

        // Act
        var result = _converter.ConvertToBase(lengthMeasure);

        // Assert
        return result;
    }

    #endregion

    #region Static Conversion Methods Tests

    [TestCase(10.0, ExpectedResult = 10.0 * 250, TestName = "ToRim_ReturnsExpectedValue")]
    public double ToRim_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToRim(value);

        // Assert
        return result;
    }

    [TestCase(10.0, ExpectedResult = 10.0 * 250 / 70, TestName = "ToDrom_ReturnsExpectedValue")]
    public double ToDrom_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToDrom(value);

        // Assert
        return result;
    }

    [TestCase(10.0, ExpectedResult = 10.0 * 250 / 70 / 6, TestName = "ToDrumod_ReturnsExpectedValue")]
    public double ToDrumod_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToDrumod(value);

        // Assert
        return result;
    }

    [TestCase(10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4, TestName = "ToDrash_ReturnsExpectedValue")]
    public double ToDrash_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToDrash(value);

        // Assert
        return result;
    }

    [TestCase(10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4 / 11, TestName = "ToDumad_ReturnsExpectedValue")]
    public double ToDumad_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToDumad(value);

        // Assert
        return result;
    }

    [TestCase(10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4 / 11 / 16, TestName = "ToDorgrosh_ReturnsExpectedValue")]
    public double ToDorgrosh_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToDorgrosh(value);

        // Assert
        return result;
    }

    [TestCase(10.0, ExpectedResult = 10.0 * 250 / 70 / 6 / 4 / 11 / 16 / 21, TestName = "ToPakash_ReturnsExpectedValue")]
    public double ToPakash_ReturnsExpectedValue(double value)
    {
        // Act
        var result = LengthFoCoDwarven.ToPakash(value);

        // Assert
        return result;
    }


    #endregion

}
