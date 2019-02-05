using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Manage
{
    public class ConfirmEmailModel
    {
        [Display(Name = "Code")]
        [Required]
        public string Code { get; set; }
    }
}