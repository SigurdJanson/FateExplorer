using FateExplorer.GameData;
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
        [TestCase("de", "Fliegen", "Stoffbearbeitung")]
        [TestCase("en", "Flying", "Clothworking")]
        public void LoadFromFile_ParseSuccessful(string Language, string Skill1, string SkillLast)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"skills_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            SkillsDB Result = JsonSerializer.Deserialize<SkillsDB>(jsonString);

            // Assert
            Assert.AreEqual(59, Result.Count);
            Assert.AreEqual(Skill1, Result[0].Name);
            Assert.AreEqual(SkillLast, Result[58].Name);
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
