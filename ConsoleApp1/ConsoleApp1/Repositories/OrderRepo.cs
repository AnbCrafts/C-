using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Repositories
{
    internal class OrderRepo
    {
        public static List<Order> orderList = new List<Order>();

        public static List<Order> GetOrders(int count)
        {
            var list = new List<Order>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Order
                {
                    OrderId = 100 + i,
                    CustomerId = i,
                    OrderDate = DateTime.Now.AddDays(-i),
                    TotalAmount = i * 250.50,
                    Status = (i % 2 == 0) ? "Completed" : "Pending"
                });
            }
            return list;
        }

        public static void AddOrder(Order order, Customer? customer, Store? store)
        {
            if (order == null)
            {
                Console.WriteLine("Order object cannot be null.\n");
                return;
            }

            if (customer != null)
            {
                order.CustomerId = customer.CustomerId;
                if (!customer.Orders.Contains(order))
                {
                    customer.Orders.Add(order);
                }
            }

            if (store != null)
            {
                order.StoreId = store.StoreId;
                if (!store.Orders.Contains(order))
                {
                    store.Orders.Add(order);
                }
            }

            orderList.Add(order);
            Console.WriteLine($"Order #{order.OrderId} placed successfully.\n");
        }

        public static Order? GetOrderById(int orderId)
        {
            var order = orderList.Find(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine($"Order #{orderId} not found.\n");
                return null;
            }
            return order;
        }

        public static void UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                Console.WriteLine($"Order #{orderId} status updated to '{newStatus}'.\n");
            }
        }

        public static void CancelOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                Console.WriteLine($"Order #{orderId} has been cancelled.\n");
            }
        }

        public static List<Order> GetOrdersByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return new List<Order>();

            var list = orderList.FindAll(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            if (list.Count == 0)
            {
                Console.WriteLine($"No orders found with status '{status}'.\n");
            }
            return list;
        }
    }
}
