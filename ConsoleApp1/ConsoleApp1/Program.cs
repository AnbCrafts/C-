using System;
using System.Collections.Generic;
using ConsoleApp1.Repositories;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Store s = new Store("Sample Store","Kolkata", "+91 9988776655");

            StoreRepo.AddStore(s);
            Console.WriteLine(s.StoreId); Console.WriteLine(s.StoreName);

           // Fetch and display single store details
           StoreRepo.GetStoreDetails(s.StoreId);

           // Fetch and display all stores details
           StoreRepo.GetAllStoresDetails();

           Console.WriteLine("Press any key to exit...");
           Console.ReadKey();
        }

       
       
    }
}
