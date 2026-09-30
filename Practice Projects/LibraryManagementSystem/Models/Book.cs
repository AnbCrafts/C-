namespace LibraryManagementSystem.Models;

/// <summary>
/// Derived class representing a Book.
/// Demonstrates Inheritance and Polymorphism.
/// </summary>
public class Book : LibraryItem
{
    public string Genre { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PageCount { get; set; }

    public Book() { }

    public Book(string title, string author, int publicationYear, string genre, string isbn, int pageCount)
        : base(title, author, publicationYear)
    {
        Genre = genre;
        ISBN = isbn;
        PageCount = pageCount;
    }

    /// <summary>
    /// Overridden method for Book late fee ($0.50 per day overdue).
    /// </summary>
    public override decimal CalculateLateFee(int daysOverdue)
    {
        return daysOverdue <= 0 ? 0m : daysOverdue * 0.50m;
    }

    public override string GetItemTypeDetails()
    {
        return $"Book [Genre: {Genre}, Pages: {PageCount}, ISBN: {ISBN}]";
    }
}
