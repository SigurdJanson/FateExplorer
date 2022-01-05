using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{

    public class EnergiesDB : DataServiceCollectionBase<EnergiesDbEntry>
    {
        // inherited
    }

    public class EnergiesDbEntry : ICharacterAttribute
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
        public EnergyRaceBaseValue[] RaceBaseValue { get; set; }

        [JsonPropertyName("advmod")]
        public DisAdvantageBaseValue[] DisAdvBaseValue { get; set; }
    }


    public class EnergyRaceBaseValue
    {
        [JsonPropertyName("id")]
        public string RaceId { get; set; }

        [JsonPropertyName("base")]
        public int Value { get; set; }
    }


    public class DisAdvantageBaseValue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("tier")]
        public string Tier { get; set; }

        [JsonPropertyName("base")]
        public int Value { get; set; }
    }
}
