using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class SkillDbEntryBase : ICharacterAttribute
    {
        [JsonPropertyName("attrID")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("classID")]
        public int ClassId { get; set; }

        [JsonPropertyName("ab1")]
        public string Ab1 { get; set; }

        [JsonPropertyName("ab2")]
        public string Ab2 { get; set; }

        [JsonPropertyName("ab3")]
        public string Ab3 { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }


        // Extra properties

        [JsonIgnore]
        public virtual SkillDomain Domain { get; }
    }
}
