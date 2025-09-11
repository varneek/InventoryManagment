using System.Text.Json;
using System.Text.Json.Serialization;

namespace InventoryManagment.web.Converter;

public class converter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal().ToString();
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        throw new JsonException("Price must be a number or a numeric string.");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}

