namespace LibraryManagementSystem.Models;

/// <summary>
/// Derived class representing a Magazine.
/// Demonstrates Inheritance and Polymorphism.
/// </summary>
public class Magazine : LibraryItem
{
    public int IssueNumber { get; set; }
    public string Publisher { get; set; } = string.Empty;

    public Magazine() { }

    public Magazine(string title, string author, int publicationYear, int issueNumber, string publisher)
        : base(title, author, publicationYear)
    {
        IssueNumber = issueNumber;
        Publisher = publisher;
    }

    /// <summary>
    /// Overridden method for Magazine late fee ($0.25 per day overdue).
    /// </summary>
    public override decimal CalculateLateFee(int daysOverdue)
    {
        return daysOverdue <= 0 ? 0m : daysOverdue * 0.25m;
    }

    public override string GetItemTypeDetails()
    {
        return $"Magazine [Issue #: {IssueNumber}, Publisher: {Publisher}]";
    }
}
