using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    public class CombatTechDB : DataServiceCollectionBase<CombatTechDbEntry>
    {
        // inherited
    }


    public class CombatTechDbEntry : ICharacterAttribute
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("primeattrID")]
        public string PrimeAttrID { get; set; }

        [JsonPropertyName("attack")]
        public bool CanAttack { get; set; }

        [JsonPropertyName("parry")]
        public bool CanParry { get; set; }

        [JsonPropertyName("ranged")]
        public bool IsRanged { get; set; }

        [JsonPropertyName("branch")]
        public CombatTechniques WeaponsBranch { get; set; }

    }


}
