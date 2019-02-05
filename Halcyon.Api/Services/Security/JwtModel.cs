using System;

namespace Halcyon.Api.Services.Security
{
    public class JwtModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
