using PracticeAssignment2.DTOs.Users;

namespace PracticeAssignment2.Services;

public interface IJwtService
{
    string GenerateToken(UserAuthRecord user);
    DateTime GetExpirationDate();
}
