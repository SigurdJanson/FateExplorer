using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace UnitTests.CharacterModel.Energies
{
    [TestFixture]
    public class CharacterAstralEnergyTests
    {
        #region ## SETUP ##

        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
        }

        private CharacterAstralEnergy CreateCharacterAstralEnergy(int AddedEnergy)
        {
            return new CharacterAstralEnergy(
                GetAESpecification(),
                CharacterEnergyClass.AE,
                AddedEnergy,
                this.mockCharacterM.Object);
        }

        #endregion


        #region ## DATA ##

        private static string EnergyId = "AE";
        private static string FilenameId => "energies";
        private static string Language => "de";

        private static EnergiesDbEntry GetAESpecification()
        {
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            EnergiesDB Result = JsonSerializer.Deserialize<EnergiesDB>(jsonString);
            return Result[EnergyId];
        }

        #endregion



        [Test]
        [TestCase(0, "R_2", 0, 35, Description = "Layariel as Elf")]
        public void Instantiate_TestMinMax(int ToAdd, string Species, int ExpMin, int ExpMax)
        {
            // Arrange
            mockCharacterM.SetupGet(c => c.SpeciesId).Returns(Species);
            mockCharacterM.SetupGet(c => c.Abilities).Returns(HeroWipfelglanz.Abilities);
            // Layariel honours only the tradition "Elves"
            mockCharacterM.Setup(c => c.HasSpecialAbility(It.Is<string>(s => s != "SA_345"))).Returns(false);
            mockCharacterM.Setup(c => c.HasSpecialAbility(It.Is<string>(s => s == "SA_345"))).Returns(true);

            // Act
            var characterAstralEnergy = this.CreateCharacterAstralEnergy(ToAdd);

            // Assert
            Assert.AreEqual(ExpMin, characterAstralEnergy.Min);
            Assert.AreEqual(ExpMax, characterAstralEnergy.Max);
            this.mockRepository.VerifyAll();
        }
    }
}
