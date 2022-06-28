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
        public enum GroupId { iMelee = 1, iRangedWeapon = 2, iAmmunition = 3, iArmor = 4, iWeaponAccessories = 5, 
            iClothing = 6, iTravelNTools = 7, iLights = 8, iBandagesNRemedies = 9, iContainers = 10, 
            iRopesNChains = 11, iBurglarTools = 12, iTools = 13, iOrientation = 14, iJewellery = 15, 
            iFinestoneNGems = 16, iStationary = 17, iBooks = 18, iMagicalArtefacts = 19,
            iAlchimica = 20, iPoisons = 21, iHerbs = 22, iMusicalInstruments = 23, iStimulantsNLuxury = 24, iAnimal = 25, 
            iAnimalSupplies = 26, iTransport = 27, iSacredTools = 28, iCeremonialObjects = 29 };


        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("gr")]
        public GroupId Group { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("template")]
        public string Template { get; set; }

        /// <summary>
        /// Structure points
        /// </summary>
        [JsonPropertyName("stp")]
        public int[] StruPo { get; set; }

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
        public int[] ReloadTime { get; set; }

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
