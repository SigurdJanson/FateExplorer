//using MudBlazor;

namespace MudBlazor
{
    public static class IconsFE
    {
        const string Ligaturisation = "fe ";
        const string Prefix = "fe-";


        public static string Concat(string IconName) => Ligaturisation + Prefix + IconName;

        // ICONS

        public static string Success => Ligaturisation + Prefix + nameof(Success);
        public static string Botch => Ligaturisation + Prefix + nameof(Botch);
        public static string PotionArcane => Ligaturisation + Prefix + nameof(PotionArcane);
        public static string PotionLife => Ligaturisation + Prefix + nameof(PotionLife);

        public static string D6(int d) => Concat($"{nameof(D6)}_{d}");
        public static string D6_1 => Ligaturisation + Prefix + nameof(D6_1);
        public static string D6_2 => Ligaturisation + Prefix + nameof(D6_2);
        public static string D6_3 => Ligaturisation + Prefix + nameof(D6_3);
        public static string D6_4 => Ligaturisation + Prefix + nameof(D6_4);
        public static string D6_5 => Ligaturisation + Prefix + nameof(D6_5);
        public static string D6_6 => Ligaturisation + Prefix + nameof(D6_6);

        public static string LocalMallClear => Ligaturisation + Prefix + nameof(LocalMallClear);
        public static string Ulisses => Ligaturisation + Prefix + nameof(Ulisses);
        public static string BackHandLeft => Ligaturisation + Prefix + nameof(BackHandLeft);
        public static string Sword => Ligaturisation + Prefix + nameof(Sword);

        // DEITIES
        public static string Boron => Ligaturisation + Prefix + nameof(Boron);
        public static string Efferd => Ligaturisation + Prefix + nameof(Efferd);
        public static string Firun => Ligaturisation + Prefix + nameof(Firun);
        public static string Hesinde => Ligaturisation + Prefix + nameof(Hesinde);
        public static string Ingerimm => Ligaturisation + Prefix + nameof(Ingerimm);
        public static string Peraine => Ligaturisation + Prefix + nameof(Peraine);
        public static string Phex => Ligaturisation + Prefix + nameof(Phex);
        public static string Praios => Ligaturisation + Prefix + nameof(Praios);
        public static string Rahja => Ligaturisation + Prefix + nameof(Rahja);
        public static string Rondra => Ligaturisation + Prefix + nameof(Rondra);
        public static string Travia => Ligaturisation + Prefix + nameof(Travia);
        public static string Tsa => Ligaturisation + Prefix + nameof(Tsa);
        public static string NamelessOne => Ligaturisation + Prefix + nameof(NamelessOne);



        // MADAS MARK - MOON PHASES
        
        /// <summary>
        /// Get icons by moon phase
        /// </summary>
        /// <param name="phase">1-12, 1 is new moon (dead mada), 4 is half, 7 is full moon (wheel),, 
        /// 10 is half, 12 the phase before new moon</param>
        /// <returns></returns>
        public static string Moon(MoonPhase phase) => Concat($"{nameof(Moon)}Phase{((int)phase)}");
    }
}
