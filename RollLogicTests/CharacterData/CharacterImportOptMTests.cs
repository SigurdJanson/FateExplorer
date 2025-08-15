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
        [TestCase("Character_Anjin Siebenstich_202202", "1.5.1")]
        [TestCase("Junis Djelef ibn Yakuban Al Thani_20220514", "1.5.1")]
        [TestCase("Character_LayarielWipfelglanz_20201025", "1.3.2")]
        [TestCase("Character_RhydderchDiistra_20230706", "1.5.1")]
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
            Assert.That(TargetClientVersion, Is.EqualTo(Result.ClientVersion));
        }



        [Test]
        [TestCase("Character_Anjin Siebenstich_202202", false, false)]
        [TestCase("Junis Djelef ibn Yakuban Al Thani_20220514", true, false)]
        [TestCase("Character_LayarielWipfelglanz_20201025", true, false)]
        [TestCase("Character_RhydderchDiistra_20230706", false, false)]
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
            Assert.That(Spellcaster, Is.EqualTo(Result.IsSpellcaster()));
            Assert.That(Blessed, Is.EqualTo(Result.IsBlessed()));
        }
    }
}
