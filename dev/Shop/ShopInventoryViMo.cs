using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FateExplorer.Shop
{
    public class ShopInventoryViMo
    {
        protected HttpClient DataSource;


        public ShopInventoryViMo(HttpClient dataSource)
        {
            DataSource = dataSource;
        }

        protected List<ShopItemM> Inventory { get; set; }


        public List<ShopItemViMo> GetStock(string Filter)
        {
            var Selected = Inventory.FindAll(i => i.Name.Contains(Filter));

            List<ShopItemViMo> Result = new();
            foreach (var item in Selected)
                Result.Add(new ShopItemViMo(item));

            return Result;
        }

        /// <summary>
        /// Load the data
        /// </summary>
        /// <returns></returns>
        public async Task InitializeGameDataAsync()
        {
            string Language = System.Globalization.CultureInfo.CurrentUICulture.Name;
            if (Language.StartsWith("de"))
                Language = "de";
            else
                Language = "en";


            string fileName = $"data/shop_{Language}.json";
            Inventory = await DataSource.GetFromJsonAsync<List<ShopItemM>>(fileName);
        }

    }

}
