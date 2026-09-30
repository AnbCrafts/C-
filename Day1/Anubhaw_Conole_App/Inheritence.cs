using System;

namespace Anubhaw_Conole_App
{
    // 1. BASE CLASS (Abstract)
    // Common base class for all shapes. Contains shared property 'Name' and common method 'DisplayInfo'.
    public abstract class Shape
    {
        public string Name { get; protected set; }

        protected Shape(string name)
        {
            Name = name;
        }

        // Abstract methods MUST be implemented (overridden) by child classes.
        public abstract double GetArea();
        public abstract double GetPerimeter();

        // Virtual method has a default implementation, but child classes CAN override it.
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"[{Name}] Area: {GetArea():F2} | Perimeter: {GetPerimeter():F2}");
        }
    }

    // 2. DERIVED CLASS: Rectangle inherits from Shape
    public class ShapeRectangle : Shape
    {
        public double Length { get; }
        public double Width { get; }

        // Use base(...) to pass arguments to the parent constructor
        public ShapeRectangle(double length, double width) : base("Rectangle")
        {
            Length = length;
            Width = width;
        }

        public override double GetArea() => Length * Width;
        public override double GetPerimeter() => 2 * (Length + Width);
    }

    // 3. MULTI-LEVEL INHERITANCE: ShapeSquare inherits from ShapeRectangle
    // A Square IS A Rectangle where length == width!
    public class ShapeSquare : ShapeRectangle
    {
        // Reuses ShapeRectangle constructor passing side for both length & width
        public ShapeSquare(double side) : base(side, side)
        {
            Name = "Square";
        }

        // Custom override for DisplayInfo
        public override void DisplayInfo()
        {
            Console.WriteLine($"[{Name} (Side: {Length})] Area: {GetArea():F2} | Perimeter: {GetPerimeter():F2}");
        }
    }
}

