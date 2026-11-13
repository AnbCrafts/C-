using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PracticeAssignment2.DTOs.Users;

namespace PracticeAssignment2.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(UserAuthRecord user)
    {
        var keyString = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is missing from configuration.");
        var issuer = _configuration["Jwt:Issuer"] ?? "HotelBookingApi";
        var audience = _configuration["Jwt:Audience"] ?? "HotelBookingApiUsers";
        var durationMinutes = double.TryParse(_configuration["Jwt:DurationInMinutes"], out var minutes) ? minutes : 120;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(durationMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public DateTime GetExpirationDate()
    {
        var durationMinutes = double.TryParse(_configuration["Jwt:DurationInMinutes"], out var minutes) ? minutes : 120;
        return DateTime.UtcNow.AddMinutes(durationMinutes);
    }
}
