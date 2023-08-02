using FateExplorer.CharacterModel;
using FateExplorer.GameData;
using FateExplorer.Shared;
using Moq;
using NUnit.Framework;

namespace UnitTests.CharacterModel
{
    [TestFixture]
    public class ResilienceMTests
    {
        private MockRepository mockRepository;

        private Mock<ICharacterM> mockCharacterM;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockCharacterM = this.mockRepository.Create<ICharacterM>();
        }



        [TestCase(10, 10, 12, "R1", ExpectedResult = 1)]
        [TestCase(10, 11, 12, "R1", ExpectedResult = 2)]
        [TestCase(13, 15, 13, "R1", ExpectedResult = 3)]
        [TestCase(10, 10, 12, "R2", ExpectedResult = 0)]
        [TestCase(10, 11, 12, "R2", ExpectedResult = 1)]
        [TestCase(12, 13, 13, "R2", ExpectedResult = 1)] // sum = 38
        [TestCase(13, 13, 13, "R2", ExpectedResult = 2)] // sum = 39
        [TestCase(13, 15, 13, "R2", ExpectedResult = 2)]
        public int Constructor_ByHero_CorrectValue(int ab1v, int ab2v, int ab3v, string RaceId)
        {
            // Arrange
            ResilienceDbEntry Db = new()
            {
                DependantAbilities = new string[] { "ATTR_1", "ATTR_2", "ATTR_3"},
                Id = "NEUTRAL", Name = "TEST", ShortName = "T",
                RaceBaseValue = new ResilienceBaseValue[]
                {
                    new ResilienceBaseValue { RaceId = "R1", Value = -4 },
                    new ResilienceBaseValue { RaceId = "R2", Value = -5 }
                }
            };

            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_1")))
                .Returns(ab1v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_2")))
                .Returns(ab2v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_3")))
                .Returns(ab3v);
            mockCharacterM.SetupGet(c => c.SpeciesId)
                .Returns(RaceId);

            // Act
            var result = new ResilienceM(Db, mockCharacterM.Object).Value;

            // Assert
            mockRepository.VerifyAll();
            return result;
        }


        [TestCase(ChrAttrId.SPI, ExpectedResult = 1+1)]
        [TestCase(ChrAttrId.TOU, ExpectedResult = 1+1)]
        public int Advantage_ValuePlusOne(string Which)
        {
            int ab1v = 10, ab2v = 11, ab3v = 12;
            string RaceId = "R2";
            // Arrange
            ResilienceDbEntry Db = new()
            {
                DependantAbilities = new string[] { "ATTR_1", "ATTR_2", "ATTR_3" },
                Id = Which,
                Name = Which,
                ShortName = "T",
                RaceBaseValue = new ResilienceBaseValue[]
                {
                    new ResilienceBaseValue { RaceId = "R1", Value = -4 },
                    new ResilienceBaseValue { RaceId = "R2", Value = -5 }
                }
            };

            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_1")))
                .Returns(ab1v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_2")))
                .Returns(ab2v);
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == "ATTR_3")))
                .Returns(ab3v);
            mockCharacterM.SetupGet(c => c.SpeciesId)
                .Returns(RaceId);
            if (Which == ChrAttrId.SPI)
            {
                mockCharacterM.Setup(c => c.HasAdvantage(ADV.IncreasedSpirit))
                    .Returns(true);
                mockCharacterM.Setup(c => c.HasDisadvantage(DISADV.DecreasedSpirit))
                    .Returns(false);
            }
            else if (Which == ChrAttrId.TOU)
            {
                mockCharacterM.Setup(c => c.HasAdvantage(ADV.IncreasedToughness))
                    .Returns(true);
                mockCharacterM.Setup(c => c.HasDisadvantage(DISADV.DecreasedToughness))
                    .Returns(false);
            }

            // Act
            var result = new ResilienceM(Db, mockCharacterM.Object).Value;

            // Assert
            mockRepository.VerifyAll();
            return result;
        }

    }
}
