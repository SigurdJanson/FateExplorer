using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// Contains the game logic for the disadvantage 'slow' (VR1, p. 177) which "reduces your hero's 
/// <c>Movement</c> by 1.
/// </summary>
[DisAdvantage(DISADV.Slow)]
public class SlowM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public SlowM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != DISADV.Slow)
            throw new ChrImportException("Given id is not slow", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Movement.AddOnSetup(-1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
