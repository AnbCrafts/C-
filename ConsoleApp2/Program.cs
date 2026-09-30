
//////using ConsoleApp2;

////////List <ListItem> MyList = new List<ListItem>();

//////////MyList.Add(new ListItem(1, "Anubhaw", "Enthusiast developer"));
//////////MyList.Add(new ListItem(2, "Rahul", "Full Stack Developer"));
//////////MyList.Add(new ListItem(3, "Priya", "Database Administrator"));
//////////MyList.Add(new ListItem(4, "Amit", "Backend Engineer"));
//////////MyList.Add(new ListItem(5, "Neha", "Frontend Developer"));
//////////MyList.Add(new ListItem(6, "Karan", "Cloud Solutions Architect"));
//////////MyList.Add(new ListItem(7, "Sneha", "Software Tester"));
//////////MyList.Add(new ListItem(8, "Rohit", "DevOps Engineer"));
//////////MyList.Add(new ListItem(9, "Pooja", "UI/UX Designer"));
//////////MyList.Add(new ListItem(10, "Vikram", "Technical Lead"));

////////Console.WriteLine("\n\nLet's Start adding data \n\n");
////////while (true)
////////{
////////    Console.Write("Id: ");
////////    int id = int.Parse(Console.ReadLine());

////////    Console.Write("Name: ");
////////    string name = Console.ReadLine();

////////    Console.Write("Description: ");
////////    string description = Console.ReadLine();

////////    MyList.Add(new ListItem(id, name, description));

////////    Console.Write("Add another item? (y/n): ");
////////    if (Console.ReadLine().ToLower() != "y")
////////    {
////////        Console.WriteLine("\n\nYour added data \n\n");
////////        foreach (var item in MyList)
////////        {
////////            //Console.WriteLine($"Id: {item.Id}, Name: {item.Name}, Description: {item.Description}");
////////            item.DisplayData();
////////        }
////////    break;

////////    }
////////}
////////Student s = new Student("101", "Anubhaw", 7, 8.7);

//////Student.AddStudent(new Student("101", "Anubhaw", 7, 8.7));
//////Student.AddStudent(new Student("102", "Rahul", 8, 7.5));
//////Student.AddStudent(new Student("103", "Priya", 6, 9.1));
//////Student.AddStudent(new Student("104", "Amit", 5, 6.8));

//////Console.WriteLine();

//////Student.GetStudentDetails("102");

//////Console.WriteLine();

//////Student.GetEligibleForPlacementStudents();

//////Console.WriteLine();

//////Student.GetBatchStatistics();

//////Console.WriteLine();

//////Student.DeleteStudent("104");

//////Console.WriteLine();

//////Student.GetBatchStatistics();

//////Console.ReadLine();



////using ConsoleApp2;

//////Entity<string> e = new Entity<string>("Student");
//////Entity<string>.AddItem(e);

////Dictionary<string, string> D = new Dictionary<string, string>();


////D.Add("Horse", "Neigh");
////D.Add("Cow", "Moo");
////D.Add("Pig", "Grunt");
////D.Add("Dog", "Bark");
////D.Add("Wolf", "Howl");
////D.Add("Lion", "Roar");
//////foreach (var item in D)
//////{
//////    Console.WriteLine($"Sound of - {item.Key} is {item.Value}\n");
//////}



////Dictionary<int, Animal> animalDictionary = new Dictionary<int, Animal>()
////{
////    {
////        1,
////        new Animal(
////            "Canis lupus familiaris",
////            "Dog",
////            "A loyal domestic animal commonly kept as a pet.",
////            true)
////    },
////    {
////        2,
////        new Animal(
////            "Felis catus",
////            "Cat",
////            "A small domesticated carnivorous mammal.",
////            true)
////    },
////    {
////        3,
////        new Animal(
////            "Elephas maximus",
////            "Asian Elephant",
////            "A large herbivorous mammal known for its intelligence.",
////            false)
////    },
////    {
////        4,
////        new Animal(
////            "Panthera leo",
////            "Lion",
////            "A large wild cat often called the king of the jungle.",
////            false)
////    },
////    {
////        5,
////        new Animal(
////            "Equus ferus caballus",
////            "Horse",
////            "A domesticated animal used for riding and transport.",
////            true)
////    },
////    {
////        6,
////        new Animal(
////            "Psittacus erithacus",
////            "African Grey Parrot",
////            "A highly intelligent bird capable of mimicking speech.",
////            true)
////    },
////    {
////        7,
////        new Animal(
////            "Ursus arctos",
////            "Brown Bear",
////            "A large omnivorous mammal found in forests and mountains.",
////            false)
////    },
////    {
////        8,
////        new Animal(
////            "Oryctolagus cuniculus",
////            "Rabbit",
////            "A small herbivorous mammal often kept as a pet.",
////            true)
////    }
////};

