using FateExplorer.Shared;
using NUnit.Framework;
using System;
using static FateExplorer.Shared.Modifier;

namespace UnitTests.Shared;

[TestFixture]
class ModifierTests
{

    [Test]
    [TestCase( 2, Modifier.Op.Add, ExpectedResult = "+2")]
    [TestCase(-3, Modifier.Op.Add, ExpectedResult = "-3")]
    [TestCase(5, Modifier.Op.Force, ExpectedResult = "= 5")]
    public string ToString(int v, Modifier.Op op)
    {
        // Arrange
        Modifier modifier = new(v, op);
        
        // Act
        string result = modifier.ToString();

        // Assert
        return result;
    }



    [Test]
    [TestCase(2, Modifier.Op.Add, false, ExpectedResult = "+2")]
    [TestCase(-3, Modifier.Op.Add, false, ExpectedResult = "-3")]
    [TestCase(+2, Modifier.Op.Halve, false, ExpectedResult = "/ 2")]
    [TestCase(5, Modifier.Op.Force, false, ExpectedResult = "= 5")]
    [TestCase(-3, Modifier.Op.Add, true, ExpectedResult = "um 3 erschwert")]
    [TestCase(1, Modifier.Op.Force, true, ExpectedResult = "nur mit einer 1 nach Modifikation")]
    public string ToString_Formatter(int v, Modifier.Op op, bool LongFormat)
    {
        // Arrange
        string Format = LongFormat ? "L" : "G";
        Modifier m = new(v, op);

        // Act
        string result = m.ToString(Format, new ModifierFormatter());

        // Assert
        return result;
    }



    [Test]
    [TestCase(+9, +2, Modifier.Op.Add, ExpectedResult = 11)]
    [TestCase(10, -2, Modifier.Op.Add, ExpectedResult = 8)]
    [TestCase(-3, +2, Modifier.Op.Add, ExpectedResult = -1)]
    [TestCase(-3, -2, Modifier.Op.Add, ExpectedResult = -5)]
    [TestCase(8, 2,  Modifier.Op.Halve, ExpectedResult = 4)]
    [TestCase(9, 2,  Modifier.Op.Halve, ExpectedResult = 5)]
    [TestCase(1, 2,  Modifier.Op.Halve, ExpectedResult = 1)]
    [TestCase(0, 2,  Modifier.Op.Halve, ExpectedResult = 0)]
    [TestCase(-2, 2, Modifier.Op.Halve, ExpectedResult = -2)]
    [TestCase(6, 2, Modifier.Op.Force, ExpectedResult = 2)]
    [TestCase(-1, 0, Modifier.Op.Force, ExpectedResult = -1)]
    public int ValueModification(int before, int v, Modifier.Op op)
    {
        // Arrange
        Modifier modifier = new(v, op);

        // Act
        int result = before + modifier;

        // Assert
        return result;
    }



    [Test]
    [TestCase(-3, Modifier.Op.Add, ExpectedResult = true)]
    [TestCase(+2, Modifier.Op.Halve, ExpectedResult = true)]
    [TestCase(0, Modifier.Op.Force, ExpectedResult = true)]
    public bool Comparison_SameType_Equals(int v, Modifier.Op op)
    {
        // Arrange
        Modifier m1 = new(v, op);
        Modifier m2 = new(v, op);

        // Act
        bool result = m1 == m2;
        bool inverted = m1 != m2;

        // Assert
        Assert.That(result, Is.EqualTo(!inverted));
        return result;
    }



    [Test]
    [TestCase(-3, Modifier.Op.Add, -1, ExpectedResult = false)]
    [TestCase(0, Modifier.Op.Force, +1, ExpectedResult = false)]
    public bool Comparison_SameType_OtherValue(int v, Modifier.Op op, int Summand)
    {
        // Arrange
        Modifier m1 = new(v, op);
        Modifier m2 = new(v + Summand, op);

        // Act
        bool result = m1 == m2;
        bool inverted = m1 != m2;

        // Assert
        Assert.That(result, Is.EqualTo(!inverted));
        return result;
    }



    [Test]
    [TestCase(3, Modifier.Op.Add, Modifier.Op.Force, ExpectedResult = false)]
    [TestCase(2, Modifier.Op.Halve, Modifier.Op.Add, ExpectedResult = false)]
    [TestCase(2, Modifier.Op.Halve, Modifier.Op.Force, ExpectedResult = false)]
    [TestCase(0, Modifier.Op.Force, Modifier.Op.Add, ExpectedResult = false)]
    public bool Comparison_SameType_OtherOp(int v, Modifier.Op op1, Modifier.Op op2)
    {
        // Arrange
        Modifier m1, m2;
        m1 = new(v, op1);
        m2 = new(v, op2);

        // Act
        bool result = m1 == m2;
        bool inverted = m1 != m2;

        // Assert
        Assert.That(result, Is.EqualTo(!inverted));
        return result;
    }


    [Test, Combinatorial]
    public void Comparison_IntValue([Values(-2, 0, 2)] int v1, [Values(-2, 0, 2)] int v2)
    {
        // Arrange
        Modifier m = new(v1, Modifier.Op.Add);

        // Act
        bool result = v2 == m;
        bool inverted = v2 != m;

        // Assert
        Assert.That(result, Is.EqualTo(!inverted));
        Assert.That(result, Is.EqualTo(v1 == v2));
    }


    [Test, Combinatorial]
    public void Comparison_IntValue([Values] Modifier.Op op1, [Values] Modifier.Op op2)
    {
        // Arrange
        Modifier m = new(2, op1);

        // Act
        bool result = op2 == m;
        bool inverted = op2 != m;

        // Assert
        Assert.That(result, Is.EqualTo(!inverted));
        Assert.That(result, Is.EqualTo(op1 == op2));
    }



    [Test]
    [TestCase(-3, Modifier.Op.Add, ExpectedResult = -536870915)]
    [TestCase(+2, Modifier.Op.Halve, ExpectedResult = 1073741826)]
    [TestCase(1, Modifier.Op.Force, ExpectedResult = 1610612737)]
    public int HashCode([Values()] int v, [Values] Modifier.Op op)
    {
        // Arrange
        Modifier m = new(v, op);

        // Act
        int result = m.GetHashCode();

        // Assert
        return result;
    }

}
