using System;

namespace Assignment
{
    // Compile-Time Polymorphism
    public class AreaCalculator
    {
        // Square
        public double CalculateArea(double side)
        {
            return side * side;
        }

        // Rectangle
        public double CalculateArea(double length, double width)
        {
            return length * width;
        }

        // Circle
        public double CalculateArea(double radius, string shapeType)
        {
            return Math.PI * radius * radius;
        }
    }

    // Runtime Polymorphism
    public class Shape
    {
        public virtual void Draw()
        {
            Console.WriteLine("Drawing Shape");
        }
    }

    public class Circle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing Circle");
        }
    }

    public class Polygon : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing Polygon");
        }
    }
}