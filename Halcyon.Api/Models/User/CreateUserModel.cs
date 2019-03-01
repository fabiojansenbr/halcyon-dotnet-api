using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.User
{
    public class CreateUserModel : BaseUserModel
    {
        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The {0} field cannot be less than {1} characters.")]
        [MaxLength(50, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}