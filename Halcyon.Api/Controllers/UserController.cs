using Halcyon.Api.Entities;
using Halcyon.Api.Models.User;
using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Response;
using Halcyon.Api.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Halcyon.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "System Administrator, User Administrator")]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IResponseService _responseService;

        public UserController(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IResponseService responseService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _responseService = responseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel<UserListModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index(int page = 1, int size = 10, string search = null, string sort = null)
        {
            var result = await _userRepository.SearchUsers(page, size, search, sort);

            var model = new UserListModel
            {
                Items = result.Items.Select(MapUser),
                Page = result.Page,
                Size = result.Size,
                TotalPages = result.TotalPages,
                TotalCount = result.TotalCount,
                HasNextPage = result.HasNextPage,
                HasPreviousPage = result.HasPreviousPage,
                Search = search,
                Sort = sort
            };

            return _responseService.GenerateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<UserModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            var model = MapUser(user);
            return _responseService.GenerateResponse(HttpStatusCode.OK, model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserModel model)
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
                DateOfBirth = model.DateOfBirth.GetValueOrDefault()
            };

            user.Roles.AddRange(model.Roles);

            await _userRepository.CreateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully created.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserModel model)
        {
            var user = await _userRepository.GetUserById(id);
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
            user.Roles = model.Roles.ToList();

            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully updated.");
        }

        [HttpPut("{id}/Lock")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Lock(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (user.Id.ToString() == HttpContext.User.Identity.Name)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Cannot lock currently logged in user.");
            }

            user.IsLockedOut = true;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully locked.");
        }

        [HttpPut("{id}/Unlock")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            user.IsLockedOut = false;
            await _userRepository.UpdateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully unlocked.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.NotFound, "User could not be found.");
            }

            if (user.Id.ToString() == HttpContext.User.Identity.Name)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "Cannot delete currently logged in user.");
            }

            await _userRepository.RemoveUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "User successfully deleted.");
        }

        private UserModel MapUser(User user)
        {
            return new UserModel
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
        }
    }
}