using Halcyon.Api.Entities;
using Halcyon.Api.Models;
using Halcyon.Api.Services.Providers;
using System.Threading.Tasks;

namespace Halcyon.Api.Repositories
{
    public interface IUserRepository
    {
        Task Initialize();

        Task<User> GetUserById(string id);

        Task<User> GetUserByEmailAddress(string emailAddress);

        Task<User> GetUserByRefreshToken(string refreshToken);

        Task<User> GetUserByLogin(Provider provider, string externalId);

        Task CreateUser(User user);

        Task UpdateUser(User user);

        Task RemoveUser(User user);

        Task<PaginatedList<User>> SearchUsers(int page, int size, string search, string sort);
    }
}
