using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Halcyon.Api.Services.Response
{
    public interface IResponseService
    {
        IActionResult GenerateResponse(HttpStatusCode code, params string[] message);

        IActionResult GenerateResponse<T>(HttpStatusCode code, T data);

        IActionResult GenerateResponse(ModelStateDictionary modelState);
    }
}
