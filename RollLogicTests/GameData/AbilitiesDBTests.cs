using FateExplorer.GameData;
using NUnit.Framework;


namespace RollLogicTests.GameData
{
    [TestFixture]
    public class AbilitiesDBTests : GameDataTestsBase<AbilitiesDB, AbilityDbEntry>
    {
        public override string FilenameId { get => "attributes"; }


        [Test]
        [TestCase("de", "MU", "KK")]
        [TestCase("en", "COU", "STR")]
        public void LoadFromFile_ParseSuccessful(string Language, string Ability1, string AbilityLast)
        {
            // Arrange
            // Act
            AbilitiesDB Result = CreateDBfromFile(Language);

            // Assert
            Assert.AreEqual(8, Result.Count);
            Assert.AreEqual(Ability1, Result[0].ShortName);
            Assert.AreEqual(AbilityLast, Result[^1].ShortName);
        }

        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
