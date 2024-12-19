#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER

using System.Security.Cryptography;

namespace System
{
    /// <summary>
    /// Provides extension methods for efficient bulk operations on UUID arrays.
    /// These methods handle byte arrays in a way that maintains UUID structure and endianness.
    /// </summary>
    public static partial class ArrayExtension
    {
        /// <summary>
        /// Fills an array with new UUIDs efficiently.
        /// This is more performant than generating UUIDs individually.
        /// </summary>
        /// <param name="array">The array to fill with new UUIDs.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static void Fill(this UUID[] array)
        {
            if (!TryFill(array))
            {
                throw new ArgumentNullException(nameof(array));
            }
        }

        /// <summary>
        /// Attempts to fill an array with new UUIDs efficiently.
        /// This is more performant than generating UUIDs individually.
        /// </summary>
        /// <param name="array">The array to fill with new UUIDs.</param>
        /// <returns>True if the array was successfully filled, false if the array is null.</returns>
        public static bool TryFill(this UUID[] array)
        {
            if (array == null)
            {
                return false;
            }

            if (array.Length == 0)
            {
                return true;
            }

            // Calculate total bytes needed (16 bytes per UUID)
            int totalBytes = array.Length * 16;

            try
            {
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

                return true;
            }
            catch
            {
                return false;
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
            array.Fill();

            return array;
        }

        /// <summary>
        /// Attempts to create and fill a new array with the specified number of UUIDs.
        /// </summary>
        /// <param name="count">The number of UUIDs to generate.</param>
        /// <param name="result">When this method returns, contains the generated UUID array if successful, or null if unsuccessful.</param>
        /// <returns>True if the array was successfully generated and filled, false otherwise.</returns>
        public static bool TryGenerate(int count, out UUID[]? result)
        {
            result = null;

            if (count < 0)
            {
                return false;
            }

            try
            {
                UUID[] array = new UUID[count];

                if (array.TryFill())
                {
                    result = array;

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Copies bytes from source array to destination array with bounds checking.
        /// </summary>
        /// <param name="source">The source array to copy from.</param>
        /// <param name="sourceIndex">The starting index in the source array.</param>
        /// <param name="destination">The destination array to copy to.</param>
        /// <param name="destinationIndex">The starting index in the destination array.</param>
        /// <param name="length">The number of bytes to copy.</param>
        /// <exception cref="ArgumentNullException">Thrown when source or destination is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when:
        /// - sourceIndex is less than 0
        /// - destinationIndex is less than 0
        /// - length is less than 0
        /// - sourceIndex + length exceeds source array length
        /// - destinationIndex + length exceeds destination array length
        /// </exception>
        /// <remarks>
        /// This method is optimized for UUID operations where:
        /// - Source array is typically 16 bytes (UUID size)
        /// - Destination array is sized appropriately for the operation
        /// - Endianness is preserved during the copy
        /// </remarks>
        public static void CopyBytes(this byte[] source, int sourceIndex, byte[] destination, int destinationIndex, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (sourceIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceIndex));
            }

            if (destinationIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationIndex));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (sourceIndex + length > source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (destinationIndex + length > destination.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Buffer.BlockCopy(source, sourceIndex, destination, destinationIndex, length);
        }

        /// <summary>
        /// Determines whether two byte arrays are equal in content.
        /// </summary>
        /// <param name="first">The first array to compare.</param>
        /// <param name="second">The second array to compare.</param>
        /// <returns>true if the arrays have the same length and content; otherwise, false.</returns>
        /// <remarks>
        /// This method is optimized for UUID comparison where:
        /// - Arrays are typically 16 bytes (UUID size)
        /// - Comparison is done in a constant time to prevent timing attacks
        /// - null arrays are handled safely (both null = true, one null = false)
        /// </remarks>
        public static bool ArrayEquals(byte[] first, byte[] second)
        {
            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            if (first.Length != second.Length)
            {
                return false;
            }

            // Use constant-time comparison to prevent timing attacks
            int result = 0;
            for (int i = 0; i < first.Length; i++)
            {
                result |= first[i] ^ second[i];
            }
            return result == 0;
        }

        /// <summary>
        /// Computes a hash code for a byte array.
        /// </summary>
        /// <param name="array">The array to compute hash code for.</param>
        /// <returns>A hash code value for the array.</returns>
        /// <remarks>
        /// This method is optimized for UUID hashing where:
        /// - Array is typically 16 bytes (UUID size)
        /// - Hash computation uses FNV-1a algorithm for better distribution
        /// - null array returns 0
        /// The FNV-1a algorithm is chosen for:
        /// - Good distribution of hash values
        /// - Fast computation
        /// - Low collision rate for UUID-sized data
        /// </remarks>
        public static int GetHashCode(byte[] array)
        {
            if (array == null)
            {
                return 0;
            }

            // Using FNV-1a hash algorithm
            const int fnvPrime = 16777619;
            const int fnvOffsetBasis = unchecked((int)2166136261);

            int hash = fnvOffsetBasis;

            for (int i = 0; i < array.Length; i++)
            {
                hash = unchecked((hash ^ array[i]) * fnvPrime);
            }

            return hash;
        }
    }
}

#endif