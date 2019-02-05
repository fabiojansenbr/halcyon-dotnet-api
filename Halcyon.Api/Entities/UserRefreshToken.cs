using System;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Entities
{
    public class UserRefreshToken
    {
        public UserRefreshToken()
        {
            Token = Guid.NewGuid().ToString();
            Issued = DateTime.UtcNow;
        }

        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(36)]
        public string Token { get; set; }

        [Required]
        public DateTime Issued { get; set; }
    }
}