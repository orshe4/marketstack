using System;
using System.Collections.Generic;

namespace Marketstack.Entities
{
    internal static class PageResponse
    {
        public const int MaxLimit = 1000;
    }

    internal class PageResponse<T>
    {        
        public Pagination Pagination { get; set; }
        public List<T> Data { get; set; }

        public bool IsLastResponse => Pagination.Count < Pagination.Limit;
        public int NextOffset => IsLastResponse ? throw new InvalidOperationException() : Pagination.Offset + Pagination.Count;

        public List<int> AllRequestOffsets()
        {
            var offsets = new List<int>();
            for (int i = Pagination.Offset + PageResponse.MaxLimit; i < Pagination.Total; i+= PageResponse.MaxLimit)
            {
                offsets.Add(i);
            }
            return offsets;
        }
    }
}
