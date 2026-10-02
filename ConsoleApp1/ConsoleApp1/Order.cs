using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }

        // Navigation property for Products included in the Order
        public List<Products> Products { get; set; } = new List<Products>();
    }
}
