using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Repositories
{
    internal class CustomerRepo
    {
        public static List<Customer> customerList = new List<Customer>();

        public static List<Customer> GetCustomers(int count)
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

        public static void AddCustomer(Customer cust, Store? store)
        {
            if (cust == null)
            {
                Console.WriteLine("Customer object cannot be null.\n");
                return;
            }

            if (store != null)
            {
                cust.StoreId = store.StoreId;
                if (!store.Customers.Contains(cust))
                {
                    store.Customers.Add(cust);
                }
            }

            customerList.Add(cust);
            Console.WriteLine($"Customer '{cust.Name}' added successfully.\n");
        }

        public static Customer? GetCustomerById(int customerId)
        {
            var cust = customerList.Find(c => c.CustomerId == customerId);
            if (cust == null)
            {
                Console.WriteLine($"Customer with ID {customerId} not found.\n");
                return null;
            }
            return cust;
        }

        public static void UpdateCustomer(int customerId, string name, string email, string phone, string address)
        {
            var cust = GetCustomerById(customerId);
            if (cust != null)
            {
                if (!string.IsNullOrWhiteSpace(name)) cust.Name = name;
                if (!string.IsNullOrWhiteSpace(email)) cust.Email = email;
                if (!string.IsNullOrWhiteSpace(phone)) cust.Phone = phone;
                if (!string.IsNullOrWhiteSpace(address)) cust.Address = address;
                Console.WriteLine($"Customer ID {customerId} updated successfully.\n");
            }
        }

        public static void RemoveCustomer(int customerId)
        {
            var cust = GetCustomerById(customerId);
            if (cust != null)
            {
                customerList.Remove(cust);
                // Also remove from linked store if applicable
                var store = StoreRepo.storeList.Find(s => s.StoreId == cust.StoreId);
                store?.Customers.Remove(cust);

                Console.WriteLine($"Customer ID {customerId} removed successfully.\n");
            }
        }

        public static List<Order> GetCustomerOrders(int customerId)
        {
            var cust = GetCustomerById(customerId);
            if (cust == null) return new List<Order>();

            if (cust.Orders.Count == 0)
            {
                Console.WriteLine($"Customer '{cust.Name}' has no order history.\n");
            }
            return cust.Orders;
        }
    }
}
