using System.Security.Cryptography;
using System.Text;

namespace System
{
    public readonly partial struct UUID
    {
        /// <summary>
        /// Creates a new database-friendly UUID optimized for the specified database type.
        /// </summary>
        /// <param name="dbType">The type of database where the UUID will be stored.</param>
        /// <returns>A new UUID instance optimized for the specified database.</returns>
        /// <remarks>
        /// Database-specific optimizations:
        /// - PostgreSQL: Uses Version 7 with timestamp first for B-tree optimization
        /// - SQL Server: Uses Version 8 with timestamp last for clustered index optimization
        /// - SQLite: Uses Version 7 (same as PostgreSQL)
        /// - Other databases: Uses Version 4 (random) as default
        /// </remarks>
        public static UUID NewDatabaseFriendly(DatabaseType dbType)
        {
            return dbType switch
            {
                DatabaseType.PostgreSQL => NewV7(),
                DatabaseType.SQLServer => NewV8(),
                DatabaseType.SQLite => NewV7(),
                _ => NewV4() // Default to random UUID
            };
        }

        /// <summary>
        /// Creates a new Version 7 UUID (optimized for PostgreSQL).
        /// </summary>
        /// <returns>A new Version 7 UUID instance.</returns>
        /// <remarks>
        /// Version 7 UUID structure:
        /// - First 48 bits: Unix timestamp in milliseconds
        /// - Next 12 bits: Sequence number for sub-millisecond ordering
        /// - Remaining bits: Random data with version and variant
        /// 
        /// This format is optimized for PostgreSQL's B-tree indexes by:
        /// - Maintaining temporal ordering
        /// - Providing sub-millisecond uniqueness
        /// - Minimizing index fragmentation
        /// </remarks>
        public static UUID NewV7()
        {
            (long unixMs, ushort sequence) = GetTimestampAndSequence();

            // 48 bits for timestamp
            ulong timestamp = (ulong)unixMs << 16;

            // Add sequence for sub-millisecond ordering
            timestamp |= (ulong)(sequence & 0xFFF); // Use 12 bits for sequence

            // Generate random bits
            ulong random = GenerateRandom();

            // Set version and variant
            SetVersionAndVariant(ref timestamp, ref random, 7);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Creates a new Version 8 UUID (optimized for SQL Server).
        /// </summary>
        /// <returns>A new Version 8 UUID instance.</returns>
        /// <remarks>
        /// Version 8 UUID structure:
        /// - First part: Random data
        /// - Last 48 bits: Unix timestamp in milliseconds
        /// - Last 16 bits of random: Sequence number
        /// 
        /// This format is optimized for SQL Server by:
        /// - Placing timestamp at the end for better clustered index performance
        /// - Using 16-bit sequence for higher sub-millisecond resolution
        /// - Reducing page splits in clustered indexes
        /// </remarks>
        public static UUID NewV8()
        {
            (long unixMs, ushort sequence) = GetTimestampAndSequence();

            // For SQL Server, we want the timestamp at the end for better indexing
            ulong timestamp = GenerateRandom();
            ulong random = ((ulong)unixMs << 16) | (ulong)(sequence & 0xFFFF);

            // Set version and variant
            SetVersionAndVariant(ref timestamp, ref random, 8);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Creates a new Version 5 UUID based on the specified namespace and name.
        /// </summary>
        /// <param name="namespaceId">The namespace UUID.</param>
        /// <param name="name">The name within the namespace.</param>
        /// <returns>A new Version 5 UUID instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when name is null or empty.</exception>
        /// <remarks>
        /// Version 5 UUID characteristics:
        /// - Uses SHA-1 hashing for name-based UUID generation
        /// - Provides consistent UUIDs for the same namespace and name
        /// - Suitable for generating UUIDs from URLs, DNs, or other named entities
        /// - More collision-resistant than Version 3 (MD5-based) UUIDs
        /// 
        /// The generated UUID will be the same for the same namespace and name combination,
        /// making it useful for creating deterministic identifiers for named resources.
        /// </remarks>
        public static UUID NewV5(UUID namespaceId, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            using SHA1 sha1 = SHA1.Create();

            // Combine namespace UUID and name
            byte[] namespaceBytes = namespaceId.ToByteArray();
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);

            byte[] combined = new byte[namespaceBytes.Length + nameBytes.Length];
            namespaceBytes.CopyTo(combined, 0);
            nameBytes.CopyTo(combined, namespaceBytes.Length);

            // Calculate hash
            byte[] hash = sha1.ComputeHash(combined);

            // Use first 16 bytes of hash
            ulong timestamp = BitConverter.ToUInt64(hash, 0);
            ulong random = BitConverter.ToUInt64(hash, 8);

            // Set version and variant
            SetVersionAndVariant(ref timestamp, ref random, 5);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Creates a new Version 4 UUID (random).
        /// </summary>
        /// <returns>A new Version 4 UUID instance.</returns>
        /// <remarks>
        /// Version 4 UUID characteristics:
        /// - Uses cryptographically secure random number generation
        /// - Provides maximum entropy for distributed systems
        /// - Suitable for general-purpose unique identifiers
        /// - No temporal or spatial correlation between generated UUIDs
        /// 
        /// This is the most commonly used UUID version when
        /// database optimization or name-based generation is not required.
        /// </remarks>
        public static UUID NewV4()
        {
            ulong timestamp = GenerateTimestamp();
            ulong random = GenerateRandom();

            // Set version and variant
            SetVersionAndVariant(ref timestamp, ref random, 4);

            return new UUID(timestamp, random);
        }
    }
}