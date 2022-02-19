using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterImport
{
    /// <summary>
    /// Handles deserialisation of fake lists that are actually an array (which would be <c>[ ... ]</c> in json)
    /// but instead there is a series of objects with keys like this:
    /// <code>
    /// "items": {
    ///    "ITEM_1": { ... }, // object of type T
    ///    "ITEM_2": { ... },
    ///    ...
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">Object type </typeparam>
    /// <remarks>
    /// Inspired by https://stackoverflow.com/questions/16085805/recursively-call-jsonserializer-in-a-jsonconverter
    /// </remarks>
    public class JsonFakeListConverter<T> : JsonConverter<List<T>>
    {
        /// <inheritdoc/>
        /// <exception cref="NotImplementedException" />
        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var Result = new List<T>();


            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return Result;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    // READ KEY
                    //string Key = reader.GetString();
                    var _ = reader.GetString(); // skip "Key"
                    T Imported;

                    // READ DATA
                    // Read the whole thing into a string and then pass it to
                    // the default deserialiser.
                    reader.Read();
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        string RawJson = "";
                        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                        {
                            RawJson = jsonDoc.RootElement.GetRawText();
                        }

                        Imported = JsonSerializer.Deserialize<T>(RawJson, options);
                    }
                    else
                        throw new JsonException("Object was expected but not found");

                    Result.Add(Imported);
                }
            }

            return Result;
        }


        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(List<T>);

    }
}
