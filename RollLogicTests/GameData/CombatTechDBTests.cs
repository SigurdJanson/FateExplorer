using FateExplorer.WPA.GameLogic;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

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
        public void LoadFromFile_ParseSuccessful()
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "combattechs_de.json"));
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
