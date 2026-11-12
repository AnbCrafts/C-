using PracticeAssignment.Models;

namespace PracticeAssignment.Repositories;

public interface IAuthRepository
{
    Task<User?> ValidateUserAsync(string username, string password);
}
