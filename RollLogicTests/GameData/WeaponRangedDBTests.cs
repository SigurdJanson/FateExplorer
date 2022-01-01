using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class WeaponRangedDBTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        private static WeaponRangedDB CreateWeaponRangedDB()
        {
            return new WeaponRangedDB();
        }

        [Test]
        [TestCase("de", "Balestrina", "Wurfmesser")]
        [TestCase("en", "Balestrina", "Throwing Knife")]
        public void LoadFromFile_ParseSuccessful(string Language, string Weapon1, string WeaponLast)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\TestDataFiles"));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"weaponsranged_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            WeaponRangedDB Result = JsonSerializer.Deserialize<WeaponRangedDB>(jsonString);

            // Assert
            Assert.AreEqual(52, Result.Count);
            Assert.AreEqual(Weapon1, Result[0].Name);
            Assert.AreEqual(WeaponLast, Result[^1].Name);
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            WeaponRangedDB DB = CreateWeaponRangedDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}
