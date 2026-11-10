using JWT_PracticeProject.Models;

namespace JWT_PracticeProject.Repositories
{
    public interface IUserRepository
    {
        Task<(QueryUser? user, string message)> ValidateUserAsync(string username, string phone);
        Task<List<QueryUser>> GetAllUsersAsync();
        Task<QueryUser?> GetUserByIdAsync(int userId);
        Task<(int newId, string message)> CreateUserAsync(QueryUser user);
        Task<(bool success, string message)> UpdateUserAsync(QueryUser user);
        Task<(bool success, string message)> DeleteUserAsync(int userId);
    }
}
