using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void AddProduct(Products product, Store? store)
        {
            if (product == null)
            {
                Console.WriteLine("Product object cannot be null.\n");
                return;
            }

            if (store != null)
            {
                product.StoreId = store.StoreId;
                if (!store.Products.Contains(product))
                {
                    store.Products.Add(product);
                }
            }

            productList.Add(product);
            Console.WriteLine($"Product '{product.ProductName}' added to catalog successfully.\n");
        }

        public static Products? GetProductById(int productId)
        {
            var prod = productList.Find(p => p.ProductId == productId);
            if (prod == null)
            {
                Console.WriteLine($"Product with ID {productId} not found.\n");
                return null;
            }
            return prod;
        }

        public static void UpdateProduct(int productId, string productName, string category, double price, int stockQuantity)
        {
            var prod = GetProductById(productId);
            if (prod != null)
            {
                if (!string.IsNullOrWhiteSpace(productName)) prod.ProductName = productName;
                if (!string.IsNullOrWhiteSpace(category)) prod.Category = category;
                if (price > 0) prod.Price = price;
                if (stockQuantity >= 0) prod.StockQuantity = stockQuantity;

                Console.WriteLine($"Product ID {productId} updated successfully.\n");
            }
        }

        public static void RemoveProduct(int productId)
        {
            var prod = GetProductById(productId);
            if (prod != null)
            {
                productList.Remove(prod);
                var store = StoreRepo.storeList.Find(s => s.StoreId == prod.StoreId);
                store?.Products.Remove(prod);

                Console.WriteLine($"Product ID {productId} removed from catalog.\n");
            }
        }

        public static List<Products> GetProductsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return new List<Products>();

            var list = productList.FindAll(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (list.Count == 0)
            {
                Console.WriteLine($"No products found in category '{category}'.\n");
            }
            return list;
        }
    }
}
