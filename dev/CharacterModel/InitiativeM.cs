namespace FateExplorer.CharacterModel;

public class InitiativeM : DerivedValue
{
    private int CachedCourage { get; set; }
    private int CachedAgility { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hero">The character of this dodge competence</param>
    public InitiativeM(ICharacterM hero) : base(ComputeValue(hero.GetAbility(AbilityM.COU), hero.GetAbility(AbilityM.AGI)))
    {
        Min = 0;
        Max = 20;
        DependencyId = [AbilityM.CON];

        CachedAgility = hero.GetAbility(AbilityM.AGI);
        CachedCourage = hero.GetAbility(AbilityM.COU);
    }

    /// <summary>
    /// Computes a valid initiative value from the given attributes
    /// </summary>
    /// <param name="EffectiveCourage">The courage of the character</param>
    /// <param name="EffectiveAgility">The agility of the character</param>
    /// <returns>An effective initiative value</returns>
    public static int ComputeValue(int EffectiveCourage, int EffectiveAgility)
        => (EffectiveCourage + EffectiveAgility + 1) / 2;


    /// <inheritdoc/>
    protected override void UpdateOnDependencyChange(string attrId, int newValue)
    {
        if (attrId == AbilityM.AGI) 
            CachedAgility = newValue;
        else if (attrId == AbilityM.COU) 
            CachedCourage = newValue;
        else 
            return;
        Effective = ComputeValue(CachedCourage, CachedAgility);
    }
}
