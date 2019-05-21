using Halcyon.Api.Entities;
using Halcyon.Api.Models;
using Halcyon.Api.Services.Providers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Halcyon.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HalcyonDbContext _context;

        public UserRepository(HalcyonDbContext context)
        {
            _context = context;
        }

        public async Task Initialize()
        {
            var indexes = new[]
            {
                new CreateIndexModel<User>(
                    new IndexKeysDefinitionBuilder<User>().Text("$**")
                ),

                new CreateIndexModel<User>(
                    new IndexKeysDefinitionBuilder<User>().Ascending(new StringFieldDefinition<User>("emailAddress")),
                    new CreateIndexOptions { Unique = true }
                )
            };

            await _context.Users.Indexes.CreateManyAsync(indexes);
        }

        public async Task<User> GetUserById(string id)
        {
            var objectId = ObjectId.Parse(id);
            var result = await _context.Users.FindAsync(a => a.Id == objectId);
            return result.FirstOrDefault();
        }

        public async Task<User> GetUserByEmailAddress(string emailAddress)
        {
            var result = await _context.Users.FindAsync(a => a.EmailAddress == emailAddress);
            return result.FirstOrDefault();
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var result = await _context.Users.FindAsync(a => a.RefreshTokens.Contains(refreshToken));
            return result.FirstOrDefault();
        }


        public async Task<User> GetUserByLogin(Provider provider, string externalId)
        {
            var result = await _context.Users.FindAsync(a => a.Logins.Any(b => b.Provider == provider && b.ExternalId == externalId));
            return result.FirstOrDefault();
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        public async Task UpdateUser(User user)
        {
            await _context.Users.ReplaceOneAsync(a => a.Id == user.Id, user);
        }

        public async Task RemoveUser(User user)
        {
            await _context.Users.DeleteOneAsync(a => a.Id == user.Id);
        }

        public async Task<PaginatedList<User>> SearchUsers(int page, int size, string search, string sort)
        {
            var searchExpression = GetSearchExpression(search);
            var sortExpression = GetSortExpression(sort);

            var totalCount = await _context.Users.Find(searchExpression).CountDocumentsAsync();

            var users = await _context.Users.Find(searchExpression)
                .Sort(sortExpression)
                .Limit(size)
                .Skip(size * (page - 1))
                .ToListAsync();

            var totalPages = (long)Math.Ceiling(totalCount / (double)size);

            return new PaginatedList<User>
            {
                Items = users,
                Page = page,
                Size = size,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages
            };
        }

        private FilterDefinition<User> GetSearchExpression(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Builders<User>.Filter.Empty;
            }

            return Builders<User>.Filter.Text(search);
        }

        private SortDefinition<User> GetSortExpression(string sort)
        {
            switch (sort?.ToLowerInvariant())
            {
                case "email_address":
                    return Builders<User>.Sort.Ascending("EmailAddress");

                case "email_address_desc":
                    return Builders<User>.Sort.Descending("EmailAddress");

                case "display_name_desc":
                    return Builders<User>.Sort.Descending("FirstName").Descending("LastName");

                default:
                    return Builders<User>.Sort.Ascending("FirstName").Ascending("LastName");
            }
        }
    }
}