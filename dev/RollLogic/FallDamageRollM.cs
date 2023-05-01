using FateExplorer.Shared;
using System;

namespace FateExplorer.RollLogic;


public class FallDamageRollM : MultiDieRoll
{
    public const int _Sides = 6;

    public Modifier GroundModifier { get; protected set; }
    public Modifier JumpModifier { get; protected set; }
    public Modifier ArmourMod { get; protected set; }
    public Modifier PaddingMod { get; protected set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dropHeight">The drop height in yards (meters).</param>
    /// <param name="groundMod">Modifier due to ground conditions.</param>
    /// <param name="jumpQuality">The quality level of a check on Body Control (Jumping).</param>
    /// <param name="armourMod">An additional free modifier (usually to factor unrecognised (dis-)advantages in</param>
    /// <param name="paddingMod">An additional free modifier (usually to factor unrecognised (dis-)advantages in</param>
    public FallDamageRollM(
        int dropHeight, int groundMod, int jumpQuality, int armourMod, int paddingMod)
        : base(_Sides, dropHeight)
    {
        GroundModifier = new Modifier(groundMod);
        JumpModifier = new Modifier(jumpQuality * 2);
        ArmourMod = new Modifier(armourMod);
        PaddingMod = new Modifier(paddingMod);
    }


    /// <inheritdoc/>
    public override int OpenRollCombined()
    {
        var result = base.OpenRollCombined();
        return result + GroundModifier + JumpModifier + ArmourMod + PaddingMod;
    }


}
