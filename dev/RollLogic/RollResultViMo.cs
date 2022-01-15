using FateExplorer.FreeDiceCupViMo;
using FateExplorer.Shared;
using System.Linq;

namespace FateExplorer.RollLogic
{
    public class RollResultViMo
    {
        /// <summary>
        /// Constructor used for free dice rolls
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dieSides"></param>
        /// <param name="cupType"></param>
        public RollResultViMo(string name, int[] dieSides, CupType cupType)
        {
            Name = name;
            DieSides = dieSides.Clone() as int[];
            CupType = cupType;
        }

        /// <summary>
        /// Describes the roll.
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// 
        /// </summary>
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
        public int? CombinedResult { get; set; } // TODO: is this required in the long run?

        /// <summary>
        /// Success level
        /// </summary>
        public RollSuccessLevel SuccessLevel { get; set; } = RollSuccessLevel.na;

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
        public string RollToString()
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
