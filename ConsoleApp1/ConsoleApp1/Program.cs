using System;
using System.Collections.Generic;
using ConsoleApp1.Repositories;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("   REPOSITORIES FULL CRUD & RELATIONSHIPS DEMO    ");
            Console.WriteLine("==================================================\n");

            // 1. Create a Store
            Store mainStore = new Store("Tech MegaStore Kolkata", "Kolkata", "+91 9876543210");
            StoreRepo.AddStore(mainStore);

            // 2. Add Employees to Store via EmployeeRepo
            Employee emp1 = new Employee { EmployeeId = 1, Name = "Anubhaw Roy", Designation = "Store Manager", Salary = 65000, Department = "Management" };
            Employee emp2 = new Employee { EmployeeId = 2, Name = "Priya Sharma", Designation = "Sales Executive", Salary = 35000, Department = "Sales" };
            EmployeeRepo.AddEmployee(emp1, mainStore);
            EmployeeRepo.AddEmployee(emp2, mainStore);

            // 3. Add Products to Catalog & Store via ProductRepo
            Products prod1 = new Products { ProductId = 101, ProductName = "Gaming Laptop", Category = "Electronics", Price = 85000, StockQuantity = 10 };
            Products prod2 = new Products { ProductId = 102, ProductName = "Wireless Headphones", Category = "Accessories", Price = 4500, StockQuantity = 25 };
            ProductRepo.AddProduct(prod1, mainStore);
            ProductRepo.AddProduct(prod2, mainStore);

            // 4. Add Customers via CustomerRepo
            Customer cust1 = new Customer { CustomerId = 1, Name = "Rohan Verma", Email = "rohan@mail.com", Phone = "9811223344", Address = "Park Street, Kolkata" };
            CustomerRepo.AddCustomer(cust1, mainStore);

            // 5. Add Orders via OrderRepo
            Order order1 = new Order
            {
                OrderId = 5001,
                OrderDate = DateTime.Now,
                TotalAmount = 89500,
                Status = "Pending",
                Products = new List<Products> { prod1, prod2 }
            };
            OrderRepo.AddOrder(order1, cust1, mainStore);

            // 6. View Complete Store Details & Hierarchy
            Console.WriteLine("\n--- DISPLAYING FULL STORE DETAILS ---");
            StoreRepo.GetStoreDetails(mainStore.StoreId);

            // 7. Test Update & Filter Facilities
            Console.WriteLine("--- TESTING UPDATE & FILTER FACILITIES ---");
            OrderRepo.UpdateOrderStatus(order1.OrderId, "Completed");
            ProductRepo.UpdateProduct(prod1.ProductId, "Gaming Laptop Pro", "Electronics", 90000, 8);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
