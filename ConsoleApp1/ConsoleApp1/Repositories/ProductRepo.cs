using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp1.Repositories
{
    public class ProductRepo
    {
        public static List<Products> productList = new List<Products>();

        private static string GetConnectionString()
        {
            var connSetting = ConfigurationManager.ConnectionStrings["StoreDB"];
            if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
            {
                return connSetting.ConnectionString;
            }
            return "Data Source=ANUBHAW;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";
        }

        // 1. ADD PRODUCT TO DB (ExecuteScalar to retrieve generated ProductId)
        public static void AddProduct(Products product, Store? store = null)
        {
            if (product == null)
            {
                Console.WriteLine("Product object cannot be null.\n");
                return;
            }

            if (store != null)
            {
                product.StoreId = store.StoreId;
            }

            string query = @"INSERT INTO practiceProjects.Products (ProductName, Category, Price, StockQuantity, StoreId)
                             VALUES (@ProductName, @Category, @Price, @StockQuantity, @StoreId);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Category", product.Category ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                    cmd.Parameters.AddWithValue("@StoreId", product.StoreId > 0 ? product.StoreId : (object)DBNull.Value);

                    try
                    {
                        conn.Open();
                        object newId = cmd.ExecuteScalar();
                        if (newId != null && newId != DBNull.Value)
                        {
                            product.ProductId = Convert.ToInt32(newId);
                        }
                        Console.WriteLine($"Product '{product.ProductName}' added to DB with ProductId = {product.ProductId}.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding product to DB: {ex.Message}\n");
                    }
                }
            }
        }

        // 2. GET ALL PRODUCTS FROM DB (ExecuteReader)
        public static List<Products> GetAllProducts()
        {
            var list = new List<Products>();
            string query = "SELECT ProductId, ProductName, Category, Price, StockQuantity, StoreId FROM practiceProjects.Products";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Products
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString() ?? string.Empty,
                                    Category = reader["Category"].ToString() ?? string.Empty,
                                    Price = Convert.ToDouble(reader["Price"]),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching products: {ex.Message}\n");
                    }
                }
            }
            return list;
        }

        // 3. GET PRODUCT BY ID (ExecuteReader)
        public static Products? GetProductById(int productId)
        {
            string query = "SELECT ProductId, ProductName, Category, Price, StockQuantity, StoreId FROM practiceProjects.Products WHERE ProductId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Products
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString() ?? string.Empty,
                                    Category = reader["Category"].ToString() ?? string.Empty,
                                    Price = Convert.ToDouble(reader["Price"]),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching product by ID: {ex.Message}\n");
                    }
                }
            }

            Console.WriteLine($"Product with ID {productId} not found in DB.\n");
            return null;
        }

        // 4. UPDATE PRODUCT IN DB (ExecuteNonQuery)
        public static void UpdateProduct(int productId, string productName, string category, double price, int stockQuantity)
        {
            string query = @"UPDATE practiceProjects.Products
                             SET ProductName = ISNULL(NULLIF(@ProductName, ''), ProductName),
                                 Category = ISNULL(NULLIF(@Category, ''), Category),
                                 Price = CASE WHEN @Price > 0 THEN @Price ELSE Price END,
                                 StockQuantity = CASE WHEN @StockQuantity >= 0 THEN @StockQuantity ELSE StockQuantity END
                             WHERE ProductId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    cmd.Parameters.AddWithValue("@ProductName", productName ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Category", category ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Product ID {productId} updated successfully in DB.\n");
                        else
                            Console.WriteLine($"Product ID {productId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating product: {ex.Message}\n");
                    }
                }
            }
        }

        // 5. REMOVE PRODUCT FROM DB (ExecuteNonQuery)
        public static void RemoveProduct(int productId)
        {
            string query = "DELETE FROM practiceProjects.Products WHERE ProductId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Product ID {productId} deleted from DB.\n");
                        else
                            Console.WriteLine($"Product ID {productId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting product: {ex.Message}\n");
                    }
                }
            }
        }

        // 6. GET PRODUCTS BY CATEGORY FROM DB (ExecuteReader)
        public static List<Products> GetProductsByCategory(string category)
        {
            var list = new List<Products>();
            string query = "SELECT ProductId, ProductName, Category, Price, StockQuantity, StoreId FROM practiceProjects.Products WHERE Category = @Category";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Category", category ?? string.Empty);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Products
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString() ?? string.Empty,
                                    Category = reader["Category"].ToString() ?? string.Empty,
                                    Price = Convert.ToDouble(reader["Price"]),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error searching products by category: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"No products found in category '{category}'.\n");
            }
            return list;
        }
    }
}
