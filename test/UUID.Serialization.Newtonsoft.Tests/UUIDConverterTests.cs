using Newtonsoft.Json;
using System;
using Xunit;

namespace UUIDSerializationNewtonsoftTests
{
    public class UUIDConverterTests
    {
        private readonly JsonSerializerSettings _settings;

        public UUIDConverterTests()
        {
            _settings = new JsonSerializerSettings
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
            string json = JsonConvert.SerializeObject(uuid, _settings);

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
            UUID result = JsonConvert.DeserializeObject<UUID>(json, _settings);

            // Assert
            Assert.Equal(uuid, result);
        }

        [Fact]
        public void Deserialize_NullValue_ThrowsException()
        {
            // Arrange
            string json = "null";

            // Act & Assert
            JsonReaderException exception = Assert.Throws<JsonReaderException>(() =>
                JsonConvert.DeserializeObject<UUID>(json, _settings));
            Assert.Contains("Cannot convert null value to UUID", exception.Message);
        }

        [Fact]
        public void Deserialize_EmptyString_ThrowsException()
        {
            // Arrange
            string json = "\"\"";

            // Act & Assert
            JsonReaderException exception = Assert.Throws<JsonReaderException>(() =>
                JsonConvert.DeserializeObject<UUID>(json, _settings));
            Assert.Contains("Cannot convert empty string to UUID", exception.Message);
        }

        [Fact]
        public void Deserialize_InvalidType_ThrowsException()
        {
            // Arrange
            string json = "123";

            // Act & Assert
            JsonReaderException exception = Assert.Throws<JsonReaderException>(() =>
                JsonConvert.DeserializeObject<UUID>(json, _settings));
            Assert.Contains("Expected string value for UUID", exception.Message);
        }

        [Fact]
        public void Deserialize_InvalidFormat_ThrowsException()
        {
            // Arrange
            string json = "\"invalid-uuid-format\"";

            // Act & Assert
            JsonReaderException exception = Assert.Throws<JsonReaderException>(() =>
                JsonConvert.DeserializeObject<UUID>(json, _settings));
            Assert.Contains("Failed to parse UUID from string", exception.Message);
        }

        [Fact]
        public void Serialize_WithJsonConverterAttribute_WorksCorrectly()
        {
            // Arrange
            TestModel model = new() { Id = new UUID() };

            // Act
            string json = JsonConvert.SerializeObject(model);
            TestModel? deserializedModel = JsonConvert.DeserializeObject<TestModel>(json);

            // Assert
            Assert.Equal(model.Id, deserializedModel.Id);
        }

        private class TestModel
        {
            [JsonConverter(typeof(UUIDConverter))]
            public UUID Id { get; set; }
        }
    }
}