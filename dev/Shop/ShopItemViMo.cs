using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
            PriceStr = Price.ToString("N2");
            WeightStr = Weight.ToString("N4");
        }

        public string Id => ItemM.Template;


        [Display(Name = "-")]
        public string Name => ItemM.Name;


        [Display(Name = "lblPrice")]
        public decimal Price => ItemM.Price;

        /// <summary>
        /// String representation of <see cref="Price"/> to reduce compuatations during rendering
        /// </summary>
        public string PriceStr { get; protected set; }


        [Display(Name = "lblWeight")]
        public double Weight => ItemM.Weight ?? 0;

        /// <summary>
        /// String representation of <see cref="Weight"/> to reduce compuatations during rendering
        /// </summary>
        public string WeightStr { get; protected set; }


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

        /// <summary>
        /// 
        /// </summary>
        public Action<MouseEventArgs> OpenItemDetails { get; set; } = e => { };
        public Action<MouseEventArgs> Add2Cart { get; set; } = e => { };
    }
}