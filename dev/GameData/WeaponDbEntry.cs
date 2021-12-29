using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.WPA.GameData
{
    public class WeaponDbEntry
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

        [JsonPropertyName("range")]
        public int Range { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

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

        [JsonPropertyName("templateID")]
        public string TemplateID { get; set; }
    }
}
