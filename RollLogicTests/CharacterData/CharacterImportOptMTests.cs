using FateExplorer.CharacterImport;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace UnitTests.CharacterData
{
    [TestFixture]
    public class CharacterImportOptMTests
    {
        [Test]
        [TestCase("Character_Anjin Siebenstich_202202", "1.4.2")]
        [TestCase("Character_Junis_202201", "1.4.2")]
        [TestCase("Character_LayarielWipfelglanz_20201025", "1.3.2")]
        [TestCase("Character_RhydderchDiistra_20220102", "1.4.2")]
        [TestCase("Character_Sina_202005", "1.4.2")]
        public void LoadCharacterWithoutErrors(string Filename, string TargetClientVersion)
        {
            // Arrange
            //var characterImportOptM = new CharacterImportOptM();
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2TestData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{Filename}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            CharacterImportOptM Result = JsonSerializer.Deserialize<CharacterImportOptM>(jsonString);

            // Assert
            Assert.AreEqual(TargetClientVersion, Result.ClientVersion);
        }



        [Test]
        [TestCase("Character_Anjin Siebenstich_202202", false, false)]
        [TestCase("Character_Junis_202201", true, false)]
        [TestCase("Character_LayarielWipfelglanz_20201025", true, false)]
        [TestCase("Character_RhydderchDiistra_20220102", false, false)]
        [TestCase("Character_Sina_202005", true, true)]
        public void LoadCharacter_Verify_AdvantagesSpellcasterAndBless(string Filename, bool Spellcaster, bool Blessed)
        {
            // Arrange
            //var characterImportOptM = new CharacterImportOptM();
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2TestData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{Filename}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            CharacterImportOptM Result = JsonSerializer.Deserialize<CharacterImportOptM>(jsonString);

            // Assert
            Assert.AreEqual(Spellcaster, Result.IsSpellcaster());
            Assert.AreEqual(Blessed, Result.IsBlessed());
        }
    }
}
