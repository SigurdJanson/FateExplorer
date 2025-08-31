using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// Contains the game logic for the disadvantage 'decreased spirit' (VR1, p. 177) which "reduce[s] 
/// the hero’s Spirit base stat by 1.
/// </summary>
[DisAdvantage(DISADV.DecreasedSpirit)]
public class DecreasedSpiritM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public DecreasedSpiritM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != DISADV.DecreasedSpirit)
            throw new ChrImportException("Given id is not Decreased Spirit", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Resiliences[ChrAttrId.SPI].AddOnSetup(-1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
