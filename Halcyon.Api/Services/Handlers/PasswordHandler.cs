using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Security;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Handlers
{
    public class PasswordHandler : IHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public PasswordHandler(
            IUserRepository userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<HandlerResult> Authenticate(IHandlerRequest model)
        {
            var user = await _userRepository.GetUserByEmailAddress(model.EmailAddress);
            if (user == null)
            {
                return null;
            }

            if(!_passwordService.VerifyPassword(model.Password, user.Password)) 
            {
                return null;
            }

            if (user.IsLockedOut || user.TwoFactorEnabled)
            {
                return new HandlerResult(isLockedOut: user.IsLockedOut, requiresTwoFactor: user.TwoFactorEnabled);
            }

            return new HandlerResult(user);
        }
    }
}
