using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UUIDSerializationEntityTests
{
    public class ModelConfigurationBuilderExtensionsTests : IDisposable
    {
        private readonly SqliteConnection _connection;

        public ModelConfigurationBuilderExtensionsTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        [Fact]
        public async Task UseUUIDAsBinary_Should_Configure_All_UUID_Properties()
        {
            // Arrange
            DbContextOptions<TestDbContext> options = TestDbContext.CreateOptions(_connection);

            await using TestDbContext context = new(options, onConfigureConventions: builder => builder.UseUUIDAsBinary());
            await context.Database.EnsureCreatedAsync();

            // Act
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                ByteUUID = uuid,
                StringUUID = uuid,
                Base64UUID = uuid
            };

            context.TestEntities.Add(entity);
            await context.SaveChangesAsync();

            // Assert
            TestEntity loadedEntity = await context.TestEntities.FirstAsync();
            Assert.Equal(uuid, loadedEntity.ByteUUID);
            Assert.Equal(uuid, loadedEntity.StringUUID);
            Assert.Equal(uuid, loadedEntity.Base64UUID);

            // Verify all values are stored as bytes
            byte[] byteValue = await GetRawBytesFromDatabase("ByteUUID", entity.Id);
            byte[] stringValue = await GetRawBytesFromDatabase("StringUUID", entity.Id);
            byte[] base64Value = await GetRawBytesFromDatabase("Base64UUID", entity.Id);

            Assert.Equal(uuid.ToByteArray(), byteValue);
            Assert.Equal(uuid.ToByteArray(), stringValue);
            Assert.Equal(uuid.ToByteArray(), base64Value);
        }

        [Fact]
        public async Task UseUUIDAsString_Should_Configure_All_UUID_Properties()
        {
            // Arrange
            DbContextOptions<TestDbContext> options = TestDbContext.CreateOptions(_connection);

            await using TestDbContext context = new(options, onConfigureConventions: builder => builder.UseUUIDAsString());
            await context.Database.EnsureCreatedAsync();

            // Act
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                ByteUUID = uuid,
                StringUUID = uuid,
                Base64UUID = uuid
            };

            context.TestEntities.Add(entity);
            await context.SaveChangesAsync();

            // Assert
            TestEntity loadedEntity = await context.TestEntities.FirstAsync();
            Assert.Equal(uuid, loadedEntity.ByteUUID);
            Assert.Equal(uuid, loadedEntity.StringUUID);
            Assert.Equal(uuid, loadedEntity.Base64UUID);

            // Verify all values are stored as strings
            string byteValue = await GetRawValueFromDatabase("ByteUUID", entity.Id);
            string stringValue = await GetRawValueFromDatabase("StringUUID", entity.Id);
            string base64Value = await GetRawValueFromDatabase("Base64UUID", entity.Id);

            Assert.Equal(uuid.ToString(), byteValue);
            Assert.Equal(uuid.ToString(), stringValue);
            Assert.Equal(uuid.ToString(), base64Value);
        }

        [Fact]
        public async Task UseUUIDAsBase64_Should_Configure_All_UUID_Properties()
        {
            // Arrange
            DbContextOptions<TestDbContext> options = TestDbContext.CreateOptions(_connection);

            await using TestDbContext context = new(options, onConfigureConventions: builder => builder.UseUUIDAsBase64());
            await context.Database.EnsureCreatedAsync();

            // Act
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                ByteUUID = uuid,
                StringUUID = uuid,
                Base64UUID = uuid
            };

            context.TestEntities.Add(entity);
            await context.SaveChangesAsync();

            // Assert
            TestEntity loadedEntity = await context.TestEntities.FirstAsync();
            Assert.Equal(uuid, loadedEntity.ByteUUID);
            Assert.Equal(uuid, loadedEntity.StringUUID);
            Assert.Equal(uuid, loadedEntity.Base64UUID);

            // Verify all values are stored as base64
            string byteValue = await GetRawValueFromDatabase("ByteUUID", entity.Id);
            string stringValue = await GetRawValueFromDatabase("StringUUID", entity.Id);
            string base64Value = await GetRawValueFromDatabase("Base64UUID", entity.Id);

            Assert.Equal(uuid.ToBase64(), byteValue);
            Assert.Equal(uuid.ToBase64(), stringValue);
            Assert.Equal(uuid.ToBase64(), base64Value);
        }

        private async Task<string> GetRawValueFromDatabase(string columnName, int id)
        {
            using SqliteCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT {columnName} FROM TestEntities WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);

            object? result = await command.ExecuteScalarAsync();
            if (result is string text)
            {
                return text;
            }

            throw new InvalidOperationException($"Expected TEXT (string) for column '{columnName}' but got '{result?.GetType().FullName ?? "null"}'.");
        }

        private async Task<byte[]> GetRawBytesFromDatabase(string columnName, int id)
        {
            using SqliteCommand command = _connection.CreateCommand();
            command.CommandText = $"SELECT {columnName} FROM TestEntities WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);

            object? result = await command.ExecuteScalarAsync();
            if (result is byte[] bytes)
            {
                return bytes;
            }

            throw new InvalidOperationException($"Expected BLOB (byte[]) for column '{columnName}' but got '{result?.GetType().FullName ?? "null"}'.");
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}