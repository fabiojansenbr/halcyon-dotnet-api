using Halcyon.Api.Services.Email;

namespace Halcyon.Api.Models.Email
{
    public class VerifyEmailModel : IEmailModel
    {
        public string Template { get; } = "VerifyEmail";

        public string ToAddress { get; set; }

        public string Code { get; set; }
    }
}