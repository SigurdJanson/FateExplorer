using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System;
using System.Globalization;

namespace UnitTests.Aventuria;

[TestFixture]
class WeightTests
{
    protected const decimal Decimal_Epsilon = 1.0E-28m; // only for numbers < 10

    public static decimal GetEpsilon(decimal x)
    {
        if (x == 0) return Decimal_Epsilon;
        // The smallest difference is 1 / (10 ^ scale), where scale is the number of decimal places.
        // decimal.GetBits()[1] contains the 16 least significant bits, where bits 0-15 represent the scale.
        x *= Math.Sign(x); // ignore the sign
        int scale = (int)Math.Truncate(Math.Log10((double)x)); //(int)(decimal.GetBits(x)[3] >> 16) & 0xFF;
        return (decimal)Math.Pow(10, -28+scale);
    }

    #region Constructor Tests

    [Test]
    public void Constructor_IntParameter_SetsValueCorrectly()
    {
        // Arrange
        int weight = 10;

        // Act
        var weightObj = new Weight(weight);

        // Assert
        Assert.That(weightObj.ToStone(), Is.EqualTo(weight));
    }

    [Test]
    public void Constructor_DecimalParameter_SetsValueCorrectly()
    {
        // Arrange
        decimal weight = 10.5m;

        // Act
        var weightObj = new Weight(weight);

        // Assert
        Assert.That(weightObj.ToStone(), Is.EqualTo(weight));
    }

    #endregion



    #region Object inherited Methods

    [Test]
    [TestCase(10, ExpectedResult = true)]
    [TestCase(10.00001, ExpectedResult = false)]
    public bool EqualsMethod_Weight_ReturnsCorrectValue(decimal value)
    {
        // Arrange
        var weight = new Weight(10m);
        object obj = new Weight(value);

        // Act
        var result = weight.Equals(obj);

        // Assert
        return result;
    }


    [Test]
    public void EqualsMethod_DifferentTypes_ReturnsFalse()
    {
        // Arrange
        var weight = new Weight(10m);
        object obj = new();

        // Act
        var result = weight.Equals(obj);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void EqualsMethod_Null_ReturnsFalse()
    {
        // Arrange
        var weight = new Weight(10m);
        object obj = null;

        // Act
        var result = weight.Equals(obj);

        // Assert
        Assert.That(result, Is.False);
    }


    [Test]
    public void GetHashCode_SameHashAsDecimal([Random(-9999, 9999, 1)] decimal inStone)
    {
        // Arrange
        var weight = new Weight(inStone);

        // Act
        var result = weight.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(inStone.GetHashCode()));
    }

    #endregion



    [Test]
    [TestCase(2, ExpectedResult = "2")]
    [TestCase(-3, ExpectedResult = "-3")]
    [TestCase(5.12345, ExpectedResult = "5,12345")]
    public string ToString_(decimal v)
    {
        // Arrange
        Weight weight = new(v);

        // Act
        string result = weight.ToString();

        // Assert
        return result;
    }

    [Test]
    [TestCase(2, -2, ExpectedResult = 0.0)]
    [TestCase(2, -2.1, ExpectedResult = -0.1)]
    [TestCase(2, -1.9, ExpectedResult = 0.1)]
    [TestCase(-3.191, 0, ExpectedResult = -3.191)]
    public decimal Plus_WeightWeight(decimal a, decimal b)
    {
        // Arrange
        Weight A = new(a);
        Weight B = new(b);

        // Act
        var result = A + B;

        // Assert
        Assert.That(result, Is.TypeOf(typeof(Weight)));
        return (decimal)result;
    }

    [Test]
    [TestCase(2, 2, ExpectedResult = 0.0)]
    [TestCase(2, -2, ExpectedResult = 4.0)]
    [TestCase(2, -2.1, ExpectedResult = 4.1)]
    [TestCase(2, -1.9, ExpectedResult = 3.9)]
    [TestCase(-3.191, 0, ExpectedResult = -3.191)]
    public decimal Minus_WeightWeight(decimal a, decimal b)
    {
        // Arrange
        Weight A = new(a);
        Weight B = new(b);

        // Act
        var result = A - B;

        // Assert
        Assert.That(result, Is.TypeOf(typeof(Weight)));
        return (decimal)result;
    }

