namespace FateExplorer.CharacterModel;

/// <summary>
/// A character attribute that is calculated based on other values.
/// Unlike <see cref="RootValue"/>s, other attributes cannot be derived from a derived value.
/// </summary>
public class DerivedValue : CharacterIstic
{
    /// <summary>
    /// Constructor <br/>
    /// Derived classes shall not only call this constructor but also set <see cref="DependencyId"/>, 
    /// <see cref="CharacterIstic.Min"/>, and <see cref="CharacterIstic.Max"/>.
    /// </summary>
    /// <param name="Value"></param>
    public DerivedValue(int Value) : base(Value)
    {
        DependencyId = [];
        Min = -100;
        Max = +100;
    }


    protected string[] DependencyId { get; init; }


    public string[] GetDependencies() => DependencyId;


    /// <summary>
    /// Tests if the characterIstic depends on the attribute <paramref name="Id"/>.
    /// </summary>
    /// <param name="Id">The id of a character attribute.</param>
    /// <returns>true/false</returns>
    public bool DependsOn(string Id)
    {
        foreach(var dep in DependencyId)
            if (dep == Id) return true;
        return false;
    }

    /// <summary>
    /// Register this method to be called when a character attribute changes that affects this value.
    /// </summary>
    /// <remarks>Override <see cref="UpdateOnDependencyChange"/> not this method.</remarks>
    public void DependencyHasChanged(string attrId, int newValue)
    {
        UpdateOnDependencyChange(attrId, newValue);
    }

    /// <summary>
    /// Handles updates when a dependency of the current object changes.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom logic that should execute when a
    /// dependency changes. The base implementation does nothing.</remarks>
    protected virtual void UpdateOnDependencyChange(string attrId, int newValue)
    {
        // override in derived classes
    }
}

