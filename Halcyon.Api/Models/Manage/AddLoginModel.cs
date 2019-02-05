using Halcyon.Api.Services.Providers;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Manage
{
    public class AddLoginModel
    {
        [Display(Name = "Provider")]
        [Required]
        public Provider Provider { get; set; }

        [Display(Name = "Access Token")]
        [Required]
        public string AccessToken { get; set; }
    }
}
