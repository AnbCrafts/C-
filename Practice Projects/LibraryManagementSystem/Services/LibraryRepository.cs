using System.Text.Json;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

/// <summary>
/// Repository implementation handling JSON file persistence.
/// Demonstrates Encapsulation and Data Access Layer decoupling.
/// </summary>
public class LibraryRepository : ILibraryRepository
{
    private readonly string _filePath;
    private List<LibraryItem> _items = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public LibraryRepository(string filePath = "library_data.json")
    {
        _filePath = filePath;
        LoadData();
    }

    public List<LibraryItem> GetAll()
    {
        return _items;
    }

    public LibraryItem? GetById(string id)
    {
        return _items.FirstOrDefault(i => i.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public void Add(LibraryItem item)
    {
        _items.Add(item);
        SaveChanges();
    }

    public bool Remove(string id)
    {
        var item = GetById(id);
        if (item == null) return false;
        _items.Remove(item);
        SaveChanges();
        return true;
    }

    public void SaveChanges()
    {
        try
        {
            string json = JsonSerializer.Serialize(_items, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Failed to save library data: {ex.Message}");
        }
    }

    public void LoadData()
    {
        if (!File.Exists(_filePath))
        {
            SeedSampleData();
            return;
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            var items = JsonSerializer.Deserialize<List<LibraryItem>>(json, _jsonOptions);
            _items = items ?? new List<LibraryItem>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Warning] Failed to load data from '{_filePath}': {ex.Message}. Initializing seed data.");
            SeedSampleData();
        }
    }

    private void SeedSampleData()
    {
        _items = new List<LibraryItem>
        {
            new Book("Clean Code", "Robert C. Martin", 2008, "Software Development", "978-0132350884", 464),
            new Book("The C# Player's Guide", "RB Whitaker", 2022, "Programming", "978-0985580155", 512),
            new Magazine("IEEE Computer", "IEEE Society", 2024, 575, "IEEE"),
            new Magazine("National Geographic", "Editorial Team", 2023, 102, "NatGeo"),
            new Audiobook("Atomic Habits", "James Clear", 2018, 335, "James Clear"),
            new Audiobook("Design Patterns", "Erich Gamma et al.", 1994, 620, "Gang of Four")
        };

        // Create an overdue demo item to test fine calculations
        var overdueBook = new Book("Refactoring", "Martin Fowler", 2019, "Software Engineering", "978-0134757599", 448);
        overdueBook.BorrowItem(14);
        // Artificially set due date in the past for fine calculation testing
        overdueBook.DueDate = DateTime.Now.AddDays(-5);
        _items.Add(overdueBook);

        SaveChanges();
    }
}
