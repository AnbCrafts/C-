using System;
using System.Collections.Generic;


namespace ConsoleApp1
{
    public class Store
    {
        
        private static int _nextStoreId = 0;
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string ContactNumber { get; set; }

        public Store( string name ,string loc , string con)
        {
            StoreId = ++_nextStoreId;
            StoreName = name;
            Location = loc;
            ContactNumber = con;
        }

        // Navigation properties establishing the hierarchy
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Products> Products { get; set; } = new List<Products>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
