using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace FateExplorer.Shop
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class PrimaryThresholdM
    {
        [JsonPropertyName("primary")]
        public List<string> Primary { get; set; }

        [JsonPropertyName("threshold")]
        public object Threshold { get; set; }
    }

    public class ShopItemM
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gr")]
        public int Gr { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("stp")]
        public object Stp { get; set; }

        [JsonPropertyName("weight")]
        public double? Weight { get; set; }

        [JsonPropertyName("at")]
        public int? At { get; set; }

        [JsonPropertyName("damageDiceNumber")]
        public int? DamageDiceNumber { get; set; }

        [JsonPropertyName("damageFlat")]
        public int? DamageFlat { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("pa")]
        public int? Pa { get; set; }

        [JsonPropertyName("combatTechnique")]
        public string CombatTechnique { get; set; }

        [JsonPropertyName("damageDiceSides")]
        public int? DamageDiceSides { get; set; }

        [JsonPropertyName("reach")]
        public int? Reach { get; set; }

        [JsonPropertyName("primaryThreshold")]
        public PrimaryThresholdM PrimaryThreshold { get; set; }

        [JsonPropertyName("reloadTime")]
        public object ReloadTime { get; set; }

        [JsonPropertyName("ammunition")]
        public string Ammunition { get; set; }

        [JsonPropertyName("range")]
        public List<int> Range { get; set; }

        [JsonPropertyName("enc")]
        public int? Enc { get; set; }

        [JsonPropertyName("pro")]
        public int? Pro { get; set; }

        [JsonPropertyName("armorType")]
        public int? ArmorType { get; set; }

        [JsonPropertyName("imp")]
        public int? Imp { get; set; }
    }

}
