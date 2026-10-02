using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int StoreId { get; set; }

        // Navigation property for Customer's Orders
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
