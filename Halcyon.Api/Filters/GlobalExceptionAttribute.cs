using Halcyon.Api.Models;
using Halcyon.Api.Services.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;

namespace Halcyon.Api.Filters
{
    public class GlobalExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GlobalExceptionAttribute(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override void OnException(ExceptionContext context)
        {
            var requestId = Activity.Current?.Id ?? context.HttpContext?.TraceIdentifier;

            var response = new ResponseModel<ErrorModel>
            {
                Messages = new[]
                {
                    context.Exception.Message
                },
                Data = new ErrorModel
                {
                    RequestId = requestId
                }
            };

            if (_hostingEnvironment.IsDevelopment())
            {
                response.Data.Type = context.Exception.GetType().ToString();
                response.Data.Message = context.Exception.Message;
                response.Data.StackTrace = context.Exception.StackTrace;
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            base.OnException(context);
        }
    }
}
