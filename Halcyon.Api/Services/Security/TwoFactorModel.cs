namespace Halcyon.Api.Services.Security
{
    public class TwoFactorModel
    {
        public string Secret { get; set; }

        public string AuthenticatorUri { get; set; }
    }
}