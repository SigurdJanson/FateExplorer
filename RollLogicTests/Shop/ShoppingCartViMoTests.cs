using FateExplorer.Shop;
using NUnit.Framework;

namespace UnitTests.Shop
{
    [TestFixture]
    public class ShoppingCartViMoTests
    {
        [Test]
        public void TotalPrice_EmptyList_0()
        {
            // Arrange
            var shoppingCartViMo = new ShoppingCartViMo();

            // Act
            var Result = shoppingCartViMo.TotalPrice;

            // Assert
            Assert.AreEqual(0.0, Result);
        }


        // 1*9 + 2*8 + 3*7 ... + 9*1 = 165
        [Test]
        public void TotalPrice_List_ReturnsProductTotal()
        {
            const int Max = 10;
            // Arrange
            var shoppingCartViMo = new ShoppingCartViMo();
            for (int i = 1; i < Max; i++)
            {
                var item = new ShopItemViMo(new ShopItemM() { Price = i });
                shoppingCartViMo.Add((item, Max - i));
            }

            // Act
            var Result = shoppingCartViMo.TotalPrice;

            // Assert
            Assert.AreEqual(165.0, Result);
        }



        //[Test] // Tested method Currently not implemented
        //public void ClearEmptyItems_NoItemsWithCount0_ListLengthDoesNotChange()
        //{
        //    const int Max = 10;
        //    // Arrange
        //    var shoppingCartViMo = new ShoppingCartViMo();
        //    for (int i = 1; i < Max; i++)
        //    {
        //        var item = new ShopItemViMo(new ShopItemM() { Price = i });
        //        shoppingCartViMo.Add((item, Max - i));
        //    }
        //    int OldCount = shoppingCartViMo.Count;

        //    // Act
        //    shoppingCartViMo.ClearEmptyItems();

        //    // Assert
        //    Assert.AreEqual(OldCount, shoppingCartViMo.Count);
        //}



        //[Test] // Tested method Currently not implemented
        //public void ClearEmptyItems_ItemsWithCount0_ListLengthShrinks()
        //{
        //    const int Max = 10;
        //    // Arrange
        //    var shoppingCartViMo = new ShoppingCartViMo();
        //    for (int i = 1; i < Max; i++)
        //    {
        //        var item = new ShopItemViMo(new ShopItemM() { Price = i });
        //        shoppingCartViMo.Add((item, Max - i));

        //        item = new ShopItemViMo(new ShopItemM() { Price = i });
        //        shoppingCartViMo.Add((item, 0));
        //    }
        //    int OldCount = shoppingCartViMo.Count;

        //    // Act
        //    shoppingCartViMo.ClearEmptyItems();

        //    // Assert
        //    Assert.AreEqual(OldCount / 2, shoppingCartViMo.Count);
        //}

    }
}
