using System.Text.Json.Serialization;
using System;
using FateExplorer.Shared;

namespace FateExplorer.Inn;

public abstract class InnNamesSharedM
{

    [JsonPropertyName("ql1")]
    public float Ql1 { get; set; }

    [JsonPropertyName("ql6")]
    public float Ql6 { get; set; }


    /// <summary>
    /// Returns the how likely the inn name can be found for a location with the given quality level.
    /// </summary>
    /// <param name="quality">A quality level ranging from 1-6.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float GetProbability(QualityLevel quality)
    {
        if (Ql1 == 0 && Ql6 == 0) return 1.0f;
        float Step = (Ql6 - Ql1) / 5;
        return quality switch
        {
            QualityLevel.Lowest => Ql1,
            QualityLevel.Low => Ql1 + Step,
            QualityLevel.Normal => Ql1 + Step * 2,
            QualityLevel.Good => Ql1 + Step * 3,
            QualityLevel.Excellent => Ql1 + Step * 4,
            QualityLevel.Luxurious => Ql6,
            _ => throw new ArgumentOutOfRangeException(nameof(quality), "Allowed quality levels range from 1 to 6")
        };
    }

}
