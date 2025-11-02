using Aventuria.Measures;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace UnitTests.Measures;

[TestFixture]
public class SquareMeasureTests
{
    private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);


    }

    private SquareMeasure CreateSquareMeasure(double value)
    {
        return new SquareMeasure(value);
    }


    [Test]
    [TestCase(1.23)]
    public void ObjEquals_Same_ReturnsTrue(double value)
    {
#nullable enable
        // Arrange
        var squareMeasure = CreateSquareMeasure(value);
        object? obj = CreateSquareMeasure(value);

        // Act
        var result = squareMeasure.Equals(obj);

        // Assert
        Assert.That(result, Is.True);
#nullable restore
    }

    [Test]
    [TestCase(1.23, 1.230000001)]
    public void ObjEquals_Different_ReturnsFalse(double value, double other)
    {
#nullable enable
        // Arrange
        var squareMeasure = CreateSquareMeasure(value);
        object? obj = CreateSquareMeasure(other);

        // Act
        var result = squareMeasure.Equals(obj);

        // Assert
        Assert.That(result, Is.False);
#nullable restore
    }

    [Test]
    [TestCase(1.23)]
    public void ObjEquals_Null_ReturnsFalse(double value)
    {
#nullable enable
        // Arrange
        var squareMeasure = CreateSquareMeasure(value);
        object? obj = null;

        // Act
        var result = squareMeasure.Equals(obj);

        // Assert
        Assert.That(result, Is.False);
#nullable restore
    }



    [Test]
    [TestCase(double.MinValue)]
    [TestCase(1.23)]
    public void GetHashCode_StateUnderTest_ExpectedBehavior(double value)
    {
        // Arrange
        var squareMeasure = this.CreateSquareMeasure(value);

        // Act
        var result = squareMeasure.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(value.GetHashCode()));
    }





    #region Constructor Tests

    [Test]
    public void Constructor_ShouldInitializeValueCorrectly()
    {
        // Arrange
        double length = 10.5;

        // Act
        var squareMeasure = new SquareMeasure(length);

        // Assert
        Assert.That((double)squareMeasure, Is.EqualTo(length));
    }


    [Test]
    public void Constructor_WithDoubleMaxValue_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var squareMeasure = new SquareMeasure(double.MaxValue);

        // Assert
        Assert.That((double)squareMeasure, Is.EqualTo(double.MaxValue));
    }

    [Test]
    public void Constructor_WithDoubleMinValue_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var squareMeasure = new SquareMeasure(double.MinValue);

        // Assert
        Assert.That((double)squareMeasure, Is.EqualTo(double.MinValue));
    }



    #endregion



    #region Equality Tests

    [TestCase(10.0, 10.0, true)]
    [TestCase(10.0, 20.0, false)]
    public void Equals_ShouldReturnTrueForSameValues(double left, double right, bool expected)
    {
        // Arrange
        var squareMeasure1 = new SquareMeasure(left);
        var squareMeasure2 = new SquareMeasure(right);

        // Act
        bool result = squareMeasure1.Equals(squareMeasure2);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 10.0, true)]
    [TestCase(10.0, 10.0000000000001, false)]
    [TestCase(10.0, 20.0, false)]
    public void EqualityOperator_ShouldReturnCorrectResult(double left, double right, bool expected)
    {
        // Arrange
        var squareMeasure1 = new SquareMeasure(left);
        var squareMeasure2 = new SquareMeasure(right);

        // Act
        bool result = squareMeasure1 == squareMeasure2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 10.0, false)]
    [TestCase(10.0, 20.0, true)]
    public void InequalityOperator_ShouldReturnCorrectResult(double left, double right, bool expected)
    {
        // Arrange
        var squareMeasure1 = new SquareMeasure(left);
        var squareMeasure2 = new SquareMeasure(right);

        // Act
        bool result = squareMeasure1 != squareMeasure2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    #endregion



    #region Arithmetic Operators Tests

    [TestCase(10.0, 5.0, 15.0)]
    public void AdditionOperator_ShouldReturnCorrectResult(double left, double right, double expected)
    {
        // Arrange
        var squareMeasure1 = new SquareMeasure(left);
        var squareMeasure2 = new SquareMeasure(right);

        // Act
        var result = squareMeasure1 + squareMeasure2;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 7.0, 3.0)]
    public void SubtractionOperator_ShouldReturnCorrectResult(double left, double right, double expected)
    {
        // Arrange
        var squareMeasure1 = new SquareMeasure(left);
        var squareMeasure2 = new SquareMeasure(right);

        // Act
        var result = squareMeasure1 - squareMeasure2;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2, 5.0)]
    public void DivisionByIntOperator_ShouldReturnCorrectResult(double left, int right, double expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(left);

        // Act
        var result = squareMeasure / right;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(11.0, 2.0, 5.5)]
    public void DivisionByDoubleOperator_ShouldReturnCorrectResult(double left, double right, double expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(left);

        // Act
        var result = squareMeasure / right;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(27.9, 9.0, 3.1)]
    public void DivisionByLengthOperator_ShouldReturnCorrectResult(double left, double right, double expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(left);
        var lengthMeasure = new LengthMeasure(right);

        // Act
        var result = squareMeasure / lengthMeasure;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected).Within(1).Ulps);
    }


    [TestCase(10.0, 2, 20.0)]
    public void MultiplicationByIntOperator_ShouldReturnCorrectResult(double left, int right, double expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(left);

        // Act
        var result = squareMeasure * right;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2.0, 20.0)]
    public void MultiplicationByDoubleOperator_ShouldReturnCorrectResult(double left, double right, double expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(left);

        // Act
        var result = squareMeasure * right;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [TestCase(10.0, 2.25, 22.5)]
    public void MultiplicationByLengthOperator_ShouldReturnCorrectResult(double left, double right, double expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(left);
        var lengthMeasure = new LengthMeasure(right);

        // Act
        var result = squareMeasure * lengthMeasure;

        // Assert
        Assert.That((double)result, Is.EqualTo(expected));
    }

    [Test]
    public void IncrementOperator_ShouldReturnCorrectResult()
    {
        // Arrange
        var squareMeasure = new SquareMeasure(10.0);

        // Act
        var result = squareMeasure++;

        // Assert
        Assert.That((double)result, Is.EqualTo(10.0));
        Assert.That((double)squareMeasure, Is.EqualTo(11.0));
    }

    [Test]
    public void DecrementOperator_ShouldReturnCorrectResult()
    {
        // Arrange
        var squareMeasure = new SquareMeasure(10.0);

        // Act
        var result = squareMeasure--;

        // Assert
        Assert.That((double)result, Is.EqualTo(10.0));
        Assert.That((double)squareMeasure, Is.EqualTo(9.0));
    }

    #endregion



    #region Identity and Min/Max Value Tests

    [Test]
    public void AdditiveIdentity_ShouldReturnZero()
    {
        // Arrange & Act
        var result = SquareMeasure.AdditiveIdentity;

        // Assert
        Assert.That((double)result, Is.EqualTo(0.0));
    }

    [Test]
    public void MultiplicativeIdentity_ShouldReturnOne()
    {
        // Arrange & Act
        var result = SquareMeasure.MultiplicativeIdentity;

        // Assert
        Assert.That((double)result, Is.EqualTo(1.0));
    }

    [Test]
    public void MinValue_ShouldReturnDoubleMinValue()
    {
        // Arrange & Act
        var result = SquareMeasure.MinValue;

        // Assert
        Assert.That((double)result, Is.EqualTo(double.MinValue));
    }

    [Test]
    public void MaxValue_ShouldReturnDoubleMaxValue()
    {
        // Arrange & Act
        var result = SquareMeasure.MaxValue;

        // Assert
        Assert.That((double)result, Is.EqualTo(double.MaxValue));
    }

    #endregion



    #region ToString Tests

    [TestCase(10.0, "10")]
    [TestCase(10.5, "10.5")]
    [SetCulture("en-US")]
    public void ToString_ShouldReturnCorrectStringRepresentation(double value, string expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(value);

        // Act
        string result = squareMeasure.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(10.5, "F2", "en-US", "10.50")]
    [TestCase(10.5, "F1", "en-US", "10.5")]
    public void ToStringWithFormat_ShouldReturnCorrectStringRepresentation(double value, string format, string cultureName, string expected)
    {
        // Arrange
        var squareMeasure = new SquareMeasure(value);
        var cultureInfo = new CultureInfo(cultureName);

        // Act
        string result = squareMeasure.ToString(format, cultureInfo);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    #endregion


    #region Edge case

    [Test]
    public void Division_Int_DivByZero_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var squareMeasure = new SquareMeasure(10.0);

        // Act & Assert
        Assert.That(() => _ = squareMeasure / 0.0, Throws.TypeOf<DivideByZeroException>());
    }

    #endregion
}

