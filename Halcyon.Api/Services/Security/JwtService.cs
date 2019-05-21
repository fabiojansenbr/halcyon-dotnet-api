using Halcyon.Api.Entities;
using Halcyon.Api.Repositories;
using Halcyon.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Security
{
    public class JwtService : IJwtService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public JwtService(
            IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenModel> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim("sub", user.Id.ToString()),
                new Claim("email", user.EmailAddress),
                new Claim("given_name", user.FirstName),
                new Claim("family_name", user.LastName),
                new Claim("picture", user.Picture)
            };

            claims.AddRange(user.Roles.Select(role => new Claim("role", role)));

            var token = new JwtSecurityToken(
                audience: "HalcyonClient",
                issuer: "HalcyonApi",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey)),
                    SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await GenerateRefreshToken(user);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private async Task<string> GenerateRefreshToken(User user)
        {
            var token = Guid.NewGuid().ToString();

            user.RefreshTokens = user.RefreshTokens.Skip(Math.Max(0, user.RefreshTokens.Count - 10)).ToList();
            user.RefreshTokens.Add(token);

            await _userRepository.UpdateUser(user);

            return token;
        }
    }
}