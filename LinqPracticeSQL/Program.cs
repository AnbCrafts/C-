using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPracticeSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> evenNumbersList = numbers.Where(num => num % 2 == 0);
            foreach (int evenNumber in evenNumbersList)
            {
                //Console.WriteLine(evenNumber);
            }

            //using Sql query
            IEnumerable<int> evenNumbers = from num in numbers where num % 2 == 0 select num;
            foreach (int evenNumber in evenNumbers)
            {
                //Console.WriteLine(evenNumber);
            }

            IEnumerable<Student> result = Student.GetAllStudents().OrderBy(s => s.Name);
            foreach (Student student in result)
            {
                //Console.WriteLine(student.Name);
            }

            IEnumerable<Student> result2 = from student in Student.GetAllStudents()
                                          orderby student.Name
                                          select student;
            foreach (Student student in result2)
            {
                //Console.WriteLine(student.Name);
            }


            //Q1 - Group By Stream and count in each stream
            var result3 = from student in Student.GetAllStudents()
                          group student by student.Stream;

            foreach (var student in result3)
            {
                //Console.WriteLine("{0} - {1}", student.Key, student.Count());
            }

            //Q2 - Unique Prerequisites (de-duplicated list of every prerequisite course across all students)
            var uniquePrerequisites = Student.GetAllStudents()
                .Where(s => s.Prerequisites != null)
                .SelectMany(s => s.Prerequisites)
                .Distinct();

            Console.WriteLine("--- Unique Prerequisites ---");
            foreach (var course in uniquePrerequisites)
            {
                Console.WriteLine(course);
            }
            Console.WriteLine();

            // Prerequisite courses that are duplicates (needed by more than 1 student)
            var duplicatePrerequisites = Student.GetAllStudents()
                .Where(s => s.Prerequisites != null)
                .SelectMany(s => s.Prerequisites)
                .GroupBy(course => course)
                .Where(g => g.Count() > 1)
                .Select(g => new { Course = g.Key, StudentCount = g.Count() });

            Console.WriteLine("--- Duplicate Prerequisites (shared by multiple students) ---");
            foreach (var item in duplicatePrerequisites)
            {
                Console.WriteLine($"{item.Course} (required by {item.StudentCount} students)");
            }
            Console.WriteLine();


            List<Graph> sampleGraphs = Graph.GetGraphs();
            for (int i = 0; i < sampleGraphs.Count; i++)
            {
                Console.WriteLine($"--- Graph {i + 1} ---");
                var adjList = Graph.GetAdjacencyList(sampleGraphs[i]);
                foreach (var kvp in adjList)
                {
                    Console.WriteLine($"{kvp.Key} -> [{string.Join(", ", kvp.Value)}]");
                }
                Console.WriteLine();
            }

            // Q3: Find vertices and edges with degree > 3
            Console.WriteLine("--- Nodes and Edges with Degree > 3 ---");
            for (int i = 0; i < sampleGraphs.Count; i++)
            {
                var adjList = Graph.GetAdjacencyList(sampleGraphs[i]);

                // 1. Vertices having out-degree > 3
                var highDegreeVertices = adjList
                    .Where(kvp => kvp.Value.Count > 3)
                    .Select(kvp => new { Vertex = kvp.Key, OutDegree = kvp.Value.Count });

                // 2. Edges originating from vertices having degree > 3
                var highDegreeEdges = adjList
                    .Where(kvp => kvp.Value.Count > 3)
                    .SelectMany(kvp => kvp.Value, (kvp, target) => $"{kvp.Key} -> {target}");

                foreach (var item in highDegreeVertices)
                {
                    Console.WriteLine($"Graph {i + 1}: Vertex '{item.Vertex}' has out-degree {item.OutDegree}");
                }

                foreach (var edge in highDegreeEdges)
                {
                    Console.WriteLine($"  High-degree edge: {edge}");
                }
            }
        }
    }
}
