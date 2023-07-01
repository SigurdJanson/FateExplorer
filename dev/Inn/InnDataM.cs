using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.Inn;




public class InnDataM
{
    /* 
     * Aventurian Dishes
     */
    [JsonPropertyName("unit"), JsonRequired]
    public string[] Unit { get; set; }

    [JsonPropertyName("categories"), JsonRequired]
    public Dictionary<int, string> Category { get; set; }

    [JsonPropertyName("dish"), JsonRequired]
    public List<InnDishM> Dish { get; set; }

    /* 
     * Inn Names
     */
    [JsonPropertyName("inns"), JsonRequired]
    public InnNameRecord InnNames { get; set; }

    public record InnNameRecord
    {
        [JsonPropertyName("qualifier"), JsonRequired]
        public InnQualifierM[] Qualifier { get; set; }

        [JsonPropertyName("namebase"), JsonRequired]
        public InnNameBaseM[] NameBase { get; set; }

        [JsonPropertyName("name"), JsonRequired]
        public InnNameM[] FullName { get; set; }
    }

    public InnQualifierM[] Qualifier => InnNames.Qualifier;
    public InnNameBaseM[] NameBase => InnNames.NameBase;
    public InnNameM[] FullName => InnNames.FullName;
}
