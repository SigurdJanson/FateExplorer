using FateExplorer.CharacterModel;
using FateExplorer.GameData;
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


        private ResilienceM CreateResilianceMByBaseValue(int Base, int Extra, string[] Abs)
        {
            return new ResilienceM(
                this.mockCharacterM.Object, Base, Extra, Abs);
        }


        private ResilienceM CreateResilianceMByValue(int Value, string[] Abs)
        {
            return new ResilienceM(
                this.mockCharacterM.Object, Value, Abs);
        }


        [TestCase(10, 10, 12, "R1", ExpectedResult = 1)]
        [TestCase(10, 11, 12, "R1", ExpectedResult = 2)]
        [TestCase(13, 15, 13, "R1", ExpectedResult = 3)]
        [TestCase(10, 10, 12, "R2", ExpectedResult = 0)]
        [TestCase(10, 11, 12, "R2", ExpectedResult = 1)]
        [TestCase(12, 13, 13, "R2", ExpectedResult = 1)] // sum = 38
        [TestCase(13, 13, 13, "R2", ExpectedResult = 2)] // sum = 39
        [TestCase(13, 15, 13, "R2", ExpectedResult = 2)]
        public int Constructor_ByHero_Correct(int ab1v, int ab2v, int ab3v, string RaceId)
        {
            // Arrange
            ResilienceDbEntry Db = new()
            {
                DependantAbilities = new string[] { "ATTR_1", "ATTR_2", "ATTR_3"},
                Id = "HP", Name = "TEST", ShortName = "T",
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
            var result = new ResilienceM(Db, mockCharacterM.Object);

            // Assert
            mockRepository.VerifyAll();
            return result.Value;
        }


        [Test]
        [TestCase("ATTR_1", 13, "ATTR_2", 15, "ATTR_3", 13, 2)]
        [TestCase("ATTR_7", 10, "ATTR_7", 10, "ATTR_8", 12, 0)]
        [TestCase("ATTR_1", 10, "ATTR_2", 11, "ATTR_3", 12, 1)]
        public void Constructor_ByValue_Correct(string ab1, int ab1v, string ab2, int ab2v, string ab3, int ab3v, int Value)
        {
            const int JunisBaseValue = -5;
            // Arrange
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab1)))
                .Returns(ab1v);
            if (ab2 != ab1) // no duplicate `GetAbility` for identical attributes
                mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab2)))
                    .Returns(ab2v);
            if (ab3 != ab1 && ab3 != ab2) // no duplicate `GetAbility` for identical attributes
                mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab3)))
                    .Returns(ab3v);


            // Act
            var resilianceM = this.CreateResilianceMByValue(Value, new string[3] { ab1, ab2, ab3 });

            // Assert
            Assert.AreEqual(Value, resilianceM.Value);
            Assert.AreEqual(JunisBaseValue, resilianceM.BaseValue);
            Assert.AreEqual(0, resilianceM.ExtraValue);

            mockRepository.VerifyAll();
        }


        [Test]
        [TestCase("ATTR_1", 13, "ATTR_2", 15, "ATTR_3", 13, -5, 1, 3)]
        [TestCase("ATTR_7", 10, "ATTR_7", 10, "ATTR_8", 12, -5, 0, 0)]
        public void Constructor_ByBaseValue_Correct(string ab1, int ab1v, string ab2, int ab2v, string ab3, int ab3v, int BaseValue, int Extra, int Expected)
        {
            // Arrange
            mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab1)))
                .Returns(ab1v);
            if (ab2 != ab1) // no duplicate `GetAbility` for identical attributes
                mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab2)))
                    .Returns(ab2v);
            if (ab3 != ab1 && ab3 != ab2) // no duplicate `GetAbility` for identical attributes
                mockCharacterM.Setup(c => c.GetAbility(It.Is<string>(s => s == ab3)))
                    .Returns(ab3v);


            // Act
            var resilianceM = this.CreateResilianceMByBaseValue(BaseValue, Extra, new string[3] { ab1, ab2, ab3 });

            Assume.That(resilianceM.BaseValue, Is.EqualTo(BaseValue));
            Assume.That(resilianceM.ExtraValue, Is.EqualTo(Extra));

            // Assert
            Assert.AreEqual(Expected, resilianceM.Value);
            this.mockRepository.VerifyAll();
        }
    }
}
