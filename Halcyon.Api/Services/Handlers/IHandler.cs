using System.Threading.Tasks;

namespace Halcyon.Api.Services.Handlers
{
    public interface IHandler
    {
        Task<HandlerResult> Authenticate(IHandlerRequest model);
    }
}