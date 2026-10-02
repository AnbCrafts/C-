using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
