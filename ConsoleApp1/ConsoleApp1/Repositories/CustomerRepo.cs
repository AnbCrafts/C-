using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp1.Repositories
{
    public class CustomerRepo
    {
        public static List<Customer> customerList = new List<Customer>();

        private static string GetConnectionString()
        {
            var connSetting = ConfigurationManager.ConnectionStrings["StoreDB"];
            if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
            {
                return connSetting.ConnectionString;
            }
            return "Data Source=ANUBHAW;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";
        }

        // 1. ADD CUSTOMER TO DB (ExecuteScalar to get generated CustomerId)
        public static void AddCustomer(Customer cust, Store? store = null)
        {
            if (cust == null)
            {
                Console.WriteLine("Customer object cannot be null.\n");
                return;
            }

            if (store != null)
            {
                cust.StoreId = store.StoreId;
            }

            string query = @"INSERT INTO practiceProjects.Customers (Name, Email, Phone, Address, StoreId)
                             VALUES (@Name, @Email, @Phone, @Address, @StoreId);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", cust.Name ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Email", cust.Email ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Phone", cust.Phone ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Address", cust.Address ?? string.Empty);
                    cmd.Parameters.AddWithValue("@StoreId", cust.StoreId > 0 ? cust.StoreId : (object)DBNull.Value);

                    try
                    {
                        conn.Open();
                        object newId = cmd.ExecuteScalar();
                        if (newId != null && newId != DBNull.Value)
                        {
                            cust.CustomerId = Convert.ToInt32(newId);
                        }
                        Console.WriteLine($"Customer '{cust.Name}' added to DB with CustomerId = {cust.CustomerId}.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding customer to DB: {ex.Message}\n");
                    }
                }
            }
        }

        // 2. GET ALL CUSTOMERS FROM DB (ExecuteReader)
        public static List<Customer> GetAllCustomers()
        {
            var list = new List<Customer>();
            string query = "SELECT CustomerId, Name, Email, Phone, Address, StoreId FROM practiceProjects.Customers";

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
                                list.Add(new Customer
                                {
                                    CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Phone = reader["Phone"].ToString() ?? string.Empty,
                                    Address = reader["Address"].ToString() ?? string.Empty,
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching customers: {ex.Message}\n");
                    }
                }
            }
            return list;
        }

        // 3. GET CUSTOMER BY ID (ExecuteReader)
        public static Customer? GetCustomerById(int customerId)
        {
            string query = "SELECT CustomerId, Name, Email, Phone, Address, StoreId FROM practiceProjects.Customers WHERE CustomerId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Customer
                                {
                                    CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    Phone = reader["Phone"].ToString() ?? string.Empty,
                                    Address = reader["Address"].ToString() ?? string.Empty,
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching customer by ID: {ex.Message}\n");
                    }
                }
            }

            Console.WriteLine($"Customer with ID {customerId} not found in database.\n");
            return null;
        }

        // 4. UPDATE CUSTOMER IN DB (ExecuteNonQuery)
        public static void UpdateCustomer(int customerId, string name, string email, string phone, string address)
        {
            string query = @"UPDATE practiceProjects.Customers
                             SET Name = ISNULL(NULLIF(@Name, ''), Name),
                                 Email = ISNULL(NULLIF(@Email, ''), Email),
                                 Phone = ISNULL(NULLIF(@Phone, ''), Phone),
                                 Address = ISNULL(NULLIF(@Address, ''), Address)
                             WHERE CustomerId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    cmd.Parameters.AddWithValue("@Name", name ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Email", email ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Phone", phone ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Address", address ?? string.Empty);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Customer ID {customerId} updated successfully in DB.\n");
                        else
                            Console.WriteLine($"Customer ID {customerId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating customer: {ex.Message}\n");
                    }
                }
            }
        }

        // 5. REMOVE CUSTOMER FROM DB (ExecuteNonQuery)
        public static void RemoveCustomer(int customerId)
        {
            string query = "DELETE FROM practiceProjects.Customers WHERE CustomerId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Customer ID {customerId} deleted from DB.\n");
                        else
                            Console.WriteLine($"Customer ID {customerId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting customer: {ex.Message}\n");
                    }
                }
            }
        }

        // 6. GET CUSTOMER ORDERS FROM DB (ExecuteReader)
        public static List<Order> GetCustomerOrders(int customerId)
        {
            var list = new List<Order>();
            string query = @"SELECT OrderId, CustomerId, StoreId, OrderDate, TotalAmount, Status 
                             FROM practiceProjects.Orders WHERE CustomerId = @CustomerId";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Order
                                {
                                    OrderId = Convert.ToInt32(reader["OrderId"]),
                                    CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0,
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    TotalAmount = Convert.ToDouble(reader["TotalAmount"]),
                                    Status = reader["Status"].ToString() ?? string.Empty
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching customer orders: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"Customer ID {customerId} has no orders in DB.\n");
            }
            return list;
        }
    }
}
