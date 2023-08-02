using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public enum AdvType { Advantage = 1, Disadvantage = -1 }


    public class DisAdvantagesDB : DataServiceCollectionBase<DisAdvantageDbEntry>
    {
    }


    public class DisAdvantageDbEntry : ICharacterAttribute
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public AdvType Type { get; set; }

        [JsonPropertyName("reco")]
        public bool Recognized{ get; set; }

    }
}
