using System;
using System.Text.Json;
using Xunit;

namespace UUIDSerializationSystemTests
{
    public class UUIDConverterTests
    {
        private readonly JsonSerializerOptions _options;

        public UUIDConverterTests()
        {
            _options = new JsonSerializerOptions
            {
                Converters = { new UUIDConverter() }
            };
        }

        [Fact]
        public void Serialize_UUID_ReturnsCorrectJsonString()
        {
            // Arrange
            UUID uuid = new();

            // Act
            string json = JsonSerializer.Serialize(uuid, _options);

            // Assert
            Assert.Equal($"\"{uuid}\"", json);
        }

        [Fact]
        public void Deserialize_ValidJsonString_ReturnsCorrectUUID()
        {
            // Arrange
            UUID uuid = new();
            string json = $"\"{uuid}\"";

            // Act
            UUID result = JsonSerializer.Deserialize<UUID>(json, _options);

            // Assert
            Assert.Equal(uuid, result);
        }

        [Fact]
        public void Deserialize_NullValue_ThrowsException()
        {
            // Arrange
            string json = "null";

            // Act & Assert
            JsonException exception = Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<UUID>(json, _options));
            Assert.Contains("Cannot convert null value to UUID", exception.Message);
        }

        [Fact]
        public void Deserialize_EmptyString_ThrowsException()
        {
            // Arrange
            string json = "\"\"";

            // Act & Assert
            JsonException exception = Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<UUID>(json, _options));
            Assert.Contains("Cannot convert empty string to UUID", exception.Message);
        }

        [Fact]
        public void Deserialize_InvalidType_ThrowsException()
        {
            // Arrange
            string json = "123";

            // Act & Assert
            JsonException exception = Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<UUID>(json, _options));
            Assert.Contains("Expected string value for UUID", exception.Message);
        }

        [Fact]
        public void Deserialize_InvalidFormat_ThrowsException()
        {
            // Arrange
            string json = "\"invalid-uuid-format\"";

            // Act & Assert
            JsonException exception = Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<UUID>(json, _options));
            Assert.Contains("Failed to parse UUID from string", exception.Message);
        }
    }
}