using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FateExplorer.GameData
{

    // BotchProvider myDeserializedClass = JsonConvert.DeserializeObject<BotchProvider>(myJsonResponse); 

    public class BotchDB
    {
        [JsonPropertyName("Effects")]
        public IReadOnlyList<BotchEntry> Botches { get; set; }

        [JsonPropertyName("Tables")]
        public IReadOnlyList<BotchTable> Tables { get; set; }


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


        public BotchEntry GetBotch(string Roll, string Type, int DiceEyes)
        {
            // Find correct table in `Tables`
            var Table = GetBotchTable(Roll, Type);

            // Get reference
            BotchEffect effect = null;
            foreach (var i in Table.Effect)
                foreach(var e in i.DiceEyes)
                    if (e == DiceEyes)
                        effect = i;

            // Return BotchEffect
            if (effect is not null)
                foreach (var b in Botches)
                    if (b.Id == effect.Ref)
                        return b;

            // if nothing found: error
            throw new Exception("Unknown botch effect");
        }


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
        public string Id { get; set;  }

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
