namespace FateExplorer.Shared;

public struct DieCheckFormula(int dieCount, int sides, int modifier)
{
    public int DieCount { get; set; } = dieCount;
    public int Sides { get; set; } = sides;
    public int Modifier { get; set; } = modifier;

    public readonly int AbsMod => System.Math.Abs(Modifier);

    public readonly bool HasMod => Modifier != 0;
    public readonly string Op => Modifier switch
    {
        > 0 => "+",
        < 0 => "-",
        _ => ""
    };
}