//////Console.WriteLine($"\n\nThese are the Animals data \n\n");
////if (animalDictionary.ContainsKey(3))
////{
////    animalDictionary[3].Description = "Ths is an updated description";
////}

////foreach (var item in animalDictionary)
////{
////    string decision = "";

////    if (item.Value.canPet)
////    { decision = ""; }
////    else
////    {
////        decision = "not";
////    }
////    Console.WriteLine("\n\n**************************************************************************");
////    Console.WriteLine($"\n\n KEY in Dictionary -> {item.Key} \n");
////    Console.WriteLine($"VALUES in Dictionary ->  \n");
////    Console.WriteLine($"Scinetific name -> {item.Value.ScientificName}\n");
////    Console.WriteLine($"Local name -> {item.Value.LocalName}\n");
////    Console.WriteLine($"About the animal -> {item.Value.Description}\n");
////    Console.WriteLine($"It can  {decision} be pet\n");
////    Console.WriteLine("**************************************************************************\n\n");



////}


//using ConsoleApp2;
//using System.Diagnostics;

//List<Order> orders =  CreateOrder(50000);
//List<Customer> customers =  CreateCustomer(10000);

//Stopwatch stopwatch = Stopwatch.StartNew();


//Dictionary<int, Customer> CustomerDictionary = customers.ToDictionary(c => c.CustomerId);

//stopwatch.Stop();

//Console.WriteLine($"\n\nTime taken to make dictionary with lists or order & customers - {stopwatch.ElapsedMilliseconds} ms\n\n");
//stopwatch.Restart();

//List <CustomerOrders> CustomerOrdersList = new List<CustomerOrders>();
//Random random = new Random();
//foreach (Order item in orders)
//{ 
//   var i= random.Next(1, 50);
//    if (CustomerDictionary.TryGetValue(item.CustomerId, out Customer? customer)) ;
//    CustomerOrdersList.Add(new CustomerOrders { CustomerId = item.CustomerId,OrderId = item.OrderId, CustomerName=customer.CustomerName, EmployeeId = i , EmployeeName = $"Employee{i}"});
//}

//stopwatch.Stop();
//Console.WriteLine($"\n\nTime taken to make dictionary with lists or order & customers - {stopwatch.ElapsedMilliseconds} ms\n\n");


//static List<Customer> CreateCustomer(int n)
//{

//    List<Customer> customers = new List<Customer>();
//    for(int i = 0; i < n; i++)
//    {
//        customers.Add(
//            new Customer
//            {
//                CustomerId = i,
//                CustomerName = $"Customer{i}",
//                CustomerEmail = $"customer{i}@gmail.com"
//            });

//    }

//    return customers;
//}

//static List<Order> CreateOrder(int n)
//{

//    List<Order> orders = new List<Order>();

//    Random random = new Random();
//    for (int i = 0; i < n; i++)
//    {

//        orders.Add(
//            new Order
//            {
//                OrderId = i,
//                CustomerId = random.Next(1,500001),
//                Amount = random.Next(100,1000)

//            });

//    }

//    return orders;
//}

//using ConsoleApp2;
//using System.Diagnostics;

//List<Order> orders = CreateOrder(50000);
//List<Customer> customers = CreateCustomer(10000);



//Stopwatch stopwatch = Stopwatch.StartNew();

//// Create Dictionary
//Dictionary<int, Customer> customerDictionary =
//    customers.ToDictionary(c => c.CustomerId);

//stopwatch.Stop();

//Console.WriteLine(
//    $"\n\nTime taken to create customer dictionary - {stopwatch.ElapsedMilliseconds} ms\n\n");

//Console.WriteLine($"Customers Count = {customers.Count}");
//Console.WriteLine($"Dictionary Count = {customerDictionary.Count}");

//stopwatch.Restart();

//// Create CustomerOrders List
//List<CustomerOrders> customerOrdersList = new List<CustomerOrders>(orders.Count);

//Random random = new Random();

//foreach (Order item in orders)
//{
//    int employeeId = random.Next(1, 50);

//    if (!customerDictionary.TryGetValue(item.CustomerId, out Customer? customer))
//    {
//        Console.WriteLine($"Customer not found: {item.CustomerId}");
//        continue;
//    }

//    if (customer == null)
//    {
//        Console.WriteLine($"Customer is null: {item.CustomerId}");
//        continue;
//    }

