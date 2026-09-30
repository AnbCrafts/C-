namespace LibraryManagementSystem.Models;

/// <summary>
/// Derived class representing an Audiobook.
/// Demonstrates Inheritance and Polymorphism.
/// </summary>
public class Audiobook : LibraryItem
{
    public int DurationMinutes { get; set; }
    public string Narrator { get; set; } = string.Empty;

    public Audiobook() { }

    public Audiobook(string title, string author, int publicationYear, int durationMinutes, string narrator)
        : base(title, author, publicationYear)
    {
        DurationMinutes = durationMinutes;
        Narrator = narrator;
    }

    /// <summary>
    /// Overridden method for Audiobook late fee ($0.75 per day overdue).
    /// </summary>
    public override decimal CalculateLateFee(int daysOverdue)
    {
        return daysOverdue <= 0 ? 0m : daysOverdue * 0.75m;
    }

    public override string GetItemTypeDetails()
    {
        TimeSpan duration = TimeSpan.FromMinutes(DurationMinutes);
        return $"Audiobook [Narrator: {Narrator}, Duration: {duration.Hours}h {duration.Minutes}m]";
    }
}
