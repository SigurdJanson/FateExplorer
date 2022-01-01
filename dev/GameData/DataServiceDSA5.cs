using FateExplorer.GameLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FateExplorer.GameData
{
    public class DataServiceDSA5 : IGameDataService
    {
        protected HttpClient DataSource;


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



        #region Combat
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


        private WeaponMeleeDB weaponsMelee;
        public WeaponMeleeDB WeaponsMelee
        {
            get
            {
                if (weaponsMelee is null)
                    throw new HttpRequestException("Data has not been loaded");
                return weaponsMelee;
            }
            protected set => weaponsMelee = value;
        }


        private WeaponRangedDB weaponsRanged;
        public WeaponRangedDB WeaponsRanged
        {
            get
            {
                if (weaponsRanged is null)
                    throw new HttpRequestException("Data has not been loaded");
                return weaponsRanged;
            }
            protected set => weaponsRanged = value;
        }
        #endregion



        #region Skills
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


        private KarmaSkillsDB karmaSkills;
        public KarmaSkillsDB KarmaSkills
        {
            get
            {
                if (karmaSkills is null)
                    throw new HttpRequestException("Data has not been loaded");
                return karmaSkills;
            }
            protected set => karmaSkills = value;
        }
        #endregion


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

            // Combat
            fileName = "data/combattechs_de.json";
            CombatTechs = await DataSource.GetFromJsonAsync<CombatTechDB>(fileName);

            fileName = "data/weaponsmelee_de.json";
            WeaponsMelee = await DataSource.GetFromJsonAsync<WeaponMeleeDB>(fileName);

            fileName = "data/weaponsranged_de.json";
            WeaponsRanged = await DataSource.GetFromJsonAsync<WeaponRangedDB>(fileName);

            // Skills
            fileName = "data/skills_de.json";
            Skills = await DataSource.GetFromJsonAsync<SkillsDB>(fileName);

            fileName = "data/arcaneskills_de.json";
            ArcaneSkills = await DataSource.GetFromJsonAsync<ArcaneSkillsDB>(fileName);

            fileName = "data/karmaskills_de.json";
            KarmaSkills = await DataSource.GetFromJsonAsync<KarmaSkillsDB>(fileName);

            
        }
    }
}
