using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeStops.Common.Helpers
{
    /// <summary>
    /// Helper class is created to help bypass SQL server restriction of 2100 parameters. Check protected BaseRepository class methods
    /// </summary>
    public static class BatchHelper
    {
        public static void Execute<TIdentifier>(IEnumerable<TIdentifier> ids, Action<List<TIdentifier>> dataAccessCode, int batchSize)
        {
            Execute(ids.ToList(), dataAccessCode, batchSize);
        }

        public static void Execute<TIdentifier>(List<TIdentifier> idsList, Action<List<TIdentifier>> dataAccessCode, int batchSize)
        {
            var chunks = SplitIntoChunks(idsList, batchSize);

            foreach (var idsToLoad in chunks)
            {
                dataAccessCode(idsToLoad);
            }
        }

        public static List<TResult> Load<TResult, TIdentifier>(IEnumerable<TIdentifier> ids, Func<List<TIdentifier>, IEnumerable<TResult>> dataAccessCode, int batchSize)
        {
            return Load(ids.ToList(), dataAccessCode, batchSize);
        }

        public static List<TResult> Load<TResult, TIdentifier>(List<TIdentifier> idsList, Func<List<TIdentifier>, IEnumerable<TResult>> dataAccessCode, int batchSize)
        {
            var chunks = SplitIntoChunks(idsList, batchSize);

            var result = new List<TResult>();

            foreach (var idsToLoad in chunks)
            {
                var items = dataAccessCode(idsToLoad);
                result.AddRange(items);
            }

            return result;
        }

        /// <summary>
        /// Splits a List<T> into multiple chunks
        /// </summary>
        /// <param name="list">The list to be chunked</param>
        /// <param name="chunkSize">The size of each chunk</param>
        /// <returns>A list of chunks</returns>
        // https://ehikioya.com/iterating-list-items-in-batches-with-csharp-chunking/
        private static List<List<T>> SplitIntoChunks<T>(List<T> list, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("Chunk size must be greater than 0");
            }

            List<List<T>> retVal = new List<List<T>>();
            int index = 0;
            while (index < list.Count)
            {
                int count = list.Count - index > chunkSize ? chunkSize : list.Count - index;
                retVal.Add(list.GetRange(index, count));
                index += chunkSize;
            }

            return retVal;
        }
    }
}
