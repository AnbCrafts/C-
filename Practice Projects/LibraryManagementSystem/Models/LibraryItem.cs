using System.Text.Json.Serialization;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models;

/// <summary>
/// Abstract base class demonstrating Abstraction and Encapsulation.
/// Defines shared properties and abstract methods for all library items.
/// Includes polymorphic JSON attributes for seamless JSON serialization.
/// </summary>
[JsonDerivedType(typeof(Book), typeDiscriminator: "book")]
[JsonDerivedType(typeof(Magazine), typeDiscriminator: "magazine")]
[JsonDerivedType(typeof(Audiobook), typeDiscriminator: "audiobook")]
public abstract class LibraryItem : IBorrowable
{
    public string Id { get; init; } = Guid.NewGuid().ToString()[..8].ToUpper();
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    // Encapsulated state properties
    public bool IsBorrowed { get; set; }
    public DateTime? BorrowDate { get; set; }
    public DateTime? DueDate { get; set; }

    protected LibraryItem() { }

    protected LibraryItem(string title, string author, int publicationYear)
    {
        Title = title;
        Author = author;
        PublicationYear = publicationYear;
    }

    public virtual void BorrowItem(int durationDays = 14)
    {
        if (IsBorrowed)
        {
            throw new InvalidOperationException($"'{Title}' is already borrowed.");
        }

        IsBorrowed = true;
        BorrowDate = DateTime.Now;
        DueDate = DateTime.Now.AddDays(durationDays);
    }

    public virtual void ReturnItem()
    {
        if (!IsBorrowed)
        {
            throw new InvalidOperationException($"'{Title}' is not currently borrowed.");
        }

        IsBorrowed = false;
        BorrowDate = null;
        DueDate = null;
    }

    public int GetOverdueDays()
    {
        if (!IsBorrowed || !DueDate.HasValue || DateTime.Now <= DueDate.Value)
        {
            return 0;
        }

        return (int)Math.Ceiling((DateTime.Now - DueDate.Value).TotalDays);
    }

    /// <summary>
    /// Abstract method demonstrating Polymorphism & Abstraction.
    /// Each derived item calculates late fee differently based on its type.
    /// </summary>
    public abstract decimal CalculateLateFee(int daysOverdue);

    /// <summary>
    /// Abstract method returning type-specific metadata string.
    /// </summary>
    public abstract string GetItemTypeDetails();

    public override string ToString()
    {
        string status = IsBorrowed ? $"[BORROWED - Due: {DueDate:yyyy-MM-dd}]" : "[AVAILABLE]";
        return $"[{Id}] \"{Title}\" by {Author} ({PublicationYear}) | {GetItemTypeDetails()} | {status}";
    }
}
