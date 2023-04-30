using FateExplorer.RollLogic;

namespace FateExplorer.ViewModel;

public interface ICheckContextViMo
{
    int FreeModifier { get; set; }

    void ResetToDefault();

    ICheckContextM ToM();
}

public class CheckContextViMo : ICheckContextViMo
{
    private BaseContextM Context { get; set; }

    public int FreeModifier
    {
        get => Context.FreeModifier; set => Context.FreeModifier = value;
    }

    public void ResetToDefault() => Context.ResetToDefault();

    public ICheckContextM ToM() => Context;

    public BaseContextM ToBaseM() => Context;
}
