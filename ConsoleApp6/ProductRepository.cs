using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ConsoleApp6
{
    public class ProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void AddProduct(Product product) {
            string insertQuery = "Insert into dbo.ProductTbl (Name,Price,Stock) values (@name,@price,@stock)";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(insertQuery,con)) { 
                    //cmd.CommandText = insertQuery;
                    //cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@stock", product.Stock);

                    con.Open();
                    int rows = Convert.ToInt32(cmd.ExecuteNonQuery());
                    if (rows > 0) {

                        Console.WriteLine($"Successfully added {rows} rows int the table");
                            
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add rows int the table");

                    }



                }
            }
        }
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string fetchQuery = "select * from dbo.ProductTbl";

            using (SqlConnection con = new SqlConnection(_connectionString)) {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(fetchQuery, con)) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {

                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                Stock = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            return products;
        }
        public Product GetProductbyId(int id)
        {
            Product product = new Product();
            string fetchOneQuery = "Select * from dbo.ProductTbl where Id = @id";


            using (SqlConnection con = new SqlConnection(_connectionString)) {
                using (SqlCommand cmd = new SqlCommand(fetchOneQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) { 
                        
                            product.Id = reader.GetInt32(0);
                            product.Name = reader.GetString(1);
                            product.Price = reader.GetDecimal(2);
                            product.Stock = reader.GetInt32(3);

                            return product;
                        }
                        else
                        {
                            Console.WriteLine("Failed to fetch Prodct with this id ");
                            return null;
                        }
                    
                    }

                }
                
            
            }

            
        }
        public void UpdateProduct(
    int id,
    string name = "",
    decimal price = 0,
    int stock = 0)
        {
            List<string> updates = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@id", id);

                    if (name != "")
                    {
                        updates.Add("Name = @Name");
                        cmd.Parameters.AddWithValue("@Name", name);
                    }

                    if (price != 0)
                    {
                        updates.Add("Price = @Price");
                        cmd.Parameters.AddWithValue("@Price", price);
                    }

                    if (stock != 0)
                    {
                        updates.Add("Stock = @Stock");
                        cmd.Parameters.AddWithValue("@Stock", stock);
                    }

                    if (updates.Count == 0)
                    {
                        Console.WriteLine("Nothing to update.");
                        return;
                    }

                    cmd.CommandText =
                        $"UPDATE dbo.ProductTbl SET {string.Join(", ", updates)} WHERE Id = @id";

                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine($"{rows} row(s) affected.");
                }
            }
        }
        public void DeleteProduct(int id)
        {
            

            string deleteQuery = "Delete from dbo.ProductTbl where Id = @id";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int row = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{row} row(s) affected by this operation");

                }

            }
        }
    }
}
