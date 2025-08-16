using FateExplorer.GameData;
using NUnit.Framework;

namespace UnitTests.GameData
{
    [TestFixture]
    public class WeaponRangedDBTests : GameDataTestsBase<WeaponRangedDB, WeaponRangedDbEntry>
    {
        public override string FilenameId { get => "weaponsranged"; }




        [Test]
        [TestCase("de", "Balestrina", "Wurfmesser")]
        [TestCase("en", "Balestrina", "Throwing Knife")]
        public void LoadFromFile_ParseSuccessful(string Language, string Weapon1, string WeaponLast)
        {
            // Arrange
            // Act
            WeaponRangedDB Result = CreateDBfromFile(Language);

            // Assert
            Assert.That(52, Is.EqualTo(Result.Count));
            Assert.That(Weapon1, Is.EqualTo(Result[0].Name));
            Assert.That(WeaponLast, Is.EqualTo(Result[^1].Name));
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
