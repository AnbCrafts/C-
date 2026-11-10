using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JWT_PracticeProject.Models;

namespace JWT_PracticeProject.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["JwtSettings:SecretKey"] ?? "ThisIsMyUltraSecretKeyForJwtAuthenticationPracticeProject2026!";
            _issuer = _configuration["JwtSettings:Issuer"] ?? "JWT_PracticeProject";
            _audience = _configuration["JwtSettings:Audience"] ?? "JWT_PracticeProjectClient";
            _accessTokenExpirationMinutes = int.TryParse(_configuration["JwtSettings:AccessTokenExpirationMinutes"], out int accessExp) ? accessExp : 15;
            _refreshTokenExpirationDays = int.TryParse(_configuration["JwtSettings:RefreshTokenExpirationDays"], out int refreshExp) ? refreshExp : 7;
        }

        public (string token, DateTime expiresAt) GenerateAccessToken(QueryUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.MobileNo ?? string.Empty),
                new Claim("FullName", user.FullName ?? string.Empty),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return (tokenHandler.WriteToken(token), expiresAt);
        }

        public (string token, DateTime expiresAt) GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            string token = Convert.ToBase64String(randomNumber);
            DateTime expiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

            return (token, expiresAt);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateLifetime = false, // We want to inspect claims of expired token during token refresh
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
