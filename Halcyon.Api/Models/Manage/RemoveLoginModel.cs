using Halcyon.Api.Services.Providers;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Manage
{
    public class RemoveLoginModel
    {
        [Display(Name = "Provider")]
        [Required]
        public Provider Provider { get; set; }

        [Display(Name = "External Id")]
        [Required]
        public string ExternalId { get; set; }
    }
}