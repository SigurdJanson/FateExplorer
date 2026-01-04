using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FateExplorer.CharacterModel;


/// <summary>
/// Resiliences are either spirit (<see cref="ChrAttrId.SPI"/>) or toughness (<see cref="ChrAttrId.TOU"/>).
/// </summary>
public class ResilienceM : DerivedValue
{
    /// <summary>
    /// Highest/lowest race base value + min attributes / 6 + modifiers from activatables
    /// </summary>
    protected const int MinResilience = -6 + (3 * 8 / 6) - 2;
    protected const int MaxResilience = -4 + (3 * 20 / 6) +2;
    private ICharacterM Hero { get; set; }



    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="gameData">Resilience object taken from <see cref="IGameDataService"/>.</param>
    /// <param name="hero">The hero this resilience belongs to.</param>
    public ResilienceM(ResilienceDbEntry gameData, ICharacterM hero) : base(0)
    {
        Hero = hero;
        Id = gameData.Id;
        DependencyId = gameData.DependantAbilities.Clone() as string[];
        int RaceBaseValue = gameData.RaceBaseValue.First(bv => bv.RaceId == Hero.SpeciesId).Value;
        Imported = ComputeValue(RaceBaseValue, hero.Abilities);
        Min = MinResilience;
        Max = MaxResilience;
        if (Imported <  Min || Imported > Max)
            throw new NotSupportedException("Resilience out of range");
    }



    /// <summary>
    /// Computes the resilience value.
    /// </summary>
    /// <param name="RaceBaseValue">The base value of the character species'</param>
    /// <param name="Abilities">
    /// If not given, the method will work with the character's base values
    /// </param>
    /// <returns></returns>
    public int ComputeValue(int RaceBaseValue, Dictionary<string, AbilityM> Abilities = null)
    {
        int AbSum = 0;
        if (Abilities is not null && Abilities.Count > 0)
        {
            foreach (var a in DependencyId)
                AbSum += Abilities[a].Effective;
        }
        else
        {
            foreach (var a in DependencyId)
                AbSum += Hero.GetAbility(a);
        }
        int V = RaceBaseValue + (int)Math.Floor((AbSum / 6.0m) + 0.5m) + ActivatableModifier; // Round rounds .5 to even numbers, not up
        return V;
    }


}
