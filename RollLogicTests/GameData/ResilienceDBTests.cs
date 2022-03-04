using FateExplorer.GameData;
using NUnit.Framework;


namespace vmCode_UnitTests.GameData
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
            Assert.AreEqual(2, Result.Count);
            Assert.AreEqual(ResId1, Result[0].Id);
            Assert.AreEqual(ResId2, Result[1].Id);
            Assert.AreEqual(ResName1, Result[0].Name);
            Assert.AreEqual(ResName2, Result[1].Name);
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}

