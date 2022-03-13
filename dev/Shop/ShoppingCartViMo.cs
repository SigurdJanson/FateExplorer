using System.Collections.Generic;

namespace FateExplorer.Shop
{
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

        /// <summary>
        /// Removes all items with a count of 0.
        /// </summary>
        public void ClearEmptyItems()
        {
            for (int i = 0; i < Count; i++)
                if (this[i].Count == 0)
                    RemoveAt(i);
        }


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
