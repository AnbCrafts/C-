using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anubhaw
{
    internal class Employee
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
            Console.WriteLine("Display method called");
            Console.WriteLine($"Id - {id} name - {name} salary - {salary} address - {address}");
            Console.WriteLine("Destrcutor will be called now");
        }
        ~Employee()
        {
            Console.WriteLine("Destrcutor called");
        }
    }

}
