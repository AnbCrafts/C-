using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anubhaw
{
    public interface Shape
    {
        void area(float s);
        void permimeter(float s);

    }


    public class square : Shape
    {
        public float side;

        public square(float s)
        {
            this.side = s;

            Console.WriteLine($"side - {this.side}");


        }
        public void area(float s)
        {
            Console.WriteLine($"The area of square is ${s * s} sq. units");
        }
        public void permimeter(float s)
        {
            Console.WriteLine($"The permimeter of square is ${s * 4}  units");
        }

    }

    public class circle : Shape
    {
        public float radius;
        public circle(float r)
        {
            this.radius = r;
            Console.WriteLine($"radius - {this.radius}");
        }

        public void area(float r)
        {
            Console.WriteLine($"The area of circle is ${(22 / 7) * r * r} sq. units");
        }
        public void permimeter(float r)
        {
            Console.WriteLine($"The permimeter of circle is ${2 * (22 / 7) * r}  units");
        }
    }
}



