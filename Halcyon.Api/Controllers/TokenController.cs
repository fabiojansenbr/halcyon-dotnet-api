using Halcyon.Api.Models.Token;
using Halcyon.Api.Services.Handlers;
using Halcyon.Api.Services.Response;
using Halcyon.Api.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Halcyon.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TokenController : Controller
    {
        private readonly Func<GrantType, IHandler> _handlerFactory;
        private readonly IJwtService _jwtService;
        private readonly IResponseService _responseService;

        public TokenController(
            IJwtService jwtService,
            Func<GrantType, IHandler> handlerFactory,
            IResponseService responseService)
        {
            _jwtService = jwtService;
            _handlerFactory = handlerFactory;
            _responseService = responseService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<JwtModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetToken([FromBody] GetTokenModel model)
        {
            var handler = _handlerFactory(model.GrantType);
            if (handler == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, $"Grant Type \"{model.GrantType}\" is not supported.");
            }

            var result = await handler.Authenticate(model);
            if (result == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "The credentials provided were invalid.");
            }

            if (result.RequiresTwoFactor || result.RequiresExternal)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, result);
            }

            if (result.IsLockedOut)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "This account has been locked out, please try again later.");
            }

            if (result.User == null)
            {
                return _responseService.GenerateResponse(HttpStatusCode.BadRequest, "The credentials provided were invalid.");
            }

            var token = await _jwtService.GenerateToken(result.User);
            return _responseService.GenerateResponse(HttpStatusCode.OK, token);
        }
    }
}