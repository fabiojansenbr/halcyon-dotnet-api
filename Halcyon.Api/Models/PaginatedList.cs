using System.Collections.Generic;

namespace Halcyon.Api.Models
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public long TotalCount { get; set; }

        public long TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}