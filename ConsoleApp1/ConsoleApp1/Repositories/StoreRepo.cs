using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Repositories
{
    internal class StoreRepo
    {
        public static List<Store> storeList = new List<Store>();

        public static void AddStore(Store store)
        {
            if (store == null)
            {
                Console.WriteLine("Store cannot be null\n");
                return;
            }

            storeList.Add(store);
            Console.WriteLine($"Store named '{store.StoreName}' added to store list.\n");
        }

        public static List<Employee> GetEmpWorkingInStore(int id)
        {
            var store = storeList.Find(c => c.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID {id} not found\n");
                return new List<Employee>();
            }

            if (store.Employees.Count == 0)
            {
                Console.WriteLine($"Store with ID {id} has no employees assigned.\n");
            }
            return store.Employees;
        }

        public static List<Products> GetStoreProducts(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID {id} not found\n");
                return new List<Products>();
            }

            if (store.Products.Count == 0)
            {
                Console.WriteLine($"Store with ID {id} has no products in inventory.\n");
            }
            return store.Products;
        }

        public static List<Order> GetStoreOrders(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID {id} not found\n");
                return new List<Order>();
            }

            if (store.Orders.Count == 0)
            {
                Console.WriteLine($"Store with ID {id} has no orders yet.\n");
            }

            return store.Orders;
        }

        public static List<Customer> GetStoreCustomers(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID {id} not found\n");
                return new List<Customer>();
            }

            if (store.Customers.Count == 0)
            {
                Console.WriteLine($"Store with ID {id} has no customers registered.\n");
            }
            return store.Customers;
        }

        public static void UpdateStoreDetails(int id, string name, string location, string contact)
        {
            var store = storeList.Find(s => s.StoreId == id);

            if (store != null)
            {
                if (!string.IsNullOrWhiteSpace(name))
                    store.StoreName = name;

                if (!string.IsNullOrWhiteSpace(location))
                    store.Location = location;

                if (!string.IsNullOrWhiteSpace(contact))
                    store.ContactNumber = contact;

                Console.WriteLine($"Store ID {id} updated successfully.\n");
            }
            else
            {
                Console.WriteLine($"Store with ID {id} not found.\n");
            }
        }

        public static void DeleteStore(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store != null)
            {
                storeList.Remove(store);
                Console.WriteLine($"Store ID {id} removed successfully.\n");
            }
            else
            {
                Console.WriteLine($"Store with ID {id} not found.\n");
            }
        }

        public static List<Store> SearchStoresByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location)) return new List<Store>();

            var list = storeList.FindAll(s => s.Location.Equals(location, StringComparison.OrdinalIgnoreCase));
            if (list.Count == 0)
            {
                Console.WriteLine($"No stores found in location '{location}'.\n");
            }
            return list;
        }

        public static Store? GetStoreDetails(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID {id} not found.\n");
                return null;
            }

            Console.WriteLine("==================================================");
            Console.WriteLine($"STORE DETAILS [ID: {store.StoreId}]");
            Console.WriteLine($"Name:           {store.StoreName}");
            Console.WriteLine($"Location:       {store.Location}");
            Console.WriteLine($"Contact Number: {store.ContactNumber}");
            Console.WriteLine("--------------------------------------------------");

            // Employees
            var employees = GetEmpWorkingInStore(id);
            Console.WriteLine("--- Employees ---");
            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("No employees found for this store.");
            }
            else
            {
                foreach (var emp in employees)
                {
                    Console.WriteLine($"  - [ID: {emp.EmployeeId}] {emp.Name} | Designation: {emp.Designation} | Dept: {emp.Department} | Salary: ${emp.Salary}");
                }
            }

            // Products
            var products = GetStoreProducts(id);
            Console.WriteLine("--- Products Inventory ---");
            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No products found for this store.");
            }
            else
            {
                foreach (var prod in products)
                {
                    Console.WriteLine($"  - [ID: {prod.ProductId}] {prod.ProductName} | Category: {prod.Category} | Price: ${prod.Price} | Stock: {prod.StockQuantity}");
                }
            }

            // Customers
            var customers = GetStoreCustomers(id);
            Console.WriteLine("--- Customers ---");
            if (customers == null || customers.Count == 0)
            {
                Console.WriteLine("No customers found for this store.");
            }
            else
            {
                foreach (var cust in customers)
                {
                    Console.WriteLine($"  - [ID: {cust.CustomerId}] {cust.Name} | Email: {cust.Email} | Phone: {cust.Phone} | Address: {cust.Address}");
                }
            }

            // Orders
            var orders = GetStoreOrders(id);
            Console.WriteLine("--- Orders ---");
            if (orders == null || orders.Count == 0)
            {
                Console.WriteLine("No orders found for this store.");
            }
            else
            {
                foreach (var order in orders)
                {
                    Console.WriteLine($"  - [Order #{order.OrderId}] Date: {order.OrderDate:yyyy-MM-dd} | Amount: ${order.TotalAmount} | Status: {order.Status}");
                }
            }

            Console.WriteLine("==================================================\n");
            return store;
        }

        public static List<Store> GetAllStoresDetails()
        {
            if (storeList == null || storeList.Count == 0)
            {
                Console.WriteLine("No stores available in the system.\n");
                return new List<Store>();
            }

            Console.WriteLine($"Listing details for all ({storeList.Count}) store(s):\n");
            foreach (var store in storeList)
            {
                GetStoreDetails(store.StoreId);
            }

            return storeList;
        }
    }
}
