using FateExplorer.Shared;
using NUnit.Framework;

namespace UnitTests.Shared
{
    [TestFixture]
    public class StringHelpersTests
    {
        [Test]
        [TestCase("DSA5/FateExplorer/skill/arcane", "DSA5/FateExplorer/skill/", ExpectedResult = 24)]
        [TestCase("DSA5/FateExplorer/skill/", "DSA5/FateExplorer/skill/karma", ExpectedResult = 24)]
        [TestCase("DSA5/FateExplorer/skill/arcane", "DSA5/FateExplorer/skill/karma", ExpectedResult = 24)]
        [TestCase("DSA5/FateExplorer/skill/arcane", "DSA1/FateExplorer/skill/arcane", ExpectedResult = 3)]
        public int Fitness_NormalFunctionCall(string a, string b)
        {
            // Arrange
            // Act
            var result = StringHelpers.Fitness(a, b);

            // Assert
            return result;
        }


        [Test]
        [TestCase("DSA5/FateExplorer/skill/arcane", "DSA5/FateExplorer/skill/", ExpectedResult = 24)]
        [TestCase("DSA5/FateExplorer/skill/", "DSA5/FateExplorer/skill/karma", ExpectedResult = 24)]
        [TestCase("DSA5/FateExplorer/skill/arcane", "DSA5/FateExplorer/skill/karma", ExpectedResult = 24)]
        [TestCase("DSA5/FateExplorer/skill/arcane", "DSA1/FateExplorer/skill/arcane", ExpectedResult = 3)]
        public int Fitness_ExtensionMethodCall(string a, string b)
        {
            // Arrange
            // Act
            var result = a.Fitness(b);

            // Assert
            return result;
        }

        [Test]
        public void Fitness_TargerIsNull_Return0()
        {
            // Arrange
            string a = "Anything you want";
            // Act
            var result = a.Fitness(null);

            // Assert
            Assert.That(0, Is.EqualTo(result));
        }
    }
}
