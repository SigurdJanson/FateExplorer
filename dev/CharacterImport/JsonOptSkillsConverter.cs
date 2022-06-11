using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterImport
{
    /// <summary>
    /// Json converter optimised for optolith skills
    /// </summary>
    public class JsonOptSkillsConverter : JsonConverter<Dictionary<string, int>>
    {
        private readonly string[] AllowedSkillNames = new string[] { "TAL", "CT", "SPELL", "LITURGY" };

        // This is used when you're converting the C# List back to a JSON format
        /// <inheritdoc/>
        /// <exception cref="NotImplementedException" />
        public override void Write(Utf8JsonWriter writer, Dictionary<string, int> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }



        // This is when you're reading the JSON object and converting it to C#
        /// <inheritdoc/>
        public override Dictionary<string, int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var Result = new Dictionary<string, int>();


            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                    continue;

                if (reader.TokenType == JsonTokenType.EndObject)
                    return Result;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    // Read key
                    string Key = reader.GetString();

                    bool isSkillKey = false;
                    foreach (var an in AllowedSkillNames)
                        if (Key.StartsWith(an))
                        {
                            isSkillKey = true;
                            break;
                        }
                    if (!isSkillKey)
                        throw new JsonException("Unknown skill. Cannot read it.");

                    // Read data
                    reader.Read();
                    if (reader.TokenType != JsonTokenType.Number)
                        throw new JsonException("Numeric skill value was expected but not found");

                    int Value = reader.GetInt32();

                    Result.Add(Key, Value);
                }
            }

            return Result;
        }


        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<string, int>);
    }
}
