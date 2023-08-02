using FateExplorer.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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


        private SpecialAbilityDB specialAbilities;
        public SpecialAbilityDB SpecialAbilities
        {
            get
            {
                if (specialAbilities is null)
                    throw new HttpRequestException("Data has not been loaded");
                return specialAbilities;
            }
            protected set => specialAbilities = value;
        }


        private DisAdvantagesDB disAdvantages;
        public DisAdvantagesDB DisAdvantages
        {
            get
            {
                if (disAdvantages is null)
                    throw new HttpRequestException("Data has not been loaded");
                return disAdvantages;
            }
            protected set => disAdvantages = value;
        }


        private BotchDB botches;
        public BotchDB Botches // not part of interface IGameDataService but access through `Get..Botch()`
        {
            get
            {
                if (botches is null)
                    throw new HttpRequestException("Data has not been loaded");
                return botches;
            }
            protected set => botches = value;
        }

        public BotchEntry GetSkillBotch(Check.Skill domain, int DiceEyes)
            => Botches.GetBotch("Skill", domain.ToString(), DiceEyes);

        public BotchEntry GetAttackBotch(CombatBranch technique, int DiceEyes)
            => Botches.GetBotch("Attack", technique.ToString(), DiceEyes);

        public BotchEntry GetParryBotch(CombatBranch technique, int DiceEyes)
            => Botches.GetBotch("Parry", technique.ToString(), DiceEyes);

        public BotchEntry GetDodgeBotch(CombatBranch technique, int DiceEyes)
            => Botches.GetBotch("Dodge", technique.ToString(), DiceEyes);


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


        /// <summary>
        /// Check if the item with the given id is a weapon.
        /// </summary>
        /// <param name="TemplateId">Template id in the data base</param>
        /// <returns>true/false</returns>
        public bool IsWeapon(string TemplateId)
            => WeaponsMelee.Data.FirstOrDefault(w => w.TemplateID == TemplateId) != default 
                || WeaponsRanged.Data.FirstOrDefault(w => w.TemplateID == TemplateId) != default;



        private Task<WeaponMeleeDB> WeaponMeleeDBPromise = null;
        private WeaponMeleeDB weaponsMelee;
        public WeaponMeleeDB WeaponsMelee
        {
            get
            {
                if (WeaponMeleeDBPromise != null) // data has not been fetched from the promise
                {
                    if (!WeaponMeleeDBPromise.IsCompleted) // if promise is ready, fetch immediately
                        if (!WeaponMeleeDBPromise.Wait(TimeSpan.FromSeconds(1))) // if not, wait a second
                            throw new HttpRequestException("Data has not been loaded");
                    weaponsMelee = WeaponMeleeDBPromise.Result;
                    WeaponMeleeDBPromise = null; // feed it to the GC
                }

                if (weaponsMelee is null)
                    throw new HttpRequestException("Data has not been loaded");
                return weaponsMelee;
            }
            protected set => weaponsMelee = value;
        }


        private Task<WeaponRangedDB> WeaponRangedDBPromise = null;
        private WeaponRangedDB weaponsRanged;
        public WeaponRangedDB WeaponsRanged
        {
            get
            {
                if (WeaponRangedDBPromise != null) // data has not been fetched from the promise
                {
                    if (!WeaponRangedDBPromise.IsCompleted) // if promise is ready, fetch immediately
                        if (!WeaponRangedDBPromise.Wait(TimeSpan.FromSeconds(1))) // if not, wait a second
                            throw new HttpRequestException("Data has not been loaded");
                    weaponsRanged = WeaponRangedDBPromise.Result;
                    WeaponRangedDBPromise = null; // feed it to the GC
                }

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


        public ResiliencesDB resiliences;
        public ResiliencesDB Resiliences
        {
            get
            {
                if (resiliences is null)
                    throw new HttpRequestException("Data has not been loaded");
                return resiliences;
            }
            protected set => resiliences = value;
        }


        private EnergiesDB energies;
        public EnergiesDB Energies
        {
            get
            {
                if (energies is null)
                    throw new HttpRequestException("Data has not been loaded");
                return energies;
            }
            protected set => energies = value;
        }

        #endregion



        #region Things

        private CurrenciesDB currencies;
        public CurrenciesDB Currencies
        {
            get
            {
                if (currencies is null)
                    throw new HttpRequestException("Data has not been loaded");
                return currencies;
            }
            protected set => currencies = value;
        }


        private CalendarDB calendar;
        public CalendarDB Calendar
        {
            get
            {
                if (calendar is null)
                    throw new HttpRequestException("Data has not been loaded");
                return calendar;
            }
            protected set => calendar = value;
        }

        #endregion



        private PraiseOrInsultDB praiseOrInsult;
        public PraiseOrInsultDB PraiseOrInsult
        {
            get
            {
                if (praiseOrInsult is null)
                    throw new HttpRequestException("Data has not been loaded");
                return praiseOrInsult;
            }
            protected set => praiseOrInsult = value;
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
        /// Determines which language should be used.
        /// </summary>
        /// <returns>An ISO-639-1 language code. If the requested app language is not supported it returns English ("en")</returns>
        protected string GetAppLanguageCode()
        {
            string Language = System.Globalization.CultureInfo.CurrentUICulture.Name;
            if (Language.StartsWith("de"))
                Language = "de";
            else
                Language = "en";
            return Language;
        }


        /// <summary>
        /// Load the data
        /// </summary>
        /// <returns></returns>
        public async Task InitializeGameDataAsync()
        {
            string Language = GetAppLanguageCode();

            string fileName = $"data/attributes_{Language}.json";
            Abilities = await DataSource.GetFromJsonAsync<AbilitiesDB>(fileName);

            fileName = $"data/specabs_{Language}.json";
            Task<SpecialAbilityDB> SpecialAbilityTask = DataSource.GetFromJsonAsync<SpecialAbilityDB>(fileName);

            fileName = $"data/dis-advantages_{Language}.json";
            Task<DisAdvantagesDB> DisadvantagesTask = DataSource.GetFromJsonAsync<DisAdvantagesDB>(fileName);

            fileName = $"data/resiliences_{Language}.json";
            Resiliences = await DataSource.GetFromJsonAsync<ResiliencesDB>(fileName);

            fileName = $"data/energies_{Language}.json";
            Energies = await DataSource.GetFromJsonAsync<EnergiesDB>(fileName);

            fileName = $"data/botches_{Language}.json";
            Botches = await DataSource.GetFromJsonAsync<BotchDB>(fileName);


            // Combat
            fileName = $"data/combattechs_{Language}.json";
            CombatTechs = await DataSource.GetFromJsonAsync<CombatTechDB>(fileName);

            fileName = $"data/weaponsmelee_{Language}.json";
            //--Task<WeaponMeleeDB> WeaponMeleeTask =
            WeaponMeleeDBPromise = DataSource.GetFromJsonAsync<WeaponMeleeDB>(fileName);

            fileName = $"data/weaponsranged_{Language}.json";
            //--Task<WeaponRangedDB> WeaponRangedTask
            WeaponRangedDBPromise = DataSource.GetFromJsonAsync<WeaponRangedDB>(fileName);


            // Skills
            fileName = $"data/arcaneskills_{Language}.json";
            Task<ArcaneSkillsDB> ArcaneSkillsTask = DataSource.GetFromJsonAsync<ArcaneSkillsDB>(fileName);

            fileName = $"data/karmaskills_{Language}.json";
            Task<KarmaSkillsDB> KarmaSkillsTask = DataSource.GetFromJsonAsync<KarmaSkillsDB>(fileName);

            fileName = $"data/skills_{Language}.json";
            Skills = await DataSource.GetFromJsonAsync<SkillsDB>(fileName);

            // Things
            fileName = $"data/currency_{Language}.json";
            currencies = await DataSource.GetFromJsonAsync<CurrenciesDB>(fileName);

            fileName = $"data/calendar_{Language}.json";
            calendar = await DataSource.GetFromJsonAsync<CalendarDB>(fileName);

            //
            fileName = $"data/praise_{Language}.json";
            praiseOrInsult = await DataSource.GetFromJsonAsync<PraiseOrInsultDB>(fileName);


            // Wrap up - only the larger files
            SpecialAbilities = await SpecialAbilityTask;
            DisAdvantages = await DisadvantagesTask;
            //--WeaponsRanged = await WeaponRangedTask;
            //--WeaponsMelee = await WeaponMeleeTask;
            ArcaneSkills = await ArcaneSkillsTask;
            KarmaSkills = await KarmaSkillsTask;
        }
    }
}
