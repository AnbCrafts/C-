using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Repositories
{
    internal class ProductRepo
    {
        public static List<Products> productList = new List<Products>();

        public static List<Products> GetProducts(int count)
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
    }
}
