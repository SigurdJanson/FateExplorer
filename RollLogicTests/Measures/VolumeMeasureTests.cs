using Aventuria.Measures;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace UnitTests.Measures;

[TestFixture]
public class VolumeMeasureTests
{
    private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);


    }

    private VolumeMeasure CreateVolumeMeasure(double value)
    {
        return new VolumeMeasure(value);
    }

    //[Test]
    //public void Equals_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var volumeMeasure = this.CreateVolumeMeasure()0;
    //    object? obj = null;

    //    // Act
    //    var result = volumeMeasure.Equals(
    //        obj);

    //    // Assert
    //    Assert.Fail();
    //    this.mockRepository.VerifyAll();
    //}



    //[Test]
    //public void ToString_StateUnderTest_ExpectedBehavior()
    //{
    //    // Arrange
    //    var volumeMeasure = this.CreateVolumeMeasure(0);

    //    // Act
    //    var result = volumeMeasure.ToString();

    //    // Assert
    //    Assert.Fail();
    //    this.mockRepository.VerifyAll();
    //}

    //[Test]
    //public void ToString_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var volumeMeasure = this.CreateVolumeMeasure(0);
    //    string? format = null;
    //    IFormatProvider? formatProvider = null;

    //    // Act
    //    var result = volumeMeasure.ToString(
    //        format,
    //        formatProvider);

    //    // Assert
    //    Assert.Fail();
    //    this.mockRepository.VerifyAll();
    //}

    //[Test]
    //public void Equals_StateUnderTest_ExpectedBehavior1()
    //{
    //    // Arrange
    //    var volumeMeasure = this.CreateVolumeMeasure(0);
    //    VolumeMeasure other = default(global::Aventuria.Measures.VolumeMeasure);

    //    // Act
    //    var result = volumeMeasure.Equals(
    //        other);

    //    // Assert
    //    Assert.Fail();
    //    this.mockRepository.VerifyAll();
    //}

    #region Constructor Tests

    [Test]
    public void Constructor_ShouldInitializeValueCorrectly()
    {
        // Arrange
        double volume = 10.5;

        // Act
        var volumeMeasure = new VolumeMeasure(volume);

        // Assert
        Assert.That((double)volumeMeasure, Is.EqualTo(volume));
    }

    [Test]
    public void Constructor_WithDoubleMaxValue_ShouldInitializeCorrectly()
    {
        // Arrange
        double volume = double.MaxValue;

        // Act
        var volumeMeasure = new VolumeMeasure(volume);

        // Assert
        Assert.That((double)volumeMeasure, Is.EqualTo(double.MaxValue));
    }

    [Test]
    public void Constructor_WithDoubleMinValue_ShouldInitializeCorrectly()
    {
        // Arrange
        double volume = double.MinValue;

        // Act
        var volumeMeasure = new VolumeMeasure(volume);

        // Assert
        Assert.That((double)volumeMeasure, Is.EqualTo(double.MinValue));
    }

    #endregion


    #region Inherited from Object Tests
    [Test]
    [TestCase(7826346476.333)]
    public void GetHashCode_StateUnderTest_ExpectedBehavior(double value)
    {
        // Arrange
        var volumeMeasure = CreateVolumeMeasure(value);

        // Act
        var result = volumeMeasure.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(value.GetHashCode()));
    }
    #endregion

    #region Explicit Conversion Tests

    [Test]
    public void ExplicitConversionToDouble_ShouldReturnCorrectValue()
    {
        // Arrange
        double volume = 15.3;
        var volumeMeasure = new VolumeMeasure(volume);

        // Act
        double result = (double)volumeMeasure;

        // Assert
        Assert.That(result, Is.EqualTo(volume));
    }

    #endregion



    #region Equality Tests

    [TestCase(10.0, 10.0, true)]
    [TestCase(10.0, 20.0, false)]
    public void Equals_ShouldReturnCorrectResult(double value1, double value2, bool expected)
    {
        // Arrange
        var volume1 = new VolumeMeasure(value1);
        var volume2 = new VolumeMeasure(value2);

        // Act
        bool result = volume1.Equals(volume2);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 10.0, true)]
    [TestCase(10.0, 20.0, false)]
    public void EqualityOperator_ShouldReturnCorrectResult(double value1, double value2, bool expected)
    {
        // Arrange
        var volume1 = new VolumeMeasure(value1);
        var volume2 = new VolumeMeasure(value2);

        // Act
        bool result = volume1 == volume2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 10.0, false)]
    [TestCase(10.0, 20.0, true)]
    public void InequalityOperator_ShouldReturnCorrectResult(double value1, double value2, bool expected)
    {
        // Arrange
        var volume1 = new VolumeMeasure(value1);
        var volume2 = new VolumeMeasure(value2);

        // Act
        bool result = volume1 != volume2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    #endregion



    #region Arithmetic Operators Tests

    [TestCase(10.0, 5.0, 15.0)]
    [TestCase(-3.0, 8.0, 5.0)]
    public void AdditionOperator_ShouldReturnCorrectResult(double value1, double value2, double expected)
    {
        // Arrange
        var volume1 = new VolumeMeasure(value1);
        var volume2 = new VolumeMeasure(value2);

        // Act
        var result = volume1 + volume2;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 5.0, 5.0)]
    [TestCase(-3.0, 8.0, -11.0)]
    public void SubtractionOperator_ShouldReturnCorrectResult(double value1, double value2, double expected)
    {
        // Arrange
        var volume1 = new VolumeMeasure(value1);
        var volume2 = new VolumeMeasure(value2);

        // Act
        var result = volume1 - volume2;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2, 5.0)]
    [TestCase(-4.0, 2, -2.0)]
    public void DivisionByIntOperator_ShouldReturnCorrectResult(double value, int divisor, double expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);

        // Act
        var result = volume / divisor;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2.0, 5.0)]
    [TestCase(-4.0, 2.0, -2.0)]
    public void DivisionByDoubleOperator_ShouldReturnCorrectResult(double value, double divisor, double expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);

        // Act
        var result = volume / divisor;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2.0, 5.0)]
    [TestCase(-4.0, 2.0, -2.0)]
    public void DivisionBySquareOperator_ShouldReturnCorrectResult(double value, double divisor, double expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);
        var squareDivisor = new SquareMeasure(divisor);

        // Act
        var result = volume / squareDivisor;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2.0, 5.0)]
    [TestCase(-4.0, 2.0, -2.0)]
    public void DivisionByLengthOperator_ShouldReturnCorrectResult(double value, double divisor, double expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);
        var lengthDivisor = new LengthMeasure(divisor);

        // Act
        var result = volume / lengthDivisor;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2, 20.0)]
    [TestCase(-4.0, 3, -12.0)]
    public void MultiplicationByIntOperator_ShouldReturnCorrectResult(double value, int multiplier, double expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);

        // Act
        var result = volume * multiplier;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2.0, 20.0)]
    [TestCase(-4.0, 2.0, -8.0)]
    public void MultiplicationByDoubleOperator_ShouldReturnCorrectResult(double value, double multiplier, double expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);

        // Act
        var result = volume * multiplier;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(10.0)]
    public void IncrementOperator_ShouldReturnCorrectResult(double value)
    {
        // Arrange
        var volume = new VolumeMeasure(value);

        // Act
        var resultBefore = volume++;

        // Assert
        Assert.That((double)resultBefore, Is.EqualTo(value));
        Assert.That((double)volume, Is.EqualTo(value+1.0));
    }

    [Test]
    [TestCase(10.0)]
    public void DecrementOperator_ShouldReturnCorrectResult(double value)
    {
        // Arrange
        var volume = new VolumeMeasure(10.0);

        // Act
        var resultBefore = volume--;

        // Assert
        Assert.That((double)resultBefore, Is.EqualTo(value));
        Assert.That((double)volume, Is.EqualTo(value - 1.0));
    }

    #endregion



    #region Identity and Min/Max Value Tests

    [Test]
    public void AdditiveIdentity_ShouldReturnZero()
    {
        // Act
        var result = VolumeMeasure.AdditiveIdentity;

        // Assert
        Assert.That((double)result, Is.EqualTo(0.0));
    }

    [Test]
    public void MultiplicativeIdentity_ShouldReturnOne()
    {
        // Act
        var result = VolumeMeasure.MultiplicativeIdentity;

        // Assert
        Assert.That((double)result, Is.EqualTo(1.0));
    }

    [Test]
    public void MinValue_ShouldReturnDoubleMinValue()
    {
        // Act
        var result = VolumeMeasure.MinValue;

        // Assert
        Assert.That((double)result, Is.EqualTo(double.MinValue));
    }

    [Test]
    public void MaxValue_ShouldReturnDoubleMaxValue()
    {
        // Act
        var result = VolumeMeasure.MaxValue;

        // Assert
        Assert.That((double)result, Is.EqualTo(double.MaxValue));
    }

    #endregion



    #region ToString Tests

    [TestCase(10.5, "10.5")]
    [TestCase(-3.14, "-3.14")]
    [SetCulture("en-US")]
    public void ToString_ShouldReturnCorrectStringRepresentation(double value, string expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);

        // Act
        string result = volume.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(10.5, "F2", "en-US", "10.50")]
    [TestCase(-3.14, "F1", "en-US", "-3.1")]
    public void ToStringWithFormat_ShouldReturnCorrectStringRepresentation(double value, string format, string cultureName, string expected)
    {
        // Arrange
        var volume = new VolumeMeasure(value);
        var culture = new CultureInfo(cultureName);

        // Act
        string result = volume.ToString(format, culture);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    #endregion



    #region Edge Case Tests

   
    [Test]
    public void DivisionByZero_MeasureLength_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var volume = new VolumeMeasure(10.0);
        var zeroVolume = new VolumeMeasure(0.0);

        // Act & Assert
        Assert.That(() => _ = volume / zeroVolume, Throws.TypeOf<DivideByZeroException>());
    }

    [Test]
    public void DivisionByZero_double_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var volume = new VolumeMeasure(10.0);
        var zeroVolume = 0.0;

        // Act & Assert
        Assert.That(() => _ = volume / zeroVolume, Throws.TypeOf<DivideByZeroException>());
    }

    [Test]
    public void DivisionByZero_int_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var volume = new VolumeMeasure(10.0);
        int zeroVolume = 0;

        // Act & Assert
        Assert.That(() => _ = volume / zeroVolume, Throws.TypeOf<DivideByZeroException>());
    }
    #endregion
}

