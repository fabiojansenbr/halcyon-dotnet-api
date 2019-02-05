using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Entities
{
    public class UserRole
    {
        public UserRole(string name)
        {
            Name = name;
        }

        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}