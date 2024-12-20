using System;
using System.Threading;
using Xunit;

namespace UUIDTests
{
    public class UUIDComparisonTests
    {
        [Fact]
        public void IsOrderedAfter_WithDifferentTimestamps_ShouldReturnCorrectOrder()
        {
            // Arrange
            UUID earlier = UUID.New();
            Thread.Sleep(2); // Ensure different timestamp
            UUID later = UUID.New();

            // Assert
            Assert.True(later.IsOrderedAfter(earlier), "Later UUID should be ordered after earlier UUID");
            Assert.False(earlier.IsOrderedAfter(later), "Earlier UUID should not be ordered after later UUID");
        }

        [Fact]
        public void IsOrderedAfter_WithSameTimestamp_ShouldUseCounter()
        {
            // Arrange & Act
            UUID[] uuids = new UUID[5];
            for (int i = 0; i < uuids.Length; i++)
            {
                uuids[i] = UUID.New();
            }

            // Assert
            for (int i = 1; i < uuids.Length; i++)
            {
                Assert.True(uuids[i].IsOrderedAfter(uuids[i - 1]),
                    "UUIDs with same timestamp should be ordered by counter");
            }
        }

        [Fact]
        public void CompareTimestamps_ShouldReturnCorrectOrder()
        {
            // Arrange
            UUID earlier = UUID.New();
            Thread.Sleep(2); // Ensure different timestamp
            UUID later = UUID.New();

            // Assert
            Assert.True(UUID.CompareTimestamps(earlier, later) < 0,
                "Earlier timestamp should compare as less than later timestamp");
            Assert.True(UUID.CompareTimestamps(later, earlier) > 0,
                "Later timestamp should compare as greater than earlier timestamp");
            Assert.Equal(0, UUID.CompareTimestamps(earlier, earlier));
        }

        [Fact]
        public void AreMonotonicallyOrdered_WithNullOrEmptyArray_ShouldReturnTrue()
        {
            // Assert
            Assert.True(UUID.AreMonotonicallyOrdered(null),
                "Null array should be considered monotonically ordered");
            Assert.True(UUID.AreMonotonicallyOrdered(Array.Empty<UUID>()),
                "Empty array should be considered monotonically ordered");
            Assert.True(UUID.AreMonotonicallyOrdered(new[] { UUID.New() }),
                "Single-element array should be considered monotonically ordered");
        }

        [Fact]
        public void AreMonotonicallyOrdered_WithOrderedArray_ShouldReturnTrue()
        {
            // Arrange
            UUID[] uuids = new UUID[5];
            for (int i = 0; i < uuids.Length; i++)
            {
                uuids[i] = UUID.New();
                Thread.Sleep(1); // Ensure different timestamps
            }

            // Assert
            Assert.True(UUID.AreMonotonicallyOrdered(uuids),
                "Array with increasing timestamps should be monotonically ordered");
        }

        [Fact]
        public void AreMonotonicallyOrdered_WithUnorderedArray_ShouldReturnFalse()
        {
            // Arrange
            UUID[] uuids = new UUID[3];
            uuids[0] = UUID.New();
            Thread.Sleep(2);
            uuids[1] = UUID.New();
            uuids[2] = uuids[0]; // Make array unordered by reusing earlier UUID

            // Assert
            Assert.False(UUID.AreMonotonicallyOrdered(uuids),
                "Array with out-of-order UUIDs should not be monotonically ordered");
        }

        [Fact]
        public void GetMonotonicCounter_ShouldExtractLast12Bits()
        {
            // Arrange
            UUID uuid = new(0, 0xFFF); // Set all counter bits to 1
            UUID uuid2 = new(0, 0x000); // Set all counter bits to 0

            // Assert
            Assert.Equal((ushort)0xFFF, uuid.GetMonotonicCounter());
            Assert.Equal((ushort)0x000, uuid2.GetMonotonicCounter());
        }
    }
}