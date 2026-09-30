
using Assignment;

// **************************** Q1 **************************************************************

Console.WriteLine("\n\nQuestion 1 \n\n");
PaymentProcessor payment = new PaymentProcessor();
        payment.CardNumber = "1234567812345678";
        payment.AccountBalance = 5000;
        payment.SecurityToken = "XYZ789TOKEN";

        Console.WriteLine("Masked Card Number: " +
                          payment.CardNumber);

        Console.WriteLine("Balance: " +
                          payment.AccountBalance);

        payment.AccountBalance = -1000;


// **************************** Q2 **************************************************************
Console.WriteLine("\n\nQuestion 2 \n\n");

PatientRecord patient =
    new PatientRecord("Anubhaw", 5000);

patient.UpdateHealthRecord("BP Normal");

patient.MarkBillAsPaid();

patient.GetDetails();



// **************************** Q3 **************************************************************
Console.WriteLine("\n\nQuestion 3 \n\n");

Employee e1 =
            new FullTimeEmployee(
                101,
                "Anubhaw",
                "Kolkata",
                50000,
                10000);

        Employee e2 =
            new ContractEmployee(
                102,
                "Komal",
                "Delhi",
                30000,
                5000);

        Employee e3 =
            new Freelancer(
                103,
                "Avijit",
                "Mumbai",
                100,
                500);

        e1.Display();
        Console.WriteLine();

        e2.Display();
        Console.WriteLine();

        e3.Display();


// **************************** Q4 **************************************************************
Console.WriteLine("\n\nQuestion 4 \n\n");


ElectricTruck truck = new ElectricTruck(
                "WB20AB1234",     
                "Tata",           
                22.5726,         
                88.3639,          
                "Electric",      
                false,            
                350,             
                200,              
                80,              
                4.5              
            );

            Console.WriteLine("=== Vehicle Details ===");
            truck.getVehicleDetails();

            Console.WriteLine();

            Console.WriteLine("=== Starting Engine ===");
            truck.StartEngine();

            Console.WriteLine();

            Console.WriteLine("=== Range Calculation ===");
            Console.WriteLine($"Estimated Range: {truck.CalculateRange()} Km");

            Console.WriteLine();

            Console.WriteLine("=== GPS Location ===");
truck.LogGPS();



// **************************** Q5 **************************************************************

Console.WriteLine("\n\nQuestion 5 \n\n");

List<Notification> notifications =
                new List<Notification>()
                {
                    new EmailNotification(),
                    new SMSNotification(),
                    new WhatsAppNotification(),
                    new PushNotification()
                };

string alertMessage =
    "Server CPU Usage Exceeded 90%";

foreach (Notification notification in notifications)
{
    notification.SendNotification(alertMessage);
}

// **************************** Q6 **************************************************************

Console.WriteLine("\n\nQuestion 6 \n\n");

Console.WriteLine("===== Compile Time Polymorphism =====");

            AreaCalculator calculator = new AreaCalculator();

            Console.WriteLine($"Area of Square : {calculator.CalculateArea(5)}");

            Console.WriteLine($"Area of Rectangle : {calculator.CalculateArea(10, 20)}");

            Console.WriteLine($"Area of Circle : {calculator.CalculateArea(7, "circle"):F2}");

            Console.WriteLine();

            Console.WriteLine("===== Runtime Polymorphism =====");

            Shape shape1 = new Circle();
            Shape shape2 = new Polygon();

            shape1.Draw();
            shape2.Draw();

// **************************** Q7 **************************************************************

Console.WriteLine("\n\nQuestion 7 \n\n");

SmartDevice[] devices =
            {
                new SmartLight("Philips"),
                new SmartThermostat("Nest"),
                new SmartAC("LG")
            };

foreach (SmartDevice device in devices)
{
    device.TurnOn();

    Console.WriteLine(
        $"Energy Consumption : {device.GetEnergyConsumption()} W");

    device.TurnOff();

    Console.WriteLine();
}



// **************************** Q8 **************************************************************

Console.WriteLine("\n\nQuestion 8 \n\n");

Loan[] loans =
            {
                new HomeLoan("Anubhaw", 3500000),
                new CarLoan("Komal", 1000000),
                new EducationLoan("Avijit", 500000)
            };

            foreach (Loan loan in loans)
            {
                Console.WriteLine("--------------------------------");

                loan.VerifyDocuments();

                if (loan.CheckEligibility())
                {
                    loan.SanctionAmount();

                    Console.WriteLine(
                        $"Interest Rate : {loan.CalculateInterestRate()}%");
                }
                else
                {
                    Console.WriteLine("Not Eligible");
                }

                Console.WriteLine();
}




// **************************** Q9 **************************************************************

Console.WriteLine("\n\nQuestion 9 \n\n");

ICloudStorageProvider[] providers =
            {
                new S3Storage(),
                new AzureStorage(),
                new GoogleStorage()
            };

            foreach (ICloudStorageProvider provider in providers)
            {
                provider.UploadFile("ProjectReport.pdf");

                provider.DownloadFile("FILE123");

                provider.DeleteFile("FILE123");

                Console.WriteLine();
}



// **************************** Q10 **************************************************************
Console.WriteLine("\n\nQuestion 10 \n\n");

WarehouseRobot robot =
                new WarehouseRobot("WR-101");

            robot.StartRobot();

            robot.PerformTask();

            robot.Navigate("Section A");

            robot.ReadSensors();

            robot.ChargeBattery();
        