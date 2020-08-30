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
    }
}
