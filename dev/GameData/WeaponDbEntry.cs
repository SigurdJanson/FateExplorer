using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class WeaponDbEntry : ICharacterAttribute
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("combattech")]
        public string CombatTech { get; set; }

        [JsonPropertyName("primeattr")]
        public string PrimeAttr { get; set; }

        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }

        [JsonPropertyName("damage")]
        public string Damage { get; set; }

        [JsonPropertyName("bonus")]
        public int Bonus { get; set; }

        [JsonPropertyName("at")]
        public int At { get; set; }

        [JsonPropertyName("pa")]
        public int Pa { get; set; }

        /// <summary>
        /// Weight of weapon in stone
        /// </summary>
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("sf")]
        public int Sf { get; set; }

        [JsonPropertyName("primeattrID")]
        public string PrimeAttrID { get; set; }

        [JsonPropertyName("combattechID")]
        public string CombatTechID { get; set; }

        [JsonPropertyName("improvised")]
        public bool Improvised { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("clsrng")]
        public bool CloseRange { get; set; }

        [JsonPropertyName("armed")]
        public bool Armed { get; set; }

        [JsonPropertyName("twohanded")]
        public bool TwoHanded { get; set; }

        [JsonPropertyName("templateID")]
        public string TemplateID { get; set; }

        [JsonIgnore]
        public string Id { get => TemplateID; set => TemplateID = value; }
    }
}
