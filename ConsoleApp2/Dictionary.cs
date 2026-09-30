using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Animal
    {
        public string ScientificName { get; set; }
        public string LocalName { get; set; }
        public string Description { get; set; }
        public bool canPet { get; set; }
        public Animal(string sName, string lName, string desc, bool p) 
        {
            ScientificName = sName;
            LocalName = lName;
            Description = desc;
            canPet = p;
        
        }
    }
}
