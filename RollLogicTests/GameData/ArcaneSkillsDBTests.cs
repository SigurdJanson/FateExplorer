using FateExplorer.GameData;
using NUnit.Framework;


namespace vmCode_UnitTests.GameData
{
    [TestFixture]
    public class ArcaneSkillsDBTests : GameDataTestsBase<ArcaneSkillsDB, ArcaneSkillDbEntry>
    {
        public override string FilenameId { get => "arcaneskills"; }



        [Test]
        [TestCase("de", "SPELL_89", "", 335)]
        [TestCase("en", "SPELL_89", "SPELL_331", 324)]
        public void LoadFromFile_ParseSuccessful(string Language, string Skill1, string SkillLast, int Count)
        {
            // Arrange
            // Act
            ArcaneSkillsDB Result = CreateDBfromFile(Language);


            // Assert
            Assert.AreEqual(Count, Result.Count);
            Assert.AreEqual(Skill1, Result[0].Id);
            Assert.AreEqual(SkillLast, Result[^1].Id);
        }


        [Test, Ignore("Not valid for arcane skill because not all have been translated")]
        [TestCase("", new string[] { "de", "en" })]
        public override void CompareLanguages_Equality(string Dummy, string[] Languages)
        {
            Assert.Inconclusive("Not valid for arcane skill because not all have been translated");
        }


        // inherited: public void Count_ContentNotLoaded_Return0()
    }
}
