using System.Security.Claims;
using JWT_PracticeProject.Models;

namespace JWT_PracticeProject.Services
{
    public interface IJwtService
    {
        (string token, DateTime expiresAt) GenerateAccessToken(QueryUser user);
        (string token, DateTime expiresAt) GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
