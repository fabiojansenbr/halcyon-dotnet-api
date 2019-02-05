using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Security;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Handlers
{
    public class TwoFactorHandler : IHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITwoFactorService _twoFactorService;

        public TwoFactorHandler(
            IUserRepository userRepository,
            IPasswordService passwordService,
            ITwoFactorService twoFactorService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _twoFactorService = twoFactorService;
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

            var verified = _twoFactorService.VerifyToken(model.VerificationCode, user.TwoFactorSecret);
            if (!verified)
            {
                return null;
            }

            if (user.IsLockedOut)
            {
                return new HandlerResult(isLockedOut: user.IsLockedOut);
            }

            return new HandlerResult(user);
        }
    }
}
