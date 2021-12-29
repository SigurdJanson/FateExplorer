using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.WPA.GameData
{
    public class ArcaneSkillsDB
    {
        [JsonPropertyName("Entries")]
        public IReadOnlyList<ArcaneSkillDbEntry> Data { get; set; }

        public ArcaneSkillDbEntry this[int i] => Data[i];

        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }



    public class ArcaneSkillDbEntry : SkillDbEntryBase
    {
        [JsonPropertyName("modagainst")]
        public string ModAgainst { get; set; }

        [JsonPropertyName("category")] // TODO: most entries in file have not category
        public string Category { get; set; }
    }


}
