using PracticeAssignment2.DTOs.Users;

namespace PracticeAssignment2.Repositories;

public interface IUserRepository
{
    Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto request, string passwordHash, CancellationToken ct = default);
    Task<UserAuthRecord?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<UserResponseDto?> GetByIdAsync(int userId, CancellationToken ct = default);
    Task<UserResponseDto?> UpdateProfileAsync(int userId, UpdateUserProfileRequestDto request, CancellationToken ct = default);
    Task<bool> ToggleStatusAsync(int userId, bool isActive, CancellationToken ct = default);
}
