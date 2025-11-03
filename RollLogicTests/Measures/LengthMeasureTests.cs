using Aventuria.Measures;
using NUnit.Framework;
using System;

namespace UnitTests.Measures;

[TestFixture]
public class LengthMeasureTests
{

    [SetUp]
    public void SetUp()
    {
        //this.mockRepository = new MockRepository(MockBehavior.Strict);
    }

    private LengthMeasure CreateLengthMeasure(double value)
    {
        return new LengthMeasure(value);
    }



    [Test]
    public void Equals_Equal_True([Random(-9999, 9999, 1)] double inPaces)
    {
#nullable enable
        // Arrange
        var lengthMeasure = CreateLengthMeasure(inPaces);
        object? obj = CreateLengthMeasure(inPaces);

        // Act
        var result = lengthMeasure.Equals(obj);

        // Assert
        Assert.That(result, Is.True);
#nullable restore
    }

    [Test]
    public void Equals_Unequal_False([Random(-9999, 9999, 1)] double inPaces, [Random(0.0001, 9999, 1)] double delta)
    {
#nullable enable
        // Arrange
        var lengthMeasure = CreateLengthMeasure(inPaces);
        object? obj = CreateLengthMeasure(inPaces + delta);

        // Act
        var result = lengthMeasure.Equals(obj);

        // Assert
        Assert.That(result, Is.False);
#nullable restore
    }

    [Test]
    public void Equals_Null_False([Random(-9999, 9999, 1)] double inPaces)
    {
#nullable enable
        // Arrange
        var lengthMeasure = CreateLengthMeasure(inPaces);
        object? obj = null;

        // Act
        var result = lengthMeasure.Equals(obj);

        // Assert
        Assert.That(result, Is.False);
#nullable restore
    }





