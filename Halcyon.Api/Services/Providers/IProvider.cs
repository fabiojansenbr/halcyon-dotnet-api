using System.Threading.Tasks;

namespace Halcyon.Api.Services.Providers
{
    public interface IProvider
    {
        Task<ProviderUserModel> GetUser(string accessToken);
    }
}