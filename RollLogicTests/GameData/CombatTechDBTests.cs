using FateExplorer.GameData;
using NUnit.Framework;


namespace UnitTests.GameData
{


    [TestFixture]
    public class CombatTechDBTests : GameDataTestsBase<CombatTechDB, CombatTechDbEntry>
    {
        public override string FilenameId { get => "combattechs"; }

        [Test]
        [TestCase("de", "CT_1", "CT_21")]
        [TestCase("en", "CT_1", "CT_21")]
        public void LoadFromFile_ParseSuccessful(string Language, string Tech1, string TechLast)
        {
            // Arrange
            // Act
            CombatTechDB Result = CreateDBfromFile(Language);
            // Assert
            Assert.That(21, Is.EqualTo(Result.Count));
            Assert.That(Tech1, Is.EqualTo(Result[0].Id));
            Assert.That(TechLast, Is.EqualTo(Result[^1].Id));
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
