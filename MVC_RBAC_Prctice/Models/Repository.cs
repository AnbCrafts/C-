using System;
using System.Data;
using System.Data.SqlClient;

namespace MVC_RBAC_Prctice.Models
{
    public class Repository
    {
        private string connectionString = "Data Source=.;Initial Catalog=AssignmentDB;User ID=sa;Password=mcc#1234;";

        public User GetUserDetails(string userName, string password)
        {
            User user = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();


                using (SqlCommand cmd = new SqlCommand("loginUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", userName);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new User
                            {
                                UserID = Convert.ToInt64(dr["UserID"]),
                                UserName = dr["UserName"].ToString(),
                                Roles = dr["Roles"].ToString(),
                                Password = dr["Password"].ToString(),
                                FullName = dr["FullName"] != DBNull.Value ? dr["FullName"].ToString() : ""
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
