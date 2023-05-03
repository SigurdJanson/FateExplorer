using FateExplorer.CharacterModel;

namespace FateExplorer.ViewModel;

public class BelongingViMo
{
    protected BelongingM Belonging { get; set; }

    public BelongingViMo(BelongingM belonging)
    {
        Belonging = belonging;
    }

    /// <inheritdoc cref="BelongingM.Id"/>
    public string Id => Belonging.Id;

    /// <inheritdoc cref="BelongingM.Name"/>
    public string Name => Belonging.Name;

    /// <inheritdoc cref="BelongingM.Amount"/>
    public int Amount => Belonging.Amount;

    /// <inheritdoc cref="BelongingM.Weight"/>
    public decimal Weight => Belonging.Weight;

    /// <inheritdoc cref="BelongingM.Price"/>
    public decimal Price => Belonging.Price;

    /// <inheritdoc cref="BelongingM.Where"/>
    public string Where => Belonging.Where;
}
