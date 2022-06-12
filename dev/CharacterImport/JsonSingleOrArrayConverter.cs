using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterImport;

public class JsonSingleOrArrayConverter<TL,TI> : JsonConverter<TL> 
    where TL : IList<TI>, new() // array or list
    where TI : struct    // some value type
{
    /// <inheritdoc />
    /// <remarks>Writes the value(s) as list</remarks>
    public override void Write(Utf8JsonWriter writer, TL value, JsonSerializerOptions options)
    {
        if (value.Count == 0) return;
        else if (value.Count == 1)
        {
            var RawJson = JsonSerializer.SerializeToUtf8Bytes<TI>(value[0], options);
            writer.WriteRawValue(RawJson);
        }
        else
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                var RawJson = JsonSerializer.SerializeToUtf8Bytes<TI>(item, options);
                writer.WriteRawValue(RawJson);
            }
            writer.WriteEndArray();
        }
    }



    /// <inheritdoc />
    public override TL Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        TL Result = new();
        TI Imported;

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    throw new JsonException($"Found an object where {typeof(TI)} or {typeof(TL)} was expected");
                case JsonTokenType.None:
                    throw new JsonException($"Found nothing where was expected a {typeof(TI)} or {typeof(TL)}");
                case JsonTokenType.Comment:
                    throw new JsonException($"Found a comment where was expected a {typeof(TI)} or {typeof(TL)}");
            }
            Imported = JsonSerializer.Deserialize<TI>(reader.ValueSpan, options);
            Result.Add(Imported);
        }
        else // The token is an array an can be read one by one
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return Result;
                }

                // READ DATA
                if (reader.TokenType == JsonTokenType.StartObject)
                    throw new JsonException("Object was NOT expected but not found");

                Imported = JsonSerializer.Deserialize<TI>(reader.ValueSpan, options);
                Result.Add(Imported);
            }
        }

        return Result;
    }

    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(TL) || objectType == typeof(TI));
    }
}






//public class SingleOrArrayListConverter<T> : JsonConverter where T : struct
//{
//    // Adapted from this answer https://stackoverflow.com/a/18997172
//    // to https://stackoverflow.com/questions/18994685/how-to-handle-both-a-single-item-and-an-array-for-the-same-property-using-json-n
//    // by Brian Rogers https://stackoverflow.com/users/10263/brian-rogers

//    readonly IContractResolver resolver;

//    public SingleOrArrayListConverter() : this() { }

//    public SingleOrArrayListConverter(IContractResolver resolver)
//    {
//        // Use the global default resolver if none is passed in.
//        this.resolver = resolver ?? new JsonSerializer().ContractResolver;
//    }

//    static bool CanConvert(Type objectType, IContractResolver resolver)
//    {
//        Type itemType;
//        JsonArrayContract contract;
//        return CanConvert(objectType, resolver, out itemType, out contract);
//    }

//    static bool CanConvert(Type objectType, IContractResolver resolver, out Type itemType, out JsonArrayContract contract)
//    {
//        if ((itemType = objectType.GetListItemType()) == null)
//        {
//            itemType = null;
//            contract = null;
//            return false;
//        }
//        // Ensure that [JsonObject] is not applied to the type.
//        if ((contract = resolver.ResolveContract(objectType) as JsonArrayContract) == null)
//            return false;
//        var itemContract = resolver.ResolveContract(itemType);
//        // Not implemented for jagged arrays.
//        if (itemContract is JsonArrayContract)
//            return false;
//        return true;
//    }

//    public override bool CanConvert(Type objectType) => CanConvert(objectType, resolver);

//    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//    {
//        Type itemType;
//        JsonArrayContract contract;

//        if (!CanConvert(objectType, serializer.ContractResolver, out itemType, out contract))
//            throw new JsonSerializationException(string.Format("Invalid type for {0}: {1}", GetType(), objectType));
//        if (reader.MoveToContent().TokenType == JsonToken.Null)
//            return null;
//        var list = (IList)(existingValue ?? contract.DefaultCreator());
//        if (reader.TokenType == JsonToken.StartArray)
//            serializer.Populate(reader, list);
//        else
//            // Here we take advantage of the fact that List<T> implements IList to avoid having to use reflection to call the generic Add<T> method.
//            list.Add(serializer.Deserialize(reader, itemType));
//        return list;
//    }



//    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    {
//        var list = value as ICollection;
//        if (list == null)
//            throw new JsonSerializationException(string.Format("Invalid type for {0}: {1}", GetType(), value.GetType()));
//        // Here we take advantage of the fact that List<T> implements IList to avoid having to use reflection to call the generic Count method.
//        if (list.Count == 1)
//        {
//            foreach (var item in list)
//            {
//                serializer.Serialize(writer, item);
//                break;
//            }
//        }
//        else
//        {
//            writer.WriteStartArray();
//            foreach (var item in list)
//                serializer.Serialize(writer, item);
//            writer.WriteEndArray();
//        }
//    }
//}















//public static partial class JsonExtensions
//{
//    public static JsonReader MoveToContent(this JsonReader reader)
//    {
//        while ((reader.TokenType == JsonToken.Comment || reader.TokenType == JsonToken.None) && reader.Read())
//            ;
//        return reader;
//    }

//    internal static Type GetListItemType(this Type type)
//    {
//        // Quick reject for performance
//        if (type.IsPrimitive || type.IsArray || type == typeof(string))
//            return null;
//        while (type != null)
//        {
//            if (type.IsGenericType)
//            {
//                var genType = type.GetGenericTypeDefinition();
//                if (genType == typeof(List<>))
//                    return type.GetGenericArguments()[0];
//            }
//            type = type.BaseType;
//        }
//        return null;
//    }
//}
