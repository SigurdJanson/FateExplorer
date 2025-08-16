using FateExplorer.GameData;
using NUnit.Framework;

namespace UnitTests.GameData
{
    [TestFixture]
    public class WeaponMeleeDBTests : GameDataTestsBase<WeaponMeleeDB, WeaponMeleeDbEntry>
    {
        public override string FilenameId { get => "weaponsmelee"; }



        [Test]
        [TestCase("de", "Basiliskenzunge", "Albernisches Langschwert")]
        [TestCase("en", "Basilisk-tongue", "Albernian Long Sword")]
        public void LoadFromFile_ParseSuccessful(string Language, string Weapon1, string WeaponLast)
        {
            // Arrange
            // Act
            WeaponMeleeDB Result = Result = CreateDBfromFile(Language);

            // Assert
            Assert.That(189, Is.EqualTo(Result.Count));
            Assert.That(Weapon1, Is.EqualTo(Result[0].Name));
            Assert.That(WeaponLast, Is.EqualTo(Result[^1].Name));
        }


        /// inherited: public void Count_ContentNotLoaded_Return0()
    }
}
