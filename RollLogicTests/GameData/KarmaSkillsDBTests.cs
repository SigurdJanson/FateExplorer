using FateExplorer.GameData;
using NUnit.Framework;

namespace UnitTests.GameData
{
    [TestFixture]
    public class KarmaSkillsDBTests : GameDataTestsBase<KarmaSkillsDB, KarmaSkillDbEntry>
    {
        public override string FilenameId { get => "karmaskills"; }

        [Test]
        [TestCase("de", "LITURGY_41", "", 328)]
        [TestCase("en", "LITURGY_41", "LITURGY_192", 316)]
        public void LoadFromFile_ParseSuccessful(string Language, string Skill1, string SkillLast, int Count)
        {
            // Arrange
            // Act
            KarmaSkillsDB Result = CreateDBfromFile(Language);

            // Assert
            Assert.That(Count, Is.EqualTo(Result.Count));
            Assert.That(Skill1, Is.EqualTo(Result[0].Id));
            Assert.That(SkillLast, Is.EqualTo(Result[^1].Id));
        }


        [Test, Ignore("Not valid for karma skill because not all have been translated")]
        [TestCase("", new string[] { "de", "en" })]
        public override void CompareLanguages_Equality(string Dummy, string[] Languages)
        {
            Assert.Inconclusive("Not valid for karma skill because not all have been translated");
        }



        // inherited: public void Count_ContentNotLoaded_Return0()

    }
}
