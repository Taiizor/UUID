using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UUIDTests
{
    public class ArrayExtensionTests
    {
        [Fact]
        public void Fill_ShouldGenerateUniqueValues()
        {
            UUID[] array = new UUID[1000];
            array.Fill();

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void Fill_ShouldThrowOnNullArray()
        {
            UUID[] array = null;
            Assert.Throws<ArgumentNullException>(() => array.Fill());
        }

        [Fact]
        public void TryFill_ShouldHandleNullAndEmptyArrays()
        {
            // Null array
            UUID[] nullArray = null;
            Assert.False(nullArray.TryFill());

            // Empty array
            UUID[] emptyArray = Array.Empty<UUID>();
            Assert.True(emptyArray.TryFill());
        }

        [Fact]
        public void TryFill_ShouldGenerateUniqueValues()
        {
            UUID[] array = new UUID[1000];
            Assert.True(array.TryFill());

            HashSet<UUID> set = new(array);
            Assert.Equal(array.Length, set.Count);
        }

        [Fact]
        public void Generate_ShouldCreateArrayOfSpecifiedSize()
        {
            int size = 100;
            UUID[] array = ArrayExtension.Generate(size);

            Assert.Equal(size, array.Length);
            Assert.DoesNotContain(array, uuid => uuid.Equals(default));
        }

        [Fact]
        public void Generate_ShouldThrowOnNegativeCount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayExtension.Generate(-1));
        }

        [Fact]
        public void TryGenerate_ShouldHandleValidAndInvalidCounts()
        {
            // Negative count
            Assert.False(ArrayExtension.TryGenerate(-1, out UUID[] result));
            Assert.Null(result);

            // Zero count
            Assert.True(ArrayExtension.TryGenerate(0, out result));
            Assert.Empty(result);

            // Valid count
            Assert.True(ArrayExtension.TryGenerate(100, out result));
            Assert.Equal(100, result.Length);
            Assert.DoesNotContain(result, uuid => uuid.Equals(default));
        }

        [Fact]
        public void Generate_And_TryGenerate_ShouldCreateUniqueValues()
        {
            // Test Generate
            UUID[] array1 = ArrayExtension.Generate(500);
            HashSet<UUID> set1 = new(array1);
            Assert.Equal(array1.Length, set1.Count);

            // Test TryGenerate
            Assert.True(ArrayExtension.TryGenerate(500, out UUID[] array2));
            HashSet<UUID> set2 = new(array2);
            Assert.Equal(array2.Length, set2.Count);

            // Test that arrays are different
            Assert.Empty(array1.Intersect(array2));
        }
    }
}
