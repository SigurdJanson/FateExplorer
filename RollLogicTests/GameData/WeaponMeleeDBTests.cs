using FateExplorer.GameData;
using NUnit.Framework;

namespace vmCode_UnitTests.GameData
{
    [TestFixture]
    public class WeaponMeleeDBTests : GameDataTestsBase<WeaponMeleeDB, WeaponMeleeDbEntry>
    {
        public override string FilenameId { get => "weaponsmelee"; }



        [Test]
        [TestCase("de", "Basiliskenzunge", "Gildenmagische Magierkugel")]
        [TestCase("en", "Basilisk-tongue", "Guild Magic Ball")]
        public void LoadFromFile_ParseSuccessful(string Language, string Weapon1, string WeaponLast)
        {
            // Arrange
            // Act
            WeaponMeleeDB Result = Result = CreateDBfromFile(Language);

            // Assert
            Assert.AreEqual(187, Result.Count);
            Assert.AreEqual(Weapon1, Result[0].Name);
            Assert.AreEqual(WeaponLast, Result[^1].Name);
        }


        /// inherited: public void Count_ContentNotLoaded_Return0()
    }
}
