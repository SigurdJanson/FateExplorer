using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FateExplorer.RollLogic;
using System.Text.Json;

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
                { SkillCheckM.checkTypeId, typeof(SkillCheckM) }
            };
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RollHandlerViMo(HttpClient dataSource)
        {
            // injection
            DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        #endregion



        /// <summary>
        /// Translate the id of a character attribute to the id required to identify
        /// the appropriate roll checker.
        /// </summary>
        /// <param name="AttributeId">An id of a character attribute like an ability 
        /// or skill like "TAL_3" or "SPELL_125".</param>
        /// <returns></returns>
        private string MatchAttributeToRollId(string AttributeId)
        {
            // Exact match
            RollMappingViMo Result;
            if (RollMappings.TryGetValue(AttributeId, out Result))
                return Result.Id;

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
        public CheckBaseM OpenRollCheck(string AttrId, ICharacterAttributDTO AttrData)
        {
            string RollId = MatchAttributeToRollId(AttrId);
            if (string.IsNullOrWhiteSpace(RollId))
                throw new NotImplementedException($"A check for {AttrId} has not yet been implemented");

            //-var CheckType = ListOfChecks[RollId];
            CheckBaseM Result;
            switch (ListOfChecks[RollId].ToString())
            {
                case
                    nameof(AbilityCheckM):
                    Result = new AbilityCheckM(AttrData.EffectiveValue, 0);
                    break;
                case
                    nameof(SkillCheckM):
                    Result = new SkillCheckM(AttrData.Id);
                    break;
                default: 
                    Result = Activator.CreateInstance(ListOfChecks[RollId], AttrData.EffectiveValue, 0) as CheckBaseM; 
                    break;

            };
            //-var Result = Activator.CreateInstance(ListOfChecks[RollId], AttrData.EffectiveValue, 0) as CheckBaseM;

            return Result;
        }


        /// <inheritdoc />
        public CheckBaseM OpenRollCheck(string AttrId, ICharacterAttributDTO AttrData, ICheckModifierM Modifier)
        {
            var Result = OpenRollCheck(AttrId, AttrData);
            Result.CheckModifier = Modifier;
            Result.NextStep();
            return Result;
        }


        /// <inheritdoc />
        public void NextRoll(CheckBaseM rollSeries)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc />
        public void FinishCheck(CheckBaseM rollSeries)
        {
            throw new NotImplementedException();
        }

    }
}
