using System;
using System.Collections.Generic;

namespace cryptopals.Lib.Extensions
{
    public static class ArrayExtensions
    {
        public static List<T[]> Chunks<T>(this T[] array, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize,
                    $"{nameof(chunkSize)} must be greater than zero.");
            }
            var chunks = new List<T[]>();
            var chunkIndex = 0;
            for (int i = 0; i < array.Length; i += chunkSize)
            {
                if (i + chunkSize > array.Length)
                {
                    chunkSize = array.Length - i;
                }
                chunks.Add(new T[chunkSize]);
                Array.Copy(array, i, chunks[chunkIndex++], 0, chunkSize);
            }
            return chunks;
        }
    }
}
