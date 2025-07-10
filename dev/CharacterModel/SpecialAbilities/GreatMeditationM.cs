using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.SpecialAbilities;

[SpecialAbility(SA.GreatMeditation)]
public class GreatMeditationM : TieredActivatableM
{
    public GreatMeditationM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != SA.GreatMeditation) 
            throw new ChrImportException("Given id is not great meditation", ChrImportException.Property.SpecialAbility);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        if (character.Energies.TryGetValue(ChrAttrId.AE, out CharacterEnergyM e))
        {
            e.Max += Tier * 6;
        }
    }

    /// <inheritdoc/>
    public override bool IsRecognized => true;
}
