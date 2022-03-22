using Microsoft.Extensions.Configuration;

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




        public AppSettings(IConfiguration config)
        {
            Config = config;
        }
    }
}
