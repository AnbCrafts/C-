using System;
using System.Collections.Generic;
using System.Text;

namespace Anubhaw_Conole_App
{
   public interface IShape
{
    double GetPerimeter();
    double GetArea();
}

public class Square : IShape
{
    private readonly double _side;

    public Square(double side)
    {
        _side = side;
    }

    public double GetPerimeter() => 4 * _side;
    public double GetArea() => _side * _side;
}

public class Rectangle : IShape
{
    private readonly double _length;
    private readonly double _width;

    public Rectangle(double length, double width)
    {
        _length = length;
        _width = width;
    }

    public double GetPerimeter() => 2 * (_length + _width);
    public double GetArea() => _length * _width;
}

public class Triangle : IShape
{
    private readonly double _a, _b, _c;

    public Triangle(double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    public double GetPerimeter() => _a + _b + _c;
    public double GetArea()
    {
        double s = GetPerimeter() / 2;
        return Math.Sqrt(s * (s - _a) * (s - _b) * (s - _c));
    }
}
}
