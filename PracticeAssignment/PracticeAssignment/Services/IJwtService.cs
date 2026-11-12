using PracticeAssignment.Models;
using PracticeAssignment.Models.DTOs;

namespace PracticeAssignment.Services;

public interface IJwtService
{
    LoginResponseDto GenerateToken(User user);
}
