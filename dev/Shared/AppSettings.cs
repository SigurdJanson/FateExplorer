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


        private string[] mostUsedSkills;
        public string[] MostUsedSkills
        {
            get 
            { 
                if (mostUsedSkills is null)
                    mostUsedSkills = Config.GetSection("FE:Skills:MostUsedSkills").Get<string[]>();
                return mostUsedSkills; 
            }
            set { mostUsedSkills = value; } 
        }


        public AppSettings(IConfiguration config)
        {
            Config = config;
        }
    }
}
