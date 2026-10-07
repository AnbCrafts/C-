using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp1.Repositories
{
    public class OrderRepo
    {
        public static List<Order> orderList = new List<Order>();

        private static string GetConnectionString()
        {
            var connSetting = ConfigurationManager.ConnectionStrings["StoreDB"];
            if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
            {
                return connSetting.ConnectionString;
            }
            return "Data Source=ANUBHAW;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";
        }

        // 1. ADD ORDER TO DB (ExecuteScalar to retrieve generated OrderId)
        public static void AddOrder(Order order, Customer? customer = null, Store? store = null)
        {
            if (order == null)
            {
                Console.WriteLine("Order object cannot be null.\n");
                return;
            }

            if (customer != null) order.CustomerId = customer.CustomerId;
            if (store != null) order.StoreId = store.StoreId;

            string query = @"INSERT INTO practiceProjects.Orders (CustomerId, StoreId, OrderDate, TotalAmount, Status)
                             VALUES (@CustomerId, @StoreId, @OrderDate, @TotalAmount, @Status);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId > 0 ? order.CustomerId : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@StoreId", order.StoreId > 0 ? order.StoreId : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate == default ? DateTime.Now : order.OrderDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(order.Status) ? "Pending" : order.Status);

                    try
                    {
                        conn.Open();
                        object newId = cmd.ExecuteScalar();
                        if (newId != null && newId != DBNull.Value)
                        {
                            order.OrderId = Convert.ToInt32(newId);
                        }
                        Console.WriteLine($"Order #{order.OrderId} placed successfully in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error placing order in DB: {ex.Message}\n");
                    }
                }
            }
        }

        // 2. GET ALL ORDERS FROM DB (ExecuteReader)
        public static List<Order> GetAllOrders()
        {
            var list = new List<Order>();
            string query = "SELECT OrderId, CustomerId, StoreId, OrderDate, TotalAmount, Status FROM practiceProjects.Orders";

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
                                list.Add(new Order
                                {
                                    OrderId = Convert.ToInt32(reader["OrderId"]),
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : 0,
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
                        Console.WriteLine($"Error fetching orders: {ex.Message}\n");
                    }
                }
            }
            return list;
        }

        // 3. GET ORDER BY ID (ExecuteReader)
        public static Order? GetOrderById(int orderId)
        {
            string query = "SELECT OrderId, CustomerId, StoreId, OrderDate, TotalAmount, Status FROM practiceProjects.Orders WHERE OrderId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", orderId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Order
                                {
                                    OrderId = Convert.ToInt32(reader["OrderId"]),
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : 0,
                                    StoreId = reader["StoreId"] != DBNull.Value ? Convert.ToInt32(reader["StoreId"]) : 0,
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    TotalAmount = Convert.ToDouble(reader["TotalAmount"]),
                                    Status = reader["Status"].ToString() ?? string.Empty
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching order by ID: {ex.Message}\n");
                    }
                }
            }

            Console.WriteLine($"Order #{orderId} not found in DB.\n");
            return null;
        }

        // 4. UPDATE ORDER STATUS IN DB (ExecuteNonQuery)
        public static void UpdateOrderStatus(int orderId, string newStatus)
        {
            string query = "UPDATE practiceProjects.Orders SET Status = @Status WHERE OrderId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", orderId);
                    cmd.Parameters.AddWithValue("@Status", newStatus ?? "Pending");

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($"Order #{orderId} status updated to '{newStatus}' in DB.\n");
                        else
                            Console.WriteLine($"Order #{orderId} not found in DB.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating order status: {ex.Message}\n");
                    }
                }
            }
        }

        // 5. CANCEL ORDER IN DB (ExecuteNonQuery)
        public static void CancelOrder(int orderId)
        {
            UpdateOrderStatus(orderId, "Cancelled");
        }

        // 6. GET ORDERS BY STATUS FROM DB (ExecuteReader)
        public static List<Order> GetOrdersByStatus(string status)
        {
            var list = new List<Order>();
            string query = "SELECT OrderId, CustomerId, StoreId, OrderDate, TotalAmount, Status FROM practiceProjects.Orders WHERE Status = @Status";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status ?? string.Empty);
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
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : 0,
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
                        Console.WriteLine($"Error searching orders by status: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"No orders found with status '{status}'.\n");
            }
            return list;
        }
    }
}
