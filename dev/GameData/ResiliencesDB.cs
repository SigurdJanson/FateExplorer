using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class ResiliencesDB : DataServiceCollectionBase<ResilienceDbEntry>
    {
        // inherited
    }

    public class ResilienceDbEntry : ICharacterAttribute
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("shortname")]
        public string ShortName { get; set; }

        [JsonPropertyName("abilities")]
        public string[] DependantAbilities { get; set; }

        [JsonPropertyName("racebase")]
        public ResilienceBaseValue[] RaceBaseValue { get; set; }
    }


    public class ResilienceBaseValue
    {
        [JsonPropertyName("id")]
        public string RaceId { get; set; }

        [JsonPropertyName("base")]
        public int Value { get; set; }
    }
}
