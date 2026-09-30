using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public partial class PartialPractice
    {
        public string Name { get; set; }

        public PartialPractice(string name)
        {
            Name = name;
        }

        public string Description { get; set; }

    }

    public class Base{

    }

    public class Derived : Base
    {

    }

}
