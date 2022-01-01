using FateExplorer.GameData;
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
        [TestCase("de", "SPELL_89", "", 335)]
        [TestCase("en", "SPELL_89", "SPELL_331", 324)]
        public void LoadFromFile_ParseSuccessful(string Language, string Skill1, string SkillLast, int Count)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"arcaneskills_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            ArcaneSkillsDB Result = JsonSerializer.Deserialize<ArcaneSkillsDB>(jsonString);

            // Assert
            Assert.AreEqual(Count, Result.Count);
            Assert.AreEqual(Skill1, Result[0].Id);
            Assert.AreEqual(SkillLast, Result[^1].Id);
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
