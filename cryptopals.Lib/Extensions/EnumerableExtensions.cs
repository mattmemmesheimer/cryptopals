using System.Collections.Generic;

namespace cryptopals.Lib.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            List<T> nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>();
                    // or nextbatch.Clear(); but see Servy's comment below
                }
            }

            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> input, int chunkSize)
        {
            using (var enumerator = input.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldChunks(enumerator, chunkSize - 1);
                }
            }
        }

        private static IEnumerable<T> YieldChunks<T>(IEnumerator<T> input, int chunkSize)
        {
            yield return input.Current;
            for (int i = 0; i < chunkSize && input.MoveNext(); i++)
            {
                yield return input.Current;
            }
        }
    }
}
