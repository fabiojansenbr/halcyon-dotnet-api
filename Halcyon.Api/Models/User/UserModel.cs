using System;
using System.Collections.Generic;

namespace Halcyon.Api.Models.User
{
    public class UserModel : UserSummaryModel
    {
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}