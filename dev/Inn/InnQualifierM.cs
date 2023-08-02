using System.Text.Json.Serialization;

namespace FateExplorer.Inn;

public class InnQualifierM : InnNamesSharedM
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name"), JsonRequired]
    public string Name { get; set; }


    //public float Ql1 { get; set; } // inherited
    //public float Ql6 { get; set; } // inherited

    [JsonPropertyName("needprefix"), JsonRequired]
    public bool NeedPrefix { get; set; }

    [JsonPropertyName("needplural"), JsonRequired]
    public bool NeedPlural { get; set; }
}
