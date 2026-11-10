using System.Data;
using Microsoft.Data.SqlClient;
using JWT_PracticeProject.Models;

namespace JWT_PracticeProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LmsConnection")
                ?? throw new InvalidOperationException("Connection string 'LmsConnection' not found in configuration.");
        }

        public async Task<(QueryUser? user, string message)> ValidateUserAsync(string username, string phone)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.loginUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", username ?? string.Empty);
                    cmd.Parameters.AddWithValue("@phone", phone ?? string.Empty);

                    SqlParameter msgParam = new SqlParameter("@message", SqlDbType.VarChar, 400)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    await conn.OpenAsync();

                    QueryUser? user = null;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = MapUserFromReader(reader);
                        }
                    }

                    string message = msgParam.Value != DBNull.Value ? msgParam.Value?.ToString() ?? "" : "";
                    return (user, message);
                }
            }
        }

        public async Task<List<QueryUser>> GetAllUsersAsync()
        {
            List<QueryUser> users = new List<QueryUser>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_GetAllQueryUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(MapUserFromReader(reader));
                        }
                    }
                }
            }

            return users;
        }

        public async Task<QueryUser?> GetUserByIdAsync(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_GetQueryUserById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        public async Task<(int newId, string message)> CreateUserAsync(QueryUser user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_InsertQueryUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FullName", (object?)user.FullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobileNo", (object?)user.MobileNo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", (object?)user.City ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@StateName", (object?)user.StateName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Country", (object?)user.Country ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", (object?)user.Gender ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QueryText", (object?)user.QueryText ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@QueryAmount", (object?)user.QueryAmount ?? DBNull.Value);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int newId = reader["NewUserId"] != DBNull.Value ? Convert.ToInt32(reader["NewUserId"]) : 0;
                            string message = reader["Result"] != DBNull.Value ? reader["Result"].ToString() ?? "" : "";
                            return (newId, message);
                        }
                    }

                    return (0, "No result returned from procedure.");
                }
            }
        }

        public async Task<(bool success, string message)> UpdateUserAsync(QueryUser user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_UpdateQueryUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FullName", (object?)user.FullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobileNo", (object?)user.MobileNo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@City", (object?)user.City ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@StateName", (object?)user.StateName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Country", (object?)user.Country ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", (object?)user.Gender ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@QueryText", (object?)user.QueryText ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@QueryAmount", (object?)user.QueryAmount ?? DBNull.Value);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int rowsAffected = reader["RowsAffected"] != DBNull.Value ? Convert.ToInt32(reader["RowsAffected"]) : 0;
                            string message = reader["Result"] != DBNull.Value ? reader["Result"].ToString() ?? "" : "";
                            return (rowsAffected > 0, message);
                        }
                    }

                    return (false, "No result returned from procedure.");
                }
            }
        }

        public async Task<(bool success, string message)> DeleteUserAsync(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("labTest.sp_DeleteQueryUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int rowsAffected = reader["RowsAffected"] != DBNull.Value ? Convert.ToInt32(reader["RowsAffected"]) : 0;
                            string message = reader["Result"] != DBNull.Value ? reader["Result"].ToString() ?? "" : "";
                            return (rowsAffected > 0, message);
                        }
                    }

                    return (false, "No result returned from procedure.");
                }
            }
        }

        private static QueryUser MapUserFromReader(SqlDataReader reader)
        {
            return new QueryUser
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() ?? string.Empty : string.Empty,
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() ?? string.Empty : string.Empty,
                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null,
                MobileNo = reader["MobileNo"] != DBNull.Value ? reader["MobileNo"].ToString() : null,
                City = reader["City"] != DBNull.Value ? reader["City"].ToString() : null,
                StateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : null,
                Country = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : null,
                Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : null,
                QueryText = reader["QueryText"] != DBNull.Value ? reader["QueryText"].ToString() : null,
                CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]),
                QueryAmount = reader["QueryAmount"] != DBNull.Value ? Convert.ToDecimal(reader["QueryAmount"]) : null
            };
        }
    }
}
