using FateExplorer;
using FateExplorer.GameData;
using FateExplorer.Shared;
using FateExplorer.Shop;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests.Shop
{
    [TestFixture]
    public class ShopInventoryViMoTests
    {
        public string FilenameId { get => "shop"; }

        #region Moq ==================
        private MockRepository mockRepository;
        private Mock<IStringLocalizer<App>> mockLl10n;
        private Mock<HttpClient> mockHttpClient;
        private Mock<AppSettings> mockAppCfg;
        private Mock<IGameDataService> mockGameData;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLl10n = mockRepository.Create<IStringLocalizer<App>>();
            this.mockHttpClient = mockRepository.Create<HttpClient>();
            this.mockAppCfg = mockRepository.Create<AppSettings>();
            this.mockGameData = mockRepository.Create<IGameDataService>();
        }
        #endregion ===================



        #region Tests =================[Test]

        [Test]
        [TestCase("de")]
        public void Load(string Language)
        {
            // Arrange
            string BasePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestHelpers.Path2wwwrootData));
            string fileName = Path.GetFullPath(Path.Combine(BasePath, $"{FilenameId}_{Language}.json"));
            string jsonString = File.ReadAllText(fileName);

            // Act
            List<ShopItemM> Result = JsonSerializer.Deserialize<List<ShopItemM>>(jsonString);

            // Assert
            Assert.AreEqual(642, Result.Count);

        }



        [Test, Ignore("")]
        [TestCase("Alchimistenlabor")]
        public void GetStock_ExactFilter_SingleHit(string Filter)
        {
            // Arrange
            var shopInventoryViMo = new ShopInventoryViMo(mockGameData.Object, mockAppCfg.Object, mockHttpClient.Object, mockLl10n.Object);

            // Act
            var result = shopInventoryViMo.GetStock(Filter, null);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test, Ignore("")]
        public async Task InitializeGameDataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var shopInventoryViMo = new ShopInventoryViMo(mockGameData.Object, mockAppCfg.Object, mockHttpClient.Object, mockLl10n.Object);

            // Act
            await shopInventoryViMo.InitializeGameDataAsync();

            // Assert
            Assert.Fail();
        }

        #endregion
    }
}
