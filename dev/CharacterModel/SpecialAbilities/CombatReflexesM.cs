using FateExplorer.Shared;

namespace FateExplorer.CharacterModel.SpecialAbilities;

[SpecialAbility(SA.CombatReflexes)]
public class CombatReflexesM : TieredActivatableM
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Nicht verwendete Parameter entfernen", Justification = "Inheritance")]
    public CombatReflexesM(string id, int tier, string[] reference, bool recognized) : base(id, tier, reference)
    {
        if (id != SA.CombatReflexes)
            throw new ChrImportException("Given id is not combat reflexes", ChrImportException.Property.SpecialAbility);
    }

    /// <inheritdoc cref="IActivatableM.Apply(CharacterM)"/>
    public override void Apply(CharacterM character)
    {
        character.Initiative.AddOnSetup(Tier * 1);
    }

    /// <inheritdoc/>
    public override bool IsRecognized => true;
}

