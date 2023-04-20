using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic;


public class BaseContextM : ICheckContextM
{
    /*
     * Implement `IStateContainer`
     */
    public event Action OnStateChanged;

    private void NotifyStateChange() => OnStateChanged?.Invoke();


    private const int FreeModifierDefault = 0;
    private int freeModifier = FreeModifierDefault;
    /// <summary>
    /// An additional additive modifier to be used in addition to the context itself.
    /// </summary>
    public int FreeModifier
    {
        get => freeModifier;
        set
        {
            if (freeModifier != value)
            {
                freeModifier = value;
                NotifyStateChange();
            }
        }
    }


    public int ApplyTotalMod(int before, Check action)
    {
        return before + FreeModifier;
    }

    public Modifier GetTotalMod(int before, Check action)
    {
        return new Modifier(FreeModifier);
    }

    public int ModDelta(int before, Check action)
    {
        return FreeModifier;
    }
}

