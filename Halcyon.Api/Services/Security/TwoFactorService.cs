using OtpNet;

namespace Halcyon.Api.Services.Security
{
    public class TwoFactorService : ITwoFactorService
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public TwoFactorModel GenerateToken(string label, string emailAddress)
        {
            var secret = KeyGeneration.GenerateRandomKey(10);
            var base32String = Base32Encoding.ToString(secret);

            var authenticatorUri = string.Format(
                AuthenticatorUriFormat,
                label,
                emailAddress,
                base32String);

            return new TwoFactorModel
            {
                Secret = base32String,
                AuthenticatorUri = authenticatorUri
            };
        }

        public bool VerifyToken(string token, string secret)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret))
            {
                return false;
            }

            var base32Bytes = Base32Encoding.ToBytes(secret);
            var otp = new Totp(base32Bytes);
            return otp.VerifyTotp(token, out var timeStepMatched);
        }
    }
}
