﻿using FateExplorer.Shop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RollLogicTests.Shop
{
    [TestFixture]
    public class ShopInventoryViMoTests
    {
        public string FilenameId { get => "shop"; }

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
            Assert.AreEqual(637, Result.Count);

        }



        [Test, Ignore("")]
        [TestCase("Alchimistenlabor")]
        public void GetStock_ExactFilter_SingleHit(string Filter)
        {
            // Arrange
            var shopInventoryViMo = new ShopInventoryViMo(null);

            // Act
            var result = shopInventoryViMo.GetStock(Filter);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [Test, Ignore("")]
        public async Task InitializeGameDataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var shopInventoryViMo = new ShopInventoryViMo(null);

            // Act
            await shopInventoryViMo.InitializeGameDataAsync();

            // Assert
            Assert.Fail();
        }
    }
}
