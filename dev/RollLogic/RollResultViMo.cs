using System.Linq;
using FateExplorer.WPA.FreeDiceCupViMo;

namespace FateExplorer.WPA.RollLogic
{
    public enum RollSuccessLevel
    {
        Botch = 1, PendingBotch = -1,
        Fail = 2,
        Success = 3,
        Critical = 4, PendingCritical = -4,
        na = 99
    }

    public struct RollResultViMo
    {
        public RollResultViMo(string name, int[] dieSides, CupType cupType) : this()
        {
            Name = name;
            DieSides = dieSides.Clone() as int[];
            CupType = cupType;

            // Defaults
            RollAgainst = null;
            RollResult = null;
            CombinedResult = -999;
            SuccessLevel = RollSuccessLevel.na;
            Modifier = 0;
        }

        /// <summary>
        /// Describes the roll.
        /// </summary>
        public string Name { get; set; }

        public CupType CupType { get; set; }

        /// <summary>
        /// The threshold the die roll has to beat.
        /// </summary>
        public int[] RollAgainst { get; set; }

        /// <summary>
        /// The n rolled dice. One for each <see cref="RollAgainst"/>.
        /// </summary>
        public int[] RollResult { get; set; }

        /// <summary>
        /// The effect of the roll. For a combat attack role this would be the hit points.
        /// For a skill rol eit is the skill level.
        /// </summary>
        public int CombinedResult { get; set; }

        /// <summary>
        /// Success level
        /// </summary>
        public RollSuccessLevel SuccessLevel { get; set; }

        /// <summary>
        /// The sum of the modifiers put into the roll.
        /// </summary>
        public int Modifier { get; set; } = 0;

        /// <summary>
        /// Number of eyes of the dice (array with sides for each die).
        /// </summary>
        public int[] DieSides { get; set; } = null;

        /// <summary>
        /// Returns the roll in the format "3d20".
        /// </summary>
        /// <returns></returns>
        public string RollIdString()
        {
            if (CupType == CupType.MixedMulti)
            {
                return string.Join(", ", DieSides.Distinct());
            }
            else
                return $"{DieSides.Length}d{DieSides[0]}"; // TODO: i18n
        }
    }
}
