using System;

namespace FateExplorer.CharacterModel.DisAdvantages;

[AttributeUsage(AttributeTargets.Class)]
public class DisAdvantageAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
