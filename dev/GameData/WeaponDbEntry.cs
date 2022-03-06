using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class WeaponDbEntry : ICharacterAttribute
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("threshold")]
        public int Threshold { get; set; }

        [JsonPropertyName("damage")]
        public string Damage { get; set; }

        public int DamageDieCount() => int.Parse(Damage.Split('W')[0]);
        public int DamageDieSides() => int.Parse(Damage.Split('W')[1]);


        /// <summary>
        /// Constant added to the hit points done by the weapon (i.e. the 3 in 1W6+3).
        /// </summary>
        [JsonPropertyName("bonus")]
        public int Bonus { get; set; }

        /// <summary>
        /// Attack modifier added by the weapon
        /// </summary>
        [JsonPropertyName("at")]
        public int At { get; set; }

        /// <summary>
        /// Parry modifier added by the weapon
        /// </summary>
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
