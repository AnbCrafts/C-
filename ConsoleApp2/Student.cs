using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public class Student
    {
        public string Roll { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }
        public double Cgpa { get; set; }

        public static List<Student> List = new List<Student>();

        public Student(string roll, string name, int semester, double cgpa)
        {
            Roll = roll;
            Name = name;
            Semester = semester;
            Cgpa = cgpa;
        }

        public static void AddStudent(Student student)
        {
            List.Add(student);
            Console.WriteLine($"Student {student.Name} added successfully.");
        }

        public static void DeleteStudent(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("No id passed to find and delete.");
                return;
            }

            Student student = List.Find(s => s.Roll == id);

            if (student == null)
            {
                Console.WriteLine($"No student found with id {id}");
                return;
            }

            List.Remove(student);
            Console.WriteLine($"Student with id {id} deleted successfully.");
        }

        public static void GetStudentDetails(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("No id passed.");
                return;
            }

            Student student = List.Find(s => s.Roll == id);
            //foreach (var item in List)
            //{
            //    if (item.Roll == id)
            //    {
            //        student = item;
            //    }
            //}

            if (student == null)
            {
                Console.WriteLine($"No student found with id {id}");
                return;
            }

            Console.WriteLine("\nStudent Details");
            Console.WriteLine($"Roll No  : {student.Roll}");
            Console.WriteLine($"Name     : {student.Name}");
            Console.WriteLine($"Semester : {student.Semester}");
            Console.WriteLine($"CGPA     : {student.Cgpa}");
        }

        public static void GetEligibleForPlacementStudents()
        {
            Console.WriteLine("\nPlacement Eligible Students (CGPA >= 8.0)\n");

            var eligibleStudents = List.Where(s => s.Cgpa >= 8.0);
            

            foreach (var student in eligibleStudents)
            {
                Console.WriteLine($"{student.Roll} | {student.Name} | {student.Cgpa}");
            }
        }

        public static void GetBatchStatistics()
        {
            if (List.Count == 0)
            {
                Console.WriteLine("No students available.");
                return;
            }

            Console.WriteLine("\nBatch Statistics");
            Console.WriteLine($"Total Students : {List.Count}");
            Console.WriteLine($"Average CGPA   : {List.Average(s => s.Cgpa):F2}");
            Console.WriteLine($"Highest CGPA   : {List.Max(s => s.Cgpa)}");
            Console.WriteLine($"Lowest CGPA    : {List.Min(s => s.Cgpa)}");
        }
    }

   
}