using Halcyon.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Halcyon.Api.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> query, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => EF.Functions.Like(a.FirstName + " " + a.LastName + " " + a.EmailAddress, $"%{search}%"));
            }

            return query;
        }

        public static IQueryable<User> Sort(this IQueryable<User> query, string sort)
        {
            switch (sort?.ToLowerInvariant())
            {
                case "email_address_desc":
                    query = query.OrderByDescending(a => a.EmailAddress);
                    break;

                case "email_address":
                    query = query.OrderBy(a => a.EmailAddress);
                    break;

                case "display_name_desc":
                    query = query.OrderByDescending(a => a.FirstName)
                        .ThenByDescending(a => a.LastName);
                    break;

                default:
                    query = query.OrderBy(a => a.FirstName)
                        .ThenBy(a => a.LastName);
                    break;
            }

            return query;
        }
    }
}
