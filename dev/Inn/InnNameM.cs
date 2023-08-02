using FateExplorer.Shared;
using System;
using System.Text.Json.Serialization;

namespace FateExplorer.Inn;

public class InnNameM : InnNamesSharedM
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("region")]
    public Region[] Region { get; set; }

    //public float Ql1 { get; set; }

    //public float Ql6 { get; set; }


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
}
