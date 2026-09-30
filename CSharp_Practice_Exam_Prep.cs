// =====================================================================
// C# EXAM PRACTICE MODULE - DAY 24, DAY 26, & DAY 27
// =====================================================================

using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpExamPrep
{
    // =================================================================
    // DAY 24: OOP CONCEPTS (Encapsulation, Abstraction, Generalization)
    // =================================================================

    // 1. Encapsulation & Data Hiding
    public class BankAccount
    {
        // Private field (hidden from outside)
        private double _balance;

        // Public property with validation logic
        public double Balance
        {
            get { return _balance; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Balance cannot be negative!");
                _balance = value;
            }
        }

        public BankAccount(double initialBalance)
        {
            Balance = initialBalance;
        }
    }

    // 2. Abstraction using Interface (100% Abstract)
    public interface IPaymentGateway
    {
        void ProcessPayment(double amount);
    }

    public class CreditCardPayment : IPaymentGateway
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"[CreditCard] Processed payment of ${amount:F2}");
        }
    }


    // =================================================================
    // DAY 26: INHERITANCE, POLYMORPHISM, PARTIAL & SEALED
    // =================================================================

    // 1. Abstract Base Class vs Interface
    public abstract class Appliance
    {
        public string Brand { get; set; }

        protected Appliance(string brand)
        {
            Brand = brand;
        }

        // Abstract method (MUST be overridden by child class)
        public abstract void TurnOn();

        // Virtual method (CAN be overridden by child class)
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Appliance Brand: {Brand}");
        }
    }

    // 2. Method Overriding (Dynamic / Runtime Polymorphism)
    public class Refrigerator : Appliance
    {
        public Refrigerator(string brand) : base(brand) { }

        public override void TurnOn()
        {
            Console.WriteLine($"[Refrigerator - {Brand}] Cooling started.");
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"[Refrigerator - {Brand}] Special Temperature Control Enabled.");
        }
    }

    // 3. Method Hiding / Shadowing (new keyword)
    public class ParentLogger
    {
        public void LogMessage()
        {
            Console.WriteLine("ParentLogger: Base Log");
        }
    }

    public class CustomLogger : ParentLogger
    {
        // 'new' keyword hides the parent method without override
        public new void LogMessage()
        {
            Console.WriteLine("CustomLogger: Child Log");
        }
    }

    // 4. Sealed Class & Sealed Method
    public class Printer
    {
        public virtual void PrintDocument()
        {
            Console.WriteLine("Standard Printer Printing...");
        }
    }

    public class LaserJetPrinter : Printer
    {
        // Sealed method prevents further derived classes from overriding PrintDocument
        public sealed override void PrintDocument()
        {
            Console.WriteLine("LaserJet High-Speed Printing...");
        }
    }

    // Sealed class cannot be inherited at all
    public sealed class FinalPrinter : LaserJetPrinter
    {
        // Cannot override PrintDocument() here because it is sealed in LaserJetPrinter!
    }


    // =================================================================
    // DAY 27: ARRAYS, GENERICS, & COLLECTIONS
    // =================================================================

    // 1. Generics (<T>) - Eliminates Boxing/Unboxing & Provides Type Safety
    public class DataContainer<T>
    {
        private T _data;

        public DataContainer(T data)
        {
            _data = data;
        }

        public T GetData()
        {
            return _data;
        }

        public void PrintTypeAndValue()
        {
            Console.WriteLine($"Type: {typeof(T).Name}, Value: {_data}");
        }
    }

    // Main Program Execution
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# EXAM PREP DEMO ===");

            // --- DAY 24 DEMO ---
            BankAccount account = new BankAccount(1000.00);
            account.Balance += 500.00;
            Console.WriteLine($"Account Balance: ${account.Balance:F2}");

            // --- DAY 26 DEMO ---
            Appliance app = new Refrigerator("Samsung");
            app.TurnOn();       // Calls Refrigerator.TurnOn()
            app.DisplayInfo();  // Calls Refrigerator.DisplayInfo()

            // Method Hiding Demo
            ParentLogger logger = new CustomLogger();
            logger.LogMessage(); // Calls ParentLogger.LogMessage() because binding is compile-time!

            // --- DAY 27 DEMO ---
            // Generic Class (No Boxing/Unboxing!)
            DataContainer<int> intContainer = new DataContainer<int>(100);
            intContainer.PrintTypeAndValue();

            DataContainer<string> stringContainer = new DataContainer<string>("Hello C#");
            stringContainer.PrintTypeAndValue();

            // Generic List vs Non-Generic ArrayList
            List<string> cities = new List<string> { "Mumbai", "Delhi", "Bangalore" }; // Type-safe
            ArrayList rawList = new ArrayList { 101, "Text", true }; // Non-generic (Boxing occurs!)

            Console.WriteLine("\nCities Count: " + cities.Count);
        }
    }
}
