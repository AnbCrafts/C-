using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem;

internal class Program
{
    private static readonly ILibraryRepository Repository = new LibraryRepository();
    private static readonly LibraryManager Manager = new LibraryManager(Repository);

    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        ShowHeader();

        bool running = true;
        while (running)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.ResetColor();
            Console.WriteLine("1. View All Items");
            Console.WriteLine("2. Add New Item (Book / Magazine / Audiobook)");
            Console.WriteLine("3. Search Catalog");
            Console.WriteLine("4. Borrow Item");
            Console.WriteLine("5. Return Item");
            Console.WriteLine("6. View Overdue Items & Fines");
            Console.WriteLine("7. Exit");
            Console.Write("\nSelect an option (1-7): ");

            string? choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ViewAllItems();
                    break;
                case "2":
                    AddNewItemMenu();
                    break;
                case "3":
                    SearchCatalogMenu();
                    break;
                case "4":
                    BorrowItemWorkflow();
                    break;
                case "5":
                    ReturnItemWorkflow();
                    break;
                case "6":
                    ViewOverdueItems();
                    break;
                case "7":
                    running = false;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Thank you for using the Library Management System! Goodbye.");
                    Console.ResetColor();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please enter a number between 1 and 7.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    private static void ShowHeader()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║         LIBRARY & RESOURCE MANAGEMENT SYSTEM (C# OOP)         ║
║          [Inheritance | Polymorphism | Abstraction | JSON]    ║
╚═══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private static void ViewAllItems()
    {
        var items = Manager.GetAllItems();
        if (items.Count == 0)
        {
            Console.WriteLine("No items in library catalog.");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"--- Catalog Items ({items.Count} Total) ---");
        Console.ResetColor();

        foreach (var item in items)
        {
            PrintItem(item);
        }
    }

    private static void PrintItem(LibraryItem item)
    {
        if (item.IsBorrowed)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(item.ToString());
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(item.ToString());
            Console.ResetColor();
        }
    }

    private static void AddNewItemMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- Select Item Type to Add ---");
        Console.ResetColor();
        Console.WriteLine("1. Book");
        Console.WriteLine("2. Magazine");
        Console.WriteLine("3. Audiobook");
        Console.Write("Choice: ");
        string? choice = Console.ReadLine()?.Trim();

        Console.Write("Title: ");
        string title = Console.ReadLine()?.Trim() ?? "Untitled";

        Console.Write("Author / Creator: ");
        string author = Console.ReadLine()?.Trim() ?? "Unknown";

        Console.Write("Publication Year: ");
        int.TryParse(Console.ReadLine()?.Trim(), out int year);
        if (year <= 0) year = DateTime.Now.Year;

        switch (choice)
        {
            case "1":
                Console.Write("Genre: ");
                string genre = Console.ReadLine()?.Trim() ?? "General";
                Console.Write("ISBN: ");
                string isbn = Console.ReadLine()?.Trim() ?? "N/A";
                Console.Write("Page Count: ");
                int.TryParse(Console.ReadLine()?.Trim(), out int pages);

                var book = new Book(title, author, year, genre, isbn, pages);
                Manager.AddItem(book);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Added Book '{book.Title}' with ID [{book.Id}].");
                break;

            case "2":
                Console.Write("Issue Number: ");
                int.TryParse(Console.ReadLine()?.Trim(), out int issueNum);
                Console.Write("Publisher: ");
                string publisher = Console.ReadLine()?.Trim() ?? "Unknown";

                var mag = new Magazine(title, author, year, issueNum, publisher);
                Manager.AddItem(mag);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Added Magazine '{mag.Title}' with ID [{mag.Id}].");
                break;

            case "3":
                Console.Write("Duration (Minutes): ");
                int.TryParse(Console.ReadLine()?.Trim(), out int duration);
                Console.Write("Narrator: ");
                string narrator = Console.ReadLine()?.Trim() ?? "Unknown";

                var audio = new Audiobook(title, author, year, duration, narrator);
                Manager.AddItem(audio);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Added Audiobook '{audio.Title}' with ID [{audio.Id}].");
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection.");
                break;
        }

        Console.ResetColor();
    }

    private static void SearchCatalogMenu()
    {
        Console.WriteLine("Search by: 1. Title  2. Author");
        Console.Write("Choice: ");
        string? mode = Console.ReadLine()?.Trim();

        Console.Write("Enter search query: ");
        string query = Console.ReadLine()?.Trim() ?? string.Empty;

        List<LibraryItem> results = mode switch
        {
            "1" => Manager.SearchByTitle(query),
            "2" => Manager.SearchByAuthor(query),
            _ => new List<LibraryItem>()
        };

        if (results.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"No items found matching '{query}'.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Found {results.Count} matching item(s):");
        Console.ResetColor();

        foreach (var item in results)
        {
            PrintItem(item);
        }
    }

    private static void BorrowItemWorkflow()
    {
        Console.Write("Enter Item ID to borrow: ");
        string id = Console.ReadLine()?.Trim() ?? string.Empty;

        var result = Manager.BorrowItem(id);
        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result.Message);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Borrowing Failed: {result.Message}");
        }
        Console.ResetColor();
    }

    private static void ReturnItemWorkflow()
    {
        Console.Write("Enter Item ID to return: ");
        string id = Console.ReadLine()?.Trim() ?? string.Empty;

        var result = Manager.ReturnItem(id);
        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result.Message);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Return Failed: {result.Message}");
        }
        Console.ResetColor();
    }

    private static void ViewOverdueItems()
    {
        var overdue = Manager.GetOverdueItems();
        if (overdue.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("No items are currently overdue!");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"--- OVERDUE ITEMS ({overdue.Count} Total) ---");
        Console.ResetColor();

        decimal totalFines = 0m;
        foreach (var (item, days, fee) in overdue)
        {
            totalFines += fee;
            Console.WriteLine($"[{item.Id}] \"{item.Title}\" ({item.GetType().Name}) | Overdue by {days} days | Late Fee Rate: {item.CalculateLateFee(1):C2}/day | Total Fee: ${fee:F2}");
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Total Accrued Fines across Library: ${totalFines:F2}");
        Console.ResetColor();
    }
}
