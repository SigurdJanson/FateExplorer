using FateExplorer.GameData;
using NUnit.Framework;

namespace vmCode_UnitTests.GameData
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
            Assert.AreEqual(52, Result.Count);
            Assert.AreEqual(Weapon1, Result[0].Name);
            Assert.AreEqual(WeaponLast, Result[^1].Name);
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
