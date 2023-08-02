using FateExplorer.Shop;
using NUnit.Framework;

namespace UnitTests.Shop;

[TestFixture()]
public class MoneyToWageTests
{
    [Test()]
    [TestCase(LaborClass.Cheap, 12, ExpectedResult = 12 * 0.5)]
    [TestCase(LaborClass.Cheap, 27, ExpectedResult = 27 * 0.5)]
    [TestCase(LaborClass.Simple, 3, ExpectedResult = 3 * 2.5)]
    [TestCase(LaborClass.Simple, 38, ExpectedResult = 38 * 2.5)]
    [TestCase(LaborClass.Qualified, 7, ExpectedResult = 7 * 7)]
    [TestCase(LaborClass.Qualified, 16, ExpectedResult = 16 * 7)]
    [TestCase(LaborClass.HighlyQualified, 19, ExpectedResult = 19 * 30)]
    [TestCase(LaborClass.HighlyQualified, 124, ExpectedResult = 124 * 30)]
    public decimal WageTest(LaborClass Labor, int Days)
    {
        // Arrange
        // Act
        var result = MoneyToWage.Wage(Labor, Days);
        // Assert
        return result;
    }



    [Test()]
    [TestCase(LaborClass.Cheap,     15, ExpectedResult = 1)]    // wage per month = 15 S
    [TestCase(LaborClass.Cheap,     30, ExpectedResult = 2)]    // wage per month = 15 S
    [TestCase(LaborClass.Simple,     3, ExpectedResult = 0.04)] // wage per month = 75 S
    [TestCase(LaborClass.Simple,    30, ExpectedResult = 0.4)]  // wage per month = 75 S
    [TestCase(LaborClass.Qualified,  7, ExpectedResult = 1.0/30)] // wage per month = 210 S
    [TestCase(LaborClass.Qualified, 30, ExpectedResult = 1.0/7)]  // wage per month = 210 S
    [TestCase(LaborClass.HighlyQualified, 19, ExpectedResult = 19.0/900)] // wage per month = 900 S
    [TestCase(LaborClass.HighlyQualified, 30, ExpectedResult = 1.0/30)] // wage per month = 900 S
    [DefaultFloatingPointTolerance(1E-8)]
    public double ValueInMonthlyWagesTest(LaborClass Labor, decimal Wage)
    {
        // Arrange
        // Act
        var result = MoneyToWage.ValueInMonthlyWages(Labor, Wage);
        // Assert
        return result;
    }
}