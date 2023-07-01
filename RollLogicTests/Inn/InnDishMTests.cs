using FateExplorer.Inn;
using FateExplorer.Shared;
using Moq;
using MudBlazor.Charts;
using NUnit.Framework;
using System;

namespace UnitTests.Inn;

[TestFixture]
public class InnDishMTests
{
    private MockRepository mockRepository;



    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Strict);
    }

    private InnDishM CreateInnDishM()
    {
        return new InnDishM();
    }





    [Test]
    [TestCase(QualityLevel.Lowest, ExpectedResult = 0.5f)]
    [TestCase(QualityLevel.Low, ExpectedResult = 0.625f)]
    [TestCase(QualityLevel.Normal, ExpectedResult = 0.75f)]
    [TestCase(QualityLevel.Good, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Excellent, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Luxurious, ExpectedResult = 1.0f)]
    public float GetProbability_Returns_CorrectProbability(QualityLevel quality)
    {
        // Arrange
        var dish = new InnDishM
        {
            QL1Probability = 0.5f,
            QL3Probability = 0.75f,
            QL4Probability = 1.0f,
            QL6Probability = 1.0f
        };

        // Act
        var result = dish.GetProbability(quality);

        // Assert
        return result;
    }

    [Test]
    [TestCase(QualityLevel.Lowest, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Low, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Normal, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Good, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Excellent, ExpectedResult = 1.0f)]
    [TestCase(QualityLevel.Luxurious, ExpectedResult = 1.0f)]
    public float GetProbability_DefaultProbability_1(QualityLevel quality)
    {
        // Arrange
        var dish = new InnDishM
        {
            QL1Probability = 0,
            QL6Probability = 0
        };

        // Act
        var result = dish.GetProbability(quality);

        // Assert
        return result;
    }



    public static object[] RegionCases =
{
        new object[] { new Region[] { Region.RivaRegion }, Region.RivaRegion, true },
        new object[] { new Region[] { Region.Gjalsker }, Region.RivaRegion, false },
        new object[] { new Region[] { Region.Orclands }, Region.RivaRegion, false }
    };


    [Test]
    [TestCaseSource(nameof(RegionCases))]
    public void CanBeFound_ReturnsCorrectValue(Region[] regions, Region current, bool inside)
    {
        // Arrange
        var innDishM = new InnDishM() { Region = regions };


        // Act
        var result = innDishM.CanBeFound(Region.RivaRegion);

        // Assert
        Assert.That(result, Is.EqualTo(inside));
        this.mockRepository.VerifyAll();
    }



    [Test]
    public void CanBeFound_EmptyRegions_AlwaysTrue()
    {
        // Arrange
        var innDishM = new InnDishM() { Region = Array.Empty<Region>() };

        Array values = Enum.GetValues(typeof(Region));
        Random random = new();
        Region Where = (Region)values.GetValue(random.Next(values.Length));

        // Act
        var result = innDishM.CanBeFound(Where);

        // Assert
        Assert.True(result);
    }

    [Test]
    public void CanBeFound_RegionsIsNull_AlwaysTrue()
    {
        // Arrange
        var innDishM = new InnDishM() { Region = null };

        Array values = Enum.GetValues(typeof(Region));
        Random random = new();
        Region Where = (Region)values.GetValue(random.Next(values.Length));

        // Act
        var result = innDishM.CanBeFound(Where);

        // Assert
        Assert.True(result);
    }



    [Test]
    [TestCase(1, Food.NonAlcoholic, ExpectedResult = true)]
    [TestCase(49, Food.NonAlcoholic, ExpectedResult = true)]
    [TestCase(50, Food.NonAlcoholic, ExpectedResult = false)]
    [TestCase(50, Food.Alcoholic, ExpectedResult = true)]
    [TestCase(99, Food.Alcoholic, ExpectedResult = true)]
    [TestCase(399, Food.Dessert, ExpectedResult = true)]
    [TestCase(400, Food.Dessert, ExpectedResult = false)]
    [TestCase(400, Food.Salad, ExpectedResult = true)]
    public bool Is_ReturnsCorrectValue(int TypeOfFood, Food FoodToFind)
    {
        // Arrange
        var dish = new InnDishM { Category = TypeOfFood };

        // Act
        var result = dish.Is(FoodToFind);

        // Assert
        return result;
    }
}
