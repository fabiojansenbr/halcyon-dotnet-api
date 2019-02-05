using Halcyon.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Halcyon.Api.Extensions
{
    public static class PagingExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int page,
            int size)
        {
            var count = await source.CountAsync();

            var result = await source
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

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