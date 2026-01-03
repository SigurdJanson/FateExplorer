using FateExplorer.Shared;
using System;
using System.Collections.Generic;

namespace FateExplorer.ViewModel;

public static class PotionDescriptorFactory
{

    public enum Potion
    {
        Healing, Arcane, Antidote, Love, Will, Invisibility, Transformation, WeaponBalm
    }

    public static List<DieCheckFormula> GetQualityLevelsFor(Potion potion) => potion switch
    {
        Potion.Healing or 
        Potion.Arcane => [new DieCheckFormula(1, 3, 0), 
            new DieCheckFormula(1, 6, 0), 
            new DieCheckFormula(1, 6, 2), 
            new DieCheckFormula(1, 6, 4), 
            new DieCheckFormula(1, 6, 6), 
            new DieCheckFormula(1, 6, 8)],
        //Potion.Antidote => ["1", "2", "3", "4", "5", "6"],
        //Potion.Love => ["", "", "", "", "", ""],
        //Potion.Will => ["SPI+1, 5 min", "SPI+2, 10 min", "SPI+2, 15 min", "SPI+2, 20 min", "SPI+3, 25 min", "SPI+3, 30 min"],
        //Potion.Invisibility => [],
        //Potion.Transformation => ["1 min", "2 min", "5 min", "10 min", "15 min", "20 min"],
        //Potion.WeaponBalm => ["5 min", "15 min", "2 hr", "8 hr", "1 day", "1 week" ],
        _ => throw new ArgumentOutOfRangeException(nameof(potion), potion, null)
    };

    public static string GetPotionLabel(Potion potion) => potion switch
    {
        Potion.Healing => "lblHealingPotion",
        Potion.Arcane => "lblArcanePotion",
        //Potion.Antidote => "lblAntidote",
        //Potion.Love => "lblLovePotion",
        //Potion.Will => "lblWillPotion",
        //Potion.Invisibility => "lblInvisibility Potion",
        _ => throw new ArgumentOutOfRangeException(nameof(potion), potion, null)
    };

    //public static string GetQualityLabel(int level) => "abbrQL" + " " + level;
}