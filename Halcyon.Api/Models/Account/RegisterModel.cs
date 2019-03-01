using Halcyon.Api.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Account
{
    public class RegisterModel : BaseUserModel
    {
        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The {0} field cannot be less than {1} characters.")]
        [MaxLength(50, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string Password { get; set; }
    }
}