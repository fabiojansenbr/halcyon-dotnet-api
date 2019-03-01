using Halcyon.Api.Entities;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Security
{
    public interface IJwtService
    {
        Task<TokenModel> GenerateToken(User user);
    }
}
