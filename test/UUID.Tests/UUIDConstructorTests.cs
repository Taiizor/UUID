using System;
using System.Collections.Generic;
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
    }
}