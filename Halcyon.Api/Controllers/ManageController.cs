using Halcyon.Api.Models.Email;
using Halcyon.Api.Models.Manage;
using Halcyon.Api.Models.User;
using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Email;
using Halcyon.Api.Services.Providers;
using Halcyon.Api.Services.Response;
using Halcyon.Api.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Halcyon.Api.Entities.User;

namespace Halcyon.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.NotFound)]
    public class ManageController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly Func<Provider, IProvider> _providerFactory;
        private readonly IPasswordService _passwordService;
        private readonly ITwoFactorService _twoFactorService;
        private readonly IEmailService _emailService;
        private readonly IResponseService _responseService;

        public ManageController(
            IUserRepository userRepository,
            Func<Provider, IProvider> providerFactory,
            IPasswordService passwordService,
            ITwoFactorService twoFactorService,
            IEmailService emailService,
            IResponseService responseService)
        {
            _userRepository = userRepository;
            _providerFactory = providerFactory;
            _passwordService = passwordService;
            _twoFactorService = twoFactorService;
            _emailService = emailService;
            _responseService = responseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<UserModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            var model = new UserModel
            {
                Id = user.Id.ToString(),
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                IsLockedOut = user.IsLockedOut,
                HasPassword = user.HasPassword,
                EmailConfirmed = user.EmailConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Picture = user.Picture,
                Roles = user.Roles,
                Logins = user.Logins.Select(a => new ExternalLoginModel(a.Provider, a.ExternalId))
            };

            return _responseService.GenerateResponse(HttpStatusCode.OK, model);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (!model.EmailAddress.Equals(user.EmailAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                var existing = await _userRepository.GetUserByEmailAddress(model.EmailAddress);
                if (existing != null)
                {
                    return _responseService.GenerateResponse(HttpStatusCode.BadRequest, $"User name \"{model.EmailAddress}\" is already taken.");
                }

                user.EmailConfirmed = false;
                user.VerifyEmailToken = null;
            }

            user.EmailAddress = model.EmailAddress;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth.GetValueOrDefault();
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Your profile has been updated.");
        }

        [HttpPut("VerifyEmail")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerifyEmail()
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (user.EmailConfirmed)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Your email address has already been verified.");
            }

            user.VerifyEmailToken = Guid.NewGuid().ToString();
            await _userRepository.UpdateUser(user);

            var email = new VerifyEmailModel
            {
                ToAddress = user.EmailAddress,
                Code = user.VerifyEmailToken
            };

            await _emailService.SendAsync(email);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Instructions as to how to verify your email address have been sent to you via email.");
        }

        [HttpPut("ConfirmEmail")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (user.VerifyEmailToken != model.Code)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Invalid token.");
            }

            user.EmailConfirmed = true;
            user.VerifyEmailToken = null;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Your email address has been verified.");
        }

        [HttpPut("SetPassword")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordModel model)
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "User already has a password set.");
            }

            user.Password = _passwordService.HashPassword(model.NewPassword);
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Your password has been set.");
        }

        [HttpPut("ChangePassword")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (!_passwordService.VerifyPassword(model.CurrentPassword, user.Password))
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Incorrect password.");
            }

            user.Password = _passwordService.HashPassword(model.NewPassword);
            user.PasswordResetToken = null;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Your password has been changed.");
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLogin([FromBody] AddLoginModel model)
        {
            var provider = _providerFactory(model.Provider);
            if (provider == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, $"Provider \"{model.Provider}\" is not supported.");
            }

            var externalUser = await provider.GetUser(model.AccessToken);
            if (externalUser == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "The credentials provided were invalid.");
            }

            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            var existing = await _userRepository.GetUserByLogin(model.Provider, externalUser.UserId);
            if (existing != null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "A user with this login already exists.");
            }

            user.Logins.Add(new UserLogin(model.Provider, externalUser.UserId));
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, $"{model.Provider} login added.");
        }

        [HttpDelete("Login")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveLogin([FromBody] RemoveLoginModel model)
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            var login = user.Logins.FirstOrDefault(a => a.Provider == model.Provider && a.ExternalId == model.ExternalId);
            if (login != null)
            {
                user.Logins.Remove(login);
                await _userRepository.UpdateUser(user);
            }

            return _responseService.GenerateResponse(HttpStatusCode.OK, $"{model.Provider} login removed.");
        }

        [HttpGet("TwoFactor")]
        [ProducesResponseType(typeof(ResponseModel<TwoFactorModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTwoFactorConfig()
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            var result = _twoFactorService.GenerateToken("Halcyon", user.EmailAddress);

            user.TwoFactorTempSecret = result.Secret;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost("TwoFactor")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EnableTwoFactor([FromBody] EnableTwoFactorModel model)
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            var verified = _twoFactorService.VerifyToken(model.VerificationCode, user.TwoFactorTempSecret);
            if (!verified)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Verification with authenticator app was unsuccessful.");
            }

            user.TwoFactorEnabled = true;
            user.TwoFactorSecret = user.TwoFactorTempSecret;
            user.TwoFactorTempSecret = null;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Two factor authentication has been enabled.");
        }

        [HttpDelete("TwoFactor")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DisableTwoFactor()
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = null;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Two factor authentication has been disabled.");
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userRepository.GetUserById(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            await _userRepository.RemoveUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Your account has been deleted.");
        }
    }
}