﻿using FateExplorer;
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
        public static string FilenameId { get => "shop"; }

        #region Moq ==================
        private MockRepository mockRepository;
        private Mock<IStringLocalizer<App>> mockLl10n;
        private Mock<HttpClient> mockHttpClient;
        //private Mock<AppSettings> mockAppCfg;
        private Mock<IGameDataService> mockGameData;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLl10n = mockRepository.Create<IStringLocalizer<App>>();
            this.mockHttpClient = mockRepository.Create<HttpClient>();
            //this.mockAppCfg = mockRepository.Create<AppSettings>();
            this.mockGameData = mockRepository.Create<IGameDataService>();
        }

       public static CurrenciesDB MockCurrenciesDB()
        {
            var result = new CurrenciesDB()
            {
                Data = new List<CurrencyDbEntry>() 
                { 
                    new() { Id = "A", Name="A", Rate=1.1M, Origin="A" },
                    new() { Id = "B", Name="B", Rate=2.2M, Origin="B" },
                    new() { Id = "C", Name="C", Rate=3.3M, Origin="C" }
                }
            };
            return result;
        }

        public ShopInventoryViMo GetShopInventory()
        {
            var result = new ShopInventoryViMo(mockGameData.Object, null, mockHttpClient.Object, mockLl10n.Object);
            return result;
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
            Assert.AreEqual(1039, Result.Count);
        }



        [Test, Ignore("")]
        public async Task InitializeGameDataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var shopInventoryViMo = new ShopInventoryViMo(mockGameData.Object, null, mockHttpClient.Object, mockLl10n.Object);

            // Act
            await shopInventoryViMo.InitializeGameDataAsync();

            // Assert
            Assert.Fail();
        }



        [Test, Ignore("")]
        [TestCase("Alchimistenlabor")]
        public async Task GetStock_ExactFilter_SingleHit(string Filter)
        {
            // Arrange
            mockGameData.SetupGet(c => c.Currencies).Returns(MockCurrenciesDB());

            var shopInventoryViMo = new ShopInventoryViMo(mockGameData.Object, null, mockHttpClient.Object, mockLl10n.Object);
            await shopInventoryViMo.InitializeGameDataAsync();
            // Act
            var result = shopInventoryViMo.GetStock(Filter, null);

            // Assert
            Assert.AreEqual(1, result.Count);
        }


        [Test]
        public void GetCurrencies_NoCurrenciesAvailable_ReturnsNull()
        {
            const int ExpectedCount = 3;
            // Arrange
            mockGameData.SetupGet(c => c.Currencies).Returns(MockCurrenciesDB());
            var Inventory = GetShopInventory();

            // Act
            var result = Inventory.GetCurrencies();

            // Assert
            int ResultLength = 0; // determine length of `result`
            using (IEnumerator<(string id,string name)> enumerator = result.GetEnumerator())
                while (enumerator.MoveNext()) ResultLength++;

            Assert.AreEqual(ExpectedCount, ResultLength);
            mockRepository.VerifyAll();
        }



        [Test] // No app config, i.e. no default
        public void GetDefaultCurrency_NoAppConfig_ReturnsEmpty()
        {
            // Arrange
            mockGameData.SetupGet(c => c.Currencies).Returns(MockCurrenciesDB());
            var Inventory = GetShopInventory();

            // Act
            var result = Inventory.GetDefaultCurrency();

            // Assert
            Assert.AreEqual(("", ""), result);
            mockRepository.VerifyAll();
        }



        [Test, Sequential]
        public void GetExchangeRate(
            [Values("A", "B", "C")] string currencyId, [Values(1.1, 2.2, 3.3)] decimal Expected)
        {
            // Arrange
            mockGameData.SetupGet(c => c.Currencies).Returns(MockCurrenciesDB());
            var Inventory = GetShopInventory();

            // Act
            var result = Inventory.GetExchangeRate(currencyId);

            // Assert
            Assert.AreEqual(Expected, result);
            mockRepository.VerifyAll();
        }

        #endregion
    }
}
