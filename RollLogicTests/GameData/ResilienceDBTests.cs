using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class ResilienceDBTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        private static ResiliencesDB CreateResiliencesDB()
        {
            return new ResiliencesDB();
        }

        [Test]
        [TestCase("de", "Zähigkeit", "Seelenkraft")]
        [TestCase("en", "Toughness", "Spirit")]
        public void LoadFromFile_ParseSuccessful(string Language, string ResName1, string ResName2)
        {
            const string ResId1 = "TOU";
            const string ResId2 = "SPI";
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\dev\\wwwroot\\data"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"resiliences_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            ResiliencesDB Result = JsonSerializer.Deserialize<ResiliencesDB>(jsonString);

            // Assert
            Assert.AreEqual(2, Result.Count);
            Assert.AreEqual(ResId1, Result[0].Id);
            Assert.AreEqual(ResId2, Result[1].Id);
            Assert.AreEqual(ResName1, Result[0].Name);
            Assert.AreEqual(ResName2, Result[1].Name);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            ResiliencesDB DB = CreateResiliencesDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}

