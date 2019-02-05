using Halcyon.Api.Models.Manage;
using Halcyon.Api.Services.Providers;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Account
{
    public class RegisterExternalModel : BaseProfileModel
    {
        [Display(Name = "Provider")]
        public Provider Provider { get; set; }

        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }
    }
}
