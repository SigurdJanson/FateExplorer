using FateExplorer.GameData;
using NUnit.Framework;

namespace RollLogicTests.GameData
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
            Assert.AreEqual(59, Result.Count);
            Assert.AreEqual(Skill1, Result[0].Name);
            Assert.AreEqual(SkillLast, Result[58].Name);
        }



        //inherited: public void Count_ContentNotLoaded_Return0()
    }
}
