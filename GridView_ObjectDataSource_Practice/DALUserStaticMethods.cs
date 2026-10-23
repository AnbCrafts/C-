using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GridView_ObjectDataSource_Practice
{
    public class QueryUserDALStatic
    {
        private static string connectionString =
            ConfigurationManager.ConnectionStrings["ObjectDataSourceDB"].ConnectionString;

        public static DataTable GetAllUsersShortDetailList(int indexNo, int pageSize, out int totalRows)
        {
            DataTable dt = new DataTable();
            totalRows = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("udp_GetDynamicallyPaginatedUsersShortDescriptionList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pgIndex", indexNo);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    SqlParameter totalRowsParam = new SqlParameter("@totalRows", SqlDbType.Int);
                    totalRowsParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(totalRowsParam);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    totalRows = Convert.ToInt32(totalRowsParam.Value);
                }
            }

            return dt;
        }
        public static DataTable GetAllUsers(int indexNo, int pageSize, out int totalRows)
        {
            DataTable dt = new DataTable();
            totalRows = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("udp_GetDynamicallyPaginatedUsersList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pgIndex", indexNo);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    SqlParameter totalRowsParam = new SqlParameter("@totalRows", SqlDbType.Int);
                    totalRowsParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(totalRowsParam);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    totalRows = Convert.ToInt32(totalRowsParam.Value);
                }
            }

            return dt;
        }
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd =
                    new SqlCommand("SELECT * FROM labTest.QueryUser", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
            }

            return dt;
        }

        public static DataTable GetUserById(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd =
                    new SqlCommand("udp_GetUserById", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@userId",
                    id);

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                da.Fill(dt);
            }

            return dt;
        }
        public static void DeleteUserById(int userId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM labTest.QueryUser
                      WHERE UserId = @UserId", con);

                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertUser(
            string userName,
            string email,
            string fullName,
            string mobileNo,
            string city,
            string stateName,
            string country,
            string gender,
            string queryText,
            decimal queryAmount)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                INSERT INTO labTest.QueryUser
                (
                    UserName,
                    Email,
                    FullName,
                    MobileNo,
                    City,
                    StateName,
                    Country,
                    Gender,
                    QueryText,
                    QueryAmount,
                    IsActive
                )
                VALUES
                (
                    @UserName,
                    @Email,
                    @FullName,
                    @MobileNo,
                    @City,
                    @StateName,
                    @Country,
                    @Gender,
                    @QueryText,
                    @QueryAmount,
                    1
                )", con);

                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@MobileNo", mobileNo);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@StateName", stateName);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@QueryText", queryText);
                cmd.Parameters.AddWithValue("@QueryAmount", queryAmount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateUser(
            int userId,
            string userName,
            string email,
            string fullName,
            string mobileNo,
            string city,
            string stateName,
            string country,
            string gender,
            string queryText,
            decimal queryAmount,
            bool isActive)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                UPDATE labTest.QueryUser
                SET
                    UserName = @UserName,
                    Email = @Email,
                    FullName = @FullName,
                    MobileNo = @MobileNo,
                    City = @City,
                    StateName = @StateName,
                    Country = @Country,
                    Gender = @Gender,
                    QueryText = @QueryText,
                    QueryAmount = @QueryAmount,
                    IsActive = @IsActive
                WHERE UserId = @UserId", con);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@MobileNo", mobileNo);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@StateName", stateName);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@QueryText", queryText);
                cmd.Parameters.AddWithValue("@QueryAmount", queryAmount);
                cmd.Parameters.AddWithValue("@IsActive", isActive);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}