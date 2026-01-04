namespace FateExplorer.CharacterModel;

/// <summary>
/// This is the characters’s tactical movement rate, which is especially important for combat.
/// </summary>
/// <remarks>Movement can directly be derived from <see cref="CharacterIstic"/>. 
/// It is neither an atomic <see cref="RootValue"/> nor does it have any dependencies to other 
/// attributes except the base value of the character's species.</remarks>
public class MovementM : CharacterIstic
{

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="raceBaseVal">The base value of the character's race as basis for the movement value.</param>
    /// <param name="hero">The character of this movement competence.</param>
    public MovementM(int raceBaseVal, ICharacterM hero) : base(ComputeMovement(raceBaseVal))
    {
        Min = 0;
        Max = 20;
    }

    /// <summary>
    /// Computes a valid movement value
    /// </summary>
    /// <returns>A movement value</returns>
    public static int ComputeMovement(int raceBaseVal)
        => raceBaseVal;

}
