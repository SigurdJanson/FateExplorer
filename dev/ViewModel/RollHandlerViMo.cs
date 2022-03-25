using FateExplorer.GameData;
using FateExplorer.RollLogic;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.ViewModel
{
    public class RollMappingViMo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("roll")]
        public string Roll { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }


    /// <summary>
    /// Factory to create all kinds of checks
    /// </summary>
    public class RollHandlerViMo : IRollHandlerViMo
    {
        #region INIT [][][][][][][][][][][][][][]

        protected HttpClient DataSource; // injected
        protected IGameDataService GameData { get; set; }  // injected



        private Dictionary<string, RollMappingViMo> rollMappings;

        [JsonPropertyName("Entries")]
        public Dictionary<string, RollMappingViMo> RollMappings
        {
            get
            {
                if (rollMappings is null)
                    throw new HttpRequestException("Data has not been loaded");
                return rollMappings;
            }
            set => rollMappings = value;
        }


        protected Dictionary<string, Type> ListOfChecks { get; set; }



        /// <inheritdoc/>
        public async Task ReadRollMappingsAsync()
        {
            string fileName = $"data/rollresolver.json";
            RollMappings = await DataSource.GetFromJsonAsync<Dictionary<string, RollMappingViMo>>(fileName);
        }


        /// <summary>
        /// Reads the mappings from a string. See also <seealso cref="ReadRollMappingsAsync"/>.
        /// </summary>
        /// <param name="jsonString">String in json format</param>
        /// <remarks>Primarily needed for unit tests</remarks>
        public void ReadRollMappings(string jsonString)
        {
            RollMappings = JsonSerializer.Deserialize<Dictionary<string, RollMappingViMo>>(jsonString);
        }



        /// <inheritdoc/>
        public void RegisterChecks()
        {
            ListOfChecks = new()
            {
                { AbilityCheckM.checkTypeId, typeof(AbilityCheckM) },
                { DodgeCheckM.checkTypeId, typeof(DodgeCheckM) },
                { SkillCheckM.checkTypeId, typeof(SkillCheckM) },
                { AttackCheckM.checkTypeId, typeof(AttackCheckM) },
                { ParryCheckM.checkTypeId, typeof(ParryCheckM) },
                { InitiativeCheckM.checkTypeId, typeof(InitiativeCheckM) }
            };
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RollHandlerViMo(HttpClient dataSource, IGameDataService gameData)
        {
            // injection
            DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            GameData = gameData ?? throw new ArgumentNullException(nameof(gameData));
        }

        #endregion



        /// <summary>
        /// Translate the id of a character attribute to the id required to identify
        /// the appropriate roll checker.
        /// </summary>
        /// <param name="AttributeId">An id of a character attribute like an ability 
        /// or skill like "TAL_3" or "SPELL_125".</param>
        /// <returns>A string (lika a path) that identifies the class to roll
        /// thus check</returns>
        private string MatchAttributeToRollId(string AttributeId)
        {
            // Exact match
            RollMappingViMo Result;
            if (RollMappings.TryGetValue(AttributeId, out Result))
                return Result.Roll;

            // Partial match (i.e. begins with)
            var Candidates = RollMappings.Keys.Where(p => AttributeId.StartsWith(p));
            if (Candidates.Any())
            {
                string FoundAttrId = Candidates.MaxBy(p => AttributeId.Fitness(AttributeId));
                if (RollMappings.TryGetValue(FoundAttrId, out Result))
                    return Result.Roll;
                else
                    return null;
            }
            else
            {
                return null;
            }
        }


        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public RollCheckResultViMo OpenRollCheck(string AttrId, ICharacterAttributDTO TargetAttr, ICharacterAttributDTO[] RollAttr = null)
        {
            string RollId = MatchAttributeToRollId(AttrId);
            if (string.IsNullOrWhiteSpace(RollId))
                throw new NotImplementedException($"A check for {TargetAttr.Name} has not yet been implemented");

            Type CheckType;
            if (!ListOfChecks.TryGetValue(RollId, out CheckType))
                throw new NotImplementedException($"A check for {TargetAttr.Name} has not yet been implemented");

            CheckBaseM Checker;
            RollCheckResultViMo Result;
            switch (CheckType.Name)
            {
                case
                    nameof(AbilityCheckM):
                    Checker = new AbilityCheckM((AbilityDTO)TargetAttr, new SimpleCheckModifierM(0), GameData);
                    break;
                case
                    nameof(SkillCheckM):
                    AbilityDTO[] abdto = Array.ConvertAll(RollAttr, new Converter<ICharacterAttributDTO, AbilityDTO>((a) => (AbilityDTO)a));
                    Checker = new SkillCheckM((SkillsDTO)TargetAttr, abdto, new SimpleCheckModifierM(0), GameData);
                    break;
                case
                    nameof(InitiativeCheckM):
                    Checker = new InitiativeCheckM((CharacterAttrDTO)TargetAttr, GameData);
                    break;
                default:
                    Checker = Activator.CreateInstance(CheckType, TargetAttr.EffectiveValue, 0) as CheckBaseM; //TODO: make this: throw new NotImplementedException();
                    break;
            };

            Result = new(Checker);
            return Result;
        }


        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public RollCheckResultViMo OpenDodgeRollCheck(string AttrId, CharacterAttrDTO TargetAttr, bool CarriesWeapon)
        {
            string RollId = MatchAttributeToRollId(AttrId);
            if (string.IsNullOrWhiteSpace(RollId))
                throw new NotImplementedException($"A check for {TargetAttr.Name} has not yet been implemented");

            Type CheckType;
            if (!ListOfChecks.TryGetValue(RollId, out CheckType))
                throw new NotImplementedException($"A check for {TargetAttr.Name} has not yet been implemented");

            CheckBaseM Checker;
            RollCheckResultViMo Result;
            switch (CheckType.Name)
            {
                case nameof(DodgeCheckM):
                    Checker = new DodgeCheckM(TargetAttr, CarriesWeapon, new SimpleCheckModifierM(0), GameData);
                    break;
                default:
                    throw new NotImplementedException();
            };

            Result = new(Checker);
            return Result;
        }


        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public RollCheckResultViMo OpenCombatRollCheck(string actionId, WeaponViMo weapon, HandsViMo Hands)
        {
            string TargetAttrName = $"{weapon.CombatTechId}/{actionId}";

            string RollId = MatchAttributeToRollId(TargetAttrName);
            if (string.IsNullOrWhiteSpace(RollId))
                throw new NotImplementedException($"A check for {TargetAttrName} has not yet been implemented");

            Type CheckType;
            if (!ListOfChecks.TryGetValue(RollId, out CheckType))
                throw new NotImplementedException($"A check for {TargetAttrName} has not yet been implemented");

            bool IsMainWeapon = Hands.MainWeapon == weapon;
            WeaponViMo OtherWeapon = IsMainWeapon ? Hands.OffWeapon : Hands.MainWeapon;

            CheckBaseM Checker;
            RollCheckResultViMo Result;
            switch (CheckType.Name)
            {
                case
                    nameof(AttackCheckM):
                    Checker = new AttackCheckM(weapon.ToWeaponM(), IsMainWeapon, OtherWeapon.ToWeaponM(),
                        new SimpleCheckModifierM(0), GameData);
                    break;
                case
                    nameof(ParryCheckM):
                    Checker = new ParryCheckM(weapon.ToWeaponM(), IsMainWeapon, OtherWeapon.ToWeaponM(),
                        new SimpleCheckModifierM(0), GameData);
                    break;
                default:
                    throw new NotImplementedException("Unknown combat roll");
            };

            Result = new(Checker);
            return Result;
        }


    }
}
