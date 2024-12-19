using System;
using System.Linq;
using Xunit;

namespace UUIDTests
{
    public class DatabaseOptimizationTests
    {
        [Fact]
        public void PostgreSQL_Optimized_UUID_Should_Be_Version7()
        {
            // Arrange & Act
            UUID uuid = UUID.NewDatabaseFriendly(DatabaseType.PostgreSQL);

            // Assert
            Assert.Equal(7, uuid.Version);
            Assert.True(Decoder.IsDatabaseOptimized(uuid));
        }

        [Fact]
        public void SQLServer_Optimized_UUID_Should_Be_Version8()
        {
            // Arrange & Act
            UUID uuid = UUID.NewDatabaseFriendly(DatabaseType.SQLServer);

            // Assert
            Assert.Equal(8, uuid.Version);
            Assert.True(Decoder.IsDatabaseOptimized(uuid));
        }

        [Fact]
        public void Sequential_Batch_Should_Maintain_Time_Order()
        {
            // Arrange
            const int batchSize = 100;

            // Act
            UUID[] batch = Toolkit.CreateSequentialBatch(batchSize);

            // Assert
            for (int i = 1; i < batchSize; i++)
            {
                Assert.True(batch[i].Time >= batch[i - 1].Time,
                    "UUIDs should maintain temporal order");
            }
        }

        [Theory]
        [InlineData(DatabaseType.PostgreSQL)]
        [InlineData(DatabaseType.SQLServer)]
        [InlineData(DatabaseType.SQLite)]
        public void Database_Friendly_UUIDs_Should_Have_Valid_Sequence(DatabaseType dbType)
        {
            // Arrange & Act
            UUID uuid = UUID.NewDatabaseFriendly(dbType);

            // Assert
            Assert.True(Decoder.TryGetSequence(uuid, out ushort sequence),
                $"Should extract sequence for {dbType}");
            Assert.True(sequence >= 0, "Sequence should be non-negative");
        }

        [Fact]
        public void Bulk_Generation_Should_Maintain_Uniqueness()
        {
            // Arrange
            const int count = 1000;
            UUID[] uuids = new UUID[count];

            // Act
            bool success = uuids.TryFill();

            // Assert
            Assert.True(success, "Bulk generation should succeed");
            int uniqueCount = uuids.Distinct().Count();
            Assert.Equal(count, uniqueCount);
        }

        [Fact]
        public void Sequential_UUIDs_Should_Have_Consistent_Version()
        {
            // Arrange
            const int batchSize = 10;

            // Act
            UUID[] postgresUUIDs = Toolkit.CreateSequentialBatch(batchSize, DatabaseType.PostgreSQL);
            UUID[] sqlServerUUIDs = Toolkit.CreateSequentialBatch(batchSize, DatabaseType.SQLServer);

            // Assert
            Assert.All(postgresUUIDs, uuid => Assert.Equal(7, uuid.Version));
            Assert.All(sqlServerUUIDs, uuid => Assert.Equal(8, uuid.Version));
        }
    }
}
