using System;
using System.Text.Json.Serialization;

namespace FateExplorer.Inn;

public class InnNameM
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("region")]
    public Region[] Region { get; set; }

    [JsonPropertyName("ql1")]
    public float Ql1 { get; set; }

    [JsonPropertyName("ql6")]
    public float Ql6 { get; set; }


    /// <summary>
    /// Can you find an inn with the name of this instance in the region <c>Where</c>?
    /// </summary>
    /// <param name="Where">A region where you are looking for an inn.</param>
    /// <returns>true/false</returns>
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


    /// <summary>
    /// Returns the how likely the inn name can be found for a location with the given quality level.
    /// </summary>
    /// <param name="ql">A quality level ranging from 1-6.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float GetProbability(int ql)
    {
        if (Ql1 == 0 && Ql6 == 0) return 1.0f;
        return ql switch
        {
            1 => Ql1,
            2 => Ql1 + (Ql6 - Ql1) / 6 * 2,
            3 => Ql1 + (Ql6 - Ql1) / 6 * 3,
            4 => Ql1 + (Ql6 - Ql1) / 6 * 4,
            5 => Ql1 + (Ql6 - Ql1) / 6 * 5,
            6 => Ql6,
            _ => throw new ArgumentOutOfRangeException(nameof(ql), "Allowed quality levels range from 1 to 6")
        };
    }
}
