using FateExplorer.CharacterData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.CharacterData
{
    [TestFixture]
    public class CharacterImportOptMTests
    {
        [Test]
        [TestCase("Character_Anjin Siebenstich_202012", "1.4.2")]
        [TestCase("Character_Junis_20200629", "1.3.2")]
        [TestCase("Character_LayarielWipfelglanz_20201025", "1.3.2")]
        [TestCase("Character_Sina_202005", "1.0.3")]
        public void LoadCharacterWithoutErrors(string Filename, string TargetClientVersion)
        {
            // Arrange
            var characterImportOptM = new CharacterImportOptM();
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{Filename}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            CharacterImportOptM Result = JsonSerializer.Deserialize<CharacterImportOptM>(jsonString);

            // Assert
            Assert.AreEqual(TargetClientVersion, Result.ClientVersion);

        }
    }
}
