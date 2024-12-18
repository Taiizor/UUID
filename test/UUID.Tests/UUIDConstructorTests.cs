using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UUIDTests
{
    public class UUIDConstructorTests
    {
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
        }

        [Fact]
        public void Base32_ShouldBeReversible()
        {
            UUID uuid = UUID.New();
            string base32 = uuid.ToBase32();

            Assert.Equal(26, base32.Length);
            Assert.Matches("^[0-9A-Z]{26}$", base32);
        }

        [Fact]
        public void Base64_AndByteArray_ShouldBeReversible()
        {
            UUID uuid = UUID.New();
            byte[] bytes = uuid.ToByteArray();
            string base64 = uuid.ToBase64();

            Assert.Equal(16, bytes.Length);
            Assert.Equal(24, base64.Length);

            byte[] destination = new byte[16];
            Assert.True(uuid.TryWriteBytes(destination));
            Assert.Equal(bytes, destination);
        }

        [Fact]
        public void Guid_Conversion_ShouldBeReversible()
        {
            UUID uuid = UUID.New();
            Guid guid = uuid.ToGuid();
            UUID convertedBack = UUID.FromGuid(guid);

            Assert.Equal(uuid, convertedBack);

            // Operator tests
            UUID fromImplicit = guid;
            Guid toExplicit = (Guid)uuid;

            Assert.Equal(uuid, fromImplicit);
            Assert.Equal(guid, toExplicit);
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
