using FateExplorer.GameData;
using NUnit.Framework;


namespace UnitTests.GameData
{
    [TestFixture]
    public class ResilienceDBTests : GameDataTestsBase<ResiliencesDB, ResilienceDbEntry>
    {
        public override string FilenameId { get => "resiliences"; }


        [Test]
        [TestCase("de", "Zähigkeit", "Seelenkraft")]
        [TestCase("en", "Toughness", "Spirit")]
        public void LoadFromFile_ParseSuccessful(string Language, string ResName1, string ResName2)
        {
            const string ResId1 = "TOU";
            const string ResId2 = "SPI";

            // Arrange
            // Act
            ResiliencesDB Result = CreateDBfromFile(Language);
            // Assert
            Assert.That(2, Is.EqualTo(Result.Count));
            Assert.That(ResId1, Is.EqualTo(Result[0].Id));
            Assert.That(ResId2, Is.EqualTo(Result[1].Id));
            Assert.That(ResName1, Is.EqualTo(Result[0].Name));
            Assert.That(ResName2, Is.EqualTo(Result[1].Name));
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}

