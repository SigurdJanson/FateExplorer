using System;

namespace FateExplorer.Shared;



public abstract class CharacterIstic
{
    protected int _effective;
    protected int _true;

    public CharacterIstic(int Value)
    {
        Imported = Value;
        _effective = Imported;
        _true = Imported;
    }


    /// <summary>
    /// Unique identifier fot the character character-istic
    /// </summary>
    public string Id { get; init; }


    /// <summary>
    /// Name of the character-istic
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// The value as determined from the imported character sheet incl. changes
    /// added by dis-/advantages or special abilities recognized by Fate Explorer.
    /// </summary>
    public int Imported { get; init; }


    /// <summary>
    /// The user may change the imported value to compensate for unrecognized (dis-)advantages, 
    /// special abilities, or any other rules.
    /// </summary>
    public virtual int True
    {
        get => _true;
        set
        {
            if (value >= Min && value <= Max)
                _true = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }


    /// <summary>
    /// The effective value after temporary modifiers have been applied (like charms, curses, etc.)
    /// </summary>
    public virtual int Effective
    {
        get => _effective;
        set
        {
            if (value >= Min && value <= Max)
                _effective = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }


    /// <summary>
    /// True and effective min value
    /// </summary>
    public int Min { get; protected set; }

    /// <summary>
    /// True and effective max value
    /// </summary>
    public int Max { get; protected set; }


    /// <summary>
    /// Using this object like <c>int</c> will return the effective value.
    /// </summary>
    public static implicit operator int(CharacterIstic c) => c.Effective;




    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"{Id} = {_effective}";


    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    public bool Equals(CharacterIstic other) => other.Id == Id;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(Object obj)
    {
        if (obj is not CharacterIstic other) return false;
        return Id == other.Id;
    }

    /// <summary>Create and deliver the value as Data Transfer Object</summary>
    public CharacterAttrDTO Get()
    {
        return new CharacterAttrDTO()
        {
            Id = Id, Name = Name, EffectiveValue = Effective, Max = Max, Min = Min
        };
    }
}



/// <summary>
/// A value that is calculated based on other values
/// </summary>
public class DerivedValue : CharacterIstic
{
    public DerivedValue(int Value) : base(Value)
    {
        DependencyId = Array.Empty<string>();
    }

    protected string[] DependencyId { get; init; }

    public string[] GetDependencies() => DependencyId;

    public bool DependsOn(string Id)
    {
        foreach(var dep in DependencyId) 
            if (dep == Id) return true;
        return false;
    }
}



public class RootValue : CharacterIstic, IStateContainer
{
    public RootValue(int Value) : base(Value)
    {
    }

    public event Action OnStateChanged;

    protected void NotifyStateChanged() => OnStateChanged?.Invoke();


    public override int True
    {
        get => _true;
        set
        {
            if (_true == value) return;
            base.True = value;
            NotifyStateChanged();
        }
    }


    public override int Effective
    {
        get => _effective;
        set
        {
            if (_effective == value) return;
            base.Effective = value;
            NotifyStateChanged();
        }
    }
}

