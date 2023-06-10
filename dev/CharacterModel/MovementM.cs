using FateExplorer.Shared;

namespace FateExplorer.CharacterModel;

/// <summary>
/// Represents the This is the characters’s tactical movement rate, which is especially important for combat.
/// </summary>
public class MovementM : IDerivedAttributeM
{

    public string[] DependentAttributes => throw new System.NotImplementedException();


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hero">The character of this dodge competence</param>
    public MovementM(int raceBaseVal, ICharacterM hero)
    {
        Value = ComputeMovement(raceBaseVal);
        if (hero.HasAdvantage(ADV.Nimble))
            Value += 1;
        if (hero.HasAdvantage(DISADV.Slow))
            Value -= 1;
    }

    /// <summary>
    /// Computes a valid movement value
    /// </summary>
    /// <returns>A movement value</returns>
    public static int ComputeMovement(int raceBaseVal)
        => raceBaseVal;

    /// <summary>
    /// The allowed minimum of the true/effective value
    /// </summary>
    public int Min { get; protected set; } = 0;

    /// <summary>
    /// The allowed maximum of the true/effective value
    /// </summary>
    public int Max { get; protected set; } = 20;

    /// <summary>
    /// The imported movement value
    /// </summary>
    public int Value { get; protected set; }

}
