// See https://aka.ms/new-console-template for more information
//using Anubhaw_Console_App;
using Anubhaw_Console_App;

//Console.WriteLine("Hello, World!");


////int num1, num2, sum;
////Console.WriteLine("Enter the first number");
////    num1 = Convert.ToInt32(Console.ReadLine());
////Console.WriteLine("Enter the second number");
////num2 = Convert.ToInt32(Console.ReadLine());

////sum = num1 + num2;

////Console.WriteLine("The sum of {0} and {1} is : {2}",num1,num2,sum);

////Student student = new Student(1, "John", "123 Main Street", "Computer Science");
////Console.WriteLine(student.Id);
////Console.WriteLine(student.Name);
////Console.WriteLine(student.Address);
////Console.WriteLine(student.Course);

//Employee emp = new Employee(286, "Anubhaw", "New Town", 15000);
//emp.display();




//Console.WriteLine("----- Base Object -----");
//Base b1 = new Base();
//b1.Display();

//Console.WriteLine("\n----- Derived Object -----");
//Derived d1 = new Derived();
//d1.Display();

//Console.WriteLine("\n----- Base Reference, Derived Object -----");
//Base b2 = new Derived();
//b2.Display();


        Console.WriteLine("----- Base Object -----");
        Base b1 = new Base();
        b1.Display();

        Console.WriteLine("\n----- Derived Object -----");
        Derived d1 = new Derived();
        d1.Display();
        d1.print();


Console.WriteLine("\n----- Base Reference, Derived Object -----");
        Base b2 = new Derived();
        b2.Display();
b2.print();

Console.ReadLine();