using System;

namespace ConsoleApp1
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public double Salary { get; set; }
        public string Department { get; set; } = string.Empty;
        public int StoreId { get; set; }
        public bool StoreAssigned { get; set; }
        public bool storeAssigned { get => StoreAssigned; set => StoreAssigned = value; }
    }
}
