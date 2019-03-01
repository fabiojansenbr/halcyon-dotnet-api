using System.Collections.Generic;

namespace Halcyon.Api.Models.User
{
    public class UpdateUserModel : BaseUserModel
    {
        public IEnumerable<string> Roles { get; set; }
    }
}