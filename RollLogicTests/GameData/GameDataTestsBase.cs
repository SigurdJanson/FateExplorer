using FateExplorer.GameData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace vmCode_UnitTests.GameData
{
    [TestFixture]
    public class GameDataTestsBase<TDB, TEntry>
        where TDB : DataServiceCollectionBase<TEntry>, new()
        where TEntry : class, ICharacterAttribute
    {
        /// <summary>
        /// Leading string to identify the json file
        /// </summary>
        public virtual string FilenameId { get; }

        /// <summary>
        /// Defines the white list to exclude properties from the deep comparison when
        /// the language files are compared.See <seealso cref="CompareLanguages_Equality"/>.
        /// </summary>
        public string[] DeepComparisonWhiteList = { "Name", "ShortName" };



        protected static TDB CreateDB()
        {
            return new TDB();
        }


        protected TDB CreateDBfromFile(string Language)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            TDB Result = JsonSerializer.Deserialize<TDB>(jsonString);

            return Result;
        }


        [SetUp]
        public void SetUp()
        {
        }





        [Test]
        [TestCase("", new string[] { "de", "en" })]
        public virtual void CompareLanguages_Equality(string Dummy, string[] Languages)
        {
            // Arrange
            List<TDB> Result = new();

            foreach (var lang in Languages)
            {
                Result.Add(CreateDBfromFile(lang));
            }

            // Act
            // Assert
            Assert.IsTrue(Result.Count == Languages.Length);
            for (int i = 0; i < Languages.Length; i++)
            {
                for (int j = 0; j < Result[0].Data.Count; j++)
                    Assert.IsTrue(TestHelpers.IsDeeplyEqual(
                        Result[0].Data[j],
                        Result[1].Data[j],
                        DeepComparisonWhiteList));
            }
        }




        [Test]
        public void Count_ContentNotLoaded_Return0()
        {
            // Arrange
            TDB DB = CreateDB();

            // Act
            int Count = DB.Count;

            // Assert
            Assert.AreEqual(0, Count);
        }
    }
}