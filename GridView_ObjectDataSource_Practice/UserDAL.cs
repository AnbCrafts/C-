using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GridView_ObjectDataSource_Practice
{
    public class QueryUserDAL
    {
        private static readonly string CS =
            ConfigurationManager.ConnectionStrings["ObjectDataSourceDB"].ConnectionString;

        public static List<QueryUser> GetAllUsers()
        {
            List<QueryUser> users = new List<QueryUser>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("udp_GetAllUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    users.Add(new QueryUser
                    {
                        UserId = Convert.ToInt32(rdr["UserId"]),
                        UserName = rdr["UserName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        FullName = rdr["FullName"].ToString(),
                        MobileNo = rdr["MobileNo"].ToString(),
                        City = rdr["City"].ToString(),
                        StateName = rdr["StateName"].ToString(),
                        Country = rdr["Country"].ToString(),
                        Gender = rdr["Gender"].ToString(),
                        QueryText = rdr["QueryText"].ToString(),
                        IsActive = Convert.ToBoolean(rdr["IsActive"]),
                        QueryAmount = rdr["QueryAmount"] == DBNull.Value
                            ? (decimal?)null
                            : Convert.ToDecimal(rdr["QueryAmount"])
                    });
                }
            }

            return users;
        }

        public static int InsertUser(
    string UserName,
    string Email,
    string FullName,
    string MobileNo,
    string City,
    string StateName,
    string Country,
    string Gender,
    string QueryText,
    decimal? QueryAmount)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("udp_CreateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@FullName", (object)FullName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MobileNo", (object)MobileNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@City", (object)City ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StateName", (object)StateName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Country", (object)Country ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", (object)Gender ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@QueryText", (object)QueryText ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@QueryAmount", (object)QueryAmount ?? DBNull.Value);

                con.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int UpdateUser(
            int UserId,
            string UserName,
            string Email,
            string FullName,
            string MobileNo,
            string City,
            string StateName,
            string Country,
            string Gender,
            string QueryText,
            bool IsActive,
            decimal? QueryAmount)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("udp_UpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@FullName", FullName);
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@City", City);
                cmd.Parameters.AddWithValue("@StateName", StateName);
                cmd.Parameters.AddWithValue("@Country", Country);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@QueryText", QueryText);
                cmd.Parameters.AddWithValue("@IsActive", IsActive);
                cmd.Parameters.AddWithValue("@QueryAmount",
                    (object)QueryAmount ?? DBNull.Value);

                con.Open();

                return cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteUserById(int UserId)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(
                    "udp_DeleteUserById",
                    con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", UserId);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}