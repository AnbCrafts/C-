using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.DTOs.Users;
using PracticeAssignment2.Repositories;
using PracticeAssignment2.Services;

namespace PracticeAssignment2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public UsersController(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Register a new customer account.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponseDto<UserResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto request, CancellationToken ct)
    {
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var createdUser = await _userRepository.RegisterAsync(request, passwordHash, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponseDto<UserResponseDto>.Ok(createdUser, "User registered successfully."));
    }

    /// <summary>
    /// Authenticate user and issue JWT bearer token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponseDto<UserLoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Invalid email or password."
            });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Account Inactive",
                Detail = "Your account is deactivated. Please contact administrator."
            });
        }

        var token = _jwtService.GenerateToken(user);
        var expiresAt = _jwtService.GetExpirationDate();

        var response = new UserLoginResponseDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role,
            Token = token,
            ExpiresAt = expiresAt
        };

        return Ok(ApiResponseDto<UserLoginResponseDto>.Ok(response, "Login successful."));
    }

    /// <summary>
    /// Get user profile by UserId (restricted to account owner or Admin).
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        if (!IsAdminOrOwner(id))
            return Forbid();

        var user = await _userRepository.GetByIdAsync(id, ct);
        if (user == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"User with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<UserResponseDto>.Ok(user));
    }

    /// <summary>
    /// Update user profile details (restricted to account owner or Admin).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponseDto<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateUserProfileRequestDto request, CancellationToken ct)
    {
        if (!IsAdminOrOwner(id))
            return Forbid();

        var updated = await _userRepository.UpdateProfileAsync(id, request, ct);
        if (updated == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"User with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<UserResponseDto>.Ok(updated, "Profile updated successfully."));
    }

    /// <summary>
    /// Toggle user active status (Admin only).
    /// </summary>
    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleStatus(int id, [FromBody] ToggleUserStatusRequestDto request, CancellationToken ct)
    {
        var success = await _userRepository.ToggleStatusAsync(id, request.IsActive, ct);
        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"User with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<bool>.Ok(true, $"User status updated to {(request.IsActive ? "Active" : "Inactive")}."));
    }

    private bool IsAdminOrOwner(int targetUserId)
    {
        var currentRole = User.FindFirstValue(ClaimTypes.Role);
        if (string.Equals(currentRole, "Admin", StringComparison.OrdinalIgnoreCase))
            return true;

        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(currentUserIdStr, out var currentUserId) && currentUserId == targetUserId;
    }
}
