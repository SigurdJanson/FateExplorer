using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic;


public class BaseContextM : ICheckContext
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


    public int ApplyTotalMod(int before, Check.Combat action)
    {
        return before + FreeModifier;
    }

    public Modifier GetTotalMod(int before, Check.Combat action)
    {
        return new Modifier(FreeModifier);
    }
}

