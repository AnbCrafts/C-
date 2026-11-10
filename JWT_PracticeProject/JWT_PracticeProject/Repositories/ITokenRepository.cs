using JWT_PracticeProject.Models;

namespace JWT_PracticeProject.Repositories
{
    public interface ITokenRepository
    {
        Task SaveRefreshTokenAsync(int userId, string token, DateTime expiresAt);
        Task<UserRefreshToken?> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);
    }
}
