using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anubhaw_Console_App
{
    public class Employee
    {
        int id;
        string name;
        string address;
        float salary;
        
        public Employee(int id, string name, string address, float salary)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.salary = salary;
        }

        public void display()
        {
            Console.WriteLine($"Id - {id} name - {name} salary - {salary} address - {address}");
        }
    }


   
}
