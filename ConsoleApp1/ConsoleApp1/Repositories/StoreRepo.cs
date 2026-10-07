using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;

namespace ConsoleApp1.Repositories
{
    public class StoreRepo
    {
        public static List<Store> storeList = new List<Store>();

        // Helper to retrieve connection string from App.config ("StoreDB")
        private static string GetConnectionString()
        {
            var connSetting = ConfigurationManager.ConnectionStrings["StoreDB"];
            if (connSetting != null && !string.IsNullOrWhiteSpace(connSetting.ConnectionString))
            {
                return connSetting.ConnectionString;
            }
            // Fallback connection string
            return "Data Source=ANUBHAW;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;";
        }

        // AUTO-SETUP: Ensures 'practiceProjects' schema and tables exist in the connected database
        public static void EnsureTablesExist()
        {
            string setupSql = @"
                IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'practiceProjects')
                BEGIN
                    EXEC('CREATE SCHEMA practiceProjects');
                END;

                IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'practiceProjects' AND t.name = 'Stores')
                BEGIN
                    CREATE TABLE practiceProjects.Stores (
                        StoreId INT PRIMARY KEY IDENTITY(1,1),
                        StoreName VARCHAR(100) NOT NULL,
                        Location VARCHAR(100) NOT NULL,
                        ContactNumber VARCHAR(20) NOT NULL
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'practiceProjects' AND t.name = 'Employees')
                BEGIN
                    CREATE TABLE practiceProjects.Employees (
                        EmployeeId INT PRIMARY KEY IDENTITY(1,1),
                        Name VARCHAR(100) NOT NULL,
                        Designation VARCHAR(50) NOT NULL,
                        Salary DECIMAL(10,2) NOT NULL,
                        Department VARCHAR(50) NOT NULL,
                        StoreId INT NULL,
                        CONSTRAINT FK_Employees_Stores FOREIGN KEY (StoreId) REFERENCES practiceProjects.Stores(StoreId) ON DELETE SET NULL
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'practiceProjects' AND t.name = 'Customers')
                BEGIN
                    CREATE TABLE practiceProjects.Customers (
                        CustomerId INT PRIMARY KEY IDENTITY(1,1),
                        Name VARCHAR(100) NOT NULL,
                        Email VARCHAR(100) NOT NULL,
                        Phone VARCHAR(20) NOT NULL,
                        Address VARCHAR(200) NOT NULL,
                        StoreId INT NULL,
                        CONSTRAINT FK_Customers_Stores FOREIGN KEY (StoreId) REFERENCES practiceProjects.Stores(StoreId) ON DELETE SET NULL
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'practiceProjects' AND t.name = 'Products')
                BEGIN
                    CREATE TABLE practiceProjects.Products (
                        ProductId INT PRIMARY KEY IDENTITY(101,1),
                        ProductName VARCHAR(100) NOT NULL,
                        Category VARCHAR(50) NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,
                        StockQuantity INT NOT NULL DEFAULT 0,
                        StoreId INT NULL,
                        CONSTRAINT FK_Products_Stores FOREIGN KEY (StoreId) REFERENCES practiceProjects.Stores(StoreId) ON DELETE SET NULL
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sys.tables t JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'practiceProjects' AND t.name = 'Orders')
                BEGIN
                    CREATE TABLE practiceProjects.Orders (
                        OrderId INT PRIMARY KEY IDENTITY(5001,1),
                        CustomerId INT NULL,
                        StoreId INT NULL,
                        OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
                        TotalAmount DECIMAL(10,2) NOT NULL,
                        Status VARCHAR(50) NOT NULL DEFAULT 'Pending',
                        CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES practiceProjects.Customers(CustomerId) ON DELETE SET NULL,
                        CONSTRAINT FK_Orders_Stores FOREIGN KEY (StoreId) REFERENCES practiceProjects.Stores(StoreId) ON DELETE SET NULL
                    );
                END;";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(setupSql, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error initializing tables: {ex.Message}");
                    }
                }
            }
        }

        // 1. ADD STORE (ExecuteScalar to retrieve newly generated IDENTITY StoreId)
        public static void AddStore(Store store)
        {
            if (store == null)
            {
                Console.WriteLine("Store object cannot be null.\n");
                return;
            }

            string query = @"INSERT INTO practiceProjects.Stores (StoreName, Location, ContactNumber) 
                            VALUES (@StoreName, @Location, @ContactNumber);
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreName", store.StoreName);
                    cmd.Parameters.AddWithValue("@Location", store.Location);
                    cmd.Parameters.AddWithValue("@ContactNumber", store.ContactNumber);

                    try
                    {
                        conn.Open();
                        object newId = cmd.ExecuteScalar();
                        if (newId != null && newId != DBNull.Value)
                        {
                            store.StoreId = Convert.ToInt32(newId);
                        }
                        Console.WriteLine($" Store '{store.StoreName}' added to database successfully with StoreID = {store.StoreId}.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error adding store to DB: {ex.Message}\n");
                    }
                }
            }
        }

        // 2. GET EMPLOYEES WORKING IN STORE (ExecuteReader)
        public static List<Employee> GetEmpWorkingInStore(int storeId)
        {
            var list = new List<Employee>();
            string query = @"SELECT EmployeeId, Name, Designation, Salary, Department, StoreId 
                            FROM practiceProjects.Employees WHERE StoreId = @StoreId";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreId", storeId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Employee
                                {
                                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Designation = reader["Designation"].ToString() ?? string.Empty,
                                    Salary = Convert.ToDouble(reader["Salary"]),
                                    Department = reader["Department"].ToString() ?? string.Empty,
                                    StoreId = Convert.ToInt32(reader["StoreId"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error fetching store employees: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"Store with ID {storeId} has no employees assigned in database.\n");
            }
            return list;
        }

        // 3. GET STORE PRODUCTS (ExecuteReader)
        public static List<Products> GetStoreProducts(int storeId)
        {
            var list = new List<Products>();
            string query = @"SELECT ProductId, ProductName, Category, Price, StockQuantity, StoreId 
                            FROM practiceProjects.Products WHERE StoreId = @StoreId";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreId", storeId);
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
                                    StoreId = Convert.ToInt32(reader["StoreId"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error fetching store products: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"Store with ID {storeId} has no products in database inventory.\n");
            }
            return list;
        }

        // 4. GET STORE CUSTOMERS (ExecuteReader)
        public static List<Customer> GetStoreCustomers(int storeId)
        {
            var list = new List<Customer>();
            string query = @"SELECT CustomerId, Name, Email, Phone, Address, StoreId 
                            FROM practiceProjects.Customers WHERE StoreId = @StoreId";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreId", storeId);
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
                                    StoreId = Convert.ToInt32(reader["StoreId"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error fetching store customers: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"Store with ID {storeId} has no registered customers in database.\n");
            }
            return list;
        }

        // 5. GET STORE ORDERS (ExecuteReader)
        public static List<Order> GetStoreOrders(int storeId)
        {
            var list = new List<Order>();
            string query = @"SELECT OrderId, CustomerId, StoreId, OrderDate, TotalAmount, Status 
                            FROM practiceProjects.Orders WHERE StoreId = @StoreId";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreId", storeId);
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
                                    StoreId = Convert.ToInt32(reader["StoreId"]),
                                    OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                    TotalAmount = Convert.ToDouble(reader["TotalAmount"]),
                                    Status = reader["Status"].ToString() ?? string.Empty
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error fetching store orders: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"Store with ID {storeId} has no orders recorded in database.\n");
            }
            return list;
        }

        // 6. UPDATE STORE DETAILS (ExecuteNonQuery)
        public static void UpdateStoreDetails(int id, string name, string location, string contact)
        {
            string query = @"UPDATE practiceProjects.Stores 
                            SET StoreName = ISNULL(NULLIF(@Name, ''), StoreName),
                                Location = ISNULL(NULLIF(@Location, ''), Location),
                                ContactNumber = ISNULL(NULLIF(@Contact, ''), ContactNumber)
                            WHERE StoreId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Location", location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact", contact ?? (object)DBNull.Value);

                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($" Store ID {id} updated successfully in database.\n");
                        else
                            Console.WriteLine($" Store ID {id} not found in database.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error updating store: {ex.Message}\n");
                    }
                }
            }
        }

        // 7. DELETE STORE (ExecuteNonQuery)
        public static void DeleteStore(int id)
        {
            string query = "DELETE FROM practiceProjects.Stores WHERE StoreId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            Console.WriteLine($" Store ID {id} deleted successfully from database.\n");
                        else
                            Console.WriteLine($" Store ID {id} not found in database.\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error deleting store: {ex.Message}\n");
                    }
                }
            }
        }

        // 8. SEARCH STORES BY LOCATION (ExecuteReader)
        public static List<Store> SearchStoresByLocation(string location)
        {
            var list = new List<Store>();
            string query = "SELECT StoreId, StoreName, Location, ContactNumber FROM practiceProjects.Stores WHERE Location = @Location";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Location", location ?? string.Empty);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Store(
                                    reader["StoreName"].ToString() ?? string.Empty,
                                    reader["Location"].ToString() ?? string.Empty,
                                    reader["ContactNumber"].ToString() ?? string.Empty)
                                {
                                    StoreId = Convert.ToInt32(reader["StoreId"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error searching stores by location: {ex.Message}\n");
                    }
                }
            }

            if (list.Count == 0)
            {
                Console.WriteLine($"No stores found in location '{location}'.\n");
            }
            return list;
        }

        // 9. GET STORE DETAILS WITH RELATIONAL LISTS (ExecuteReader)
        public static Store? GetStoreDetails(int id)
        {
            Store? store = null;
            string query = "SELECT StoreId, StoreName, Location, ContactNumber FROM practiceProjects.Stores WHERE StoreId = @Id";

            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                store = new Store(
                                    reader["StoreName"].ToString() ?? string.Empty,
                                    reader["Location"].ToString() ?? string.Empty,
                                    reader["ContactNumber"].ToString() ?? string.Empty)
                                {
                                    StoreId = Convert.ToInt32(reader["StoreId"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error fetching store details: {ex.Message}\n");
                        return null;
                    }
                }
            }

            if (store == null)
            {
                Console.WriteLine($"Store with ID {id} not found in database.\n");
                return null;
            }

            // Populate relational child lists from SQL Server
            store.Employees = GetEmpWorkingInStore(id);
            store.Products = GetStoreProducts(id);
            store.Customers = GetStoreCustomers(id);
            store.Orders = GetStoreOrders(id);

            Console.WriteLine("==================================================");
            Console.WriteLine($"STORE DETAILS [ID: {store.StoreId}]");
            Console.WriteLine($"Name:           {store.StoreName}");
            Console.WriteLine($"Location:       {store.Location}");
            Console.WriteLine($"Contact Number: {store.ContactNumber}");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine($"--- Employees ({store.Employees.Count}) ---");
            foreach (var emp in store.Employees)
            {
                Console.WriteLine($"  - [ID: {emp.EmployeeId}] {emp.Name} | Designation: {emp.Designation} | Dept: {emp.Department} | Salary: ${emp.Salary}");
            }

            Console.WriteLine($"--- Products Inventory ({store.Products.Count}) ---");
            foreach (var prod in store.Products)
            {
                Console.WriteLine($"  - [ID: {prod.ProductId}] {prod.ProductName} | Category: {prod.Category} | Price: ${prod.Price} | Stock: {prod.StockQuantity}");
            }

            Console.WriteLine($"--- Customers ({store.Customers.Count}) ---");
            foreach (var cust in store.Customers)
            {
                Console.WriteLine($"  - [ID: {cust.CustomerId}] {cust.Name} | Email: {cust.Email} | Phone: {cust.Phone} | Address: {cust.Address}");
            }

            Console.WriteLine($"--- Orders ({store.Orders.Count}) ---");
            foreach (var order in store.Orders)
            {
                Console.WriteLine($"  - [Order #{order.OrderId}] Date: {order.OrderDate:yyyy-MM-dd} | Amount: ${order.TotalAmount} | Status: {order.Status}");
            }

            Console.WriteLine("==================================================\n");
            return store;
        }

        // 10. GET ALL STORES DETAILS (ExecuteReader)
        public static List<Store> GetAllStoresDetails()
        {
            var stores = new List<Store>();
            string query = "SELECT StoreId FROM practiceProjects.Stores";

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
                                int id = Convert.ToInt32(reader["StoreId"]);
                                var store = GetStoreDetails(id);
                                if (store != null) stores.Add(store);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Error fetching all stores: {ex.Message}\n");
                    }
                }
            }

            return stores;
        }
    }
}
