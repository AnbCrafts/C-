// See https://aka.ms/new-console-template for more information
using Anubhaw;

Console.WriteLine("Hello, World!");


//Employee emp = new Employee(286, "Anubhaw", "New Town", 15000);
//emp.display();


//emp = null; 
//GC.Collect();
//GC.WaitForPendingFinalizers();
//Console.WriteLine("End of Main");
//Console.ReadLine();




Console.WriteLine("Enter the length of the side of the square");

float size = float.Parse(Console.ReadLine());
Shape sq = new square(size);
Console.WriteLine($"This squar has side {size}");
sq.area(size);
sq.permimeter(size);

Console.WriteLine("Enter the length of the radius of the circle");

float radius = float.Parse(Console.ReadLine());
Shape circle = new circle(radius);
Console.WriteLine($"This circle has side {radius}");
circle.area(radius);
circle.permimeter(radius);
