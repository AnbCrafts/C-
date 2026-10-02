using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Repositories
{
    internal class EmployeeRepo
    {

        public static List<Employee> empLoyeeList = new List<Employee>();

        public static void AddEmployee(Employee emp, Store store)
        {
            if (emp == null)
            {
                Console.WriteLine("An employee object is required to add employee\n");
                return;
            }

            if (store == null)
            {
                emp.storeAssigned = false;
                Console.WriteLine($"Store was not provided or null so the employee {emp.Name} has no store assigned\n");
            }
            else
            {
                emp.storeAssigned = true;
                emp.StoreId = store.StoreId;
                if (!store.Employees.Contains(emp))
                {
                    store.Employees.Add(emp);
                }
            }

            empLoyeeList.Add(emp);
            Console.WriteLine($"Employee named {emp.Name} Added in the emp list\n");
        }

        public static List<Employee> GetEmployeeWorkingInStore(Store store)
        {
            if (store == null)
            {
                return new List<Employee>();
            }

            return empLoyeeList.FindAll(emp => emp.StoreId == store.StoreId);
        }

        

       
    }
}
