using System;

public class Base
{
    public void Print()
    {
        Console.WriteLine("Base class Print method");
    }

    public virtual void Display()
    {
        Console.WriteLine("Base class Display method");
    }
}

public class Derived : Base
{
    public override void Display()
    {
        Console.WriteLine("First Display called from Derived");

        base.Display();

        Console.WriteLine("Then Display called from Derived");
    }

    public void print()
    {


        Console.WriteLine(" print called from Derived");
    }
}

