using System;
using System.Collections.Generic;

namespace Halcyon.Api.Models.Manage
{
    public class ProfileModel
    {
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool HasPassword { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string Picture { get; set; }

        public IEnumerable<ExternalLoginModel> Logins { get; set; }
    }
}
