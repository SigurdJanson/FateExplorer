using FateExplorer.WPA.GameData;
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
        public void LoadFromFile_ParseSuccessful()
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, "karmaskills_de.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            KarmaSkillsDB Result = JsonSerializer.Deserialize<KarmaSkillsDB>(jsonString);

            // Assert
            Assert.AreEqual(328, Result.Count);
            Assert.AreEqual("LITURGY_41", Result[0].Id);
            Assert.AreEqual("", Result[Result.Count-1].Id);
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
