using Aventuria.Measures;
using NUnit.Framework;
using System;
using Aventuria;


namespace UnitTests.Measures;

[TestFixture]
public class LengthFoCoRohalImperialTests
{

    private LengthFoCoRohalImperial _converter;
    private DereCultureInfo _dereCulture;

    [SetUp]
    public void Setup()
    {
        _dereCulture = new DereCultureInfo("MidRealm", "en");
        _converter = new LengthFoCoRohalImperial(_dereCulture)
        { 
            DereCulture = _dereCulture
        };
    }

    internal static LengthFoCoRohalImperial CreateTestObject()
    {
        return new LengthFoCoRohalImperial(new DereCultureInfo("MidRealm", "de"))
        {
            DereCulture = new DereCultureInfo("MidRealm", "de")
        };
    }




    #region ConvertByPurpose Tests

    [Test]
    [TestCase("t", 100.0, ExpectedResult = 109.36132983377077865266841645 / 1094)] // Travel
    [TestCase("b", 10.0, ExpectedResult = 720.0)]  // Body (small)
    [TestCase("m", 100.0, ExpectedResult = 109.36132983377077865266841645)] // Mining (small)
    [TestCase("c", 100.0, ExpectedResult = 328.08398950131233595800524935)] // Construction (small)
    [TestCase("f", 100.0, ExpectedResult = 7200.0)]  // Fabric (small)
    [TestCase("d", 100.0, ExpectedResult = 109.36132983377077865266841645 / 2)] // Depth (small)
    public double ConvertByPurpose_ReturnsExpectedResult(string format, double value)
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(value);

        // Act
        var result = _converter.ConvertByPurpose(lengthMeasure, format);

        // Assert
        return result;
    }

    [Test]
    public void ConvertByPurpose_InvalidFormat_ThrowsNotSupportedException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(100.0);
        var invalidFormat = "x";

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => _converter.ConvertByPurpose(lengthMeasure, invalidFormat));
    }

    [Test]
    public void ConvertByPurpose_NullFormat_ThrowsArgumentException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(100.0);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _converter.ConvertByPurpose(lengthMeasure, null));
    }

    [Test]
    public void ConvertByPurpose_EmptyFormat_ThrowsArgumentException()
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(100.0);

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
            LengthFoCoRohalImperial.ToHalfThumb,
            LengthFoCoRohalImperial.ToHand,
            LengthFoCoRohalImperial.ToFoot,
            LengthFoCoRohalImperial.ToYard,
            LengthFoCoRohalImperial.ToMiddenmile
        ];
        var lengthFoCo = CreateTestObject();
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
        var lengthMeasure = new LengthMeasure(100.0);
        var unsupportedSize = (StandardMeasureSize)999; // invalid size

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => _converter.ConvertBySize(lengthMeasure, unsupportedSize));
    }

    #endregion



    #region ConvertToBase Tests

    [Test]
    [TestCase(100.0, ExpectedResult = 109.36132983377078)]
    public double ConvertToBase_ReturnsExpectedResult(double value)
    {
        // Arrange
        var lengthMeasure = new LengthMeasure(value);

        // Act
        var result = _converter.ConvertToBase(lengthMeasure);

        // Assert
        return result;
    }

    #endregion



    #region Static Conversion Method Tests

    [Test]
    [TestCase(100.0, ExpectedResult = 7200.0)]
    public double ToToHalfThumb_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToHalfThumb(value);

        // Assert
        return result;
    }

    [Test]
    [TestCase(100.0, ExpectedResult = 3937.007874015748031)] //3937.00787402
    public double ToInch_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToInch(value);

        // Assert
        return result;
    }


    [Test]
    [TestCase(100.0, ExpectedResult = 3937.007874015748031 / 4)] //
    public double ToHand_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToHand(value);

        // Assert
        return result;


    }


    [Test]
    [TestCase(100.0, ExpectedResult = 328.08398950131233595800524935)] //
    public double ToFoot_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToFoot(value);

        // Assert
        return result;
    }

    
    [Test]
    [TestCase(100.0, ExpectedResult = 109.36132983377077865266841645)] //
    public double ToYard_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToYard(value);

        // Assert
        return result;
    }


    [Test]
    [TestCase(100.0, ExpectedResult = 109.36132983377077865266841645 / 1094)] //
    public double ToMiddenmile_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToMiddenmile(value);

        // Assert
        return result;
    }


    [Test]
    [TestCase(100.0, ExpectedResult = 109.36132983377077865266841645/2)] //
    public double ToFathom_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToFathom(value);

        // Assert
        return result;
    }


    [Test]
    [TestCase(100.0, ExpectedResult = 109.36132983377077865266841645/10)] //
    public double ToPlummet_ReturnsExpectedResult(double value)
    {
        // Act
        var result = LengthFoCoRohalImperial.ToPlummet(value);

        // Assert
        return result;
    }

    #endregion
}



