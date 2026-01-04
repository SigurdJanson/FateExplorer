using FateExplorer.Shared;
using System;


namespace FateExplorer.CharacterModel;

/// <summary>
/// A character attribute that is atomic. It is not calculated based on other values.
/// It can be modified by special abilities or dis-/advantages.
/// Derived attributes (see <see cref="DerivedValue"/>) are calculated based on these. Therefore,
/// it allows other attributes to register as listeners to state changes.
/// </summary>
public class RootValue : CharacterIstic//, IStateContainer
{
    public delegate void StateChangedHandler(string attrId, int newValue);


    public RootValue(int Value) : base(Value)
    {
    }


    // EVENT HANDLING

    public event StateChangedHandler OnStateChanged;

    protected void NotifyStateChanged() => OnStateChanged?.Invoke(Id, Effective);


    // OVERRIDE PROPERTIES TO NOTIFY ON CHANGE

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

