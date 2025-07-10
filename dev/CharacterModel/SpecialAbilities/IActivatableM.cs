
namespace FateExplorer.CharacterModel;

/// <summary>
/// An interface to represent advantages, disadvantages, and special abilities.
/// </summary>
public interface IActivatableM
{
    /// <summary>
    /// A string id
    /// </summary>
    string Id { get; }

    /// <summary>
    /// A tier (i.e. a level) of the special ability. 0 if it is ignored i.e. the
    /// special ability has no levels.
    /// </summary>
    int Tier { get; }


    /// <summary>
    /// Determines whether the special ability is known by fate explorer
    /// and included in computations.
    /// </summary>
    bool IsRecognized { get; }

    /// <summary>
    /// Any skill the special ability might be linked to.<br/>
    /// <example>
    /// E.g. the reference for Hruruzat is "CT_9" because it is a special brawling ability.
    /// </example>
    /// </summary>
    string[] Reference { get; }

    /// <summary>
    /// Used to initialize effects of activatables on values, like "Great Meditation" that 
    /// increases astral energy or many special abiliities that add applications to skills.
    /// </summary>
    /// <param name="character">The character to be modified</param>
    void Apply(CharacterM character);
}
