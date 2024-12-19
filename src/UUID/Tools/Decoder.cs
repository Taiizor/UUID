namespace System
{
    /// <summary>
    /// Provides methods for decoding and extracting information from UUIDs.
    /// This decoder can analyze UUID versions 4, 5, 7, and 8 to extract timestamps,
    /// sequence numbers, and provide version-specific information.
    /// </summary>
    public static partial class Decoder
    {
        /// <summary>
        /// Attempts to extract the timestamp from a UUID.
        /// </summary>
        /// <param name="uuid">The UUID to decode.</param>
        /// <param name="timestamp">When this method returns, contains the extracted timestamp if successful.</param>
        /// <returns>True if the timestamp was successfully extracted; otherwise, false.</returns>
        /// <remarks>
        /// Timestamp extraction is supported for:
        /// - Version 7: First 48 bits contain Unix timestamp in milliseconds
        /// - Version 8: Last 48 bits contain Unix timestamp in milliseconds
        /// Version 4 (random) and Version 5 (name-based) UUIDs do not contain timestamps.
        /// </remarks>
        public static bool TryGetTimestamp(UUID uuid, out DateTimeOffset timestamp)
        {
            timestamp = default;

            switch (uuid.Version)
            {
                case 4:
                    // V4 is purely random, no timestamp
                    return false;

                case 5:
                    // V5 is name-based hash, no timestamp
                    return false;

                case 7:
                    // For v7, timestamp is in the first 48 bits
                    long msTimestamp = (long)(uuid._timestamp >> 16);
                    timestamp = DateTimeOffset.FromUnixTimeMilliseconds(msTimestamp);
                    return true;

                case 8:
                    // For v8 (SQL Server), timestamp is in the last 48 bits
                    msTimestamp = (long)(uuid.Random >> 16);
                    timestamp = DateTimeOffset.FromUnixTimeMilliseconds(msTimestamp);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Attempts to extract the sequence number from a UUID.
        /// </summary>
        /// <param name="uuid">The UUID to decode.</param>
        /// <param name="sequence">When this method returns, contains the extracted sequence if successful.</param>
        /// <returns>True if the sequence was successfully extracted; otherwise, false.</returns>
        /// <remarks>
        /// Sequence extraction is supported for:
        /// - Version 7: Last 12 bits of timestamp field
        /// - Version 8: Last 16 bits of random field
        /// Version 4 (random) and Version 5 (name-based) UUIDs do not contain sequence numbers.
        /// </remarks>
        public static bool TryGetSequence(UUID uuid, out ushort sequence)
        {
            sequence = default;

            switch (uuid.Version)
            {
                case 4:
                    // V4 is purely random, no sequence
                    return false;

                case 5:
                    // V5 is name-based hash, no sequence
                    return false;

                case 7:
                    // For v7, sequence is in the last 12 bits of timestamp
                    sequence = (ushort)(uuid._timestamp & 0xFFF);
                    return true;

                case 8:
                    // For v8, sequence is in the last 16 bits
                    sequence = (ushort)(uuid.Random & 0xFFFF);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the UUID is database-optimized.
        /// </summary>
        /// <param name="uuid">The UUID to check.</param>
        /// <returns>True if the UUID is optimized for database usage; otherwise, false.</returns>
        /// <remarks>
        /// Database-optimized UUIDs include:
        /// - Version 7: Optimized for PostgreSQL (timestamp first)
        /// - Version 8: Optimized for SQL Server (timestamp last)
        /// These versions are designed to maintain sequential order in database indexes.
        /// </remarks>
        public static bool IsDatabaseOptimized(UUID uuid)
        {
            return uuid.Version is 7 or 8;
        }

        /// <summary>
        /// Determines if the UUID is name-based.
        /// </summary>
        /// <param name="uuid">The UUID to check.</param>
        /// <returns>True if the UUID is name-based; otherwise, false.</returns>
        /// <remarks>
        /// Name-based UUIDs include:
        /// - Version 3: Using MD5 hash (not implemented)
        /// - Version 5: Using SHA-1 hash
        /// These versions generate consistent UUIDs for the same namespace and name.
        /// </remarks>
        public static bool IsNameBased(UUID uuid)
        {
            return uuid.Version is 3 or 5;
        }

        /// <summary>
        /// Determines if the UUID is randomly generated.
        /// </summary>
        /// <param name="uuid">The UUID to check.</param>
        /// <returns>True if the UUID is randomly generated; otherwise, false.</returns>
        /// <remarks>
        /// Only Version 4 UUIDs are purely random.
        /// These UUIDs use a cryptographically secure random number generator
        /// and are suitable for general-purpose unique identifiers.
        /// </remarks>
        public static bool IsRandom(UUID uuid)
        {
            return uuid.Version is 4;
        }

        /// <summary>
        /// Gets a description of the UUID version and its purpose.
        /// </summary>
        /// <param name="uuid">The UUID to get description for.</param>
        /// <returns>A string describing the UUID version and its purpose.</returns>
        /// <remarks>
        /// Supported versions:
        /// - Version 4: Random UUID using cryptographic RNG
        /// - Version 5: Name-based UUID using SHA-1 hash
        /// - Version 7: Time-ordered UUID optimized for PostgreSQL
        /// - Version 8: Time-ordered UUID optimized for SQL Server
        /// Other versions will return an "Unknown or unsupported version" message.
        /// </remarks>
        public static string GetVersionDescription(UUID uuid)
        {
            return uuid.Version switch
            {
                4 => "Version 4: Random UUID",
                5 => "Version 5: Name-based UUID using SHA-1 hashing",
                7 => "Version 7: Time-ordered UUID optimized for PostgreSQL",
                8 => "Version 8: Time-ordered UUID optimized for SQL Server",
                _ => $"Version {uuid.Version}: Unknown or unsupported version"
            };
        }
    }
}