using FateExplorer.GameData;
using FateExplorer.Shop;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using static FateExplorer.Shop.MerchantViMo;

namespace vmCode_UnitTests.Shop
{
    [TestFixture]
    public class MerchantViMoTests
    {
        private MockRepository mockRepository;

        private Mock<IGameDataService> mockGameDataService;

        private AbilitiesDB MockAbilitiesDB(string Language = "de")
        {
            string FilenameId = "attributes";

            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            AbilitiesDB Result = JsonSerializer.Deserialize<AbilitiesDB>(jsonString);

            return Result;
        }



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockGameDataService = this.mockRepository.Create<IGameDataService>();
        }

        private MerchantViMo CreateMerchantViMo()
        {
            return new MerchantViMo(this.mockGameDataService.Object);
        }

        [Test]
        [TestCase(MerchantViMo.ExperienceLevel.Novice, ExpectedResult = 3)]
        [TestCase(MerchantViMo.ExperienceLevel.Advanced, ExpectedResult = 6)]
        [TestCase(MerchantViMo.ExperienceLevel.Competent, ExpectedResult = 9)]
        [TestCase(MerchantViMo.ExperienceLevel.Proficient, ExpectedResult = 12)]
        [TestCase(MerchantViMo.ExperienceLevel.Expert, ExpectedResult = 15)]
        [TestCase(MerchantViMo.ExperienceLevel.Legend, ExpectedResult = 18)]
        public int SetExperience_ReturnSkillPoints(ExperienceLevel Level)
        {
            // Arrange
            var merchantViMo = this.CreateMerchantViMo();

            // Act
            merchantViMo.TradeExperience = Level;

            // Assert
            this.mockRepository.VerifyAll();
            return merchantViMo.TradeSkillValue; ;
        }



        [Test]
        [TestCase(MerchantViMo.ExperienceLevel.Novice, ExpectedResult = 1)]
        [TestCase(MerchantViMo.ExperienceLevel.Advanced, ExpectedResult = 2)]
        [TestCase(MerchantViMo.ExperienceLevel.Competent, ExpectedResult = 3)]
        [TestCase(MerchantViMo.ExperienceLevel.Proficient, ExpectedResult = 4)]
        [TestCase(MerchantViMo.ExperienceLevel.Expert, ExpectedResult = 5)]
        [TestCase(MerchantViMo.ExperienceLevel.Legend, ExpectedResult = 6)]
        public int Haggle_AbilitiesGt20_QLMatchesExperience(ExperienceLevel Level)
        {
            // Arrange
            var merchantViMo = this.CreateMerchantViMo();
            mockGameDataService.SetupGet(p => p.Abilities)
                .Returns(MockAbilitiesDB());

            // Arrange abilities: important to set them to max to
            // receive a predictable QL
            merchantViMo.Sagacity = 21;
            merchantViMo.Intuition = 21;
            merchantViMo.Charisma = 21;
            merchantViMo.TradeExperience = Level;

            // Act
            var result = merchantViMo.Haggle();

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }


        // Careful: this test may fail in case of an extremely extraordinary roll result
        [Test]
        [TestCase(MerchantViMo.ExperienceLevel.Novice, ExpectedResult = 0)]
        [TestCase(MerchantViMo.ExperienceLevel.Advanced, ExpectedResult = 0)]
        public int Haggle_LowAbilities_QLIsAlways0(ExperienceLevel Level)
        {
            // Arrange
            var merchantViMo = this.CreateMerchantViMo();
            mockGameDataService.SetupGet(p => p.Abilities)
                .Returns(MockAbilitiesDB());

            // Arrange abilities: important to set them to max to
            // receive a predictable QL
            merchantViMo.Sagacity = 0;
            merchantViMo.Intuition = 0;
            merchantViMo.Charisma = 0;
            merchantViMo.TradeExperience = Level;

            // Act
            var result = merchantViMo.Haggle();
            Assume.That(result, Is.Zero);

            // Assert
            this.mockRepository.VerifyAll();
            return result;
        }


        [Test]
        [TestCase(MerchantViMo.ExperienceLevel.Novice, 1, 0, 1.1)]
        [TestCase(MerchantViMo.ExperienceLevel.Novice, 1, 6, 0.5)]
        [TestCase(MerchantViMo.ExperienceLevel.Proficient, 4, 0, 1.4)]
        [TestCase(MerchantViMo.ExperienceLevel.Proficient, 4, 4, 1)]
        [TestCase(MerchantViMo.ExperienceLevel.Proficient, 4, 6, 0.8)]
        [TestCase(MerchantViMo.ExperienceLevel.Legend, 6, 0, 1.5, Description = "Checks the boundary of 0.5")]
        [TestCase(MerchantViMo.ExperienceLevel.Legend, 6, 3, 1.3)]
        [TestCase(MerchantViMo.ExperienceLevel.Legend, 6, 6, 1.0)]
        public void DeterminePrice_AbilitiesGt20_PriceMatchesDifference(
            ExperienceLevel Level, int ExpectedQL, int BuyerQL, double ExpectedMultiplier)
        {
            const double Price = 63.5;
            // Arrange
            var merchantViMo = this.CreateMerchantViMo();
            mockGameDataService.SetupGet(p => p.Abilities)
                .Returns(MockAbilitiesDB());

            // Arrange abilities: important to set them to max to
            // receive a predictable QL
            merchantViMo.Sagacity = 21;
            merchantViMo.Intuition = 21;
            merchantViMo.Charisma = 21;
            merchantViMo.TradeExperience = Level;
            merchantViMo.Haggle();
            Assume.That(merchantViMo.HaggleQL, Is.EqualTo(ExpectedQL));

            // Act
            var result = merchantViMo.DeterminePrice(Price, BuyerQL);

            // Assert
            Assert.AreEqual(Price * ExpectedMultiplier, result);
            this.mockRepository.VerifyAll();
        }
    }
}
