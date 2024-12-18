using System;
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

            UUID uuid = new UUID(timestamp, random);

            Assert.Equal(timestamp, uuid.Time);
            Assert.Equal(random, uuid.Random);
        }

        [Fact]
        public void Constructor_ShouldPreserveValues()
        {
            // Test min values
            UUID minUuid = new UUID(ulong.MinValue, ulong.MinValue);
            Assert.Equal(ulong.MinValue, minUuid.Time);
            Assert.Equal(ulong.MinValue, minUuid.Random);

            // Test max values
            UUID maxUuid = new UUID(ulong.MaxValue, ulong.MaxValue);
            Assert.Equal(ulong.MaxValue, maxUuid.Time);
            Assert.Equal(ulong.MaxValue, maxUuid.Random);
        }

        [Fact]
        public void Comparison_ShouldWorkCorrectly()
        {
            UUID uuid1 = new UUID(1, 1);
            UUID uuid2 = new UUID(1, 2);
            UUID uuid3 = new UUID(2, 1);

            // Equality
            Assert.True(uuid1.Equals(uuid1));
            Assert.False(uuid1.Equals(uuid2));
            Assert.False(uuid1.Equals(null));
            Assert.False(uuid1.Equals("not a UUID"));

            // CompareTo
            Assert.Equal(0, uuid1.CompareTo(uuid1));
            Assert.True(uuid1.CompareTo(uuid2) < 0);
            Assert.True(uuid2.CompareTo(uuid1) > 0);
            Assert.True(uuid1.CompareTo(uuid3) < 0);
            Assert.True(uuid3.CompareTo(uuid1) > 0);

            // Operators
            Assert.True(uuid1 == new UUID(1, 1));
            Assert.True(uuid1 != uuid2);
            Assert.True(uuid1 < uuid2);
            Assert.True(uuid2 > uuid1);
            Assert.True(uuid1 <= uuid2);
            Assert.True(uuid2 >= uuid1);
        }

        [Fact]
        public void GetHashCode_ShouldBeConsistent()
        {
            UUID uuid1 = new UUID(1, 1);
            UUID uuid2 = new UUID(1, 1);
            UUID uuid3 = new UUID(1, 2);

            Assert.Equal(uuid1.GetHashCode(), uuid2.GetHashCode());
            Assert.NotEqual(uuid1.GetHashCode(), uuid3.GetHashCode());
        }

        [Fact]
        public void ToString_ShouldBeConsistentWithParse()
        {
            UUID original = new UUID(123456789, 987654321);
            string str = original.ToString();
            UUID parsed = UUID.Parse(str);

            Assert.Equal(original, parsed);
            Assert.Equal(32, str.Length);
        }
    }
}
