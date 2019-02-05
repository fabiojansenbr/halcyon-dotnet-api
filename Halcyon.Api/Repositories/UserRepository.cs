using Halcyon.Api.Entities;
using Halcyon.Api.Extensions;
using Halcyon.Api.Models;
using Halcyon.Api.Services.Providers;
using Microsoft.EntityFrameworkCore;
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
            await _context.Database.MigrateAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await Users.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<User> GetUserByEmailAddress(string emailAddress)
        {
            return await Users.FirstOrDefaultAsync(a => a.EmailAddress == emailAddress);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await Users.FirstOrDefaultAsync(a => a.RefreshTokens.Any(b => b.Token == refreshToken));
        }

        public async Task<User> GetUserByLogin(Provider provider, string externalId)
        {
            return await Users.FirstOrDefaultAsync(a => a.Logins.Any(b => b.Provider == provider && b.ExternalId == externalId));
        }

        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<User>> SearchUsers(int page, int size, string search, string sort)
        {
            return await Users
                .Search(search)
                .Sort(sort)
                .ToPaginatedListAsync(page, size);
        }

        private IQueryable<User> Users => _context.Users
            .Include(a => a.RefreshTokens)
            .Include(a => a.Logins)
            .Include(a => a.Roles);
    }
}
