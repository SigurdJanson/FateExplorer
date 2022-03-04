using FateExplorer.GameData;
using NUnit.Framework;


namespace vmCode_UnitTests.GameData
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
            Assert.AreEqual(21, Result.Count);
            Assert.AreEqual(Tech1, Result[0].Id);
            Assert.AreEqual(TechLast, Result[^1].Id);
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
