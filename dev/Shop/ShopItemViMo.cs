using System;
using System.ComponentModel.DataAnnotations;

namespace FateExplorer.Shop
{
    public class ShopItemViMo
    {
        protected ShopItemM ItemM { get; set; }


        public ShopItemViMo(ShopItemM shopItemM)
        {
            ItemM = shopItemM;
        }

        [Display(Name = "-")]
        public string Name => ItemM.Name;

        [Display(Name = "lblPrice")]
        public double Price => ItemM.Price;

        [Display(Name = "lblWeight")]
        public double Weight => ItemM.Weight ?? 0;

        public ShopItemM.GroupId GroupId => ItemM.Group;

        string group;
        /// <summary>
        /// The (localised) group. Localisation will not be handled by this class.
        /// </summary>
        [Display(Name = "hide")]
        public string Group { get => group ?? GroupId.ToString(); set => group = value; }


        [Display(Name = "lblStruPoints")]
        public int[] StruPo => ItemM.StruPo;


        [Display(Name = "lblItemLength")]
        public int? Length => ItemM.Length;
    }
}