using FateExplorer.GameData;
using NUnit.Framework;


namespace UnitTests.GameData
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
            Assert.That(8, Is.EqualTo(Result.Count));
            Assert.That(Ability1, Is.EqualTo(Result[0].ShortName));
            Assert.That(AbilityLast, Is.EqualTo(Result[^1].ShortName));
        }

        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
