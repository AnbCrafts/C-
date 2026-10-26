using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPracticeSQL
{
    internal class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int TotalMarks { get; set; }
        public string Stream { get; set; }
        public List<string> Courses { get; set; } = new List<string>();
        public List<string> Prerequisites { get; set; } = new List<string>();

        public static List<Student> GetAllStudents()
        {
            List<Student> listStudents = new List<Student>
            {
                new Student
                {
                    StudentID = 101,
                    Name = "Tom",
                    TotalMarks = 800,
                    Stream = "Science",
                    Courses = new List<string> { "Calculus", "Classical Mechanics" },
                    Prerequisites = new List<string> { "Math 101", "Physics 101" }
                },
                new Student
                {
                    StudentID = 102,
                    Name = "Mary",
                    TotalMarks = 900,
                    Stream = "Science",
                    Courses = new List<string> { "Organic Chemistry", "Biochem" },
                    Prerequisites = new List<string> { "Math 101", "Chemistry 101" }
                },
                new Student
                {
                    StudentID = 103,
                    Name = "Valarie",
                    TotalMarks = 800,
                    Stream = "Arts",
                    Courses = new List<string> { "Literature", "World History" },
                    Prerequisites = new List<string> { "English 101", "History 101" }
                },
                new Student
                {
                    StudentID = 104,
                    Name = "John",
                    TotalMarks = 800,
                    Stream = "Arts",
                    Courses = new List<string> { "Macroeconomics", "Sociology" },
                    Prerequisites = new List<string> { "Math 101", "English 101" }
                },
            };
            return listStudents;
        }
    }
}
