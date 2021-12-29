using FateExplorer.WPA.GameLogic;
using Moq;
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
        public void LoadFromFile_ParseSuccessful()
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "attributes_de.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            AbilitiesDB Result = JsonSerializer.Deserialize<AbilitiesDB>(jsonString);

            // Assert
            Assert.AreEqual(8, Result.Count);
            Assert.AreEqual("MU", Result[0].ShortName);
            Assert.AreEqual("KK", Result[7].ShortName);
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
