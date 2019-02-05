namespace Halcyon.Api.Services.Security
{
    public interface ITwoFactorService
    {
        TwoFactorModel GenerateToken(string label, string emailAddress);

        bool VerifyToken(string token, string secret);
    }
}