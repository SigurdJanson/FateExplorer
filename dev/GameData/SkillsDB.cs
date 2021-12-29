using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.WPA.GameData
{
    public class SkillsDB
    {
        [JsonPropertyName("Entries")]
        public IReadOnlyList<SkillDbEntry> Data { get; set; }

        public SkillDbEntry this[int i] => Data[i];

        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }



    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class SkillDbEntry : SkillDbEntryBase
    {
        // Inherited properties
        // TODO: no URLs in file
    }


}
