using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// Contains the game logic for the advantage 'Increased Toughness' (VR1, p. 166) which "improve[s] 
/// the hero’s Toughness base stat by 1.
/// </summary>
[DisAdvantage(ADV.IncreasedToughness)]
public class IncreasedToughnessM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public IncreasedToughnessM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != ADV.IncreasedToughness)
            throw new ChrImportException("Given id is not Increased Toughness", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Resiliences[ChrAttrId.TOU].AddOnSetup(1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
