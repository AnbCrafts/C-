using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TestPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString)) 
            {
                con.Open();
                string query = "Select COUNT(*) from [dbo].[BillOfMaterials]";
                string fetchQuery = "Select * from [dbo].[BillOfMaterials]";
                //string fetchQuery = 
                string twoQuery = "Select * from [dbo].[BillOfMaterials]; select * from [dbo].[CompanyHolidays]";
                string insertQuery = "INSERT INTO [practiceProjects].[Stores] ([StoreName], [Location], [ContactNumber]) VALUES ('Downtown Tech Store', 'New York, NY', '555-0101')";
                string procedure = "udp_GetBOMByMaterialParentID";
                //using (SqlCommand cmd = new SqlCommand(query,con))  
                //{
                //    int totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                //    Console.WriteLine($"Total {totalCount} Bill Of Materials found");
                
                //}
               
                //using (SqlCommand cmd = new SqlCommand(fetchQuery, con))
                //{
                //    SqlDataReader reader = cmd.ExecuteReader();
                //    while (reader.Read()) 
                //    { 
                //        Console.WriteLine($"ParentPartId - {Convert.ToString(reader["ParentPartId"])}");

                //    }
                //}

                con.Close();
                con.Open();
                //using (SqlCommand cmd = new SqlCommand(insertQuery,con))
                //{
                //    int affectedRows = Convert.ToInt32(cmd.ExecuteNonQuery());
                //    Console.WriteLine($"Total {affectedRows} inserted in Store Table");

                //}


                //using(SqlDataAdapter da = new SqlDataAdapter(fetchQuery, con))
                //{
                //    DataTable dt = new DataTable();
                //    da.Fill(dt);

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        Console.WriteLine($"{dr["ParentPartID"]}");
                //    }
                    
                //}

                using (SqlCommand cmd = new SqlCommand(procedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter p = new SqlParameter("@message", SqlDbType.VarChar, 100);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@id",300);
                    cmd.Parameters.Add(p);
                    using (SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine($"Child - {dr["ChildPartID"]} Quantity - {dr["QuantityRequired"]}");
                        }

                    }
                        Console.WriteLine($"Output Message: {p.Value}");




                }

                //using (SqlDataAdapter da = new SqlDataAdapter(twoQuery, con))
                //{
                //    DataSet ds = new DataSet();
                //    da.Fill(ds);
                //    ds.Tables[0].TableName = "BOM";
                //    ds.Tables[1].TableName = "Holidays";

                //    Console.WriteLine("BOM Table - \n");
                //    foreach(DataRow dr in ds.Tables["BOM"].Rows)
                //    {
                //        Console.WriteLine($"{dr["ParentPartID"]}");
                //    }

                //    Console.WriteLine("Materials Table - \n");
                //    foreach (DataRow dr in ds.Tables["Holidays"].Rows)
                //    {
                //        Console.WriteLine($"{dr["HolidayName"]}");
                //    }


                //}


                Console.ReadLine();


            }
        
        
        }
    }
}
