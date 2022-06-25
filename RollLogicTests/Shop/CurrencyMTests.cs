using FateExplorer.GameData;
using FateExplorer.Shop;
using NUnit.Framework;

namespace UnitTests.Shop
{
    [TestFixture]
    public class CurrencyMTests
    {
        [Test]
        public void TestCurrency()
        {
            // Arrange
            // Act
            var currencyM = new CurrencyM(new CurrencyDbEntry()
            {
                Id = "TestId",
                Name = "TestName",
                Origin = "TestRegion",
                Rate = 1.25
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("TestId", currencyM.Id);
                Assert.AreEqual("TestName", currencyM.Name);
                Assert.AreEqual("TestRegion", currencyM.Origin);
                Assert.AreEqual(1.25, currencyM.Rate);
            });
        }
    }
}
