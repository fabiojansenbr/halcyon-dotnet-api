using Halcyon.Api.Entities;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Security
{
    public interface IJwtService
    {
        Task<JwtModel> GenerateToken(User user);
    }
}
