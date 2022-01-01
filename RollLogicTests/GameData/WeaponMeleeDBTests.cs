using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class WeaponMeleeDBTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        private static WeaponMeleeDB CreateWeaponMeleeDB()
        {
            return new WeaponMeleeDB();
        }

        [Test]
        [TestCase("de", "Basiliskenzunge", "Yeti-Keule")]
        [TestCase("en", "Basilisk-tongue", "Yeti-Keule")]
        public void LoadFromFile_ParseSuccessful(string Language, string Weapon1, string WeaponLast)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"weaponsmelee_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            WeaponMeleeDB Result = JsonSerializer.Deserialize<WeaponMeleeDB>(jsonString);

            // Assert
            Assert.AreEqual(184, Result.Count);
            Assert.AreEqual(Weapon1, Result[0].Name);
            Assert.AreEqual(WeaponLast, Result[^1].Name);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            WeaponMeleeDB DB = CreateWeaponMeleeDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
