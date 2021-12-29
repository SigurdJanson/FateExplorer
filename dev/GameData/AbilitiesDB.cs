using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace FateExplorer.WPA.GameLogic
{

    public class AbilitiesDB
    {
        [JsonPropertyName("Entries")]
        public IReadOnlyList<AbilityDbEntry> Data { get; set; }

        public AbilityDbEntry this[int i] => Data[i];

        [JsonIgnore]
        public int Count { get => Data?.Count ?? 0; }
    }


    public class AbilityDbEntry
    {
        [JsonPropertyName("attrID")]
        public string AttrID { get; set; }

        [JsonPropertyName("shortname")]
        public string ShortName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }


}
