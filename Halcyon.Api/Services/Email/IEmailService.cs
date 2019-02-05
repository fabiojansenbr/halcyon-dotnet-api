using System.Threading.Tasks;

namespace Halcyon.Api.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(IEmailModel model);
    }
}