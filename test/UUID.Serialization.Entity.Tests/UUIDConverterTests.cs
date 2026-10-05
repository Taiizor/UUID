using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UUIDSerializationEntityTests
{
    public class UUIDConverterTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly TestDbContext _context;

        public UUIDConverterTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            DbContextOptions<TestDbContext> options = TestDbContext.CreateOptions(_connection);

            _context = new TestDbContext(options, onModelCreating: builder =>
            {
                builder.Entity<TestEntity>(entity =>
                {
                    entity.Property(e => e.ByteUUID).UseUUIDAsBinary<TestEntity>();
                    entity.Property(e => e.StringUUID).UseUUIDAsString<TestEntity>();
                    entity.Property(e => e.Base64UUID).UseUUIDAsBase64<TestEntity>();
                });
            });
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Should_Save_And_Load_UUID_As_Byte()
        {
            // Arrange
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                ByteUUID = uuid
            };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            TestEntity loadedEntity = await _context.TestEntities.FirstAsync();

            // Assert
            Assert.Equal(uuid, loadedEntity.ByteUUID);
            Assert.Equal(uuid.ToByteArray(), await GetRawBytesFromDatabase("ByteUUID", entity.Id));
        }

        [Fact]
        public async Task Should_Save_And_Load_Multiple_UUID_As_Byte()
        {
            // Arrange
            UUID uuid1 = UUID.New();
            UUID uuid2 = UUID.New();

            TestEntity entity1 = new() { ByteUUID = uuid1 };
            TestEntity entity2 = new() { ByteUUID = uuid2 };

            // Act
            _context.TestEntities.AddRange(entity1, entity2);
            await _context.SaveChangesAsync();

            await _context.Entry(entity1).ReloadAsync();
            await _context.Entry(entity2).ReloadAsync();

            // Assert
            Assert.Equal(uuid1, entity1.ByteUUID);
            Assert.Equal(uuid2, entity2.ByteUUID);
            Assert.Equal(uuid1.ToByteArray(), await GetRawBytesFromDatabase("ByteUUID", entity1.Id));
            Assert.Equal(uuid2.ToByteArray(), await GetRawBytesFromDatabase("ByteUUID", entity2.Id));
        }

        [Fact]
        public async Task Should_Update_UUID_As_Byte()
        {
            // Arrange
            UUID originalUuid = UUID.New();
            UUID New = UUID.New();

            TestEntity entity = new() { ByteUUID = originalUuid };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            entity.ByteUUID = New;
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();

            // Assert
            Assert.Equal(New, entity.ByteUUID);
            Assert.Equal(New.ToByteArray(), await GetRawBytesFromDatabase("ByteUUID", entity.Id));
        }

        [Fact]
        public async Task Should_Save_And_Load_UUID_As_String()
        {
            // Arrange
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                StringUUID = uuid
            };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            TestEntity loadedEntity = await _context.TestEntities.FirstAsync();

            // Assert
            Assert.Equal(uuid, loadedEntity.StringUUID);
            Assert.Equal(uuid.ToString(), await GetRawValueFromDatabase("StringUUID", entity.Id));
        }

        [Fact]
        public async Task Should_Save_And_Load_Multiple_UUID_As_String()
        {
            // Arrange
            UUID uuid1 = UUID.New();
            UUID uuid2 = UUID.New();

            TestEntity entity1 = new() { StringUUID = uuid1 };
            TestEntity entity2 = new() { StringUUID = uuid2 };

            // Act
            _context.TestEntities.AddRange(entity1, entity2);
            await _context.SaveChangesAsync();

            await _context.Entry(entity1).ReloadAsync();
            await _context.Entry(entity2).ReloadAsync();

            // Assert
            Assert.Equal(uuid1, entity1.StringUUID);
            Assert.Equal(uuid2, entity2.StringUUID);
            Assert.Equal(uuid1.ToString(), await GetRawValueFromDatabase("StringUUID", entity1.Id));
            Assert.Equal(uuid2.ToString(), await GetRawValueFromDatabase("StringUUID", entity2.Id));
        }

        [Fact]
        public async Task Should_Update_UUID_As_String()
        {
            // Arrange
            UUID originalUuid = UUID.New();
            UUID New = UUID.New();

            TestEntity entity = new() { StringUUID = originalUuid };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            entity.StringUUID = New;
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();

            // Assert
            Assert.Equal(New, entity.StringUUID);
            Assert.Equal(New.ToString(), await GetRawValueFromDatabase("StringUUID", entity.Id));
        }

        [Fact]
        public async Task Should_Save_And_Load_UUID_As_Base64()
        {
            // Arrange
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                Base64UUID = uuid
            };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();
            TestEntity loadedEntity = await _context.TestEntities.FirstAsync();

            // Assert
            Assert.Equal(uuid, loadedEntity.Base64UUID);
            Assert.Equal(uuid.ToBase64(), await GetRawValueFromDatabase("Base64UUID", entity.Id));
        }

        [Fact]
        public async Task Should_Save_And_Load_Multiple_UUID_As_Base64()
        {
            // Arrange
            UUID uuid1 = UUID.New();
            UUID uuid2 = UUID.New();

            TestEntity entity1 = new() { Base64UUID = uuid1 };
            TestEntity entity2 = new() { Base64UUID = uuid2 };

            // Act
            _context.TestEntities.AddRange(entity1, entity2);
            await _context.SaveChangesAsync();

            await _context.Entry(entity1).ReloadAsync();
            await _context.Entry(entity2).ReloadAsync();

            // Assert
            Assert.Equal(uuid1, entity1.Base64UUID);
            Assert.Equal(uuid2, entity2.Base64UUID);
            Assert.Equal(uuid1.ToBase64(), await GetRawValueFromDatabase("Base64UUID", entity1.Id));
            Assert.Equal(uuid2.ToBase64(), await GetRawValueFromDatabase("Base64UUID", entity2.Id));
        }

        [Fact]
        public async Task Should_Update_UUID_As_Base64()
        {
            // Arrange
            UUID originalUuid = UUID.New();
            UUID New = UUID.New();

            TestEntity entity = new() { Base64UUID = originalUuid };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            entity.Base64UUID = New;
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();

            // Assert
            Assert.Equal(New, entity.Base64UUID);
            Assert.Equal(New.ToBase64(), await GetRawValueFromDatabase("Base64UUID", entity.Id));
        }

        [Fact]
        public async Task Should_Save_And_Load_All_UUID_Types()
        {
            // Arrange
            UUID uuid = UUID.New();
            TestEntity entity = new()
            {
                ByteUUID = uuid,
                StringUUID = uuid,
                Base64UUID = uuid
            };

            // Act
            _context.TestEntities.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).ReloadAsync();

            // Assert
            Assert.Equal(uuid, entity.ByteUUID);
            Assert.Equal(uuid, entity.StringUUID);
            Assert.Equal(uuid, entity.Base64UUID);
            Assert.Equal(uuid.ToByteArray(), await GetRawBytesFromDatabase("ByteUUID", entity.Id));
            Assert.Equal(uuid.ToString(), await GetRawValueFromDatabase("StringUUID", entity.Id));
            Assert.Equal(uuid.ToBase64(), await GetRawValueFromDatabase("Base64UUID", entity.Id));
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
            _context.Dispose();
            _connection.Dispose();
        }
    }
}