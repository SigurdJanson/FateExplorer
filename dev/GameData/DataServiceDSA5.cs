using FateExplorer.WPA.GameLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FateExplorer.WPA.GameData
{
    public class DataServiceDSA5 : IGameDataService
    {
        HttpClient DataSource;

        private AbilitiesDB abilities;
        public AbilitiesDB Abilities
        { 
            get
            {
                if (abilities is null)
                    throw new HttpRequestException("Data has not been loaded");
                return abilities;
            }
            protected set => abilities = value;
        }


        private CombatTechDB combatTechs;
        public CombatTechDB CombatTechs
        {
            get
            {
                if (combatTechs is null)
                    throw new HttpRequestException("Data has not been loaded");
                return combatTechs;
            }
            protected set => combatTechs = value;
        }


        private SkillsDB skills;
        public SkillsDB Skills
        {
            get
            {
                if (skills is null)
                    throw new HttpRequestException("Data has not been loaded");
                return skills;
            }
            protected set => skills = value;
        }


        private ArcaneSkillsDB arcaneSkills;
        public ArcaneSkillsDB ArcaneSkills
        {
            get
            {
                if (arcaneSkills is null)
                    throw new HttpRequestException("Data has not been loaded");
                return arcaneSkills;
            }
            protected set => arcaneSkills = value;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource">Injection of HttpClient</param>
        public DataServiceDSA5(HttpClient dataSource)
        {
            DataSource = dataSource;
        }


        /// <summary>
        /// Load the data
        /// </summary>
        /// <returns></returns>
        public async Task InitializeGameDataAsync()
        {
            string fileName = "data/attributes_de.json";
            Abilities = await DataSource.GetFromJsonAsync<AbilitiesDB>(fileName);
            
            fileName = "data/combattechs_de.json";
            CombatTechs = await DataSource.GetFromJsonAsync<CombatTechDB>(fileName);

            fileName = "data/skills_de.json";
            Skills = await DataSource.GetFromJsonAsync<SkillsDB>(fileName);

            fileName = "data/arcaneskills_de.json";
            ArcaneSkills = await DataSource.GetFromJsonAsync<ArcaneSkillsDB>(fileName);
        }
    }
}
