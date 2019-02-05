using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Providers;
using System;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Handlers
{
    public class ExternalHandler : IHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly Func<Provider, IProvider> _providerFactory;

        public ExternalHandler(
            IUserRepository userRepository,
            Func<Provider, IProvider> providerFactory)
        {
            _userRepository = userRepository;
            _providerFactory = providerFactory;
        }

        public async Task<HandlerResult> Authenticate(IHandlerRequest model)
        {
            var provider = _providerFactory(model.Provider);
            if (provider == null)
            {
                return null;
            }

            var externalUser = await provider.GetUser(model.AccessToken);
            if (externalUser == null)
            {
                return null;
            }

            var user = await _userRepository.GetUserByLogin(model.Provider, externalUser.UserId);
            if (user == null)
            {
                return new HandlerResult(requiresExternal: true);
            }

            if (user.IsLockedOut)
            {
                return new HandlerResult(isLockedOut: true);
            }

            return new HandlerResult(user);
        }
    }
}
