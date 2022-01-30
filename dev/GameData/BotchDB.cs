using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{
    /// <summary>
    /// Access to the botch effects as specified in the rule book.
    /// </summary>
    public class BotchDB
    {
        [JsonPropertyName("Effects")]
        public IReadOnlyList<BotchEntry> Botches { get; set; }

        [JsonPropertyName("Tables")]
        public IReadOnlyList<BotchTable> Tables { get; set; }


        /// <summary>
        /// Returns the overview of all botch effects for a certain roll
        /// </summary>
        /// <param name="Roll">A unique identifier for the particular check, parry, dodge, skill</param>
        /// <param name="Type">
        /// A unique identifier to specifiy the type of combat check (shield, umarmed, ranged) or skill (arcane or karma).
        /// </param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public BotchTable GetBotchTable(string Roll, string Type)
        {
            foreach (var t in Tables)
            {
                if (t.Type != Type) continue;
                foreach (var r in t.Roll)
                    if (r == Roll)
                        return t;
            }

            throw new KeyNotFoundException("Unknown botch table");
        }


        /// <summary>
        /// Returns a particular botch effect.
        /// </summary>
        /// <inheritdoc cref="GetBotchTable(string, string)"/>
        /// <param name="DiceEyes">The result of the botch effect roll (2d6).</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public BotchEntry GetBotch(string Roll, string Type, int DiceEyes)
        {
            // Find correct table in `Tables`
            var Table = GetBotchTable(Roll, Type);

            // Get reference
            BotchEffect effect = null;
            foreach (var i in Table.Effect)
                foreach (var e in i.DiceEyes)
                    if (e == DiceEyes)
                        effect = i;

            // Return BotchEffect
            if (effect is not null)
                foreach (var b in Botches)
                    if (b.Id == effect.Ref)
                        return b;

            // if nothing found: error
            throw new KeyNotFoundException("Unknown botch effect");
        }

        /// <summary>
        /// Return a relative URL that points to a documentation of the given botch
        /// </summary>
        /// <param name="Roll"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string GetUrl(string Roll, string Type)
        {
            // Find correct table in `Tables`
            var Table = GetBotchTable(Roll, Type);

            // Return URL
            return Table.Url;
        }
    }



    public class BotchEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("descr")]
        public string Descr { get; set; }
    }


    public class BotchEffect
    {
        [JsonPropertyName("dr")]
        public int[] DiceEyes { get; set; }

        [JsonPropertyName("ref")]
        public string Ref { get; set; }
    }


    public class BotchTable
    {
        [JsonPropertyName("Roll")]
        public string[] Roll { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Url")]
        public string Url { get; set; }

        [JsonPropertyName("Effect")]
        public IReadOnlyList<BotchEffect> Effect { get; set; }
    }


}
