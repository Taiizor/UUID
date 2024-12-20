using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UUIDTests
{
    public class UUIDConstructorTests
    {
        [Fact]
        public void Constructor_ShouldSetTimestampAndRandom()
        {
            ulong timestamp = 123456789;
            ulong random = 987654321;

            UUID uuid = new(timestamp, random);

            Assert.Equal(random, uuid.Random);
            Assert.True(uuid.Time.ToUnixTimeMilliseconds() > 0);
        }

        [Fact]
        public void DefaultConstructor_ShouldCreateValidUUID()
        {
            UUID uuid = new();
            Assert.True(uuid.Time > DateTimeOffset.UnixEpoch);
            Assert.True(uuid.Random != 0);
        }

        [Fact]
        public void New_ShouldGenerateUniqueValues()
        {
            HashSet<UUID> set = new();
            for (int i = 0; i < 10000; i++)
            {
                UUID uuid = UUID.New();
                Assert.True(set.Add(uuid), "Generated UUID is not unique");
            }
        }

        [Fact]
        public void New_ShouldHaveMonotonicTimestamps()
        {
            UUID previous = UUID.New();
            for (int i = 0; i < 1000; i++)
            {
                UUID current = UUID.New();
                Assert.True(current.Time >= previous.Time);
                previous = current;
            }
        }

        [Fact]
        public void Parse_ShouldHandleValidInput()
        {
            UUID uuid = UUID.New();
            string str = uuid.ToString();
            UUID parsed = UUID.Parse(str);
            Assert.Equal(uuid, parsed);
        }

        [Fact]
        public void Parse_ShouldThrowOnInvalidInput()
        {
            Assert.Throws<ArgumentNullException>(() => UUID.Parse(null!));
            Assert.Throws<FormatException>(() => UUID.Parse("invalid"));
            Assert.Throws<FormatException>(() => UUID.Parse("1234567890123456789012345678901")); // 31 chars
            Assert.Throws<FormatException>(() => UUID.Parse("123456789012345678901234567890123")); // 33 chars
        }

        [Fact]
        public void TryParse_ShouldHandleValidAndInvalidInput()
        {
            UUID uuid = UUID.New();
            string str = uuid.ToString();

            Assert.True(UUID.TryParse(str, out UUID parsed));
            Assert.Equal(uuid, parsed);

            Assert.False(UUID.TryParse(null, out _));
            Assert.False(UUID.TryParse("invalid", out _));
            Assert.False(UUID.TryParse("1234567890123456789012345678901", out _)); // 31 chars
            Assert.False(UUID.TryParse("123456789012345678901234567890123", out _)); // 33 chars
        }

        [Fact]
        public void Base32_ShouldBeValid()
        {
            UUID uuid = UUID.New();
            string base32 = uuid.ToBase32();

            Assert.Equal(26, base32.Length);
            Assert.Matches("^[0-9A-Z]{26}$", base32);
        }

        [Fact]
        public void Base64_ShouldBeValid()
        {
            UUID uuid = UUID.New();
            string base64 = uuid.ToBase64();

            Assert.Equal(24, base64.Length);
            byte[] decoded = Convert.FromBase64String(base64);
            Assert.Equal(16, decoded.Length);
        }

        [Fact]
        public void ByteArray_Operations_ShouldBeValid()
        {
            UUID uuid = UUID.New();

            // ToByteArray
            byte[] bytes = uuid.ToByteArray();
            Assert.Equal(16, bytes.Length);

            // TryWriteBytes with byte[]
            byte[] destination = new byte[16];
            Assert.True(uuid.TryWriteBytes(destination));
            Assert.Equal(bytes, destination);

            // TryWriteBytes with null/small array
            Assert.False(uuid.TryWriteBytes(null));
            Assert.False(uuid.TryWriteBytes(new byte[15]));
        }

        [Fact]
        public void TryWriteStringify_ShouldHandleValidAndInvalidInput()
        {
            UUID uuid = UUID.New();
            char[] destination = new char[32];

            Assert.True(uuid.TryWriteStringify(destination));
            Assert.Equal(uuid.ToString(), new string(destination));

            Assert.False(uuid.TryWriteStringify(null));
            Assert.False(uuid.TryWriteStringify(new char[31]));
        }

        [Fact]
        public void Guid_Conversions_ShouldBeReversible()
        {
            UUID uuid = UUID.New();
            Guid guid = uuid.ToGuid();
            UUID convertedBack = UUID.FromGuid(guid);

            Assert.Equal(uuid, convertedBack);

            // Operator tests
            UUID fromImplicit = guid;  // Guid -> UUID implicit
            Guid toImplicit = uuid;    // UUID -> Guid implicit

            Assert.Equal(uuid, fromImplicit);
            Assert.Equal(guid, toImplicit);
        }

        [Fact]
        public void Comparison_Operators_ShouldWorkCorrectly()
        {
            UUID uuid1 = new(1, 1);
            UUID uuid2 = new(1, 2);
            UUID uuid3 = new(2, 1);

            // Equality
            Assert.True(uuid1 == new UUID(1, 1));
            Assert.False(uuid1 == uuid2);
            Assert.True(uuid1 != uuid2);
            Assert.False(uuid1 != new UUID(1, 1));

            // Comparison
            Assert.True(uuid1 < uuid2);
            Assert.True(uuid1 <= uuid2);
            Assert.True(uuid2 > uuid1);
            Assert.True(uuid2 >= uuid1);
            Assert.True(uuid1 < uuid3);
            Assert.True(uuid3 > uuid1);
        }

        [Fact]
        public void GetHashCode_ShouldBeConsistent()
        {
            UUID uuid1 = new(1, 1);
            UUID uuid2 = new(1, 1);
            UUID uuid3 = new(1, 2);

            Assert.Equal(uuid1.GetHashCode(), uuid2.GetHashCode());
            Assert.NotEqual(uuid1.GetHashCode(), uuid3.GetHashCode());
        }

        [Fact]
        public async Task New_ShouldBeThreadSafe()
        {
            HashSet<UUID> set = new();
            List<Task> tasks = new();
            object lockObj = new();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        UUID uuid = UUID.New();
                        lock (lockObj)
                        {
                            Assert.True(set.Add(uuid), "UUID collision detected in multi-threaded scenario");
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        [Fact]
        public void FromBase64_ShouldBeReversible()
        {
            UUID original = UUID.New();
            string base64 = original.ToBase64();
            UUID fromBase64 = UUID.FromBase64(base64);

            Assert.Equal(original, fromBase64);
        }

        [Fact]
        public void FromBase64_ShouldHandleInvalidInput()
        {
            Assert.Throws<ArgumentNullException>(() => UUID.FromBase64(null));
            Assert.Throws<FormatException>(() => UUID.FromBase64("invalid"));
            Assert.Throws<FormatException>(() => UUID.FromBase64("aaa=")); // Too short
            Assert.Throws<FormatException>(() => UUID.FromBase64(new string('A', 32))); // Too long
        }

        [Fact]
        public void TryFromBase64_ShouldHandleValidAndInvalidInput()
        {
            UUID original = UUID.New();
            string base64 = original.ToBase64();

            Assert.True(UUID.TryFromBase64(base64, out UUID result));
            Assert.Equal(original, result);

            Assert.False(UUID.TryFromBase64(null, out _));
            Assert.False(UUID.TryFromBase64("", out _));
            Assert.False(UUID.TryFromBase64("invalid", out _));
            Assert.False(UUID.TryFromBase64("aaa=", out _));
            Assert.False(UUID.TryFromBase64(new string('A', 32), out _));
        }

        [Fact]
        public void FromByteArray_ShouldBeReversible()
        {
            UUID original = UUID.New();
            byte[] bytes = original.ToByteArray();
            UUID fromBytes = UUID.FromByteArray(bytes);

            Assert.Equal(original, fromBytes);
        }

        [Fact]
        public void FromByteArray_ShouldHandleInvalidInput()
        {
            Assert.Throws<ArgumentNullException>(() => UUID.FromByteArray(null));
            Assert.Throws<ArgumentException>(() => UUID.FromByteArray(new byte[15])); // Too short
            Assert.Throws<ArgumentException>(() => UUID.FromByteArray(new byte[17])); // Too long
        }

        [Fact]
        public void TryFromByteArray_ShouldHandleValidAndInvalidInput()
        {
            UUID original = UUID.New();
            byte[] bytes = original.ToByteArray();

            Assert.True(UUID.TryFromByteArray(bytes, out UUID result));
            Assert.Equal(original, result);

            Assert.False(UUID.TryFromByteArray(null, out _));
            Assert.False(UUID.TryFromByteArray(new byte[15], out _)); // Too short
            Assert.False(UUID.TryFromByteArray(new byte[17], out _)); // Too long
        }

        [Fact]
        public async Task ToInt64_HandlesMultipleThreads()
        {
            // Arrange
            const int threadCount = 10;
            ConcurrentDictionary<long, bool> results = new();
            Task[] tasks = new Task[threadCount];

            // Act
            for (int i = 0; i < threadCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    UUID uuid = UUID.New();
                    long value = uuid.ToInt64();
                    return results.TryAdd(value, true);
                });
            }
            await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(threadCount, results.Count);
        }

        [Fact]
        public void ToInt64_SameUUID_ReturnsSameValue()
        {
            // Arrange
            UUID uuid = UUID.New();

            // Act
            long value1 = uuid.ToInt64();
            long value2 = uuid.ToInt64();
            long value3 = (long)uuid; // Implicit operator

            // Assert
            Assert.Equal(value1, value2);
            Assert.Equal(value2, value3);
        }

        [Fact]
        public void ToInt64_DifferentUUIDs_ReturnsDifferentValues()
        {
            // Arrange
            UUID uuid1 = UUID.New();
            UUID uuid2 = UUID.New();

            // Act
            long value1 = uuid1.ToInt64();
            long value2 = uuid2.ToInt64();

            // Assert
            Assert.NotEqual(value1, value2);
        }

        [Fact]
        public void ToInt64_PreservesTimeOrdering()
        {
            // Arrange
            UUID uuid1 = UUID.New();
            Thread.Sleep(10); // Ensure different timestamp
            UUID uuid2 = UUID.New();

            // Act
            long value1 = uuid1.ToInt64();
            long value2 = uuid2.ToInt64();

            // Assert
            Assert.True(value2 > value1, "Later UUID should convert to larger long value");
        }

        [Theory]
        [InlineData(0UL, 0UL)]  // Minimum values
        [InlineData(ulong.MaxValue, ulong.MaxValue)]  // Maximum values
        [InlineData(123456UL, 789012UL)]  // Random values
        public void ToInt64_WithSpecificValues_IsConsistent(ulong timestamp, ulong random)
        {
            // Arrange
            UUID uuid = new(timestamp, random);

            // Act
            long value1 = uuid.ToInt64();
            long value2 = uuid.ToInt64();

            // Assert
            Assert.Equal(value1, value2);
            Assert.True(value1 >= 0, "Converted long value should be non-negative");
        }

        [Fact]
        public void TestUUIDVersion()
        {
            // Arrange & Act
            UUID uuid = UUID.New();

            // Assert
            Assert.Equal(0x07, uuid.Version); // UUIDv7
        }

        [Fact]
        public void TestUUIDVariant()
        {
            // Arrange & Act
            UUID uuid = UUID.New();

            // Assert
            Assert.Equal(0x02, uuid.Variant); // RFC 4122
        }

        [Fact]
        public void TestMonotonicOrderingInSameMillisecond()
        {
            // Arrange
            const int count = 1000;
            List<UUID> uuids = new();

            // Act - Generate UUIDs as fast as possible to get many in the same millisecond
            for (int i = 0; i < count; i++)
            {
                uuids.Add(UUID.New());
            }

            // Assert
            for (int i = 1; i < uuids.Count; i++)
            {
                Assert.True(uuids[i] > uuids[i - 1],
                    "UUIDs should maintain monotonic ordering even within the same millisecond");
            }
        }

        [Fact]
        public void TestUUIDFormatStructure()
        {
            // Arrange
            UUID uuid = UUID.New();
            string uuidStr = uuid.ToString();

            // Act & Assert
            // Check timestamp portion (first 48 bits = 12 hex chars)
            string timestampHex = uuidStr[..12];
            Assert.Equal(12, timestampHex.Length);
            Assert.True(long.TryParse(timestampHex, NumberStyles.HexNumber, null, out _));

            // Check version (13th character should be 7)
            Assert.Equal('7', uuidStr[12]);

            // Check counter portion (next 12 bits = 3 hex chars)
            string counterHex = uuidStr.Substring(13, 3);
            Assert.Equal(3, counterHex.Length);
            Assert.True(int.TryParse(counterHex, NumberStyles.HexNumber, null, out int counter));
            Assert.True(counter <= 0xFFF);

            // Check remaining random bits
            string randomHex = uuidStr[16..];
            Assert.Equal(16, randomHex.Length);
            Assert.True(long.TryParse(randomHex, NumberStyles.HexNumber, null, out _));
        }

        [Fact]
        public void TestThreadSafetyWithMonotonicCounter()
        {
            // Arrange
            const int threadCount = 10;
            const int uuidsPerThread = 1000;
            ConcurrentBag<UUID> allUuids = new ConcurrentBag<UUID>();
            List<Task> tasks = new List<Task>();

            // Act
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < uuidsPerThread; j++)
                    {
                        allUuids.Add(UUID.New());
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            // Assert
            List<UUID> sortedUuids = allUuids.OrderBy(u => u).ToList();

            // Check for uniqueness
            Assert.Equal(threadCount * uuidsPerThread, sortedUuids.Distinct().Count());

            // Check for monotonic ordering
            for (int i = 1; i < sortedUuids.Count; i++)
            {
                Assert.True(sortedUuids[i] > sortedUuids[i - 1],
                    "UUIDs should maintain monotonic ordering across threads");
            }
        }

        [Fact]
        public void TestTimeAccuracy()
        {
            // Arrange
            DateTimeOffset before = DateTimeOffset.UtcNow;
            Thread.Sleep(1); // Ensure we're in a new millisecond

            // Act
            UUID uuid = UUID.New();

            Thread.Sleep(1); // Ensure we're in a new millisecond
            DateTimeOffset after = DateTimeOffset.UtcNow;

            // Assert
            Assert.True(uuid.Time >= before);
            Assert.True(uuid.Time <= after);
        }
    }
}