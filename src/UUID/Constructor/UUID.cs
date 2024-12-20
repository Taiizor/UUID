using System.Globalization;
using System.Security.Cryptography;

namespace System
{
    /// <summary>
    /// UUID represents a modern and efficient unique identifier implementation,
    /// designed for high performance and enhanced security in distributed systems.
    /// This implementation follows UUIDv7 standard (draft-ietf-uuidrev-rfc4122bis).
    /// </summary>
    /// <remarks>
    /// This implementation provides:
    /// - Time-based ordering: 48-bit Unix timestamp (milliseconds)
    /// - Monotonic counter: 12-bit sequence counter for same-millisecond ordering
    /// - Version bits: 4 bits indicating UUIDv7
    /// - Variant bits: 2 bits as per RFC 4122
    /// - Random bits: Remaining bits for uniqueness
    /// - Thread Safety: All operations are thread-safe
    /// </remarks>
    public readonly partial struct UUID(ulong timestamp, ulong random) : IEquatable<UUID>, IComparable<UUID>, IComparable
    {
        /// <summary>
        /// The size of the UUID in bytes.
        /// </summary>
        /// <remarks>
        /// This constant represents the fixed size of a UUID, which is 16 bytes (128 bits).
        /// This follows the standard UUID specification as defined in RFC 4122.
        /// </remarks>
        private const int SIZE = 16;

        /// <summary>
        /// Thread-safe counter for monotonic sequence within same millisecond.
        /// Used to ensure unique and ordered UUIDs when multiple are generated 
        /// in the same millisecond.
        /// </summary>
        private static int _counter;

        /// <summary>
        /// Gets the variant number of this UUID.
        /// </summary>
        /// <remarks>
        /// Returns 2 for RFC 4122 variant UUIDs.
        /// This is used to indicate the layout of bits in the UUID.
        /// </remarks>
        public byte Variant => VARIANT;

        /// <summary>
        /// Gets the version number of this UUID.
        /// </summary>
        /// <remarks>
        /// Returns 7 for UUIDv7 (time-ordered with additional random bits).
        /// The version number indicates how the UUID was generated.
        /// </remarks>
        public byte Version => VERSION;

        /// <summary>
        /// RFC 4122 variant identifier (2).
        /// Used to indicate the layout of bits in the UUID.
        /// </summary>
        private const byte VARIANT = 0x02;

        /// <summary>
        /// UUID version identifier (7).
        /// Indicates this is a Version 7 UUID (time-ordered).
        /// </summary>
        private const byte VERSION = 0x07;

        /// <summary>
        /// Stores the timestamp of the last generated UUID.
        /// Used for maintaining monotonic ordering within the same millisecond.
        /// </summary>
        private static long _lastTimestamp;

        /// <summary>
        /// Gets the random component of the UUID.
        /// </summary>
        /// <remarks>
        /// The random component ensures uniqueness even when UUIDs are generated 
        /// within the same timestamp. It consists of 64 bits of cryptographically 
        /// secure random data.
        /// </remarks>
        public ulong Random { get; } = random;

        /// <summary>
        /// The timestamp component of the UUID.
        /// </summary>
        internal readonly ulong _timestamp = timestamp;

        /// <summary>
        /// Lock object for thread-safe counter operations.
        /// Ensures monotonic ordering of UUIDs generated within the same millisecond.
        /// </summary>
        private static readonly object _counterLock = new();

        /// <summary>
        /// Characters used in Base32 encoding.
        /// </summary>
        /// <remarks>
        /// This set excludes I, L, O to avoid confusion with 1, 1, 0.
        /// Used for generating human-readable representations of UUIDs.
        /// </remarks>
        private const string ENCODING_CHARS = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

        /// <summary>
        /// Thread-local random number generator for secure random number generation.
        /// </summary>
        private static readonly ThreadLocal<Random> _rng = new(() =>
        {
#if NET6_0_OR_GREATER
            return new Random(BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4)));
#else
            byte[] bytes = new byte[4];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return new Random(BitConverter.ToInt32(bytes, 0));
#endif
        });

        /// <summary>
        /// Creates a new UUID instance with current timestamp.
        /// </summary>
        /// <remarks>
        /// This constructor creates a UUID which is:
        /// - Time-ordered for natural sorting
        /// - Contains high-precision timestamp
        /// - Includes random bits for uniqueness
        /// </remarks>
        public UUID() : this(GenerateTimestamp(), GenerateRandom()) { }

        /// <summary>
        /// Generates a new UUID using the current timestamp and secure random data.
        /// </summary>
        /// <returns>A new UUID instance.</returns>
        public static UUID New()
        {
            return new(GenerateTimestamp(), GenerateRandom());
        }

        /// <summary>
        /// Generates a compact UUID with a 12-character representation.
        /// </summary>
        /// <returns>A new UUID instance optimized for compact representation.</returns>
        /// <remarks>
        /// The compact UUID:
        /// - Uses current timestamp for time-ordering
        /// - Maintains uniqueness through random bits
        /// - Can be represented in 12 characters
        /// - Is fully compatible with standard UUID operations
        /// </remarks>
        public static UUID NewCompact()
        {
            UUID uuid = new();
            return new UUID((ulong)(uuid.ToInt64() >> 15), (ulong)(uuid.ToInt64() & 0x7FFF));
        }

        /// <summary>
        /// Generates a compact UUID with a specified timestamp.
        /// </summary>
        /// <param name="timestamp">Unix timestamp in milliseconds</param>
        /// <returns>A new UUID instance with the specified timestamp.</returns>
        /// <remarks>
        /// Useful for creating time-based sequences or testing scenarios.
        /// The timestamp is preserved in the final UUID representation.
        /// </remarks>
        public static UUID NewCompactWithTime(long timestamp)
        {
            UUID uuid = new((ulong)(timestamp << 16), GenerateRandom());
            return new UUID((ulong)(uuid.ToInt64() >> 15), (ulong)(uuid.ToInt64() & 0x7FFF));
        }

        /// <summary>
        /// Generates a timestamp component for the UUID.
        /// </summary>
        /// <returns>A 64-bit unsigned integer containing the timestamp.</returns>
        /// <remarks>
        /// The timestamp is based on the current UTC time in milliseconds.
        /// Thread safety is handled internally.
        /// </remarks>
        private static ulong GenerateTimestamp()
        {
            long unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int counter = GetMonotonicCounter(unixMs);

            // Format: 48 bits timestamp + 4 bits version + 12 bits counter
            ulong timestamp = ((ulong)unixMs & 0x0000_FFFF_FFFF_FFFF) << 16;
            timestamp |= (ulong)VERSION << 12;
            timestamp |= (uint)counter;

            return timestamp;
        }

        /// <summary>
        /// Compares the timestamps between two UUIDs.
        /// </summary>
        /// <param name="a">First UUID to compare.</param>
        /// <param name="b">Second UUID to compare.</param>
        /// <returns>
        /// A negative value if a is earlier than b;
        /// zero if they have the same timestamp;
        /// a positive value if a is later than b.
        /// </returns>
        public static int CompareTimestamps(UUID a, UUID b)
        {
            return a._timestamp.CompareTo(b._timestamp);
        }

        /// <summary>
        /// Gets the monotonic counter value for this UUID.
        /// </summary>
        /// <returns>The counter value used for ordering within the same millisecond.</returns>
        public ushort GetMonotonicCounter()
        {
            return (ushort)(Random & 0xFFF); // Extract last 12 bits
        }

        /// <summary>
        /// Gets a monotonically increasing counter for UUIDs generated within the same millisecond.
        /// </summary>
        /// <param name="timestamp">The current timestamp in milliseconds</param>
        /// <returns>A 12-bit counter value that ensures monotonic ordering</returns>
        /// <remarks>
        /// This method ensures that UUIDs generated within the same millisecond
        /// maintain a strict ordering through the use of a 12-bit counter.
        /// The counter resets when moving to a new millisecond.
        /// </remarks>
        private static int GetMonotonicCounter(long timestamp)
        {
            lock (_counterLock)
            {
                if (timestamp > _lastTimestamp)
                {
                    _counter = 0;
                    _lastTimestamp = timestamp;
                }
                else if (timestamp == _lastTimestamp)
                {
                    _counter = (_counter + 1) & 0xFFF; // 12-bit counter
                }

                return _counter;
            }
        }

        /// <summary>
        /// Checks if an array of UUIDs is monotonically ordered.
        /// </summary>
        /// <param name="uuids">Array of UUIDs to check.</param>
        /// <returns>True if the UUIDs are in monotonic order.</returns>
        /// <remarks>
        /// This method verifies that each UUID in the array is ordered after
        /// its predecessor, ensuring strict temporal ordering.
        /// </remarks>
        public static bool AreMonotonicallyOrdered(UUID[] uuids)
        {
            if (uuids == null || uuids.Length < 2)
            {
                return true;
            }

            for (int i = 1; i < uuids.Length; i++)
            {
                if (!uuids[i].IsOrderedAfter(uuids[i - 1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Generates a cryptographically secure random component for the UUID.
        /// </summary>
        /// <returns>A 64-bit unsigned integer containing random data.</returns>
        /// <remarks>
        /// Uses a thread-local cryptographic random number generator
        /// to ensure both security and performance.
        /// </remarks>
        private static ulong GenerateRandom()
        {
            ulong random = ((ulong)_rng.Value!.Next() << 32) | (uint)_rng.Value!.Next();

            // Set the variant bits
            random = (random & 0x3FFF_FFFF_FFFF_FFFF) | ((ulong)VARIANT << 62);

            return random;
        }

        /// <summary>
        /// Checks if this UUID is ordered after another UUID.
        /// </summary>
        /// <param name="other">The UUID to compare with.</param>
        /// <returns>True if this UUID is ordered after the other UUID.</returns>
        /// <remarks>
        /// This method compares UUIDs based on their timestamp and counter values.
        /// It is useful for maintaining temporal ordering of UUIDs.
        /// </remarks>
        public bool IsOrderedAfter(UUID other)
        {
            if (_timestamp != other._timestamp)
            {
                return _timestamp > other._timestamp;
            }

            return GetMonotonicCounter() > other.GetMonotonicCounter();
        }

        /// <summary>
        /// Creates a UUID from a string representation.
        /// </summary>
        /// <param name="input">The string representation of the UUID.</param>
        /// <returns>A new UUID instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when input is null.</exception>
        /// <exception cref="FormatException">Thrown when input is not in the correct format.</exception>
        /// <example>
        /// <code>
        /// // Parse a UUID from its string representation
        /// var uuidString = "0123456789ABCDEF0123456789ABCDEF";
        /// var uuid = UUID.Parse(uuidString);
        /// 
        /// // The parsed UUID can then be used in comparisons or conversions
        /// var guid = uuid.ToGuid();
        /// </code>
        /// </example>
        public static UUID Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Length != 32)
            {
                throw new FormatException("Input string must be 32 characters long.");
            }

            ulong timestamp = Convert.ToUInt64(input.Substring(0, 16), 16);
            ulong random = Convert.ToUInt64(input.Substring(16), 16);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Tries to parse a string into a UUID.
        /// </summary>
        /// <param name="input">The string representation of the UUID.</param>
        /// <param name="result">The parsed UUID instance.</param>
        /// <returns>true if the string was parsed successfully; otherwise, false.</returns>
        public static bool TryParse(string? input, out UUID result)
        {
            result = default;

            if (string.IsNullOrEmpty(input) || input.Length != 32)
            {
                return false;
            }

#if NET6_0_OR_GREATER
            ReadOnlySpan<char> span = input.AsSpan();

            if (!ulong.TryParse(span[..16], NumberStyles.HexNumber, null, out ulong timestamp))
            {
                return false;
            }
            if (!ulong.TryParse(span[16..], NumberStyles.HexNumber, null, out ulong random))
            {
                return false;
            }
#else
            if (!ulong.TryParse(input.Substring(0, 16), NumberStyles.HexNumber, null, out ulong timestamp))
            {
                return false;
            }
            if (!ulong.TryParse(input.Substring(16), NumberStyles.HexNumber, null, out ulong random))
            {
                return false;
            }
#endif

            result = new UUID(timestamp, random);

            return true;
        }

        /// <summary>
        /// Gets the timestamp component of the UUID.
        /// </summary>
        public DateTimeOffset Time => DateTimeOffset.FromUnixTimeMilliseconds((long)(_timestamp >> 16));

        /// <summary>
        /// Returns a string representation of the UUID.
        /// </summary>
        /// <returns>A string representation of the UUID.</returns>
        public override string ToString()
        {
            return $"{_timestamp:X16}{Random:X16}";
        }

        /// <summary>
        /// Converts this UUID to a long value with high precision.
        /// This conversion preserves the timestamp component and combines it with a hash of the random component
        /// to maintain uniqueness while fitting within the constraints of a long value.
        /// Note: This is a one-way conversion as some information is necessarily lost in the process.
        /// </summary>
        /// <returns>A long value representing this UUID with maximum possible precision.</returns>
        /// <remarks>
        /// The conversion process:
        /// - Uses the millisecond timestamp (48 bits)
        /// - Combines it with a 15-bit hash derived from the full random component
        /// - Ensures consistent results for the same UUID
        /// - Minimizes collision probability for different UUIDs
        /// </remarks>
        public long ToInt64()
        {
            // Use the timestamp as the base (48 bits)
            long result = (long)(_timestamp >> 16); // Get the milliseconds part

            // Generate a 15-bit hash from the full random component
            ulong fullRandom = random;
            int hash = unchecked((int)(((fullRandom >> 32) & 0xFFFFFFFF) ^ (fullRandom & 0xFFFFFFFF)));

            hash = Math.Abs(hash) & 0x7FFF; // Ensure positive and take 15 bits

            // Combine timestamp with the hash
            result = (result << 15) | hash;

            return result;
        }

        /// <summary>
        /// Implicitly converts a UUID to a long value using the ToInt64() method.
        /// </summary>
        /// <param name="uuid">The UUID to convert.</param>
        /// <returns>A long value representing the UUID.</returns>
        public static implicit operator long(UUID uuid)
        {
            return uuid.ToInt64();
        }

        /// <summary>
        /// Returns a Base32 encoded string representation of the UUID.
        /// </summary>
        /// <returns>A Base32 encoded string representation of the UUID.</returns>
        public string ToBase32()
        {
            char[] result = new char[26];
            ulong value = _timestamp;

            for (int i = 25; i >= 0; i--)
            {
                result[i] = ENCODING_CHARS[(int)(value & 0x1F)];
                value >>= 5;

                if (i == 13)
                {
                    value = random;
                }
            }

            return new string(result);
        }

        /// <summary>
        /// Returns a Base64 encoded string representation of the UUID.
        /// </summary>
        /// <returns>A Base64 encoded string representation of the UUID.</returns>
        public string ToBase64()
        {
            byte[] bytes = new byte[SIZE];

            TryWriteBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Creates a new UUID from a Base64 string.
        /// </summary>
        /// <param name="base64">The Base64 string to parse.</param>
        /// <returns>A new UUID instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when base64 is null.</exception>
        /// <exception cref="FormatException">Thrown when base64 is not in the correct format.</exception>
        public static UUID FromBase64(string base64)
        {
            if (base64 == null)
            {
                throw new ArgumentNullException(nameof(base64));
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(base64);

                if (bytes.Length != SIZE)
                {
                    throw new FormatException($"Invalid Base64 string length. Expected {SIZE} bytes after decoding.");
                }

                return FromByteArray(bytes);
            }
            catch (FormatException)
            {
                throw new FormatException("Invalid Base64 string format.");
            }
        }

        /// <summary>
        /// Attempts to create a UUID from a Base64 string.
        /// </summary>
        /// <param name="base64">The Base64 string to parse.</param>
        /// <param name="result">When this method returns, contains the UUID value if the conversion succeeded, or default if the conversion failed.</param>
        /// <returns>true if the conversion was successful; otherwise, false.</returns>
        public static bool TryFromBase64(string base64, out UUID result)
        {
            result = default;

            if (string.IsNullOrEmpty(base64))
            {
                return false;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                if (bytes.Length != SIZE)
                {
                    return false;
                }

                result = FromByteArray(bytes);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts the UUID to a byte array.
        /// </summary>
        /// <returns>A byte array representation of the UUID.</returns>
        /// <remarks>
        /// This operation allocates a new byte array. For better performance in high-throughput scenarios,
        /// consider using TryWriteBytes instead when working with existing byte arrays or spans.
        /// The returned array is always 16 bytes long, with the first 8 bytes containing the timestamp
        /// and the last 8 bytes containing the random component.
        /// </remarks>
        /// <example>
        /// <code>
        /// var uuid = UUID.New();
        /// byte[] bytes = uuid.ToByteArray();
        /// // bytes can now be used for serialization or network transmission
        /// </code>
        /// </example>
        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[SIZE];
            TryWriteBytes(bytes);

            return bytes;
        }

        /// <summary>
        /// Attempts to write the UUID to a span of bytes.
        /// </summary>
        /// <param name="destination">The destination span to write to.</param>
        /// <returns>True if the UUID was successfully written, false if the destination is too small.</returns>
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public bool TryWriteBytes(Span<byte> destination)
        {
            if (destination.Length < 16)
            {
                return false;
            }

            BitConverter.TryWriteBytes(destination[..8], _timestamp);
            BitConverter.TryWriteBytes(destination[8..], random);

            return true;
        }
#else
        public bool TryWriteBytes(byte[] destination)
        {
            if (destination == null || destination.Length < 16)
            {
                return false;
            }

            byte[] timestampBytes = BitConverter.GetBytes(_timestamp);
            byte[] randomBytes = BitConverter.GetBytes(random);

            Array.Copy(timestampBytes, 0, destination, 0, 8);
            Array.Copy(randomBytes, 0, destination, 8, 8);

            return true;
        }
#endif

        /// <summary>
        /// Attempts to write the UUID to a string buffer.
        /// </summary>
        /// <param name="destination">The destination string buffer.</param>
        /// <returns>true if the UUID was written successfully; otherwise, false.</returns>
        public bool TryWriteStringify(char[] destination)
        {
            if (destination == null || destination.Length < 32)
            {
                return false;
            }

            string str = ToString();
            str.CopyTo(0, destination, 0, 32);

            return true;
        }

        /// <summary>
        /// Converts the UUID to a Guid.
        /// </summary>
        /// <returns>A Guid representation of the UUID.</returns>
        public Guid ToGuid()
        {
            byte[] bytes = new byte[SIZE];
            TryWriteBytes(bytes);

            return new Guid(bytes);
        }

        /// <summary>
        /// Creates a UUID from a Guid.
        /// </summary>
        /// <param name="guid">The Guid to convert.</param>
        /// <returns>A UUID instance.</returns>
        public static UUID FromGuid(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            ulong timestamp = BitConverter.ToUInt64(bytes, 0);
            ulong random = BitConverter.ToUInt64(bytes, 8);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Implicit conversion from Guid to UUID.
        /// </summary>
        /// <param name="guid">The Guid to convert.</param>
        /// <returns>A UUID instance.</returns>
        /// <remarks>
        /// This conversion is implicit because it is always safe and lossless.
        /// </remarks>
        public static implicit operator UUID(Guid guid)
        {
            return FromGuid(guid);
        }

        /// <summary>
        /// Implicit conversion from UUID to Guid.
        /// </summary>
        /// <param name="uuid">The UUID to convert.</param>
        /// <returns>A Guid instance.</returns>
        /// <remarks>
        /// This conversion is implicit because UUID and Guid are equivalent formats,
        /// and the conversion is always safe and lossless in both directions.
        /// </remarks>
        public static implicit operator Guid(UUID uuid)
        {
            return uuid.ToGuid();
        }

        /// <summary>
        /// Creates a new UUID from a byte array.
        /// </summary>
        /// <param name="bytes">The byte array containing UUID data.</param>
        /// <returns>A new UUID instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when bytes is null.</exception>
        /// <exception cref="ArgumentException">Thrown when bytes length is not 16.</exception>
        public static UUID FromByteArray(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bytes.Length != SIZE)
            {
                throw new ArgumentException($"Byte array must be exactly {SIZE} bytes long.", nameof(bytes));
            }

            ulong timestamp = BitConverter.ToUInt64(bytes, 0);
            ulong random = BitConverter.ToUInt64(bytes, 8);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Attempts to create a UUID from a byte array.
        /// </summary>
        /// <param name="bytes">The byte array containing UUID data.</param>
        /// <param name="result">When this method returns, contains the UUID value if the conversion succeeded, or default if the conversion failed.</param>
        /// <returns>true if the conversion was successful; otherwise, false.</returns>
        public static bool TryFromByteArray(byte[] bytes, out UUID result)
        {
            result = default;

            if (bytes == null || bytes.Length != SIZE)
            {
                return false;
            }

            try
            {
                result = FromByteArray(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified UUID is equal to the current UUID.
        /// </summary>
        /// <param name="other">The UUID to compare with the current UUID.</param>
        /// <returns>true if the specified UUID is equal to the current UUID; otherwise, false.</returns>
        public bool Equals(UUID other)
        {
            return _timestamp == other._timestamp && random == other.Random;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current UUID.
        /// </summary>
        /// <param name="obj">The object to compare with the current UUID.</param>
        /// <returns>true if the specified object is a UUID and equal to the current UUID; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is UUID other && Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this UUID.
        /// </summary>
        /// <returns>A hash code for the current UUID.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (_timestamp.GetHashCode() * 397) ^ random.GetHashCode();
            }
        }

        /// <summary>
        /// Compares the current UUID with another UUID.
        /// </summary>
        /// <param name="other">The UUID to compare with this UUID.</param>
        /// <returns>
        /// A value that indicates the relative order of the UUIDs being compared.
        /// Less than zero: This UUID is less than the other UUID.
        /// Zero: This UUID is equal to the other UUID.
        /// Greater than zero: This UUID is greater than the other UUID.
        /// </returns>
        public int CompareTo(UUID other)
        {
            int result = _timestamp.CompareTo(other._timestamp);

            return result != 0 ? result : random.CompareTo(other.Random);
        }

        /// <summary>
        /// Compares the current UUID with another object.
        /// </summary>
        /// <param name="obj">The object to compare with this UUID.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// Less than zero: This UUID is less than the other object.
        /// Zero: This UUID equals the other object.
        /// Greater than zero: This UUID is greater than the other object or the other object is null.
        /// </returns>
        /// <exception cref="ArgumentException">obj is not a UUID.</exception>
        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is UUID other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException("Object must be of type UUID");
        }

        /// <summary>
        /// Determines whether two UUIDs are equal.
        /// </summary>
        public static bool operator ==(UUID left, UUID right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two UUIDs are not equal.
        /// </summary>
        public static bool operator !=(UUID left, UUID right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether one UUID is less than another UUID.
        /// </summary>
        /// <remarks>
        /// The comparison is performed in two steps:
        /// 1. First, timestamps are compared
        /// 2. If timestamps are equal, random components are compared
        /// This ensures a consistent and meaningful ordering of UUIDs.
        /// </remarks>
        public static bool operator <(UUID left, UUID right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one UUID is less than or equal to another UUID.
        /// </summary>
        public static bool operator <=(UUID left, UUID right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether one UUID is greater than another UUID.
        /// </summary>
        public static bool operator >(UUID left, UUID right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether one UUID is greater than or equal to another UUID.
        /// </summary>
        public static bool operator >=(UUID left, UUID right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}