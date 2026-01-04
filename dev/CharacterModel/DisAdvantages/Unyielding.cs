using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// Contains the game logic for the advantage 'unyielding' (VR2, p. 132) which "increased the character's 
/// <c>WoundThreshold</c> by 1.
/// </summary>
[DisAdvantage(ADV.Unyielding)]
public class UnyieldingM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public UnyieldingM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != ADV.Unyielding)
            throw new ChrImportException("Given id is not unyielding", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.WoundThreshold.AddOnSetup(+1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
