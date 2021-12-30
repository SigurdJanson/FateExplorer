using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using FateExplorer.WPA.GameData;


namespace RollLogicTests.GameData
{


    [TestFixture]
    public class CombatTechDBTests
    {
        private static CombatTechDB CreateCombatTechDB()
        {
            return new CombatTechDB();
        }


        [Test]
        [TestCase("de", "CT_1", "CT_21")]
        [TestCase("en", "CT_1", "CT_21")]
        public void LoadFromFile_ParseSuccessful(string Language, string Tech1, string TechLast)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"combattechs_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            CombatTechDB Result = JsonSerializer.Deserialize<CombatTechDB>(jsonString);

            // Assert
            Assert.AreEqual(21, Result.Count);
            Assert.AreEqual("CT_1", Result[0].Id);
            Assert.AreEqual("CT_21", Result[20].Id);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            CombatTechDB DB = CreateCombatTechDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