//    if (customerDictionary.TryGetValue(item.CustomerId, out Customer ? customer1)
//    {
//        customerOrdersList.Add(
//            new CustomerOrders
//            {
//                CustomerId = item.CustomerId,
//                OrderId = item.OrderId,
//                CustomerName = customer.CustomerName,
//                EmployeeId = employeeId,
//                EmployeeName = $"Employee{employeeId}"
//            });
//    }
//}

//stopwatch.Stop();

//Console.WriteLine(
//    $"\n\nTime taken to create CustomerOrdersList - {stopwatch.ElapsedMilliseconds} ms\n\n");

//static List<Customer> CreateCustomer(int n)
//{
//    List<Customer> customers = new List<Customer>();

//    for (int i = 0; i < n; i++)
//    {
//        customers.Add(
//            new Customer
//            {
//                CustomerId = i,
//                CustomerName = $"Customer{i}",
//                CustomerEmail = $"customer{i}@gmail.com"
//            });
//    }

//    return customers;
//}

//static List<Order> CreateOrder(int n)
//{
//    List<Order> orders = new List<Order>();

//    Random random = new Random();

//    for (int i = 0; i < n; i++)
//    {
//        orders.Add(
//            new Order
//            {
//                OrderId = i,

//                // Match customer IDs created in CreateCustomer()
//                CustomerId = random.Next(0, 10000),

//                Amount = random.Next(100, 1000)
//            });
//    }

//    return orders;
//}




using ConsoleApp2;
using System;
using System.Diagnostics;

List<Order> orders = CreateOrder(50000);
List<Customer> customers = CreateCustomer(10000);



Stopwatch stopwatch = Stopwatch.StartNew();

//****************************************USING LIST ******************************************

List<CustomerOrders> customerOrdersList = new List<CustomerOrders>();

Random rand = new Random();

foreach (Order order in orders)
{
    int employeeId = rand.Next(1, 50);

    customerOrdersList.Add(
        new CustomerOrders
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            CustomerName = customers.Find(c => c.CustomerId == order.CustomerId)?.CustomerName ?? "Unknown",
            EmployeeId = employeeId,
            EmployeeName = $"Employee{employeeId}"
        });
}

stopwatch.Stop();

Console.WriteLine(
    $"\n\nTime taken to search in customer list and add in customer ordr class - {stopwatch.ElapsedMilliseconds} ms\n\n");

//****************************************USING DICTIONARY ******************************************

// Create Dictionary
//Dictionary<int, Customer> customerDictionary =
//    customers.ToDictionary(c => c.CustomerId);

//stopwatch.Stop();

//Console.WriteLine(
//    $"\n\nTime taken to create customer dictionary - {stopwatch.ElapsedMilliseconds} ms\n\n");

//Console.WriteLine($"Customers Count = {customers.Count}");
//Console.WriteLine($"Dictionary Count = {customerDictionary.Count}");



//stopwatch.Restart();

// Create CustomerOrders List
//List<CustomerOrders> customerOrdersList = new List<CustomerOrders>(orders.Count);

//Random random = new Random();

//foreach (Order item in orders)
//{
//    int employeeId = random.Next(1, 50);

//    if (customerDictionary.TryGetValue(item.CustomerId, out Customer? customer))
//    {
//        customerOrdersList.Add(
//            new CustomerOrders
//            {
//                CustomerId = item.CustomerId,
//                OrderId = item.OrderId,
//                CustomerName = customer.CustomerName,
//                EmployeeId = employeeId,
//                EmployeeName = $"Employee{employeeId}"
//            });
//    }
//}



//stopwatch.Stop();

//Console.WriteLine(
//    $"\n\nTime taken to create CustomerOrdersList - {stopwatch.ElapsedMilliseconds} ms\n\n");

static List<Customer> CreateCustomer(int n)
{
    List<Customer> customers = new List<Customer>();

    for (int i = 0; i < n; i++)
    {
        customers.Add(
            new Customer
            {
                CustomerId = i,
                CustomerName = $"Customer{i}",
                CustomerEmail = $"customer{i}@gmail.com"
            });
    }

    return customers;
}

static List<Order> CreateOrder(int n)
{
    List<Order> orders = new List<Order>();

    Random random = new Random();

    for (int i = 0; i < n; i++)
    {
        orders.Add(
            new Order
            {
                OrderId = i,

                // Match customer IDs created in CreateCustomer()
                CustomerId = random.Next(0, 10000),

                Amount = random.Next(100, 1000)
            });
    }

    return orders;
}