using FateExplorer.WPA.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class AbilitiesDBTests
    {

        [SetUp]
        public void SetUp()
        {
        }

        private static AbilitiesDB CreateAbilitiesDB()
        {
            return new AbilitiesDB();
        }

        [Test]
        [TestCase("de", "MU", "KK")]
        [TestCase("en", "COU", "STR")]
        public void LoadFromFile_ParseSuccessful(string Language, string Ability1, string AbilityLast)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"attributes_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            AbilitiesDB Result = JsonSerializer.Deserialize<AbilitiesDB>(jsonString);

            // Assert
            Assert.AreEqual(8, Result.Count);
            Assert.AreEqual(Ability1, Result[0].ShortName);
            Assert.AreEqual(AbilityLast, Result[Result.Count-1].ShortName);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            AbilitiesDB DB = CreateAbilitiesDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
