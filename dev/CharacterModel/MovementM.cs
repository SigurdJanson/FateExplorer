using FateExplorer.Shared;

namespace FateExplorer.CharacterModel;

/// <summary>
/// Represents the This is the characters’s tactical movement rate, which is especially important for combat.
/// </summary>
public class MovementM : CharacterIstic
{

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="raceBaseVal">The base value of the character's race as basis for the movement value.</param>
    /// <param name="hero">The character of this dodge competence.</param>
    public MovementM(int raceBaseVal, ICharacterM hero) : base(ComputeMovement(raceBaseVal))
    {
        //--Value = ComputeMovement(raceBaseVal);
        if (hero.HasAdvantage(ADV.Nimble))
            Imported += 1;
        if (hero.HasAdvantage(DISADV.Slow))
            Imported -= 1;
        _effective = Imported;
        _true = Imported;
        Min = 0;
        Max = 20;
    }

    /// <summary>
    /// Computes a valid movement value
    /// </summary>
    /// <returns>A movement value</returns>
    public static int ComputeMovement(int raceBaseVal)
        => raceBaseVal;


    ///// <summary>
    ///// The imported movement value
    ///// </summary>
    //public int Value { get; protected set; }

}
