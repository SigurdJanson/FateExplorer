using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UnitTests.GameData
{
    [TestFixture]
    public class BotchDBTests
    {

        [SetUp]
        public void SetUp()
        {
        }

        private static BotchDB CreateBotchDB()
        {
            return new BotchDB();
        }



        [Test]
        public void LoadFromFile_ParseSuccessful(
            [Values("Attack", "Parry", "Dodge")] string Roll,
            [Values("Melee", "Unarmed", "Ranged", "Shield")] string Type,
            [Range(2, 12)] int DiceEyes,
            [Values("de", "en")] string Language)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"botches_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            BotchDB Result = JsonSerializer.Deserialize<BotchDB>(jsonString);

            if (Roll == "Parry" && Type == "Ranged")
                Assert.Throws<KeyNotFoundException>(() => Result.GetBotch(Roll, Type, DiceEyes));
            else
                Assert.NotNull(Result.GetBotch(Roll, Type, DiceEyes));
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            //-BotchDB DB = CreateBotchDB();

            // Act
            //-int Count = DB.Count;

            // Assert
            //-Assert.AreEqual(0, Count);
        }
    }
}
