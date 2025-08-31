using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;

/// <summary>
/// Contains the game logic for the disadvantage 'Decreased Toughness' (VR1, p. 172) which "reduce[s] 
/// the hero's Toughness base stat by 1.
/// </summary>
[DisAdvantage(DISADV.DecreasedToughness)]
public class DecreasedToughnessM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public DecreasedToughnessM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != DISADV.DecreasedToughness)
            throw new ChrImportException("Given id is not Decreased Toughness", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Resiliences[ChrAttrId.TOU].AddOnSetup(-1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
