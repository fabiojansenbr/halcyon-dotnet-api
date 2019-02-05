using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Account
{
    public class ResetPasswordModel
    {
        [Display(Name = "Code")]
        [Required]
        public string Code { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Display(Name = "New Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The {0} field cannot be less than {1} characters.")]
        [MaxLength(50, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string NewPassword { get; set; }
    }
}