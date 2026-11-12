using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment.Models.DTOs;
using PracticeAssignment.Repositories;
using PracticeAssignment.Services;

namespace PracticeAssignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtService _jwtService;

    public AuthController(IAuthRepository authRepository, IJwtService jwtService)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Authenticates a user and returns a signed JWT Bearer Token.
    /// </summary>
    /// <param name="loginDto">User credentials (username & password)</param>
    /// <returns>Signed JWT token and user info</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _authRepository.ValidateUserAsync(loginDto.Username, loginDto.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new { message = "User account is deactivated." });
        }

        var tokenResponse = _jwtService.GenerateToken(user);
        return Ok(tokenResponse);
    }
}
