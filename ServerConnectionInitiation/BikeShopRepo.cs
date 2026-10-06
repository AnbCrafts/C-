using System;
using System.Configuration;   

using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;

namespace ServerConnectionInitiation
{
    public class BikeshopRepo
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        public List<Bikeshop> bikeshops = new List<Bikeshop>();

        public void AddBikeShop(Bikeshop bikeshop)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO [labTest].[Bikeshop] (Name, Price) VALUES (@Name, @Price)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", bikeshop.Name);
                    cmd.Parameters.AddWithValue("@Price", bikeshop.Price);
                    cmd.ExecuteNonQuery();
                }
            }
            bikeshops.Add(bikeshop);
        }
        public List<Bikeshop> GetAllBikeShop()
        {
            List<Bikeshop> list = new List<Bikeshop>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Name, Price FROM [labTest].[Bikeshop]";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bikeshop b = new Bikeshop
                            {
                                Name = Convert.ToString(reader["Name"]),
                                Price = Convert.ToInt32(reader["Price"])
                            };
                            list.Add(b);
                        }
                    }
                }
            }


            return list;
        }
        public Bikeshop GetBikeById(int id)
        {
            Bikeshop b = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM labTest.Bikeshop WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            b = new Bikeshop
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"]),
                                Price = Convert.ToInt32(reader["Price"])

                            };
                        }
                    }
                }
            }

            return b;
        }
        public void DeleteBikeById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM labTest.Bikeshop WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Bike deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No bike found with that id.");
                    }
                }
            }
        }
        public void UpdateBikeById(int id, string name = "", decimal price = 0)
        {
            Bikeshop bike = GetBikeById(id);

            if (bike != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                UPDATE labTest.Bikeshop
                SET Name = @Name,
                    Price = @Price
                WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Price", price);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Bike updated successfully.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Bike not found.");
            }
        }
        
        public void CreateDataTable(string selectQuery)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                    da.SelectCommand.CommandType = CommandType.Text;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Console.WriteLine("Returned by CreateDataTable Method\n\n");

                    foreach (DataRow row in dt.Rows)
                    {
                        Console.WriteLine(
                            row["Name"] + ",  " +
                            row["Id"] + ",  " +
                            row["Price"]);
                    }
                }
            }
            catch (Exception e)

            {
                Console.WriteLine("Some error occured - \n"+ e.Message);
            }

            Console.ReadKey();
        }

        public void CreateDataSet(string selectQuery)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(selectQuery, connection);
                    DataSet dataSet = new DataSet();
                    da.Fill(dataSet);
                    dataSet.Tables[0].TableName = "Bikes";
                    Console.WriteLine("Returned by CreateDataSet Method\n\n");
                    foreach (DataRow row in dataSet.Tables["Bikes"].Rows)
                    {

                        Console.WriteLine(row["Id"] + ",  " + row["Name"] + ",  " + row["Price"]);
                    }
                }
            }
            catch (Exception e)

            {
                Console.WriteLine("Some error occured - \n" + e.Message);
            }

            Console.ReadKey();
        }
        public void CreateDataSetByProcedure(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();

                    da.SelectCommand = new SqlCommand("usp_GetDataById", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@Id", id);
                    


                    DataSet dataSet = new DataSet();
                    da.Fill(dataSet);

                    dataSet.Tables[0].TableName = "Bikes";

                    Console.WriteLine("Returned by CreateDataSetByProcedure Method\n");

                    foreach (DataRow row in dataSet.Tables["Bikes"].Rows)
                    {
                        Console.WriteLine(
                            row["Id"] + ", " +
                            row["Name"] + ", " +
                            row["Price"]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Some error occurred:\n" + e.Message);
            }

            Console.ReadKey();
        }

    }

}