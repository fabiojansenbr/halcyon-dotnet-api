using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Account
{
    public class ForgotPasswordModel
    {
        [Display(Name = "Email Address")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}