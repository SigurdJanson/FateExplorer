using FateExplorer.WPA.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class SkillDBTests
    {
        private static SkillsDB CreateSkillDB()
        {
            return new SkillsDB();
        }

        [Test]
        public void LoadFromFile_ParseSuccessful()
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "skills_de.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            SkillsDB Result = JsonSerializer.Deserialize<SkillsDB>(jsonString);

            // Assert
            Assert.AreEqual(59, Result.Count);
            Assert.AreEqual("Fliegen", Result[0].Name);
            Assert.AreEqual("Stoffbearbeitung", Result[58].Name);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            SkillsDB DB = CreateSkillDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
