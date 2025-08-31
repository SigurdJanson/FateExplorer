using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.DisAdvantages;


/// <summary>
/// Contains the game logic for the disadvantage 'increased spirit' (VR1, p. 166) which "improve[s] 
/// the hero's Spirit base stat by 1."
/// </summary>
[DisAdvantage(ADV.IncreasedSpirit)]
public class IncreasedSpiritM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public IncreasedSpiritM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != ADV.IncreasedSpirit)
            throw new ChrImportException("Given id is not Increased Spirit", ChrImportException.Property.DisAdvantage);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Resiliences[ChrAttrId.SPI].AddOnSetup(1);
    }


    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
