using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FateExplorer.WPA.GameData
{
    public class WeaponRangedDB : DataServiceCollectionBase<WeaponRangedDbEntry>
    {
        // inherited
    }



    public class WeaponRangedDbEntry : WeaponDbEntry
    {
        // Inherited properties 

        // New properties
        [JsonPropertyName("loadtime")]
        public string LoadTime { get; set; }

        [JsonPropertyName("ammo")]
        public string Ammo { get; set; }

    }
}
