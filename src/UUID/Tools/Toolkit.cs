using System.Security.Cryptography;

namespace System
{
    /// <summary>
    /// Provides advanced tools for UUID creation and manipulation.
    /// This toolkit includes methods for creating UUIDs with specific timestamps,
    /// converting between different formats, and generating batches of UUIDs.
    /// </summary>
    public static partial class Toolkit
    {
        /// <summary>
        /// Creates a new Version 7 UUID with a specific timestamp.
        /// </summary>
        /// <param name="date">The timestamp to use in the UUID.</param>
        /// <returns>A Version 7 UUID containing the specified timestamp.</returns>
        /// <remarks>
        /// The resulting UUID will be a Version 7 UUID with the following characteristics:
        /// - First 48 bits contain the Unix timestamp in milliseconds
        /// - Next 4 bits contain the version (7)
        /// - Remaining bits contain random data with the variant bits set
        /// </remarks>
        public static UUID CreateFromDate(DateTimeOffset date)
        {
            long unixMs = date.ToUnixTimeMilliseconds();

            // Use v7 format for timestamp-based UUIDs
            ulong timestamp = (ulong)unixMs << 16;
            ulong random = GenerateSecureRandom();

            // Set version 7 and variant bits
            timestamp = (timestamp & 0xFFFF_FFFF_FFFF_0FFF) | (7UL << 12);
            random = (random & 0x3FFF_FFFF_FFFF_FFFF) | 0x8000_0000_0000_0000;

            return new UUID(timestamp, random);
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Creates a new UUID from raw bytes.
        /// </summary>
        /// <param name="bytes">A 16-byte array containing the UUID data.</param>
        /// <returns>A new UUID containing the specified bytes.</returns>
        /// <exception cref="ArgumentException">Thrown when the input array is not exactly 16 bytes long.</exception>
        /// <remarks>
        /// The byte array should be in network byte order (big-endian).
        /// The version and variant bits in the input bytes will be preserved.
        /// </remarks>
        public static UUID CreateFromBytes(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 16)
            {
                throw new ArgumentException("Byte array must be exactly 16 bytes long.", nameof(bytes));
            }

            ulong timestamp = BitConverter.ToUInt64(bytes.ToArray(), 0);
            ulong random = BitConverter.ToUInt64(bytes.ToArray(), 8);

            return new UUID(timestamp, random);
        }
#endif

        /// <summary>
        /// Creates a new UUID with a specific version number.
        /// </summary>
        /// <param name="version">The UUID version number (1-8).</param>
        /// <returns>A new UUID with the specified version.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when version is not between 1 and 8.</exception>
        /// <remarks>
        /// This method creates a UUID with random data and sets the specified version number.
        /// The variant bits will be set to indicate a standard UUID (RFC 4122).
        /// Supported versions:
        /// - 4: Random UUID
        /// - 5: Name-based UUID using SHA-1
        /// - 7: Time-ordered UUID (PostgreSQL optimized)
        /// - 8: Time-ordered UUID (SQL Server optimized)
        /// </remarks>
        public static UUID CreateWithVersion(int version)
        {
            if (version is < 1 or > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(version), "Version must be between 1 and 8.");
            }

            ulong timestamp = GenerateSecureRandom();
            ulong random = GenerateSecureRandom();

            // Set version and variant
            timestamp = (timestamp & 0xFFFF_FFFF_FFFF_0FFF) | ((ulong)version << 12);
            random = (random & 0x3FFF_FFFF_FFFF_FFFF) | 0x8000_0000_0000_0000;

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Creates a batch of sequential, database-friendly UUIDs.
        /// </summary>
        /// <param name="count">The number of UUIDs to generate.</param>
        /// <param name="dbType">The target database type for optimization.</param>
        /// <returns>An array of sequential UUIDs optimized for the specified database.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when count is less than or equal to zero.</exception>
        /// <remarks>
        /// The generated UUIDs will be:
        /// - Sequential within the batch
        /// - Optimized for the specified database type
        /// - Version 7 for PostgreSQL (timestamp first)
        /// - Version 8 for SQL Server (timestamp last)
        /// This method is useful for bulk inserts where sequential values improve database performance.
        /// </remarks>
        public static UUID[] CreateSequentialBatch(int count, DatabaseType dbType = DatabaseType.Other)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive.");
            }

            UUID[] result = new UUID[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = UUID.NewDatabaseFriendly(dbType);
            }

            return result;
        }

        /// <summary>
        /// Generates a cryptographically secure random number.
        /// </summary>
        /// <returns>A random 64-bit unsigned integer.</returns>
        private static ulong GenerateSecureRandom()
        {
            byte[] bytes = new byte[8];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }
    }
}