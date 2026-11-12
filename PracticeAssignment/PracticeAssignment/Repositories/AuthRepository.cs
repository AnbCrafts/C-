using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment.Models;

namespace PracticeAssignment.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not found in configuration.");
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand("labTest.sp_ValidateUser", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Explicit parameter bindings to prevent SQL injection
        cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username });
        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 256) { Value = password });

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        return null;
    }
}
