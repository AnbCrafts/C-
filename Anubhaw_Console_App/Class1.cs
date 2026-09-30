using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anubhaw_Console_App
{
    public class Student
    {
        int id;
        string name;
        string address;
        string course;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string Course
        {
            get { return course; }
            set { course = value; }
        }
          public Student(int id, string name, string address, string course)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.course = course;
        }
    }
}
