namespace Halcyon.Api.Services.Security
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 10);
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}