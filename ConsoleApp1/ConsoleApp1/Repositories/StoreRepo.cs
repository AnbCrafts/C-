using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Repositories
{
    internal class StoreRepo
    {
        public static List<Store> storeList= new List<Store>();

        public static void AddStore(Store store)
        {
            storeList.Add(store);
            Console.WriteLine($"Store named {store.StoreName} Added in the sotre list\n");

        }
        public static List<Employee> getEmpWorkingInStore(int id)
        {
            List<Employee> empWorking = new List<Employee>();
            empWorking = storeList.Find(c=>c.StoreId== id)?.Employees;
            if (empWorking.Count() == 0)
            {
            Console.WriteLine($"Store with ID - {id} has no employees added\n");

            }
            return empWorking;
        }

        public static List<Order> getStoreOrders(int id)
        {
            List<Order> orderList= new List<Order>();
            orderList = storeList.Find(s=>s.StoreId== id)?.Orders;
            if (orderList.Count()==0)
            {
                Console.WriteLine($"Store with ID - {id} has no orders yet\n");

            }

            return orderList;
        }

        public static List<Customer> getStoreCustomers(int id)
        {

            List<Customer> customerList = new List<Customer>();
            customerList = storeList.Find(s => s.StoreId == id)?.Customers;
            if (customerList.Count() == 0)
            {
                Console.WriteLine($"Store with ID - {id} has no customers yet\n");

            }
            return customerList;
        }


        

        
       

    }
}
