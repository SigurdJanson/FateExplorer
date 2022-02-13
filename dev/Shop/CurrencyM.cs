using System.Text.Json.Serialization;

namespace FateExplorer.Shop
{
    public class CurrencyM
    {
        [JsonConstructor]
        public CurrencyM(
            string id,
            string origin,
            string name,
             double rate
        )
        {
            this.Id = id;
            this.Origin = origin;
            this.Name = name;
            this.Rate = rate;
        }

        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("origin")]
        public string Origin { get; }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("rate")]
        public double Rate { get; }
    }
}