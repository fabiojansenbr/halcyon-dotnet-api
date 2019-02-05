using Halcyon.Api.Services.Providers;

namespace Halcyon.Api.Services.Handlers
{
    public interface IHandlerRequest
    {
        string EmailAddress { get; }

        string Password { get; }

        string RefreshToken { get; }

        Provider Provider { get; }

        string AccessToken { get; }

        string VerificationCode { get; }
    }
}
