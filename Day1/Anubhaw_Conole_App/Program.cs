using Anubhaw_Conole_App;

Console.WriteLine("=== 1. USING INTERFACES (IShape) ===");
var interfaceShapes = new List<IShape>
{
    new Square(5),
    new Rectangle(4, 6),
    new Triangle(3, 4, 5)
};

foreach (var shape in interfaceShapes)
{
    Console.WriteLine($"[Interface {shape.GetType().Name}] Area: {shape.GetArea():F2} | Perimeter: {shape.GetPerimeter()}");
}

Console.WriteLine("\n=== 2. USING INHERITANCE (Shape base class) ===");
var inheritanceShapes = new List<Shape>
{
    new ShapeRectangle(10, 5),
    new ShapeSquare(7)
};

foreach (var shape in inheritanceShapes)
{
    // DisplayInfo() is defined in the base class 'Shape' and overridden in child classes!
    shape.DisplayInfo();
}
