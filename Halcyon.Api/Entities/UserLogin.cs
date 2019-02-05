using Halcyon.Api.Services.Providers;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Entities
{
    public class UserLogin
    {
        public UserLogin(Provider provider, string externalId)
        {
            Provider = provider;
            ExternalId = externalId;
        }

        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        public Provider Provider { get; set; }

        [Required]
        [MaxLength(50)]
        public string ExternalId { get; set; }
    }
}