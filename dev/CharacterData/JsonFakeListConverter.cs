using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterData
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
        ///// <summary>
        ///// Dummy to fool JsonSerializer into not using this converter recursively
        ///// </summary>
        //public T ToBeImported { get; set; }

        //-private string[] AllowedNames = new string[] { "ITEM" };


        // 
        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

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
                    string Key = reader.GetString();
                    T Imported;

                    //bool isValidKey = false;
                    //foreach (var an in AllowedNames)
                    //    if (Key.StartsWith(an))
                    //    {
                    //        isValidKey = true;
                    //        break;
                    //    }
                    //if (!isValidKey)
                    //    throw new JsonException("Unknow key. Cannot read it.");

                    // READ DATA
                    // Read the whole thing into a string and then pass it to
                    // the default deserialiser.
                    reader.Read();
                    if (reader.TokenType == JsonTokenType.StartObject)
                    {
                        string RawJson = "";
                        //-string Dummy = "";
                        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                        {
                            RawJson = jsonDoc.RootElement.GetRawText();
                        }

                        //while (reader.TokenType != JsonTokenType.EndObject)
                        //{
                        //    switch (reader.TokenType)
                        //    {
                        //        case JsonTokenType.PropertyName: RawJson += "\"" + reader.GetString() + "\":"; break;
                        //        case JsonTokenType.String: RawJson += "\"" + reader.GetString() + "\"" + ","; break;
                        //        case JsonTokenType.Number: RawJson += reader.GetDouble() + ","; break;
                        //        case JsonTokenType.True:
                        //        case JsonTokenType.False: RawJson += reader.GetBoolean().ToString().ToLower() + ","; break;
                        //        case JsonTokenType.StartArray: RawJson += "["; break;
                        //        case JsonTokenType.EndArray: RawJson += "]" + ","; break;
                        //        //case JsonTokenType.EndObject: RawJson += "}"; break;
                        //        case JsonTokenType.StartObject: RawJson += "{"; break;
                        //    }
                        //    reader.Read();
                        //}
                        //RawJson += "}"; // Let us not forget the end token
                        Imported = JsonSerializer.Deserialize<T>(RawJson, options);

                        //-T ObjectResult = JsonSerializer.Deserialize<T>(jsonString);
                    }
                    else
                        throw new JsonException("Object was expected but not found");

                    Result.Add(Imported);
                }
            }

            return Result;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(List<T>);

    }
}
