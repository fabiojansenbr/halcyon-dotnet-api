using Halcyon.Api.Entities;
using Halcyon.Api.Models.Account;
using Halcyon.Api.Models.Email;
using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Email;
using Halcyon.Api.Services.Providers;
using Halcyon.Api.Services.Response;
using Halcyon.Api.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using static Halcyon.Api.Entities.User;

namespace Halcyon.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly Func<Provider, IProvider> _providerFactory;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly IResponseService _responseService;

        public AccountController(
            IUserRepository userRepository,
            Func<Provider, IProvider> providerFactory,
            IPasswordService passwordService,
            IEmailService emailService,
            IResponseService responseService)
        {
            _userRepository = userRepository;
            _providerFactory = providerFactory;
            _passwordService = passwordService;
            _emailService = emailService;
            _responseService = responseService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var existing = await _userRepository.GetUserByEmailAddress(model.EmailAddress);
            if (existing != null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, $"User name \"{model.EmailAddress}\" is already taken.");
            }

            var user = new User
            {
                EmailAddress = model.EmailAddress,
                Password = _passwordService.HashPassword(model.Password),
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth.GetValueOrDefault(),
                VerifyEmailToken = Guid.NewGuid().ToString()
            };

            await _userRepository.CreateUser(user);

            var email = new VerifyEmailModel
            {
                ToAddress = model.EmailAddress,
                Code = user.VerifyEmailToken
            };

            await _emailService.SendAsync(email);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully registered.");
        }

        [HttpPost("RegisterExternal")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterExternal([FromBody] RegisterExternalModel model)
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

            var existing = await _userRepository.GetUserByEmailAddress(model.EmailAddress);
            if (existing != null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, $"User name \"{model.EmailAddress}\" is already taken.");
            }

            existing = await _userRepository.GetUserByLogin(model.Provider, externalUser.UserId);
            if (existing != null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "A user with this login already exists.");
            }

            var user = new User
            {
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth.GetValueOrDefault()
            };

            user.Logins.Add(new UserLogin(model.Provider, externalUser.UserId));

            await _userRepository.CreateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully registered.");
        }

        [HttpPut("ForgotPassword")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userRepository.GetUserByEmailAddress(model.EmailAddress);
            if (user != null)
            {
                user.PasswordResetToken = Guid.NewGuid().ToString();
                await _userRepository.UpdateUser(user);

                var email = new PasswordResetEmailModel
                {
                    ToAddress = model.EmailAddress,
                    Code = user.PasswordResetToken
                };

                await _emailService.SendAsync(email);
            }

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Instructions as to how to reset your password have been sent to you via email.");
        }

        [HttpPut("ResetPassword")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userRepository.GetUserByEmailAddress(model.EmailAddress);
            if (user == null || user.PasswordResetToken != model.Code)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Invalid token.");
            }

            user.Password = _passwordService.HashPassword(model.NewPassword);
            user.PasswordResetToken = null;
            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = null;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Your password has been reset.");
        }
    }
}