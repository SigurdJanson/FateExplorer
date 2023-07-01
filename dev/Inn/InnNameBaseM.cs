using System.Text.Json.Serialization;
using System;

namespace FateExplorer.Inn;

public class InnNameBaseM : InnNamesSharedM
{
    private string _Plural; // field to suport the `Plural` property


    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Singular { get; set; }

    [JsonPropertyName("plural")]
    public string Plural 
    { 
        get
        {
            if (PluralIsSuffix)
                return $"{Singular}{_Plural[1..]}";
            else if (string.IsNullOrWhiteSpace(_Plural))
                return Singular;
            return _Plural;
        }
        set => _Plural = value;
    }

    [JsonPropertyName("prefix")]
    public string Prefix { get; set; }



    /// <summary>
    /// Tells if the plural form of this base name must be appended to the singular or if it is a whole word.
    /// </summary>
    public bool PluralIsSuffix => _Plural?.StartsWith('-') ?? false;
}
