namespace FateExplorer.CharacterModel;


/// <summary>
/// Represents the derived attribute "Wound Threshold"
/// </summary>
public class WoundThresholdM : DerivedValue
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hero">The character of this dodge competence</param>
    public WoundThresholdM(ICharacterM hero) : base(ComputeValue(hero.GetAbility(AbilityM.CON)))
    {
        //--Hero = hero;
        Min = 0;
        Max = 12;
        DependencyId = [AbilityM.CON];
    }

    /// <summary>
    /// Computes a valid wound threshold value from the given attributes
    /// </summary>
    /// <param name="EffectiveConstitution">The constitution of the character</param>
    /// <returns>An effective initiative value</returns>
    public static int ComputeValue(int EffectiveConstitution)
        => (EffectiveConstitution + 1) / 2;


    /// <inheritdoc/>
    protected override void UpdateOnDependencyChange(string attrId, int newValue)
    {
        if (attrId != AbilityM.CON) return;
        Effective = ComputeValue(newValue);
    }

}