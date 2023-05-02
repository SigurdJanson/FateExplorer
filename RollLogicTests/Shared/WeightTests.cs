using FateExplorer.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Shared;

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


    [Test]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(2, ExpectedResult = 2.0 / 1000)]
    [TestCase(-3, ExpectedResult = -3.0 / 1000)]
    [TestCase(5.12345, ExpectedResult = 5.12345/1000)]
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

}
