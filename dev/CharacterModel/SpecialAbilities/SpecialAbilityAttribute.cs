using System;

namespace FateExplorer.CharacterModel.SpecialAbilities;

[AttributeUsage(AttributeTargets.Class)]
public class SpecialAbilityAttribute : Attribute
{
    public string Name { get; }
    public SpecialAbilityAttribute(string name) => Name = name;
}