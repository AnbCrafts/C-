using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public interface IEntity
    {
        int Id { get; set; }
        string Type { get; set; }
    }

    public class Entity<T> : IEntity
    {
        private static int NextId = 1;

        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;

        public static List<T> items = new();

        public Entity(string type)
        {
            Id = NextId++;
            Type = type;
        }

        public static void AddItem(T item)
        {
            items.Add(item);
        }

        public static void RemoveItem(T item)
        {
            items.Remove(item);
        }

        public static T? GetItem(Predicate<T> predicate)
        {
            return items.Find(predicate);
        }

        public static void DisplayItems()
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }

   
}