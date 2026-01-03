using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic;

public class EnergyPotionRollM : DieRollM
{
    private static readonly int[] DieSides = { 3, 6, 6, 6, 6, 6 };
    private static readonly int[] Modifier = { 0, 0, 2, 4, 6, 8 };

    public int QualityLevel { get; init; }
    public SimpleCheckModificatorM PotionModifier { get; init; }

    public EnergyPotionRollM(int QualityLevel) : base(QualityLevel > 1 ? 6 : 3)
    {
        if (QualityLevel < 1 || QualityLevel > 6) throw new ArgumentOutOfRangeException(nameof(QualityLevel), "Quality levels must be between 1 and 6");
        int AddedMod = Modifier[QualityLevel - 1];
        PotionModifier = new SimpleCheckModificatorM(new Modifier(AddedMod));
        this.QualityLevel = QualityLevel;
    }


    /// <inheritdoc/>
    public override int[] Roll()
    {
        base.Roll(); // executes the roll
        OpenRoll[0] = PotionModifier.Apply(OpenRoll[0]);

        return OpenRoll;
    }
}
