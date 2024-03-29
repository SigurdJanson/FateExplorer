﻿using FateExplorer.RollLogic;
using System;
using System.Collections.Generic;

namespace FateExplorer.FreeDiceCupViMo
{
    public class DiceCupHolderViMo
    {
        public DiceCupHolderViMo()
        {
            CupList = new()
            {
                new DiceCupViMo("lblAbility", "descFreeAbility", new int[1] { 20 }, true),
                new DiceCupViMo("lblSkill", "descFreeSkill", new int[3] { 20, 20, 20 }, true),
                new DiceCupViMo("lblSix", "descFreeSix", new int[1] { 6 }, true),
                new DiceCupViMo("lblBotch", "descFreeBotch", new int[2] { 6, 6 }, true)
            };

            CupRollResults = new();
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
            if (diceCup is null) throw new ArgumentNullException(nameof(diceCup));
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

            RollResultViMo Result = new(Cup.Name, Cup.Sides, Cup.Type);

            Result.RollResult = Cup.GetRollResult().Clone() as int[];
            Result.CombinedResult = Cup.GetCombinedRollResult();

            // Dircetly add result to list
            CupRollResults.Add(Result);
            if (CupRollResults.Count > CupRollResultsMax) CupRollResults.RemoveAt(0);
        }

    }
}
