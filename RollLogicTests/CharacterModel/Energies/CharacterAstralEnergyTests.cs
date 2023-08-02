using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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



        private static void MockSpecialAbility(Mock<ICharacterM> mock, string[] Advantages)
            => mock.Setup(c => c.HasSpecialAbility(It.IsAny<string>()))
                .Returns((string s) => Advantages.Contains(s));
        private static void MockHasAdvantage(Mock<ICharacterM> mock, string[] Advantages)
        {
            mock.Setup(c => c.HasAdvantage(It.IsAny<string>()))
                .Returns((string s) => Advantages.Contains(s));
            Dictionary<string, IActivatableM> AdvDict = new();
            foreach(var a in Advantages)
            {
                AdvDict.Add(a, new TieredActivatableM(a, 1, null)); // NOTE: only tier 1, no reference
            };
            mock.SetupGet(c => c.Advantages).Returns(AdvDict);
        }
        private static void MockHasDisadvantage(Mock<ICharacterM> mock, string[] Advantages)
            => mock.Setup(c => c.HasDisadvantage(It.IsAny<string>()))
                .Returns((string s) => Advantages.Contains(s));

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
            MockSpecialAbility(mockCharacterM, new string[] { SA.TraditionElf });
            MockHasAdvantage(mockCharacterM, HeroWipfelglanz.Advantages);
            MockHasDisadvantage(mockCharacterM, HeroWipfelglanz.Disadvantages);
            
            // Act
            var characterAstralEnergy = this.CreateCharacterAstralEnergy(ToAdd);

            // Assert
            Assert.AreEqual(ExpMin, characterAstralEnergy.Min);
            Assert.AreEqual(ExpMax, characterAstralEnergy.Max);
            this.mockRepository.VerifyAll();
        }
    }
}
