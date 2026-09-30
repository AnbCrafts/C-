using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;    // DOT NET DATA PROVIDER

namespace ServerConnectionInitiation
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=AssignmentDB;User ID=sa;Password=mcc#1234";
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();
                string query = "select * from Assignment.products  INNER JOIN Assignment.productVariant ON Assignment.products.productId = Assignment.productVariant.productId";
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) 
                    {
                        Console.WriteLine($"ID : {reader["productId"]}");

                    }
                
                }
            }
        }
    }
}
