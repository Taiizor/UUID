using System.Security.Cryptography;

namespace UUID
{
    /// <summary>
    /// UUID represents a modern and efficient unique identifier implementation,
    /// designed for high performance and enhanced security in distributed systems.
    /// </summary>
    /// <remarks>
    /// This implementation provides:
    /// - Monotonicity: Identifiers are sortable by creation time
    /// - Security: Uses cryptographically secure random numbers
    /// - Performance: Optimized for high-performance scenarios
    /// - Compatibility: Full integration with .NET ecosystem
    /// </remarks>
    public readonly struct UUID(ulong timestamp, ulong random) : IEquatable<UUID>, IComparable<UUID>
    {
        /// <summary>
        /// The size of the UUID in bytes.
        /// </summary>
        private const int SIZE = 16;

        /// <summary>
        /// The timestamp component of the UUID.
        /// </summary>
        private readonly ulong _timestamp = timestamp;

        /// <summary>
        /// Thread-local random number generator for secure random number generation.
        /// </summary>
        private static readonly ThreadLocal<RandomNumberGenerator> _rng = new(RandomNumberGenerator.Create);

        /// <summary>
        /// Creates a new UUID instance with current timestamp.
        /// </summary>
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
        /// Generates a timestamp component for the UUID.
        /// </summary>
        /// <returns>A 64-bit unsigned integer containing the timestamp and additional random data.</returns>
        private static ulong GenerateTimestamp()
        {
            byte[] bytes = new byte[2];
            _rng.Value!.GetBytes(bytes);

            long unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return (((ulong)unixMs & 0x0000_FFFF_FFFF_FFFF) << 16)
                   | ((ulong)bytes[0] << 8)
                   | bytes[1];
        }

        /// <summary>
        /// Generates a random component for the UUID.
        /// </summary>
        /// <returns>A 64-bit random unsigned integer.</returns>
        private static ulong GenerateRandom()
        {
            byte[] bytes = new byte[8];
            _rng.Value!.GetBytes(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Creates a UUID from a string representation.
        /// </summary>
        /// <param name="input">The string representation of the UUID.</param>
        /// <returns>A new UUID instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when input is null.</exception>
        /// <exception cref="FormatException">Thrown when input is not in the correct format.</exception>
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

            // Early return if input is null or not the correct length
            if (input == null || input.Length != 32)
            {
                return false;
            }

            try
            {
                ulong timestamp = Convert.ToUInt64(input.Substring(0, 16), 16);
                ulong random = Convert.ToUInt64(input.Substring(16), 16);

                result = new UUID(timestamp, random);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the timestamp component of the UUID.
        /// </summary>
        public DateTimeOffset Time => DateTimeOffset.FromUnixTimeMilliseconds((long)(_timestamp >> 16));

        /// <summary>
        /// Gets the random component of the UUID.
        /// </summary>
        public ulong Random { get; } = random;

        /// <summary>
        /// Returns a string representation of the UUID.
        /// </summary>
        /// <returns>A string representation of the UUID.</returns>
        public override string ToString()
        {
            return $"{_timestamp:x16}{Random:x16}";
        }

        /// <summary>
        /// Returns a Base32 encoded string representation of the UUID.
        /// </summary>
        /// <returns>A Base32 encoded string representation of the UUID.</returns>
        public string ToBase32()
        {
            const string ENCODING_CHARS = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
            char[] result = new char[26];
            ulong value = _timestamp;

            for (int i = 25; i >= 0; i--)
            {
                result[i] = ENCODING_CHARS[(int)(value & 0x1F)];
                value >>= 5;

                if (i == 13)
                {
                    value = Random;
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
        /// Converts the UUID to a byte array.
        /// </summary>
        /// <returns>A byte array representation of the UUID.</returns>
        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[SIZE];
            TryWriteBytes(bytes);

            return bytes;
        }

        /// <summary>
        /// Attempts to write the UUID to a byte array.
        /// </summary>
        /// <param name="destination">The destination byte array.</param>
        /// <returns>true if the UUID was written successfully; otherwise, false.</returns>
        public bool TryWriteBytes(byte[] destination)
        {
            if (destination == null || destination.Length < SIZE)
            {
                return false;
            }

            BitConverter.GetBytes(_timestamp).CopyTo(destination, 0);
            BitConverter.GetBytes(Random).CopyTo(destination, 8);

            return true;
        }

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
        /// Converts the UUID to a System.Guid.
        /// </summary>
        /// <returns>A System.Guid representation of the UUID.</returns>
        public Guid ToGuid()
        {
            byte[] bytes = new byte[SIZE];
            TryWriteBytes(bytes);

            return new Guid(bytes);
        }

        /// <summary>
        /// Creates a UUID from a System.Guid.
        /// </summary>
        /// <param name="guid">The System.Guid to convert.</param>
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
        /// Explicit conversion from UUID to Guid.
        /// </summary>
        /// <param name="uuid">The UUID to convert.</param>
        /// <returns>A Guid instance.</returns>
        /// <remarks>
        /// This conversion is explicit to make it clear that you are converting between different UUID formats.
        /// </remarks>
        public static explicit operator Guid(UUID uuid)
        {
            return uuid.ToGuid();
        }

        /// <summary>
        /// Determines whether the specified UUID is equal to the current UUID.
        /// </summary>
        /// <param name="other">The UUID to compare with the current UUID.</param>
        /// <returns>true if the specified UUID is equal to the current UUID; otherwise, false.</returns>
        public bool Equals(UUID other)
        {
            return _timestamp == other._timestamp && Random == other.Random;
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
                return (_timestamp.GetHashCode() * 397) ^ Random.GetHashCode();
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

            return result != 0 ? result : Random.CompareTo(other.Random);
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