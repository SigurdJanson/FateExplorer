using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UnitTests.GameData
{
    [TestFixture]
    public class CalendarDBTests
    {

        [SetUp]
        public void SetUp()
        {
        }

        private static CalendarDB CreateCalendarDB()
        {
            return new CalendarDB();
        }



        [Test]
        public void LoadFromFile_ParseSuccessful(
            [Values("de", "en")] string Language)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"calendar_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            CalendarDB Result = JsonSerializer.Deserialize<CalendarDB>(jsonString);

            //if (Roll == "Parry" && Type == "Ranged")
            //    Assert.Throws<KeyNotFoundException>(() => Result.GetBotch(Roll, Type, DiceEyes));
            //else
            //    Assert.NotNull(Result.GetBotch(Roll, Type, DiceEyes));
            Assert.AreEqual(7, Result.Generic.WeekDays.Count);
            Assert.AreEqual(13, Result.Generic.Month.Count);
        }

        [Test, Ignore("not available")]
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
