using System;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.User
{
    public abstract class BaseUserModel
    {
        [Display(Name = "Email Address")]
        [Required]
        [EmailAddress]
        [MaxLength(254, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string EmailAddress { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "The {0} field cannot be longer than {1} characters.")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}