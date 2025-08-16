using FateExplorer.GameData;
using NUnit.Framework;

namespace UnitTests.GameData
{
    [TestFixture]
    public class SkillDBTests : GameDataTestsBase<SkillsDB, SkillDbEntry>
    {
        public override string FilenameId { get => "skills"; }

        [Test]
        [TestCase("de", "Fliegen", "Stoffbearbeitung")]
        [TestCase("en", "Flying", "Clothworking")]
        public void LoadFromFile_ParseSuccessful(string Language, string Skill1, string SkillLast)
        {
            // Arrange
            // Act
            SkillsDB Result = CreateDBfromFile(Language);
            // Assert
            Assert.That(59, Is.EqualTo(Result.Count));
            Assert.That(Skill1, Is.EqualTo(Result[0].Name));
            Assert.That(SkillLast, Is.EqualTo(Result[58].Name));
        }



        //inherited: public void Count_ContentNotLoaded_Return0()
    }
}
