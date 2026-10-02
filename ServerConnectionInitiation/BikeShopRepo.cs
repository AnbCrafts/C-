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
                                Name = reader["Name"] ,
                                Price = reader["Price"] 
                            };
                            list.Add(b);
                        }
                    }
                }
            }

            bikeshops = list;
            return list;
        }


    }
}
