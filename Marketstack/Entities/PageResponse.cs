using System;
using System.Collections.Generic;

namespace Marketstack.Entities
{
    internal class PageResponse<T>
    {
        public const int MaxLimit = 100;
        public Pagination Pagination { get; set; }
        public List<T> Data { get; set; }

        public bool IsLastResponse => Pagination.Count < Pagination.Limit;
        public int NextOffset => IsLastResponse ? throw new InvalidOperationException() : Pagination.Offset + Pagination.Count;

        public List<int> AllRequestOffsets()
        {
            var offsets = new List<int>();
            for (int i = 100; i < Pagination.Total; i+=MaxLimit)
            {
                offsets.Add(i);
            }
            return offsets;
        }
    }
}
