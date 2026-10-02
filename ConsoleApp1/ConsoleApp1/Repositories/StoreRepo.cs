using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Repositories
{
    internal class StoreRepo
    {
        public static List<Store> storeList= new List<Store>();

        public static void AddStore(Store store)
        {
            if (store == null)
            {
                Console.WriteLine("Store cannot be null\n");
                return;
            }

            storeList.Add(store);
            Console.WriteLine($"Store named {store.StoreName} Added in the store list\n");
        }
        
        public static List<Employee> GetEmpWorkingInStore(int id)
        {
            var store = storeList.Find(c => c.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID - {id} not found\n");
                return new List<Employee>();
            }

            if (store.Employees.Count == 0)
            {
                Console.WriteLine($"Store with ID - {id} has no employees added\n");
            }
            return store.Employees;
        }


        public static List<Order> GetStoreOrders(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID - {id} not found\n");
                return new List<Order>();
            }

            if (store.Orders.Count == 0)
            {
                Console.WriteLine($"Store with ID - {id} has no orders yet\n");
            }

            return store.Orders;
        }


        public static List<Customer> GetStoreCustomers(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID - {id} not found\n");
                return new List<Customer>();
            }

            if (store.Customers.Count == 0)
            {
                Console.WriteLine($"Store with ID - {id} has no customers yet\n");
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

                Console.WriteLine("Store updated successfully.");
            }
            else
            {
                Console.WriteLine("Store not found.");
            }
        }

        public static Store? GetStoreDetails(int id)
        {
            var store = storeList.Find(s => s.StoreId == id);
            if (store == null)
            {
                Console.WriteLine($"Store with ID - {id} not found.\n");
                return null;
            }

            Console.WriteLine("==================================================");
            Console.WriteLine($"STORE DETAILS [ID: {store.StoreId}]");
            Console.WriteLine($"Name:           {store.StoreName}");
            Console.WriteLine($"Location:       {store.Location}");
            Console.WriteLine($"Contact Number: {store.ContactNumber}");
            Console.WriteLine("--------------------------------------------------");

            // Call employee details method with proper empty list check
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

            // Call customer details method with proper empty list check
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

            // Call order details method with proper empty list check
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
