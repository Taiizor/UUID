#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER

using System.Security.Cryptography;

namespace System.Extensions
{
    /// <summary>
    /// Provides extension methods for efficient bulk operations on UUID arrays.
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// Fills an array with new UUIDs efficiently.
        /// This is more performant than generating UUIDs individually.
        /// </summary>
        /// <param name="array">The array to fill with new UUIDs.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static void Fill(UUID[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0)
            {
                return;
            }

            // Calculate total bytes needed (16 bytes per UUID)
            int totalBytes = array.Length * 16;

            // Generate all random bytes at once
            byte[] randomBytes = new byte[totalBytes];
            RandomNumberGenerator.Fill(randomBytes);

            // Fill array with UUIDs
            for (int i = 0, offset = 0; i < array.Length; i++, offset += 16)
            {
                ulong timestamp = BitConverter.ToUInt64(randomBytes, offset);
                ulong random = BitConverter.ToUInt64(randomBytes, offset + 8);
                array[i] = new UUID(timestamp, random);
            }
        }

        /// <summary>
        /// Creates and fills a new array with the specified number of UUIDs.
        /// </summary>
        /// <param name="count">The number of UUIDs to generate.</param>
        /// <returns>An array containing the specified number of new UUIDs.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when count is negative.</exception>
        public static UUID[] Generate(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            }

            UUID[] array = new UUID[count];
            Fill(array);
            return array;
        }
    }
}
#endif