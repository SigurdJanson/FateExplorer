using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FateExplorer.CharacterImport;

/// <summary>
/// Json converter to import SID fields used in Optolith files for (dis-)advantages and special abilities.
/// These fields can be either numeric or strings. To avoid dynamic types this class provides a proper
/// interface.
/// This  converter interprets every SID as string. Integers (int32) are converted to strings. Other types
/// are not supported.
/// </summary>
public class JsonActivatableSIDConverter : JsonConverter<string>
{
    // This is used when you're converting the C# List back to a JSON format
    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (value is null) return;
        int IntVal;
        if (Int32.TryParse(value, out IntVal))
        {
            writer.WriteNumberValue(IntVal);
        } 
        else 
        {
            writer.WriteStringValue(value);
        }
    }



    // This is when you're reading the JSON object and converting it to C#
    /// <inheritdoc/>
    /// <exception cref="NotSupportedException">Trying to read a type that is not <c>string</c>, <c>Int32</c> or <c>null</c>.</exception>
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string Result = reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.Number => Int32ToString(ref reader),
            JsonTokenType.String => reader.GetString(),
            _ => throw new NotSupportedException($"Type {reader.TokenType} not supported")
        };
        return Result;
    }

    /// <summary>
    /// Helper that tries to read in int32 value from the reader and converts it to a string.
    /// </summary>
    /// <inheritdoc cref="Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
    /// <returns>An int32 represented as string.</returns>
    /// <exception cref="JsonException"></exception>
    protected static string Int32ToString(ref Utf8JsonReader reader)
    {
        int IntValue;
        if (reader.TryGetInt32(out IntValue))
            return IntValue.ToString();
        else
            throw new JsonException("Int32 expected");
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType) => objectType == typeof(string);
}
