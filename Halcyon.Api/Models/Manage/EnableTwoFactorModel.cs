using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Manage
{
    public class EnableTwoFactorModel
    {
        [Display(Name = "Verification Code")]
        [Required]
        public string VerificationCode { get; set; }
    }
}
