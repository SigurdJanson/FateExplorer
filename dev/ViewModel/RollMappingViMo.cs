using System.Text.Json.Serialization;

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
}
