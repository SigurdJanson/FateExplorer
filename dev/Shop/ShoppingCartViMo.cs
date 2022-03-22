using System.Collections.Generic;

namespace FateExplorer.Shop
{
    /// <summary>
    /// The shopping cart for the "bazaar" (i.e. shop). It stores the items in the cart as tupels.
    /// </summary>
    public class ShoppingCartViMo : List<(ShopItemViMo Item, int Count)>
    {
        /// <summary>
        /// Gets the total price of all items in the default currency (i.e. silverthalers)
        /// </summary>
        public double TotalPrice
        {
            get
            {
                double Result = 0;
                ForEach(delegate ((ShopItemViMo item, int count) i)
                {
                    Result += i.count * i.item.Price;
                });
                return Result;
            }
        }

        /// <summary>
        /// The total count of items in the shoppings cart. Duplicates are counted twice.
        /// </summary>
        public int ItemCount
        {
            get
            {
                int Result = 0;
                ForEach(delegate ((ShopItemViMo item, int count) i)
                {
                    Result += i.count;
                });
                return Result;

            }
        }


        ///// <summary>
        ///// Removes all items with a count of 0.
        ///// </summary>
        // CURRENTLY NOT NEEDED
        //public void ClearEmptyItems()
        //{
        //    for (int i = 0; i < Count; i++)
        //        if (this[i].Count == 0)
        //            RemoveAt(i);
        //}


        /// <summary>
        /// Adds a new item to the cart. Duplicates are checked for. In case of
        /// duplicates there will be no new item but the count of existing items
        /// is increased.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public new void Add((ShopItemViMo Item, int Count) item)
        {
            if (item.Count == 0) return;

            for (int i = 0; i < Count; i++)
                if (this[i].Item == item.Item)
                {
                    int ExistingCount = this[i].Count;
                    item.Count += ExistingCount;
                    RemoveAt(i);
                }

            base.Add(item);
        }
    }
}
