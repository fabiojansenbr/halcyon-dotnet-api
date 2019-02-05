using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Halcyon.Api.Entities
{
    public class User
    {
        public User()
        {
            Logins = new List<UserLogin>();
            Roles = new List<UserRole>();
            RefreshTokens = new List<UserRefreshToken>();
        }

        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        [MaxLength(254)]
        public string EmailAddress { get; set; }

        [MaxLength(128)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(36)]
        public string VerifyEmailToken { get; set; }

        [MaxLength(36)]
        public string PasswordResetToken { get; set; }

        [MaxLength(50)]
        public string TwoFactorSecret { get; set; }

        [MaxLength(50)]
        public string TwoFactorTempSecret { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsLockedOut { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool HasPassword => !string.IsNullOrEmpty(Password);

        public List<UserLogin> Logins { get; set; }

        public List<UserRole> Roles { get; set; }

        public List<UserRefreshToken> RefreshTokens { get; set; }

        public string Picture
        {
            get
            {
                var md5Hasher = MD5.Create();
                var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(EmailAddress));
                var sb = new StringBuilder();

                foreach (var b in data)
                {
                    sb.Append(b.ToString("x2"));
                }

                return $"https://secure.gravatar.com/avatar/{sb}?d=mm";
            }
        }
    }
}