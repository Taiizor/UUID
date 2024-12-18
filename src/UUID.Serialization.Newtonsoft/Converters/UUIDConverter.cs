using Newtonsoft.Json;

namespace System
{
    /// <summary>
    /// Provides JSON conversion functionality for UUID using Newtonsoft.Json
    /// </summary>
    public class UUIDConverter : JsonConverter<UUID>
    {
        /// <summary>
        /// Reads the JSON representation of the object
        /// </summary>
        /// <param name="reader">The JsonReader to read from</param>
        /// <param name="objectType">Type of the object</param>
        /// <param name="existingValue">The existing value of object being read</param>
        /// <param name="hasExistingValue">True if there is an existing value</param>
        /// <param name="serializer">The calling serializer</param>
        /// <returns>The object value</returns>
        /// <exception cref="JsonReaderException">Thrown when the JSON value is not a string or is null</exception>
        public override UUID ReadJson(JsonReader reader, Type objectType, UUID existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.TokenType == JsonToken.Null)
            {
                throw new JsonReaderException("Cannot convert null value to UUID");
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonReaderException($"Expected string value for UUID, but got {reader.TokenType}");
            }

            string value = (string)reader.Value;

            if (string.IsNullOrEmpty(value))
            {
                throw new JsonReaderException("Cannot convert empty string to UUID");
            }

            try
            {
                return UUID.Parse(value);
            }
            catch (Exception ex)
            {
                throw new JsonReaderException($"Failed to parse UUID from string '{value}'", ex);
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object
        /// </summary>
        /// <param name="writer">The JsonWriter to write to</param>
        /// <param name="value">The value to write</param>
        /// <param name="serializer">The calling serializer</param>
        /// <exception cref="ArgumentNullException">Thrown when the writer is null</exception>
        public override void WriteJson(JsonWriter writer, UUID value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteValue(value.ToString());
        }
    }
}