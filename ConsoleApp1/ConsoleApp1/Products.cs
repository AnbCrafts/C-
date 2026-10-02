using System;

namespace ConsoleApp1
{
    public class Products
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public int StoreId { get; set; }
    }
}
