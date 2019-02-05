using Halcyon.Api.Services.Email;

namespace Halcyon.Api.Models.Email
{
    public class PasswordResetEmailModel : IEmailModel
    {
        public string Template { get; } = "ResetPassword";

        public string ToAddress { get; set; }

        public string Code { get; set; }
    }
}