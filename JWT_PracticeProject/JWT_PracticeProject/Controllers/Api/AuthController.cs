using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWT_PracticeProject.Models;
using JWT_PracticeProject.Repositories;
using JWT_PracticeProject.Services;

namespace JWT_PracticeProject.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IJwtService _jwtService;

        public AuthController(
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Please provide both username and mobile number."
                });
            }

            // Stored procedure call: labTest.loginUsers
            var (user, dbMessage) = await _userRepository.ValidateUserAsync(request.UserName, request.MobileNo);

            if (user == null)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = string.IsNullOrWhiteSpace(dbMessage) ? "Invalid credentials." : dbMessage
                });
            }

            // Generate Access Token (JWT)
            var (accessToken, accessExpires) = _jwtService.GenerateAccessToken(user);

            // Generate Refresh Token
            var (refreshToken, refreshExpires) = _jwtService.GenerateRefreshToken();

            // Save Refresh Token using stored procedure: labTest.sp_SaveRefreshToken
            await _tokenRepository.SaveRefreshTokenAsync(user.UserId, refreshToken, refreshExpires);

            // Also set cookies for smooth MVC web page navigation if desired
            Response.Cookies.Append("jwt_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = accessExpires
            });

            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = refreshExpires
            });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = string.IsNullOrWhiteSpace(dbMessage) ? "Login successful." : dbMessage,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessExpires,
                RefreshTokenExpiresAt = refreshExpires,
                User = user
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AccessToken) || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Both access token and refresh token are required."
                });
            }

            // Extract claims from expired access token
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid access token signature or format."
                });
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? principal.FindFirst("UserId")?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid user identifier in token."
                });
            }

            // Fetch stored refresh token via stored procedure: labTest.sp_GetRefreshToken
            var storedToken = await _tokenRepository.GetRefreshTokenAsync(request.RefreshToken);
            if (storedToken == null || storedToken.UserId != userId)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Refresh token is invalid."
                });
            }

            if (storedToken.IsRevoked)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Refresh token has been revoked."
                });
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Refresh token has expired. Please log in again."
                });
            }

            // Fetch current user details via stored procedure: labTest.sp_GetQueryUserById
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "User account not found or is inactive."
                });
            }

            // Generate new token pair (refresh token rotation)
            var (newAccessToken, accessExpires) = _jwtService.GenerateAccessToken(user);
            var (newRefreshToken, refreshExpires) = _jwtService.GenerateRefreshToken();

            // Save new refresh token via stored procedure: labTest.sp_SaveRefreshToken
            await _tokenRepository.SaveRefreshTokenAsync(user.UserId, newRefreshToken, refreshExpires);

            // Update cookies
            Response.Cookies.Append("jwt_token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = accessExpires
            });

            Response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = refreshExpires
            });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Token refreshed successfully.",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt = accessExpires,
                RefreshTokenExpiresAt = refreshExpires,
                User = user
            });
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new { success = false, message = "Refresh token is required." });
            }

            // Revoke via stored procedure: labTest.sp_RevokeRefreshToken
            await _tokenRepository.RevokeRefreshTokenAsync(request.RefreshToken);

            Response.Cookies.Delete("jwt_token");
            Response.Cookies.Delete("refresh_token");

            return Ok(new { success = true, message = "Token revoked successfully." });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? identity.FindFirst("UserId")?.Value;
            var username = identity.FindFirst(ClaimTypes.Name)?.Value;
            var email = identity.FindFirst(ClaimTypes.Email)?.Value;
            var mobile = identity.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var fullName = identity.FindFirst("FullName")?.Value;

            return Ok(new
            {
                UserId = userId,
                UserName = username,
                Email = email,
                MobileNo = mobile,
                FullName = fullName,
                Claims = identity.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }
    }
}
