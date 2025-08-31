using FateExplorer.RollLogic;
using System;
using System.Collections.Generic;

namespace FateExplorer.FreeDiceCupViMo;

public class DiceCupHolderViMo
{
    private static readonly int[] AbilityDiceSides = [20];
    private static readonly int[] SkillDiceSides = [20, 20, 20];
    private static readonly int[] SixSides = [6];
    private static readonly int[] BotchDiceSides = [6, 6];
    private static readonly int[] ThreeSides = [3];

    public DiceCupHolderViMo()
    {
        CupList =
        [
            new DiceCupViMo("lblAbility", "descFreeAbility", AbilityDiceSides, true),
            new DiceCupViMo("lblSkill", "descFreeSkill", SkillDiceSides, true),
            new DiceCupViMo("lblSix", "descFreeSix", SixSides, true),
            new DiceCupViMo("lblBotch", "descFreeBotch", BotchDiceSides, true),
            new DiceCupViMo("lblD3", "descFreeD3", ThreeSides, true)
        ];

        CupRollResults = [];
    }

    public List<DiceCupViMo> CupList { get; protected set; }

    public int Count { get => CupList.Count; }

    public DiceCupViMo this[int i] => CupList[i];




    public void AddCup(string name, string descr, int[] eyes)
    {
        if (eyes.Length == 0) return;
        CupList.Add(new DiceCupViMo(name, descr, eyes));
    }

    public void AddCup(DiceCupViMo diceCup)
    {
        ArgumentNullException.ThrowIfNull(diceCup);
        CupList.Add(diceCup);
    }

    public int CupRollResultsMax { get; set; } = 4;

    public List<RollResultViMo> CupRollResults { get; protected set; }

    public static IEnumerable<RollResultViMo> ReverseResults(IList<RollResultViMo> items)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            yield return items[i];
        }
    }


    public void RollCup(DiceCupViMo Cup)
    {
        Cup.Roll();

        RollResultViMo Result = new(Cup.Name, Cup.Sides, Cup.Type)
        {
            RollResult = Cup.GetRollResult().Clone() as int[],
            CombinedResult = Cup.GetCombinedRollResult()
        };

        // Dircetly add result to list
        CupRollResults.Add(Result);
        if (CupRollResults.Count > CupRollResultsMax) CupRollResults.RemoveAt(0);
    }

}
