using FateExplorer.Shared;
using System;
using System.Text.Json.Serialization;
using Aventuria;

namespace FateExplorer.Inn;


public enum Food
{
    NonAlcoholic = 1,
    Alcoholic = 50,
    BreadDish = 100,
    MainDish = 200,
    Dessert = 300,
    Salad = 400
}


public class InnDishM
{
    //{"offer":"Wasser","unit":1,"price":0,"category":1,"region":206,"ql1":0.5,"ql3":0.75,"ql4":1,"ql6":1}
    [JsonPropertyName("offer")]
    public string Name { get; set; }

    [JsonPropertyName("unit")]
    public DishUnit Unit { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("category")]
    public int Category { get; set; }

    [JsonPropertyName("region")]
    public Region[] Region { get; set; }


    [JsonPropertyName("ql1")]
    public float QL1Probability { get; set; }

    [JsonPropertyName("ql3")]
    public float QL3Probability { get; set; }

    [JsonPropertyName("ql4")]
    public float QL4Probability { get; set; }

    [JsonPropertyName("ql6")]
    public float QL6Probability { get; set; }


    /// <inheritdoc cref="InnNameM.GetProbability(int)"/>
    public float GetProbability(QualityLevel ql)
    {
        if (QL1Probability == 0 && QL6Probability == 0) return 1.0f;
        return ql switch
        {
            QualityLevel.Lowest => QL1Probability,
            QualityLevel.Low    => (QL1Probability + QL3Probability) / 2,
            QualityLevel.Normal => QL3Probability,
            QualityLevel.Good   => QL4Probability,
            QualityLevel.Excellent => (QL4Probability + QL6Probability) / 2,
            QualityLevel.Luxurious => QL6Probability,
            _ => throw new ArgumentOutOfRangeException(nameof(ql), "Allowed quality levels range from 1 to 6")
        };
    }


    /// <inheritdoc cref="InnNameM.CanBeFound(Region)"/>
    public bool CanBeFound(Region Where)
    {
        if (Where == 0) return true;
        if (Region is null || Region.Length == 0) return true; // not restricted to any region
        foreach (Region r in Region)
        {
            if (r == Where) return true;
        }
        return false;
    }

    public bool Is(Food food)
    {
        bool result = food switch
        {
            Food.Salad => Category >= (int)Food.Salad,
            Food.Dessert => Category >= (int)Food.Dessert && Category < (int)Food.Salad,
            Food.MainDish => Category >= (int)Food.MainDish && Category < (int)Food.Dessert,
            Food.BreadDish => Category >= (int)Food.BreadDish && Category < (int)Food.MainDish,
            Food.Alcoholic => Category >= (int)Food.Alcoholic && Category < (int)Food.BreadDish,
            Food.NonAlcoholic => Category >= (int)Food.NonAlcoholic && Category < (int)Food.Alcoholic,
            _ => throw new ArgumentException("Unknown type of food", nameof(food))
        };
        return result;
    }
}
