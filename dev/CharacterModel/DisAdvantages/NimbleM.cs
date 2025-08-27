using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// Contains the game logic for the advantage 'nimble' (VR1, p. 168) which "raises your hero's 
/// <c>Movement</c> by 1.
/// </summary>

[DisAdvantage(ADV.Nimble)]
public class NimbleM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public NimbleM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != ADV.Nimble)
            throw new ChrImportException("Given id is not nimble", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Movement.AddOnSetup(1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