    [Test]
    public void GetHashCode_StateUnderTest_ExpectedBehavior([Random(-9999, 9999, 1)] double inPaces)
    {
        // Arrange
        var lengthMeasure = CreateLengthMeasure(inPaces);

        // Act
        var result = lengthMeasure.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(inPaces.GetHashCode()));
    }



    [Test]
    [SetCulture("en-US")]
    public void ToString_StateUnderTest_ExpectedBehavior()
    {
#nullable enable
        // Arrange
        var lengthMeasure = CreateLengthMeasure(1.234);
        string? format = null;
        IFormatProvider? formatProvider = null;

        // Act
        var result = lengthMeasure.ToString(format, formatProvider);

        // Assert
        Assert.That(result, Is.EqualTo("1.234"));
#nullable restore
    }



    [Test]
    public void Equals_SameValue_DifferentInstance_True([Random(-9999, 9999, 1)] double inPaces)
    {
        // Arrange
        LengthMeasure lengthMeasure = CreateLengthMeasure(inPaces);
        LengthMeasure other = CreateLengthMeasure(inPaces);

        // Act
        var result = lengthMeasure.Equals(other);

        // Assert
        Assert.That(result, Is.True);
    }




    // Equality Tests
    [Test]
    public void Equals_ShouldReturnTrueForSameValues()
    {
        // Arrange
        var length1 = new LengthMeasure(5.0);
        var length2 = new LengthMeasure(5.0);

        // Act
        bool result = length1.Equals(length2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void EqualityOperator_ShouldReturnTrueForSameValues()
    {
        // Arrange
        var length1 = new LengthMeasure(3.0);
        var length2 = new LengthMeasure(3.0);

        // Act & Assert
        Assert.That(length1 == length2, Is.True);
    }

    [Test]
    public void InequalityOperator_ShouldReturnTrueForDifferentValues()
    {
        // Arrange
        var length1 = new LengthMeasure(4.0);
        var length2 = new LengthMeasure(6.0);

        // Act & Assert
        Assert.That(length1 != length2, Is.True);
    }

    // Arithmetic Operators Tests
    [Test]
    public void AdditionOperator_ShouldReturnCorrectSum()
    {
        // Arrange
        var length1 = new LengthMeasure(2.0);
        var length2 = new LengthMeasure(3.0);
        var expected = new LengthMeasure(5.0);

        // Act
        var result = length1 + length2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void SubtractionOperator_ShouldReturnCorrectDifference()
    {
        // Arrange
        var length1 = new LengthMeasure(5.0);
        var length2 = new LengthMeasure(2.0);
        var expected = new LengthMeasure(3.0);

        // Act
        var result = length1 - length2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void IncrementOperator_ShouldIncrementValueByOne()
    {
        // Arrange
        var length = new LengthMeasure(4.0);
        var expected = new LengthMeasure(5.0);

        // Act
        var result = ++length; // decrement before assignment
        Assume.That((double)length, Is.EqualTo(5.0));
        var after = length++; // decrement after assignment

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        Assert.That(after, Is.EqualTo(expected));
    }

    [Test]
    public void DecrementOperator_ShouldDecrementValueByOne()
    {
        // Arrange
        var length = new LengthMeasure(6.0);
        var expected = new LengthMeasure(5.0);

        // Act
        var result = --length; // decrement before assignment
        Assume.That((double)length, Is.EqualTo(5.0));
        var after = length--; // decrement after assignment

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        Assert.That(after, Is.EqualTo(expected));
    }

    // Division Operators Tests
    [Test]
    public void DivisionByLength_ShouldReturnCorrectRatio()
    {
        // Arrange
        var length1 = new LengthMeasure(10.0);
        var length2 = new LengthMeasure(2.0);
        double expected = 5.0;

        // Act
        double result = length1 / length2;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void DivisionByInt_ShouldReturnCorrectLength()
    {
        // Arrange
        var length = new LengthMeasure(10.0);
        int divisor = 2;
        var expected = new LengthMeasure(5.0);

        // Act
        var result = length / divisor;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void DivisionByDouble_ShouldReturnCorrectLength()
    {
        // Arrange
        var length = new LengthMeasure(10.0);
        double divisor = 2.5;
        var expected = new LengthMeasure(4.0);

        // Act
        var result = length / divisor;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    // Multiplication Operators Tests
    [Test]
    public void MultiplicationByInt_ShouldReturnCorrectLength()
    {
        // Arrange
        var length = new LengthMeasure(5.0);
        int multiplier = 3;
        var expected = new LengthMeasure(15.0);

        // Act
        var result = length * multiplier;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void MultiplicationByDouble_ShouldReturnCorrectLength()
    {
        // Arrange
        var length = new LengthMeasure(5.0);
        double multiplier = 2.5;
        var expected = new LengthMeasure(12.5);

        // Act
        var result = length * multiplier;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }


    [Test]
    [TestCase(5.0, 2.5)]
    public void Multiplication_ByLength_ShouldReturnCorrectSquare(double a, double b)
    {
        // Arrange
        var lengthA = new LengthMeasure(a);
        var lengthB = new LengthMeasure(b);
        var expected = new SquareMeasure(a*b);

        // Act
        var result = lengthA * lengthB;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }


    // Identity and Min/Max Value Tests
    [Test]
    public void AdditiveIdentity_ShouldBeZero()
    {
        // Arrange & Act
        var identity = LengthMeasure.AdditiveIdentity;

        // Assert
        Assert.That(identity, Is.EqualTo(new LengthMeasure(0)));
    }

    [Test]
    public void MultiplicativeIdentity_ShouldBeOne()
    {
        // Arrange & Act
        var identity = LengthMeasure.MultiplicativeIdentity;

        // Assert
        Assert.That(identity, Is.EqualTo(new LengthMeasure(1)));
    }

    [Test]
    public void MinValue_ShouldBeDoubleMinValue()
    {
        // Arrange & Act
        var minValue = LengthMeasure.MinValue;

        // Assert
        Assert.That(minValue, Is.EqualTo(new LengthMeasure(double.MinValue)));
    }

    [Test]
    public void MaxValue_ShouldBeDoubleMaxValue()
    {
        // Arrange & Act
        var maxValue = LengthMeasure.MaxValue;

        // Assert
        Assert.That(maxValue, Is.EqualTo(new LengthMeasure(double.MaxValue)));
    }





    // Edge Cases Tests
    [Test]
    public void Constructor_WithZeroValue_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var lengthMeasure = new LengthMeasure(0);

        // Assert
        Assert.That((double)lengthMeasure, Is.EqualTo(0));
    }

    [Test]
    public void DivisionByZero_ShouldThrowDivideByZeroException()
    {
        // Arrange
        var length = new LengthMeasure(10.0);
        var zeroLength = new LengthMeasure(0.0);

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _ = length / zeroLength);
    }

    [Test]
    public void MultiplicationByZero_ShouldReturnZeroLength()
    {
        // Arrange
        var length = new LengthMeasure(10.0);
        var expected = new LengthMeasure(0);

        // Act
        var result = length * 0;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }




    [Test]
    [TestCase(1.0)]
    public void ToYards(double inPaces) //[Random(-9999, 9999, 1)]
    {
        // Arrange
        const double MPerYard = 0.9144;
        var lengthMeasure = this.CreateLengthMeasure(inPaces);

        // Act
        var result = lengthMeasure.ToYards();

        // Assert
        Assert.That(result, Is.EqualTo(inPaces / MPerYard));
    }



    [Test]
    public void ToPaces([Random(-9999, 9999, 1)] double inPaces)
    {
        // Arrange
        var lengthMeasure = CreateLengthMeasure(inPaces);

        // Act
        var result = lengthMeasure.ToPaces();

        // Assert
        Assert.That(result, Is.EqualTo(inPaces));
    }


    [Test]
    public void ToDrumod([Random(-9999, 9999, 1)] double inPaces)
    {
        // Arrange
        const double MeterPerDrumod = 1.68;
        var lengthMeasure = CreateLengthMeasure(inPaces);

        // Act
        var result = lengthMeasure.ToDrumod();

        // Assert
        Assert.That(result, Is.EqualTo(inPaces / MeterPerDrumod));
    }
}
