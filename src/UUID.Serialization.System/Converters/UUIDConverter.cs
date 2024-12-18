using System.Text.Json;
using System.Text.Json.Serialization;

namespace System
{
    /// <summary>
    /// Provides JSON conversion functionality for UUID using System.Text.Json
    /// </summary>
    public class UUIDConverter : JsonConverter<UUID>
    {
        /// <summary>
        /// Reads and converts the JSON to UUID
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="typeToConvert">The type to convert</param>
        /// <param name="options">The serializer options</param>
        /// <returns>A UUID instance</returns>
        /// <exception cref="JsonException">Thrown when the JSON value is not a string or is null</exception>
        public override UUID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                throw new JsonException("Cannot convert null value to UUID");
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected string value for UUID, but got {reader.TokenType}");
            }

            string value = reader.GetString();

            if (string.IsNullOrEmpty(value))
            {
                throw new JsonException("Cannot convert empty string to UUID");
            }

            try
            {
                return UUID.Parse(value);
            }
            catch (Exception ex)
            {
                throw new JsonException($"Failed to parse UUID from string '{value}'", ex);
            }
        }

        /// <summary>
        /// Writes a UUID value as JSON
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="value">The UUID value to write</param>
        /// <param name="options">The serializer options</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null</exception>
        public override void Write(Utf8JsonWriter writer, UUID value, JsonSerializerOptions options)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStringValue(value.ToString());
        }
    }
}