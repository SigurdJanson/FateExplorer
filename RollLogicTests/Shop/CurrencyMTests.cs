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
                Rate = 1.25M
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That("TestId", Is.EqualTo(currencyM.Id));
                Assert.That("TestName", Is.EqualTo(currencyM.Name));
                Assert.That("TestRegion", Is.EqualTo(currencyM.Origin));
                Assert.That(1.25, Is.EqualTo(currencyM.Rate));
            });
        }
    }
}
