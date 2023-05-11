using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FateExplorer.CharacterModel;

public class ResilienceM
{
    private ICharacterM Hero { get; set; }

    /// <summary>
    /// Identifies the resilience. Shall be <see cref="ChrAttrId.SPI"/> or <see cref="ChrAttrId.TOU"/>.
    /// </summary>
    private string Id { get; set; }

    /// <summary>
    /// For character generation each race has a base value.
    /// </summary>
    public int BaseValue { get; protected set; }

    /// <summary>
    /// Extra points from dis-/advantages added to the base value.
    /// </summary>
    public int? ExtraValue { get; protected set; } = null;

    /// <summary>
    /// List of ids to identify the attributes needed to compute this attribute.
    /// </summary>
    public string[] DependentAbilities { get; set; }

    private int? value = null;
    public int Value
    {
        get
        {
            this.value ??= ComputeValue();

            return (int)this.value;
        }
        protected set
        {
            this.value = value;
        }
    }



    /// <summary>
    /// Computes the resilience value.
    /// </summary>
    /// <param name="Abilities">
    /// If not given, the method will work with the character's base values
    /// </param>
    /// <returns></returns>
    public int ComputeValue(Dictionary<string, int> Abilities = null)
    {
        int AbSum = 0;
        foreach (var a in DependentAbilities)
            AbSum += Abilities?[a] ?? Hero.GetAbility(a);

        int Mod = 0;
        if (Id == ChrAttrId.SPI)
        {
            if (Hero.HasAdvantage(ADV.IncreasedSpirit))
                ++Mod;
            if (Hero.HasDisadvantage(DISADV.DecreasedSpirit))
                --Mod;
        } 
        else if (Id == ChrAttrId.TOU)
        {
            if (Hero.HasAdvantage(ADV.IncreasedToughness))
                ++Mod;
            if (Hero.HasDisadvantage(DISADV.DecreasedToughness))
                --Mod;
        }


        int V = BaseValue + (int)Math.Floor((AbSum / 6.0m) + 0.5m) + (ExtraValue ?? 0) + Mod; // Round rounds .5 to even numbers, not up

        return V;
    }


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="gameData">Resilience object taken from <see cref="IGameDataService"/>.</param>
    /// <param name="hero">The hero this resilience belongs to.</param>
    public ResilienceM(ResilienceDbEntry gameData, ICharacterM hero)
    {
        Hero = hero;
        Id = gameData.Id;
        DependentAbilities = gameData.DependantAbilities.Clone() as string[];
        int RaceBaseValue = gameData.RaceBaseValue.First(bv => bv.RaceId == Hero.SpeciesId).Value;
        BaseValue = RaceBaseValue;
        ExtraValue = 0; // Unknown at this points, because we do not know dis-/advantages
    }

}
