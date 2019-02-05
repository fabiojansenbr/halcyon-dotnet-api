using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Halcyon.Api.Services.Response
{
    public class ResponseService : IResponseService
    {
        public IActionResult GenerateResponse(HttpStatusCode code, params string[] messages)
        {
            return GenerateResponse(code, null as object, messages);
        }

        public IActionResult GenerateResponse<T>(HttpStatusCode code, T data)
        {
            return GenerateResponse(code, data, null);
        }

        public IActionResult GenerateResponse(ModelStateDictionary modelState)
        {
            var statusCode = modelState.IsValid
                ? HttpStatusCode.OK
                : HttpStatusCode.BadRequest;

            var messages = modelState.Values
                .SelectMany(a => a.Errors)
                .Select(a => a.ErrorMessage);

            return GenerateResponse(statusCode, null as object, messages);
        }

        private static IActionResult GenerateResponse<T>(HttpStatusCode code, T data, IEnumerable<string> messages)
        {
            var response = new ResponseModel<T>
            {
                Data = data,
                Messages = messages
            };

            return new ObjectResult(response)
            {
                StatusCode = (int)code
            };
        }
    }
}