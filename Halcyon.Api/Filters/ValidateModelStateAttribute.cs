using Halcyon.Api.Services.Response;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Halcyon.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private readonly IResponseService _responseService;

        public ValidateModelStateAttribute(IResponseService responseService)
        {
            _responseService = responseService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = _responseService.GenerateResponse(context.ModelState);
            }
        }
    }
}
