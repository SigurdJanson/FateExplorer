using Aventuria;
using Moq;
using NUnit.Framework;
using System.Xml.Linq;


namespace UnitTests.Aventuria;

class TestEnum : Enumeration
{
    public TestEnum(string name, int value) : base(name, value) { }
}



[TestFixture()]
public class EnumerationTests
{
    private static TestEnum Rohal => new("Rohal der Weise", 1);
    private static TestEnum Borbarad => new("Borbarad", 2);

    [Test()]
    public void ToStringTest()
    {
        // Arrange
        TestEnum DemiGod1 = Rohal;

        // Act
        var result = Rohal.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("Rohal der Weise"));
    }


    [Test()]
    public void GetAllTest()
    {
        // Arrange
        TestEnum DemiGod1 = Rohal;
        TestEnum DemiGod2 = Borbarad;
        TestEnum[] Expected = [DemiGod1, DemiGod2];

        // Act
        var result = TestEnum.GetAll<TestEnum>();

        // Assert
        int i = 0;
        foreach(var r in result)
        {
            Assert.That(r, Is.EqualTo(Expected[i]));
            i++;
        }
    }


    [Test()]
    [TestCase("Phileasson", 0)]
    [TestCase("Om Follker", 3)]
    [TestCase("Raluff", 0)]
    public void EqualsOp_ToInt_Same_True(string name, int code)
    {
        // Arrange
        TestEnum testEnum = new(name, code);

        // Act
        var result = testEnum == code;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test()]
    [TestCase("Phileasson", 0)]
    [TestCase("Om Follker", 3)]
    [TestCase("Raluff", 0)]
    public void UnequalsOp_ToInt_Same_False(string name, int code)
    {
        // Arrange
        TestEnum testEnum = new(name, code);

        // Act
        var result = testEnum != code;

        // Assert
        Assert.That(result, Is.False);
    }





    #region IComparable


    [Test()]
    [TestCase(0, 0, ExpectedResult = 0)]
    [TestCase(0, 1, ExpectedResult = -1)]
    [TestCase(1, 0, ExpectedResult = 1)]
    [TestCase(1, 1, ExpectedResult = 0)]
    public int CompareToTest(int RohalNr, int BorbaradNr)
    {
        // Arrange
        TestEnum Rohal = new("Rohal", RohalNr);
        TestEnum Borbarad = new("Borbarad", BorbaradNr);

        // Act
        var result = Rohal.CompareTo(Borbarad);

        // Assert
        if (RohalNr == BorbaradNr)
            Assert.That(result, Is.EqualTo(Borbarad.CompareTo(Rohal)));
        else
            Assert.That(result, Is.Not.EqualTo(Borbarad.CompareTo(Rohal)));
        return System.Math.Sign(result);
    }



    [Test()]
    [TestCase("Phileasson", 0)]
    [TestCase("Om Follker", 3)]
    [TestCase("Raluff", 0)]
    public void GetHashCodeTest(string name, int code)
    {
        // Arrange
        TestEnum testEnum = new(name, code);

        // Act
        var result = testEnum.GetHashCode();

        //Assert
        Assert.That(result, Is.EqualTo(code.GetHashCode()));
    }



    [Test]
    public void EqualOp_SameEnumeration_True([Values(true, false)] bool CompareToNull)
    {
#nullable enable
        // Arrange
        TestEnum? DemiGod1 = CompareToNull ? null : Rohal;
        TestEnum? DemiGod2 = CompareToNull ? null : Rohal;

        // Act
        var result = DemiGod1 == DemiGod2;
#nullable restore

        // Assert
        Assert.That(result, Is.True);
    }



    [Test]
    public void EqualOp_OtherEnumeration_False([Values(true, false)] bool CompareToNull)
    {
#nullable enable
        // Arrange
        TestEnum DemiGod1 = Rohal;
        TestEnum? DemiGod2 = CompareToNull ? null : Borbarad;

        // Act
        var result = DemiGod1 == DemiGod2;
#nullable restore
        // Assert
        Assert.That(result, Is.False);
    }



    public void UnequalOp_SameEnumeration_False([Values(true, false)] bool CompareToNull)
    {
#nullable enable
        // Arrange
        TestEnum? DemiGod1 = CompareToNull ? null : Rohal;
        TestEnum? DemiGod2 = CompareToNull ? null : Rohal;

        // Act
        var result = DemiGod1 != DemiGod2;
#nullable restore

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void UnequalOp_OtherEnumeration_True([Values(true, false)] bool CompareToNull)
    {
        // Arrange
        TestEnum DemiGod1 = Rohal;
        TestEnum DemiGod2 = CompareToNull ? null : Borbarad;

        // Act
        var result = DemiGod1 != DemiGod2;

        // Assert
        Assert.That(result, Is.True);
    }


    [Test]
    public void LesserOp_OtherEnumeration_True()
    {
        // Arrange
        // Act
        var result = Rohal < Borbarad;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void GreaterOp_OtherEnumeration_True()
    {
        // Arrange
        // Act
        var result = Rohal > Borbarad;

        // Assert
        Assert.That(result, Is.False);
    }


    #endregion

}