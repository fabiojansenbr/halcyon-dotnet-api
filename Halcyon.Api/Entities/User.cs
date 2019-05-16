using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Halcyon.Api.Entities
{
    public class User
    {
        public User()
        {
            Id = ObjectId.GenerateNewId();
            Logins = new List<UserLogin>();
            Roles = new List<string>();
            RefreshTokens = new List<UserRefreshToken>();
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string EmailAddress { get; set; }

        [BsonIgnoreIfDefault]
        public string Password { get; set; }

        [BsonRequired]
        public string FirstName { get; set; }

        [BsonRequired]
        public string LastName { get; set; }

        [BsonRequired]
        [BsonDateTimeOptions]
        public DateTime DateOfBirth { get; set; }

        [BsonIgnoreIfDefault]
        public string VerifyEmailToken { get; set; }

        [BsonIgnoreIfDefault]
        public string PasswordResetToken { get; set; }

        [BsonIgnoreIfDefault]
        public string TwoFactorSecret { get; set; }

        [BsonIgnoreIfDefault]
        public string TwoFactorTempSecret { get; set; }

        [BsonIgnoreIfDefault]
        public bool EmailConfirmed { get; set; }

        [BsonIgnoreIfDefault]
        public bool IsLockedOut { get; set; }

        [BsonIgnoreIfDefault]
        public bool TwoFactorEnabled { get; set; }

        public bool HasPassword => !string.IsNullOrEmpty(Password);

        public List<UserLogin> Logins { get; set; }

        public List<string> Roles { get; set; }

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