using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Repositories
{
    internal class EmployeeRepo
    {
        public static List<Employee> empLoyeeList = new List<Employee>();

        public static void AddEmployee(Employee emp, Store? store)
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

        public static Employee? GetEmployeeById(int empId)
        {
            var emp = empLoyeeList.Find(e => e.EmployeeId == empId);
            if (emp == null)
            {
                Console.WriteLine($"Employee with ID {empId} not found.\n");
                return null;
            }
            return emp;
        }

        public static void UpdateEmployee(int empId, string name, string designation, double salary, string department)
        {
            var emp = GetEmployeeById(empId);
            if (emp != null)
            {
                if (!string.IsNullOrWhiteSpace(name)) emp.Name = name;
                if (!string.IsNullOrWhiteSpace(designation)) emp.Designation = designation;
                if (salary > 0) emp.Salary = salary;
                if (!string.IsNullOrWhiteSpace(department)) emp.Department = department;

                Console.WriteLine($"Employee ID {empId} updated successfully.\n");
            }
        }

        public static void RemoveEmployee(int empId)
        {
            var emp = GetEmployeeById(empId);
            if (emp != null)
            {
                empLoyeeList.Remove(emp);
                var store = StoreRepo.storeList.Find(s => s.StoreId == emp.StoreId);
                store?.Employees.Remove(emp);

                Console.WriteLine($"Employee ID {empId} removed successfully.\n");
            }
        }

        public static List<Employee> GetEmployeesByDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department)) return new List<Employee>();

            var list = empLoyeeList.FindAll(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
            if (list.Count == 0)
            {
                Console.WriteLine($"No employees found in department '{department}'.\n");
            }
            return list;
        }
    }
}
