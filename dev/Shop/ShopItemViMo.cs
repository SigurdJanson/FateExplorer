namespace FateExplorer.Shop
{
    public class ShopItemViMo
    {
        protected ShopItemM ItemM { get; set; }


        public ShopItemViMo(ShopItemM shopItemM)
        {
            ItemM = shopItemM;
        }

        public string Name => ItemM.Name;

        public double Price => ItemM.Price;

        public double Weight => ItemM.Weight ?? 0;

        public ShopItemM.GroupId Group => ItemM.Group;
    }
}