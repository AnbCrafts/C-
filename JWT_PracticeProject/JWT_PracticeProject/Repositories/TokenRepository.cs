using System.Data;
using Microsoft.Data.SqlClient;
using JWT_PracticeProject.Models;

namespace JWT_PracticeProject.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _connectionString;

        public TokenRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LmsConnection")
                ?? throw new InvalidOperationException("Connection string 'LmsConnection' not found in configuration.");
        }

        public async Task SaveRefreshTokenAsync(int userId, string token, DateTime expiresAt)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_SaveRefreshToken", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@ExpiresAt", expiresAt);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<UserRefreshToken?> GetRefreshTokenAsync(string token)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_GetRefreshToken", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Token", token);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserRefreshToken
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Token = reader["Token"].ToString() ?? string.Empty,
                                ExpiresAt = Convert.ToDateTime(reader["ExpiresAt"]),
                                IsRevoked = Convert.ToBoolean(reader["IsRevoked"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                RevokedAt = reader["RevokedAt"] != DBNull.Value ? Convert.ToDateTime(reader["RevokedAt"]) : null
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_RevokeRefreshToken", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Token", token);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
