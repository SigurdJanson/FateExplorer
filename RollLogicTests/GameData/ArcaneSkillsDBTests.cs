using FateExplorer.WPA.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class ArcaneSkillsDBTests
    {
        private static ArcaneSkillsDB CreateArcaneSkillsDB()
        {
            return new ArcaneSkillsDB();
        }

        [Test]
        public void LoadFromFile_ParseSuccessful()
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "arcaneskills_de.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            ArcaneSkillsDB Result = JsonSerializer.Deserialize<ArcaneSkillsDB>(jsonString);

            // Assert
            Assert.AreEqual(335, Result.Count);
            Assert.AreEqual("SPELL_89", Result[0].Id);
            Assert.AreEqual("", Result[Result.Count-1].Id);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            ArcaneSkillsDB DB = CreateArcaneSkillsDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
