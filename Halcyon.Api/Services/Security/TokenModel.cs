using System;

namespace Halcyon.Api.Services.Security
{
    public class TokenModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
