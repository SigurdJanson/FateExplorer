using FateExplorer.GameData;
using NUnit.Framework;


namespace UnitTests.GameData
{
    [TestFixture]
    public class EnergiesDBTests : GameDataTestsBase<EnergiesDB, EnergiesDbEntry>
    {
        public override string FilenameId { get => "energies"; }

        [Test]
        [TestCase("de", new string[] { "Lebensenergie", "Astralenergie", "Karmaenergie" })]
        [TestCase("en", new string[] { "Life Energy", "Arcane Energy", "Karma Energy" })]
        public void LoadFromFile_ParseSuccessful(string Language, string[] EnName)
        {
            string[] ResId = new string[] { "LP", "AE", "KP" };

            // Arrange
            // Act
            EnergiesDB Result = CreateDBfromFile(Language);

            // Assert
            Assert.AreEqual(EnName.Length, Result.Count);
            for (int i = 0; i < EnName.Length; i++)
            {
                Assert.AreEqual(ResId[i], Result[i].Id);
                Assert.AreEqual(EnName[i], Result[i].Name);
            }
        }




        //[Test]
        //[TestCase( "", new string[] { "de", "en" } )]
        //public void CompareLanguages_Equality(string Dummy, string[] Languages)
        //{
        //    // Arrange
        //    List<EnergiesDB> Result = new();

        //    foreach (var lang in Languages)
        //    {
        //        Result.Add(CreateDBfromFile(lang, "energies_"));
        //    }

        //    // Act
        //    // Assert
        //    Assert.IsTrue(Result.Count == Languages.Length);
        //    for (int i = 0; i < Languages.Length; i++)
        //    {
        //        for(int j = 0; j < Result[0].Data.Count; j++)
        //            Assert.IsTrue(TestHelpers.IsDeeplyEqual(
        //                Result[0].Data[j], 
        //                Result[1].Data[j], 
        //                new string[] { "Name", "ShortName" }));
        //    }
        //}


        //inherited: public void Count_ContentNotLoaded_Return0()
    }

}
