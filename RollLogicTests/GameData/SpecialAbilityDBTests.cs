using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FateExplorer.GameData;
using NUnit.Framework;


namespace RollLogicTests.GameData
{
    [TestFixture]
    public class SpecialAbilityDBTests : GameDataTestsBase<SpecialAbilityDB, SpecialAbilityDbEntry>
    {
        public override string FilenameId { get => "specabs"; }


        [Test]
        [TestCase("de", "*Aspektkenntnis", "Weg d. Zofe")]
        //[TestCase("en", "Basilisk-tongue", "Guild Magic Ball"), Ignore("")]
        public void LoadFromFile_ParseSuccessful(string Language, string SA1, string SALast)
        {
            // Arrange
            // Act
            SpecialAbilityDB Result = CreateDBfromFile(Language);

            // Assert
            Assert.AreEqual(1436, Result.Count);
            Assert.AreEqual(SA1, Result[0].Name);
            Assert.AreEqual(SALast, Result[^1].Name);
        }

        [Test, Ignore("No english version, yet")]
        [TestCase("", new string[] { "de", "en" })]
        public override void CompareLanguages_Equality(string Dummy, string[] Languages)
        {

        }

        /// inherited: public void Count_ContentNotLoaded_Return0()
    }
}
