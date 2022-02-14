﻿using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FateExplorer.Shop
{
    public class ShopInventoryViMo
    {
        protected HttpClient DataSource;
        protected IStringLocalizer<App> l10n;

        public ShopInventoryViMo(HttpClient dataSource, IStringLocalizer<App> localizer)
        {
            DataSource = dataSource;
            l10n = localizer;
        }


        /// <summary>
        /// List of available items
        /// </summary>
        protected List<ShopItemViMo> Inventory { get; set; }

        /// <summary>
        /// List of available currencies
        /// </summary>
        protected List<CurrencyM> Currencies { get; set; }

        /// <summary>
        /// Get a list of objects DSA allows to purchase.
        /// </summary>
        /// <param name="Filter">A free text filter. Will be applied to the inventory 
        /// items' names and the group, unless a <see cref="this.GroupId"/> is given 
        /// in which case only the item names will be used.</param>
        /// <param name="GroupId">Limits the returned items to the desired group</param>
        /// <returns>A list of items.</returns>
        public List<ShopItemViMo> GetStock(string Filter, int? GroupId)
        {
            List<ShopItemViMo> Selected;

            Selected = (string.IsNullOrWhiteSpace(Filter), GroupId is null) switch
            {
                (true, true) => Inventory,
                (true, false) => Inventory.FindAll(i => (int)i.GroupId == GroupId),
                (false, true) => Inventory.FindAll(i => i.Name.Contains(Filter, StringComparison.CurrentCultureIgnoreCase) ||
                       i.Group.Contains(Filter, StringComparison.CurrentCultureIgnoreCase)),
                (false, false) => Inventory.FindAll(i => (int)i.GroupId == GroupId)
                                           .FindAll(i => i.Name.Contains(Filter, StringComparison.CurrentCultureIgnoreCase))
            };
            if (Selected is null) Selected = new();

            return Selected;
        }


        /// <summary>
        /// Get the localised names of the group names (represented by <see cref="ShopItemM.GroupId"/>
        /// </summary>
        /// <returns>The names as tupels with the id as int and the name</returns>
        public IEnumerable<(int id, string name)> GetGroups()
        {
            List<(int id, string name)> Groups = new();
            foreach (var g in Enum.GetValues(typeof(ShopItemM.GroupId)))
                Groups.Add(((int)g, l10n[g.ToString()]));
            Groups.Sort((a, b) => a.name.CompareTo(b.name));

            return Groups;
        }
        

        /// <summary>
        /// Returns a list of available currencies
        /// </summary>
        /// <returns>LIst of tupels with currency id and name</returns>
        public IEnumerable<(string id, string name)> GetCurrencies()
        {
            List<(string id, string name)> Result = new();
            foreach (var c in Currencies)
                Result.Add((id: c.Id, name: c.Name));
            return Result;
        }

        /// <summary>
        /// Multiply silverthalers with this rate to get the value of the currency with the given id.
        /// </summary>
        /// <param name="currencyId">Currency id</param>
        /// <returns>An exchange rate</returns>
        public double GetExchangeRate(string currencyId)
        {
            return Currencies.Find(c => c.Id == currencyId).Rate;
        }



        /// <summary>
        /// Load the data
        /// </summary>
        public async Task InitializeGameDataAsync()
        {
            string Language = System.Globalization.CultureInfo.CurrentUICulture.Name;
            if (Language.StartsWith("de"))
                Language = "de";
            else
                Language = "en";


            string fileName = $"data/shop_{Language}.json";
            List<ShopItemM> inventoryM;
            inventoryM = await DataSource.GetFromJsonAsync<List<ShopItemM>>(fileName);

            Inventory = new();
            foreach (var item in inventoryM)
            {
                Inventory.Add(new ShopItemViMo(item) 
                { 
                    Group = l10n[item.Group.ToString()] // need to localise
                } );

            }

            fileName = $"data/currency_{Language}.json";
            Currencies = await DataSource.GetFromJsonAsync<List<CurrencyM>>(fileName);
        }

    }

}