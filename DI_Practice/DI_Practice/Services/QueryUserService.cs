using System.Data;
using Microsoft.Data.SqlClient;
using DI_Practice.Models;

namespace DI_Practice.Services
{
    public class QueryUserService : IQueryUserService
    {
        private readonly string _connectionString;

        public QueryUserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DI_PracticeConnection")
                ?? throw new InvalidOperationException("Connection string 'DI_PracticeConnection' not found.");
        }

        public List<QueryUser> GetAllQueries()
        {
            var list = new List<QueryUser>();

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("[labTest].[sp_GetAllQueryUsers]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new QueryUser
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    UserName = reader["UserName"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    FullName = reader["FullName"].ToString() ?? string.Empty,
                    MobileNo = reader["MobileNo"].ToString() ?? string.Empty,
                    City = reader["City"].ToString() ?? string.Empty,
                    StateName = reader["StateName"].ToString() ?? string.Empty,
                    Country = reader["Country"].ToString() ?? string.Empty,
                    Gender = reader["Gender"].ToString() ?? string.Empty,
                    QueryText = reader["QueryText"].ToString() ?? string.Empty,
                    CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                    IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]),
                    QueryAmount = reader["QueryAmount"] != DBNull.Value ? Convert.ToDecimal(reader["QueryAmount"]) : null
                });
            }

            return list;
        }
    }
}
