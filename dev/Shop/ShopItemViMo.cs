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

        public ShopItemM.GroupId GroupId => ItemM.Group;

        string group;
        /// <summary>
        /// The (localised) group. Localisation will not be handled by this class.
        /// </summary>
        public string Group { get => group ?? GroupId.ToString(); set => group = value; }
    }
}