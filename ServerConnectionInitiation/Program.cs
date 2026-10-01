using System;
using System.Collections.Generic;
using System.Configuration;    // DOT NET DATA PROVIDER
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ServerConnectionInitiation
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string connectionString = "Data Source=.;Initial Catalog=AssignmentDB;User ID=sa;Password=mcc#1234";

            //using app.config file

            string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();
                string query = "select * from Assignment.products  INNER JOIN Assignment.productVariant ON Assignment.products.productId = Assignment.productVariant.productId";
      
                SqlCommand cmd = new SqlCommand(query, conn);
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) 
                    {
                        Console.WriteLine($"ID : {reader["productId"]} Name : {reader["productName"]}");

                    }
                    conn.Close();

                    conn.Open();

                    string insertQuery = "INSERT INTO [labTest].[products]\r\n(productName, category, finishType, binderType, vocLevel, createdAt)\r\nVALUES\r\n('Apex3 Shield2','Exterior','Satin','Pure Acrylic',45.00,'2026-09-12')";
                    SqlCommand cmdI = new SqlCommand(insertQuery, conn);
                    int rowsAffected = cmdI.ExecuteNonQuery();
                    Console.WriteLine("Inserted Rows = " + rowsAffected);


                    Console.WriteLine("---------------");


                    Console.WriteLine("Using Data Reader");
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        Console.WriteLine(sdr["productId"] + ",  " + sdr["productName"] );
                    }
                    conn.Close();

                    Console.WriteLine("---------------");


   
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select * from Assignment.products", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);



                    Console.WriteLine("---------------");


                    Console.WriteLine("Using Data Table");


                    foreach (DataRow row in dt.Rows)
                    {
                        Console.WriteLine(row["productId"] + ",  " + row["productName"]);
                    }
                    Console.WriteLine("---------------");

                }
            }
        }
    }
}
