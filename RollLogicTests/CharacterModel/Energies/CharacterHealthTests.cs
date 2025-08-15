using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace UnitTests.CharacterModel.Energies
{
    [TestFixture]
    public class CharacterHealthTests
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CharValues">The characters base values for LP</param>
        /// <param name="AddedEnergy"></param>
        /// <returns></returns>
        private CharacterHealth CreateCharacterHealth(int AddedEnergy)
        {
            return new CharacterHealth(
                GetLpSpecification(), 
                CharacterEnergyClass.LP,
                AddedEnergy,
                this.mockCharacterM.Object);
        }


        private static void MockHasAdvantage(Mock<ICharacterM> mock, string[] Advantages)
            => mock.Setup(c => c.HasAdvantage(It.IsAny<string>()))
                .Returns((string s) => Advantages.Contains(s));
        private static void MockHasDisadvantage(Mock<ICharacterM> mock, string[] Advantages)
            => mock.Setup(c => c.HasDisadvantage(It.IsAny<string>()))
                .Returns((string s) => Advantages.Contains(s));

        #endregion


        #region ## DATA ##

        private static string EnergyId = "LP";
        private static string FilenameId => "energies";
        private static string Language => "de";

        private static EnergiesDbEntry GetLpSpecification()
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
        [TestCase(0, "R_2", -11, 24, Description = "Layariel as Elf")]
        [TestCase(0, "R_1", -11, 27, Description = "Layariel as Human: race base value of 5 instead of 2")]
        [TestCase(0, "R_4", -11, 30, Description = "Layariel as Swarf: race base value of 8 instead of 2")]
        public void Instantiate_LpValue_Fits_SpeciesId(int ToAdd, string Species, int ExpMin, int ExpMax)
        {
            // Arrange
            mockCharacterM.SetupGet(c => c.SpeciesId).Returns(Species);
            mockCharacterM.SetupGet(c => c.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_7")))
                .Returns(HeroWipfelglanz.AbilityValues["ATTR_7"]);
            MockHasAdvantage(mockCharacterM, HeroWipfelglanz.Advantages);
            MockHasDisadvantage(mockCharacterM, HeroWipfelglanz.Disadvantages);

            // Act
            var characterHealth = this.CreateCharacterHealth(ToAdd);

            // Assert
            Assert.That(ExpMin, Is.EqualTo(characterHealth.Min));
            Assert.That(ExpMax, Is.EqualTo(characterHealth.Max));
            this.mockRepository.VerifyAll();
        }




        [Test]
        [TestCase(0, "R_2", -11, 24, Description = "")]
        [TestCase(2, "R_2", -11, 24+2, Description = "Player bought 2 LP")]
        [TestCase(7, "R_2", -11, 24+7, Description = "Player bought 7 LP")]
        public void Instantiate_LpValue_Fits_AddedEnergy(int ToAdd, string Species, int ExpMin, int ExpMax)
        {
            // Arrange
            mockCharacterM.SetupGet(c => c.SpeciesId).Returns(Species);
            mockCharacterM.SetupGet(c => c.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_7")))
                .Returns(HeroWipfelglanz.AbilityValues["ATTR_7"]);
            MockHasAdvantage(mockCharacterM, HeroWipfelglanz.Advantages);
            MockHasDisadvantage(mockCharacterM, HeroWipfelglanz.Disadvantages);

            // Act
            var characterHealth = this.CreateCharacterHealth(ToAdd);

            // Assert
            Assert.That(ExpMin, Is.EqualTo(characterHealth.Min));
            Assert.That(ExpMax, Is.EqualTo(characterHealth.Max));
            this.mockRepository.VerifyAll();
        }



        // Test dependant abilities




        [Test]
        [TestCase(0, "R_2", 18, 12, 6, 5, Description = "Layariel as Elf")]
        public void CalcThresholds_LayarielsValues(int ToAdd, string Species, int Pain1, int Pain2, int Pain3, int Pain4)
        {
            // Arrange
            mockCharacterM.SetupGet(c => c.SpeciesId).Returns(Species);
            mockCharacterM.SetupGet(c => c.Abilities).Returns(HeroWipfelglanz.Abilities);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_7")))
                .Returns(HeroWipfelglanz.AbilityValues["ATTR_7"]);
            MockHasAdvantage(mockCharacterM, HeroWipfelglanz.Advantages);
            MockHasDisadvantage(mockCharacterM, HeroWipfelglanz.Disadvantages);

            var characterHealth = this.CreateCharacterHealth(ToAdd);

            int EffMax = -1; // i.e. ignore the effective maximum

            // Act
            characterHealth.CalcThresholds(EffMax);

            // Assert
            Assert.That(Pain1, Is.EqualTo(characterHealth.Thresholds[0]));
            Assert.That(Pain2, Is.EqualTo(characterHealth.Thresholds[1]));
            Assert.That(Pain3, Is.EqualTo(characterHealth.Thresholds[2]));
            Assert.That(Pain4, Is.EqualTo(characterHealth.Thresholds[3]));
            this.mockRepository.VerifyAll();
        }
    }
}
