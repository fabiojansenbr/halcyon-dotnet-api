using System;
using System.Collections.Generic;

namespace Halcyon.Api.Models.User
{
    public class UserModel
    {
        public string Id { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsLockedOut { get; set; }

        public bool HasPassword { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string Picture { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<ExternalLoginModel> Logins { get; set; }
    }
}