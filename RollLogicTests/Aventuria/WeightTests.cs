using Aventuria;
using Aventuria.Measures;
using NUnit.Framework;
using System.Globalization;

namespace UnitTests.Aventuria;

[TestFixture]
class WeightTests
{
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
    public bool Eq(decimal a, decimal b)
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
    public bool NEq(decimal a, decimal b)
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
    #endregion
}
