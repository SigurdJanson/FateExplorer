namespace FateExplorer.CharacterModel;

public interface IDerivedAttributeM
{
    /// <summary>
    /// Return a list of id's that identify the dependencies of an derived attribute.
    /// </summary>
    string[] DependentAttributes { get; }
}
