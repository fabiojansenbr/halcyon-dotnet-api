using System;
using System.Collections.Generic;

namespace Halcyon.Api.Models
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public long TotalCount { get; set; }

        public long TotalPages => (long)Math.Ceiling(TotalCount / (double)Size);

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}