    [Test]
    [TestCase(2, 2, ExpectedResult = true)]
    [TestCase(-0, 0, ExpectedResult = true)]
    [TestCase(2, -2, ExpectedResult = false)]
    [TestCase(2, -2.1, ExpectedResult = false)]
    [TestCase(2, -1.9, ExpectedResult = false)]
    [TestCase(-3.191, 0, ExpectedResult = false)]
    public bool EqOperator(decimal a, decimal b)
    {
        // Arrange
        Weight A = new(a);
        Weight B = new(b);

        // Act
        var result = A == B;

        // Assert
        return result;
    }

    [Test]
    [TestCase(2, 2, ExpectedResult = false)]
    [TestCase(-0, 0, ExpectedResult = false)]
    [TestCase(2, -2, ExpectedResult = true)]
    [TestCase(2, -2.1, ExpectedResult = true)]
    [TestCase(2, -1.9, ExpectedResult = true)]
    [TestCase(-3.191, 0, ExpectedResult = true)]
    public bool NEqOperator(decimal a, decimal b)
    {
        // Arrange
        Weight A = new(a);
        Weight B = new(b);

        // Act
        var result = A != B;

        // Assert
        return result;
    }


    /*
     * CONVERSION
     */
    #region CONVERSION
    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 2.0 / 1000)]
    [TestCase(-3, ExpectedResult = -3.0 / 1000)]
    [TestCase(5.12345, ExpectedResult = 5.12345 / 1000)]
    [TestCase(1000, ExpectedResult = 1)]
    public decimal ToCuboids(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToCuboids();

        // Assert
        return result;
    }

    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 2.0 / 100)]
    [TestCase(-3, ExpectedResult = -3.0 / 100)]
    [TestCase(5.12345, ExpectedResult = 5.12345 / 100)]
    [TestCase(1000, ExpectedResult = 10.0)]
    public decimal ToSack(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToSack();

        // Assert
        return result;
    }

    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 2.0)]
    [TestCase(-3, ExpectedResult = -3.0)]
    [TestCase(5.12345, ExpectedResult = 5.12345)]
    public decimal ToStone(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToStone();

        // Assert
        return result;
    }

    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 80.0)]
    [TestCase(-3, ExpectedResult = -3.0 * 40)]
    [TestCase(5.12345, ExpectedResult = 5.12345 * 40)]
    [TestCase(1, ExpectedResult = 40)]
    public decimal ToOunce(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToOunce();

        // Assert
        return result;
    }

    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 80.0 * 25)]
    [TestCase(-3, ExpectedResult = -3.0 * 40 * 25)]
    [TestCase(5.12345, ExpectedResult = 5.12345 * 40 * 25)]
    [TestCase(1, ExpectedResult = 1000)]
    public decimal ToScruple(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToScruple();

        // Assert
        return result;
    }

    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 80.0 * 25 * 5)]
    [TestCase(-3, ExpectedResult = -3.0 * 40 * 25 * 5)]
    [TestCase(5.12345, ExpectedResult = 5.12345 * 40 * 25 * 5)]
    [TestCase(1, ExpectedResult = 5000)]
    public decimal ToCarat(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToCarat();

        // Assert
        return result;
    }

    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 80.0 * 25 * 5 * 5)]
    [TestCase(-3, ExpectedResult = -3.0 * 40 * 25 * 5 * 5)]
    [TestCase(5.12345, ExpectedResult = 5.12345 * 40 * 25 * 5 * 5)]
    [TestCase(1, ExpectedResult = 25000)]
    public decimal ToGran(decimal w)
    {
        // Arrange
        Weight W = new(w);

        // Act
        var result = W.ToGran();

        // Assert
        return result;
    }
    #endregion



    /*
     * ToString
     */
    #region TO_STRING
    [Test]
    //[TestCase(2.7654, "g", ExpectedResult = "2.7654")]
    [TestCase(2.7654, ExpectedResult = "2,7654")]
    [TestCase(234.7654, ExpectedResult = "234,7654")]
    public string ToString(decimal Value)
    {
        // Arrange
        Weight W = new(Value);

        // Act
        var result = W.ToString();

        // Assert
        return result;
    }

    [Test]
    [TestCase(2.7654, "g", ExpectedResult = "2,765")]
    [TestCase(2.7654, "G", ExpectedResult = "2,765 Stein")]
    [TestCase(2.7654, "r", ExpectedResult = "2,765 St")]
    [TestCase(2.7654, "R", ExpectedResult = "0 Q 2 St 30 oz 15 s 2 kt 0,0000 gr")]
    [TestCase(2.7654, "g4", ExpectedResult = "2,7654")]
    [TestCase(2.7654, "G4", ExpectedResult = "2,7654 Stein")]
    [TestCase(2.7654, "r4", ExpectedResult = "2,7654 St")]
    [TestCase(0.0654, "r4", ExpectedResult = "2,6160 oz")] //
    public string ToString_WithFormat(decimal Value, string Format)
    {
        // Arrange
        Weight W = new(Value);

        // Act
        var result = W.ToString(Format, new WeightFormatter(CultureInfo.GetCultureInfo("de-DE")));

        // Assert
        return result;
    }


    [Test]
    [TestCase(2.7654, "g", ExpectedResult = "2,765")]
    [TestCase(2.7654, "G", ExpectedResult = "2,765 stone")]
    [TestCase(2.7654, "r", ExpectedResult = "2,765 st")]
    [TestCase(2.7654, "R", ExpectedResult = "0 C 2 st 30 oz 15 s 2 ct 0,0000 gr")]
    [TestCase(2.7654, "g4", ExpectedResult = "2,7654")]
    [TestCase(2.7654, "G4", ExpectedResult = "2,7654 stone")]
    [TestCase(2.7654, "r4", ExpectedResult = "2,7654 st")]
    [TestCase(0.0654, "r4", ExpectedResult = "2,6160 oz")] //
    public string ToString_WithFormat_CultureEn(decimal Value, string Format)
    {
        // Arrange
        //---CultureInfo.CurrentCulture = new("en");
        Weight W = new(Value);

        // Act
        var result = W.ToString(Format, new WeightFormatter(CultureInfo.GetCultureInfo("en-EN")));

        // Assert
        return result;
    }
    #endregion




    /*
     * GENERIC MATHS INTERFACES
     */
    #region GENERIC_MATH

    [Test]
    [TestCase(2.01, ExpectedResult = 2.01)]
    [TestCase(-2.01, ExpectedResult = 2.01)]
    [TestCase(0.0000, ExpectedResult = 0.0000)]
    public decimal Abs_Weigh_ReturnsCorrectValuet(decimal a)
    {
        // Arrange
        Weight A = new(a);
        // Act
        var result = Weight.Abs(A);
        // Assert
        return (decimal)result;
    }



    [Test]
    [TestCase(2, 2, ExpectedResult = 1)]
    [TestCase(2, -2, ExpectedResult = -1)]
    [TestCase(2, 4, ExpectedResult = 0.5)]
    [TestCase(26, 25, ExpectedResult = 1.04)]
    [TestCase(2, 1, ExpectedResult = 2)] // Edge case - returns identity
    public decimal Division_Weight_Int_NotZero(decimal a, int b)
    {
        // Arrange
        Weight A = new(a);

        // Act
        var result = A / b;

        // Assert
        return (decimal)result;
    }

    [Test]
    [TestCase(2, 2.0, ExpectedResult = 1)]
    [TestCase(2, -2.0, ExpectedResult = -1)]
    [TestCase(2, 4.0, ExpectedResult = 0.5)]
    [TestCase(26, 25.0, ExpectedResult = 1.04)]
    [TestCase(26, 6.5, ExpectedResult = 4.0)]
    [TestCase(2, 1.0, ExpectedResult = 2.0)] // Edge case - returns identity
    public decimal Division_Weight_Double_NotZero(decimal a, decimal b)
    {
        // Arrange
        Weight A = new(a);

        // Act
        var result = A / b;

        // Assert
        return (decimal)result;
    }

    [Test]
    [TestCase(2, -2, ExpectedResult = -4)]
    [TestCase(2, 3, ExpectedResult = 6)]
    [TestCase(26, 25, ExpectedResult = 26 * 25)]
    [TestCase(2, 1, ExpectedResult = 2)] // Edge case - returns identity
    [TestCase(2, 0, ExpectedResult = 0)] // Edge case - returns zero
    public decimal Multiplication_Weight_Int(decimal a, int b)
    {
        // Arrange
        Weight A = new(a);

        // Act
        var result = A * b;

        // Assert
        return (decimal)result;
    }

    [Test]
    [TestCase(2, 3.0, ExpectedResult = 6.0)]
    [TestCase(2, -2.0, ExpectedResult = -4)]
    [TestCase(26, 6.5, ExpectedResult = 26 * 6.5)]
    [TestCase(3, 0.33, ExpectedResult = 0.99)]
    [TestCase(2, 1.0, ExpectedResult = 2.0)] // Edge case - returns identity
    [TestCase(2, 0.0, ExpectedResult = 0.0)] // Edge case - returns zero
    public decimal Multiplication_Weight_Double_NotZero(decimal a, decimal b)
    {
        // Arrange
        Weight A = new(a);

        // Act
        var result = A * b;

        // Assert
        return (decimal)result;
    }

    [Test]
    [TestCase(2)]
    [TestCase(-12)]
    [TestCase(3.3333)]
    [TestCase(1)] // Edge case - returns identity
    [TestCase(0)] // Edge case - returns zero
    public void Addition_AdditiveIdentity(decimal a)
    {
        Assert.That(Weight.AdditiveIdentity + new Weight(a), Is.EqualTo(new Weight(a)));
    }

    [Test]
    [TestCase(2)]
    [TestCase(-12)]
    [TestCase(3.3333)]
    [TestCase(1)] // Edge case - returns identity
    [TestCase(0)] // Edge case - returns zero
    public void Multiplication_MultiplicativeIdentity(decimal a)
    {
        Assert.That(Weight.MultiplicativeIdentity * a, Is.EqualTo(new Weight(a)));
    }


    [Test]
    public void IncrementOperator_ShouldIncrementValueByOne()
    {
        // Arrange
        var w = new Weight(4.1m);
        var expected = new Weight(5.1m);

        // Act
        var result = ++w; // decrement before assignment
        Assume.That((decimal)w, Is.EqualTo(5.1));
        var after = w++; // decrement after assignment

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        Assert.That(after, Is.EqualTo(expected));
    }

    [Test]
    public void DecrementOperator_ShouldIncrementValueByOne()
    {
        // Arrange
        var w = new Weight(4.1m);
        var expected = new Weight(3.1m);

        // Act
        var result = --w; // decrement before assignment
        Assume.That((decimal)w, Is.EqualTo(3.1));
        var after = w--; // decrement after assignment

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        Assert.That(after, Is.EqualTo(expected));
    }
    #endregion



    /*
     * STATIC PROPERTIES
     */
    #region Static Properties Tests - INumberBase

    [Test]
    public void RefValue_ReturnsWeightWithValueOfOne()
    {
        // Arrange & Act
        var result = Weight.RefValue;

        // Assert
        Assert.That(result, Is.EqualTo(new Weight(1)));
    }

    [Test]
    public void One_ReturnsWeightWithValueOfOne()
    {
        // Arrange & Act
        var one = Weight.One;

        // Assert
        Assert.That(one, Is.EqualTo(new Weight(1)));
    }

    [Test]
    public void Zero_ReturnsWeightWithValueOfZero()
    {
        // Arrange & Act
        var zero = Weight.Zero;

        // Assert
        Assert.That(zero, Is.EqualTo(new Weight(0)));
    }

    #endregion

    #region Utility Methods - INumberBase

    [TestCase(1.0, 0, ExpectedResult = false)]
    [TestCase(0.0, +1, ExpectedResult = false)]
    [TestCase(0.0, 0, ExpectedResult = true)]
    [TestCase(0.0, -1, ExpectedResult = false)]
    public bool IsZero_ReturnsCorrectValue(decimal weightValue, int deltaSign)
    {
        // Arrange
        var weight = new Weight(weightValue + deltaSign * GetEpsilon(weightValue));

        // Act
        bool result = Weight.IsZero(weight);

        // Assert
        return result;
    }

    [TestCase(-10, ExpectedResult = true)]
    [TestCase(10, ExpectedResult = false)]
    public bool IsNegative_ReturnsCorrectValue(decimal weightValue)
    {
        // Arrange
        var weight = new Weight(weightValue);

        // Act
        bool result = Weight.IsNegative(weight);

        // Assert
        return result;
    }

    [TestCase(-10, ExpectedResult = false)]
    [TestCase(10, ExpectedResult = true)]
    public bool IsPositive_ReturnsCorrectValue(decimal weightValue)
    {
        // Arrange
        var weight = new Weight(weightValue);

        // Act
        bool result = Weight.IsPositive(weight);

        // Assert
        return result;
    }


    [TestCase(-10, 0, ExpectedResult = true)]
    [TestCase(12, 0, ExpectedResult = true)]
    [TestCase(13, -1, ExpectedResult = false)] // not integer
    [TestCase(13, 0, ExpectedResult = true)]
    [TestCase(0, +1, ExpectedResult = false)] // not integer
    [TestCase(0, -1, ExpectedResult = false)] // not integer
    public bool IsInteger_ReturnsCorrectValue(decimal weightValue, int deltaSign)
    {
        // Arrange
        var weight = new Weight(weightValue + deltaSign * GetEpsilon(weightValue));

        // Act
        bool result = Weight.IsInteger(weight);

        // Assert
        return result;
    }



    [TestCase(-10, 0, ExpectedResult = true)]
    [TestCase(12, 0, ExpectedResult = true)]
    [TestCase(13, -1, ExpectedResult = false)] // not integer
    [TestCase(13, 0, ExpectedResult = false)]
    [TestCase(0, +1, ExpectedResult = false)] // not integer
    [TestCase(0, -1, ExpectedResult = false)] // not integer
    public bool IsEvenInteger_ReturnsCorrectValue(decimal weightValue, int deltaSign)
    {
        // Arrange
        var weight = new Weight(weightValue + deltaSign * GetEpsilon(weightValue));

        // Act
        bool result = Weight.IsEvenInteger(weight);

        // Assert
        return result;
    }

    [TestCase(-10, 0, ExpectedResult = false)]
    [TestCase(12, 0, ExpectedResult = false)]
    [TestCase(13, -1, ExpectedResult = false)]
    [TestCase(13, 0, ExpectedResult = true)]
    [TestCase(0, 1, ExpectedResult = false)]
    public bool IsOddInteger_ReturnsCorrectValue(decimal weightValue, int deltaSign)
    {
        // Arrange
        var weight = new Weight(weightValue + deltaSign * GetEpsilon(weightValue));

        // Act
        bool result = Weight.IsOddInteger(weight);

        // Assert
        return result;
    }

    [TestCase(-101, -1, ExpectedResult = -101)]
    [TestCase(-10, +1, ExpectedResult = -9)]
    [TestCase(17, -1, ExpectedResult = 16)]
    public decimal Truncate_ReturnsCorrectValue(decimal weightValue, int deltaSign)
    {
        // Arrange
        var weight = new Weight(weightValue + deltaSign * GetEpsilon(weightValue));

        // Act
        var result = Weight.Truncate(weight);

        // Assert
        return (decimal)result;
    }

    #endregion
}
