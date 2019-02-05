using Halcyon.Api.Repositories;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Handlers
{
    public class RefreshTokenHandler : IHandler
    {
        private readonly IUserRepository _userRepository;

        public RefreshTokenHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<HandlerResult> Authenticate(IHandlerRequest model)
        {
            var user = await _userRepository.GetUserByRefreshToken(model.RefreshToken);
            if (user == null)
            {
                return null;
            }

            if (user.IsLockedOut)
            {
                return new HandlerResult(isLockedOut: true);
            }

            user.RefreshTokens.RemoveAll(a => a.Token == model.RefreshToken);

            return new HandlerResult(user);
        }
    }
}
