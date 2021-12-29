using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.WPA.GameLogic
{
    public class CombatTechDB
    {
        [JsonPropertyName("Entries")]
        public IReadOnlyList<CombatTechDbEntry> Data { get; set; }

        public CombatTechDbEntry this[int i] => Data[i];

        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }


    public class CombatTechDbEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("primeattrID")]
        public string PrimeAttrID { get; set; }

        [JsonPropertyName("attack")]
        public bool CanAttack { get; set; }

        [JsonPropertyName("parry")]
        public bool CanParry { get; set; }

        [JsonPropertyName("ranged")]
        public bool IsRanged { get; set; }
    }


}
