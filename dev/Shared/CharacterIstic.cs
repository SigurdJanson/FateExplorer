using System;


namespace FateExplorer.Shared;



public abstract class CharacterIstic
{
    protected int _effectivemod;
    protected int _truemod;

    public CharacterIstic(int Value)
    {
        Imported = Value;
        _effectivemod = 0;
        _truemod = 0;
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
    public virtual int Imported { get; set; }


    /// <summary>
    /// The user may change the imported value to compensate for unrecognized (dis-)advantages, 
    /// special abilities, or any other rules.
    /// </summary>
    public virtual int True
    {
        get => Imported + _truemod;
        set
        {
            if (value >= Min && value <= Max)
            {
                _truemod = value - Imported;

            }
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }


    /// <summary>
    /// The effective value after temporary modifiers have been applied (like charms, curses, etc.)
    /// </summary>
    public virtual int Effective
    {
        get => True + _effectivemod;
        set
        {
            if (value >= Min && value <= Max)
                _effectivemod = value - True;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }


    /// <summary>
    /// True and effective min value
    /// </summary>
    public int Min { get; protected set; } = Int32.MinValue;

    /// <summary>
    /// True and effective max value
    /// </summary>
    public int Max { get; protected set; } = Int32.MaxValue;


    /// <summary>
    /// Using this object like <c>int</c> will return the effective value.
    /// </summary>
    public static implicit operator int(CharacterIstic c) => c.Effective;




    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"{Id} = {Effective}";


    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    public bool Equals(CharacterIstic other) => other.Id == Id;

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns><c>true</c> if the specified object is equal to the current object.</returns>
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

    /// <summary>
    /// Used by special abilities or dis-/advantages to modify the
    /// value during setup.
    /// </summary>
    /// <param name="value">The value that is added to the base value after import</param>
    public virtual void AddOnSetup(int value)
    {
        Imported += value;
    }
}


/// <summary>
/// A character attribute that is calculated based on other values.
/// Unlike <see cref="RootValue"/>s, other attributes cannot be derived from a derived value.
/// </summary>
public class DerivedValue : CharacterIstic
{
    protected int _imported;

    /// <summary>
    /// A modifier set by an activatable, i.e. a special ability or dis-/advantage.
    /// These are added to the value '<see cref="CharacterIstic.Imported"/>'.
    /// </summary>
    public int ActivatableModifier { get; protected set; } = 0;



    /// <inheritdoc/>
    public override int Imported 
    { 
        get => _imported + ActivatableModifier; 
        set
        {
            if (value >= Min && value <= Max)
                _imported = value - ActivatableModifier;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }


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


    /// <inheritdoc/>
    public override void AddOnSetup(int value)
    {
        ActivatableModifier = value;
        _truemod = 0;
        _effectivemod = 0;
    }
}



/// <summary>
/// A character attribute that is atomic. It is not calculated based on other values.
/// It can be modified by special abilities or dis-/advantages.
/// Derived attributes (see <see cref="DerivedValue"/>) are calculated based on these. Therefore,
/// it allows other attributes to register as listeners to state changes.
/// </summary>
public class RootValue : CharacterIstic, IStateContainer
{
    public RootValue(int Value) : base(Value)
    {
    }

    public event Action OnStateChanged;

    protected void NotifyStateChanged() => OnStateChanged?.Invoke();


    public override int True
    {
        get => base.True;
        set
        {
            int oldmod = _truemod;
            base.True = value; // correct _truemod
            if (oldmod != _truemod)
                NotifyStateChanged();
        }
    }


    public override int Effective
    {
        get => base.Effective;
        set
        {
            int oldmod = _effectivemod;
            base.Effective = value;
            if (oldmod != _effectivemod)
                NotifyStateChanged();
        }
    }
}

