using System.Collections.Generic;

namespace FateExplorer.WPA.FreeDiceCupViMo
{
    public class DiceCupHolderViMo
    {
        public DiceCupHolderViMo()
        {
            CupList = new();
            CupList.Add(new DiceCupViMo("Ability", "Plain d20 rolls", new int[1] { 20 }, true));
            CupList.Add(new DiceCupViMo("Skill", "3d20 rolls", new int[3] { 20, 20, 20 }, true));
            CupList.Add(new DiceCupViMo("Six", "Plain d6 rolls", new int[1] { 6 }, true));
            CupList.Add(new DiceCupViMo("Botch", "Rolls with 2d6", new int[2] { 6, 6 }, true));
        }

        public List<DiceCupViMo> CupList { get; protected set; }

        public int Count { get => CupList.Count; }

        public DiceCupViMo this[int i] => CupList[i];

        public void AddCup(string name, string descr, int[] eyes)
        {
            if (eyes.Length == 0) return;
            CupList.Add(new DiceCupViMo(name, descr, eyes));
        }
    }
}
