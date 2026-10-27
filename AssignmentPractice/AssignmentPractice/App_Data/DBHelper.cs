using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagement
{
    public static class DbHelper
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        public static DataTable ExecuteDataTable(string storedProcedureName, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        
        public static int ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters = null)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        
        public static object ExecuteScalar(string storedProcedureName, SqlParameter[] parameters = null)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    result = cmd.ExecuteScalar();
                }
            }

            return result;
        }
    }
}