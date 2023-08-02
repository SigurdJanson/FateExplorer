using System.Text.Json.Serialization;

namespace FateExplorer.GameData;

public class PraiseOrInsultDB
{

    [JsonPropertyName("Fate")]
    public string[] FateQuote { get; init; }

    [JsonPropertyName("Praise")]
    public PraiseOrInsultEntry[] Praise { get; init; }

    [JsonPropertyName("Insult")]
    public PraiseOrInsultEntry[] Insult { get; init; }

    [JsonPropertyName("PraisePhrase")]
    public string[] PraisePhrase { get; init; }

    [JsonPropertyName("Positives")]
    public string[] PositiveAdjective { get; init; }
}


public class PraiseOrInsultEntry
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("scope")]
    public int Scope { get; set; }
}

