using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Manage
{
    public class ChangePasswordModel : SetPasswordModel
    {
        [Display(Name = "Current Password")]
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}