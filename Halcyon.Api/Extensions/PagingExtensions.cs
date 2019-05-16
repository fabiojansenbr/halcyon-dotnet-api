using Halcyon.Api.Models;
using System;
using System.Linq;

namespace Halcyon.Api.Extensions
{
    public static class PagingExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(
            this IQueryable<T> source,
            int page,
            int size)
        {
            var count = source.Count();

            var result = source
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return new PaginatedList<T>
            {
                Page = page,
                Size = size,
                TotalCount = count,
                Items = result
            };
        }

        public static TOut MapPaginatedList<TOut, TIn, TMap>(
            this PaginatedList<TIn> source,
            Func<TIn, TMap> mapper) where TOut : PaginatedList<TMap>, new()
        {
            return new TOut
            {
                Page = source.Page,
                Size = source.Size,
                TotalCount = source.TotalCount,
                Items = source.Items.Select(mapper)
            };
        }
    }
}