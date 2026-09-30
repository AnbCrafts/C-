using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. CUSTOMERS ===");
            var customers = GetCustomers();
            foreach (var c in customers)
            {
                Console.WriteLine($"ID: {c.CustomerId}, Name: {c.Name}, Email: {c.Email}, Phone: {c.Phone}, Address: {c.Address}");
            }

            Console.WriteLine("\n=== 2. EMPLOYEES ===");
            var employees = GetEmployees();
            foreach (var e in employees)
            {
                Console.WriteLine($"ID: {e.EmployeeId}, Name: {e.Name}, Designation: {e.Designation}, Dept: {e.Department}, Salary: ${e.Salary:F2}");
            }

            Console.WriteLine("\n=== 3. ORDERS ===");
            var orders = GetOrders();
            foreach (var o in orders)
            {
                Console.WriteLine($"ID: {o.OrderId}, CustomerId: {o.CustomerId}, Date: {o.OrderDate:yyyy-MM-dd}, Amount: ${o.TotalAmount:F2}, Status: {o.Status}");
            }

            Console.WriteLine("\n=== 4. PRODUCTS ===");
            var products = GetProducts();
            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}, Category: {p.Category}, Price: ${p.Price:F2}, Stock: {p.StockQuantity}");
            }

            Console.WriteLine("\n=== 5. STORES ===");
            var stores = GetStores();
            foreach (var s in stores)
            {
                Console.WriteLine($"ID: {s.StoreId}, Name: {s.StoreName}, Location: {s.Location}, Contact: {s.ContactNumber}");
            }
        }

        // Helper Method for Customers
        public static List<Customer> GetCustomers(int count = 5)
        {
            var list = new List<Customer>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Customer
                {
                    CustomerId = i,
                    Name = $"Customer{i}",
                    Email = $"customer{i}@mail.com",
                    Phone = $"987654320{i}",
                    Address = $"Street {i}, City"
                });
            }
            return list;
        }

        // Helper Method for Employees
        public static List<Employee> GetEmployees(int count = 5)
        {
            var list = new List<Employee>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Employee
                {
                    EmployeeId = i,
                    Name = $"Employee{i}",
                    Designation = $"Designation{i}",
                    Salary = 30000 + (i * 5000),
                    Department = $"Dept{i}"
                });
            }
            return list;
        }

        // Helper Method for Orders
        public static List<Order> GetOrders(int count = 5)
        {
            var list = new List<Order>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Order
                {
                    OrderId = 100 + i,
                    CustomerId = i,
                    OrderDate = DateTime.Now.AddDays(-i),
                    TotalAmount = i * 250.50,
                    Status = (i % 2 == 0) ? "Completed" : "Pending"
                });
            }
            return list;
        }

        // Helper Method for Products
        public static List<Products> GetProducts(int count = 5)
        {
            var list = new List<Products>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Products
                {
                    ProductId = i,
                    ProductName = $"Product{i}",
                    Category = $"Category{(i % 3) + 1}",
                    Price = i * 19.99,
                    StockQuantity = i * 10
                });
            }
            return list;
        }

        // Helper Method for Stores
        public static List<Store> GetStores(int count = 5)
        {
            var list = new List<Store>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Store
                {
                    StoreId = i,
                    StoreName = $"Store{i}",
                    Location = $"Location{i}",
                    ContactNumber = $"1800-000-00{i}"
                });
            }
            return list;
        }
    }
}
