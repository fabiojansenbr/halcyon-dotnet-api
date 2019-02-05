using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Manage
{
    public class SetPasswordModel
    {
        [Display(Name = "New Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The {0} field cannot be less than {1} characters.")]
        [MaxLength(50, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string NewPassword { get; set; }
    }
}