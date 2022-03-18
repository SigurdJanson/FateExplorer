using FateExplorer.FreeDiceCupViMo;
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
        /// Constructor to wrap a roll
        /// </summary>
        /// <param name="roll"></param>
        public RollResultViMo(IRollM roll)
        {
            Name = roll.ToString();
            DieSides = roll.Sides.Clone() as int[];
            CupType = CupType.None; // unknown

            RollAgainst = null;
            RollResult = roll.OpenRoll.Clone() as int[];
            CombinedResult = roll.OpenRollCombined();
            Modifier = roll.ModifiedBy;
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
        public int? CombinedResult { get; set; }

        /// <summary>
        /// Success level
        /// </summary>
        public RollSuccess.Level SuccessLevel { get; set; } = RollSuccess.Level.na;

        /// <summary>
        /// The sum of the modifiers put into the roll.
        /// </summary>
        public int[] Modifier { get; set; }

        /// <summary>
        /// Number of eyes of the dice (array with sides for each die).
        /// </summary>
        public int[] DieSides { get; set; } = null;

        /// <summary>
        /// Returns the roll in the format ndM (e.g. "3d20").
        /// </summary>
        /// <param name="DieCharacter">The character representing the die 
        /// ("W" for German, "d" for English, ...)</param>
        /// <returns>A string representing the dice used in the roll</returns>
        public string RollToString(char DieCharacter = 'd')
        {
            if (CupType == CupType.MixedMulti)
            {
                return string.Join(", ", DieSides.Distinct());
            }
            else
                return $"{DieSides.Length}{DieCharacter}{DieSides[0]}";
        }
    }
}
