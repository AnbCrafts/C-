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
            if(emp == null)
            {
                Console.WriteLine("An employee object is required to add employee\n");

            }
            if(store == null) {
                emp.storeAssigned = false;
                Console.WriteLine($"Store was not proided or null so the employee {emp.Name} has no store assigned\n");
            }

            empLoyeeList.Add(emp);
            Console.WriteLine($"Employee named {emp.Name} Added in the emp list\n");



        }

        public static List<Employee> GetEmployeeWorkingInStore(Store store) { 
            
            List<Employee> list = new List<Employee>();
            empLoyeeList.ForEach( emp => { })
            return list;
                
        }

        

       
    }
}
