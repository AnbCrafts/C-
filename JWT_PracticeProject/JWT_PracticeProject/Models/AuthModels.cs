using System.ComponentModel.DataAnnotations;

namespace JWT_PracticeProject.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Used as the password/credential according to labTest.loginUsers (@username, @phone)
        /// </summary>
        [Required(ErrorMessage = "Mobile No / Password is required")]
        public string MobileNo { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? AccessTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public QueryUser? User { get; set; }
    }

    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class UserRefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
