using Halcyon.Api.Models.Manage;
using System.Collections.Generic;

namespace Halcyon.Api.Models.User
{
    public abstract class BaseUserModel : BaseProfileModel
    {
        public IEnumerable<string> Roles { get; set; }
    }
}