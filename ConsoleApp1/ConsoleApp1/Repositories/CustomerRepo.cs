using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Repositories
{
    internal class CustomerRepo
    {
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
    }
}
