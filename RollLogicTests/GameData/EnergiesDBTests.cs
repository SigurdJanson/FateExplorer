using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RollLogicTests.GameData
{
    [TestFixture]
    public class EnergiesDBTests
    {

        [SetUp]
        public void SetUp()
        {
        }

        private static EnergiesDB CreateEnergiesDB()
        {
            return new EnergiesDB();
        }


        [Test]
        [TestCase("de", new string[] { "Lebensenergie", "Astralenergie", "Karmaenergie" } )]
        [TestCase("en", new string[] { "Life Energy", "Arcane Energy", "Karma Energy" } )]
        public void LoadFromFile_ParseSuccessful(string Language, string[] EnName)
        {
            string[] ResId = new string[] { "LP", "AE", "KP" };

            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"energies_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            EnergiesDB Result = JsonSerializer.Deserialize<EnergiesDB>(jsonString);

            // Assert
            Assert.AreEqual(EnName.Length, Result.Count);
            for (int i = 0; i < EnName.Length; i++)
            {
                Assert.AreEqual(ResId[i], Result[i].Id);
                Assert.AreEqual(EnName[i], Result[i].Name);
            }
        }


        [Test]
        [TestCase( "", new string[] { "de", "en" } )]
        public void CompareLanguages_Equality(string Dummy, string[] Languages)
        {
            // Arrange
            List<EnergiesDB> Result = new();

            foreach (var lang in Languages)
            {
                string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
                string fileName = Path.GetFullPath(Path.Combine(BasePath, $"energies_{lang}.json"));
                string jsonString = File.ReadAllText(fileName);
                Result.Add(JsonSerializer.Deserialize<EnergiesDB>(jsonString));
            }

            // Act
            // Assert
            Assert.IsTrue(Result.Count == Languages.Length);
            for (int i = 0; i < Languages.Length; i++)
            {
                for(int j = 0; j < Result[0].Data.Count; j++)
                    Assert.IsTrue(TestHelpers.IsDeeplyEqual(
                        Result[0].Data[j], 
                        Result[1].Data[j], 
                        new string[] { "Name", "ShortName" }));
            }
        }

        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            EnergiesDB DB = CreateEnergiesDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }

}
