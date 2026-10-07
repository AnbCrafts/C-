using System;
using System.Collections.Generic;
using ConsoleApp1.Repositories;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Ensure schema 'practiceProjects' and all 5 tables exist in SQL Server
            StoreRepo.EnsureTablesExist();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("==================================================");
                Console.WriteLine("        STORE & INVENTORY MANAGEMENT (SQL DB)     ");
                Console.WriteLine("==================================================");
                Console.WriteLine("--- STORE OPERATIONS ---");
                Console.WriteLine("1. Add a New Store");
                Console.WriteLine("2. View Store Details by ID (with Employees, Products, Customers, Orders)");
                Console.WriteLine("3. View All Stores");
                Console.WriteLine("4. Update Store Details");
                Console.WriteLine("5. Delete Store by ID");
                Console.WriteLine("--- ENTITY OPERATIONS ---");
                Console.WriteLine("6. Add an Employee to a Store");
                Console.WriteLine("7. Add a Product to a Store");
                Console.WriteLine("8. Register a Customer to a Store");
                Console.WriteLine("9. Place an Order");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Store Name: ");
                        string sName = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Store Location: ");
                        string sLoc = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Contact Number: ");
                        string sCon = Console.ReadLine() ?? string.Empty;

                        Store newStore = new Store(sName, sLoc, sCon);
                        StoreRepo.AddStore(newStore);
                        break;

                    case "2":
                        Console.Write("Enter Store ID to retrieve: ");
                        if (int.TryParse(Console.ReadLine(), out int getId))
                            StoreRepo.GetStoreDetails(getId);
                        else
                            Console.WriteLine("Invalid Store ID.\n");
                        break;

                    case "3":
                        StoreRepo.GetAllStoresDetails();
                        break;

                    case "4":
                        Console.Write("Enter Store ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.Write("New Store Name (blank to keep): ");
                            string uName = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Location (blank to keep): ");
                            string uLoc = Console.ReadLine() ?? string.Empty;
                            Console.Write("New Contact (blank to keep): ");
                            string uCon = Console.ReadLine() ?? string.Empty;

                            StoreRepo.UpdateStoreDetails(updateId, uName, uLoc, uCon);
                        }
                        break;

                    case "5":
                        Console.Write("Enter Store ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                            StoreRepo.DeleteStore(delId);
                        break;

                    case "6":
                        Console.Write("Enter Store ID for Employee: ");
                        int.TryParse(Console.ReadLine(), out int empStoreId);
                        Console.Write("Enter Employee Name: ");
                        string eName = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Designation: ");
                        string eDesig = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Department: ");
                        string eDept = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Salary: ");
                        double.TryParse(Console.ReadLine(), out double eSal);

                        Employee emp = new Employee { Name = eName, Designation = eDesig, Department = eDept, Salary = eSal, StoreId = empStoreId };
                        EmployeeRepo.AddEmployee(emp);
                        break;

                    case "7":
                        Console.Write("Enter Store ID for Product: ");
                        int.TryParse(Console.ReadLine(), out int prodStoreId);
                        Console.Write("Enter Product Name: ");
                        string pName = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Category: ");
                        string pCat = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Price: ");
                        double.TryParse(Console.ReadLine(), out double pPrice);
                        Console.Write("Enter Stock Quantity: ");
                        int.TryParse(Console.ReadLine(), out int pStock);

                        Products prod = new Products { ProductName = pName, Category = pCat, Price = pPrice, StockQuantity = pStock, StoreId = prodStoreId };
                        ProductRepo.AddProduct(prod);
                        break;

                    case "8":
                        Console.Write("Enter Store ID for Customer: ");
                        int.TryParse(Console.ReadLine(), out int custStoreId);
                        Console.Write("Enter Customer Name: ");
                        string cName = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Email: ");
                        string cEmail = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Phone: ");
                        string cPhone = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Address: ");
                        string cAddr = Console.ReadLine() ?? string.Empty;

                        Customer cust = new Customer { Name = cName, Email = cEmail, Phone = cPhone, Address = cAddr, StoreId = custStoreId };
                        CustomerRepo.AddCustomer(cust);
                        break;

                    case "9":
                        Console.Write("Enter Store ID for Order: ");
                        int.TryParse(Console.ReadLine(), out int ordStoreId);
                        Console.Write("Enter Customer ID: ");
                        int.TryParse(Console.ReadLine(), out int ordCustId);
                        Console.Write("Enter Total Order Amount: ");
                        double.TryParse(Console.ReadLine(), out double ordTotal);

                        Order ord = new Order { StoreId = ordStoreId, CustomerId = ordCustId, OrderDate = DateTime.Now, TotalAmount = ordTotal, Status = "Completed" };
                        OrderRepo.AddOrder(ord);
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting System...");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.\n");
                        break;
                }
            }
        }
    }
}
