using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class KarmaSkillsDBTests
    {
        private static KarmaSkillsDB CreateKarmaSkillsDB()
        {
            return new KarmaSkillsDB();
        }

        [Test]
        [TestCase("de", "LITURGY_41", "", 328)]
        [TestCase("en", "LITURGY_41", "LITURGY_192", 316)]
        public void LoadFromFile_ParseSuccessful(string Language, string Skill1, string SkillLast, int Count)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"karmaskills_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            KarmaSkillsDB Result = JsonSerializer.Deserialize<KarmaSkillsDB>(jsonString);

            // Assert
            Assert.AreEqual(Count, Result.Count);
            Assert.AreEqual(Skill1, Result[0].Id);
            Assert.AreEqual(SkillLast, Result[^1].Id);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            KarmaSkillsDB DB = CreateKarmaSkillsDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
