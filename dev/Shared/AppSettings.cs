using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FateExplorer.Shared
{
    public class AppSettings
    {
        readonly IConfiguration Config;

        private bool? showImprovisedWeapons;
        public bool ShowImprovisedWeapons
        {
            get 
            {   
                if (!showImprovisedWeapons.HasValue)
                    showImprovisedWeapons = Config.GetValue<bool>("FE:Weapons:ShowImprovisedWeapons");
                return showImprovisedWeapons.Value;
            }
            set { showImprovisedWeapons = value; }
        }


        private List<string> mostUsedSkills;
        public List<string> MostUsedSkills
        {
            get 
            { 
                if (mostUsedSkills is null)
                    mostUsedSkills = Config.GetSection("FE:Skills:MostUsedSkills").Get<List<string>>();
                return mostUsedSkills; 
            }
            set { mostUsedSkills = value; } 
        }


        private string defaultCurrency;
        /// <summary>
        /// Get the id of the default currency
        /// </summary>
        public string DefaultCurrency
        {
            get
            {
                if (defaultCurrency is null)
                    defaultCurrency = Config.GetValue<string>("FE:DefaultCurrency");
                return defaultCurrency;
            }
            set { defaultCurrency = value; }
        }



        public AppSettings(IConfiguration config)
        {
            Config = config;
        }
    }
}
