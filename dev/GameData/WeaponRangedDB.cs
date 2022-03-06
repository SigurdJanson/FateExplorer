using System.Text.Json.Serialization;


namespace FateExplorer.GameData
{
    public class WeaponRangedDB : DataServiceCollectionBase<WeaponRangedDbEntry>
    {
        // inherited
    }



    public class WeaponRangedDbEntry : WeaponDbEntry
    {
        // Inherited properties 

        // New properties
        /// <summary>
        /// Time to load the ranged weapon in actions
        /// </summary>
        [JsonPropertyName("loadtime")]
        public int LoadTime { get; set; }

        [JsonPropertyName("ammo")]
        public string Ammo { get; set; }

        [JsonPropertyName("range")]
        public WeaponsRange Range { get; set; }
    }

    public class WeaponsRange
    {
        [JsonPropertyName("close")]
        public int Close { get; set; }

        [JsonPropertyName("medium")]
        public int Medium { get; set; }

        [JsonPropertyName("far")]
        public int Far { get; set; }

        public int[] ToArray() => new int[3] { Close, Medium, Far };
    }

}
