using System.Text.Json.Serialization;

namespace FateExplorer.GameData;

public class CurrenciesDB : DataServiceCollectionBase<CurrencyDbEntry>
{
    // inherited
}



public class CurrencyDbEntry : ICharacterAttribute
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("origin")]
    public string Origin { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("rate")]
    public double Rate { get; set; }
}
