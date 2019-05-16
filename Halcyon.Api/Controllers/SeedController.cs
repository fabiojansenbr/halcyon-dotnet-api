using Halcyon.Api.Entities;
using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Response;
using Halcyon.Api.Services.Security;
using Halcyon.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Halcyon.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class SeedController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IResponseService _responseService;
        private readonly SeedSettings _seedSettings;

        public SeedController(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IResponseService responseService,
            IOptions<SeedSettings> seedSettings)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _responseService = responseService;
            _seedSettings = seedSettings.Value;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Index()
        {
            await _userRepository.Initialize();

            var user = new User
            {
                EmailAddress = _seedSettings.EmailAddress,
                Password = _passwordService.HashPassword(_seedSettings.Password),
                FirstName = "System",
                LastName = "Administrator",
                DateOfBirth = new DateTime(1970, 1, 1)
            };

            user.Roles.Add("System Administrator");

            var existing = await _userRepository.GetUserByEmailAddress(user.EmailAddress);
            if (existing != null)
            {
                await _userRepository.RemoveUser(existing);
            }

            await _userRepository.CreateUser(user);

            return _responseService.GenerateResponse(HttpStatusCode.OK, "Database seeded.");
        }
    }
}