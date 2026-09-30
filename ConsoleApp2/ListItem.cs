using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ListItem(int id, string name , string description) { 
            Id = id;
            Name = name;    
            Description = description;
        
        }
        public void DisplayData()
        {
            Console.WriteLine("\nThis is data \n");
            Console.WriteLine($"ID -> {Id}\n");
            Console.WriteLine($"Name -> {Name}\n");
            Console.WriteLine($"Description -> {Description}\n");

        }
    }
}
