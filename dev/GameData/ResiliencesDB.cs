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
        public (string Id, int Value)[] RaceBaseValue { get; set; }
    }
}
