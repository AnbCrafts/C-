namespace LibraryManagementSystem.Interfaces;

/// <summary>
/// Interface defining borrowing and returning behaviors for library items.
/// Highlights the Interface OOP concept (contract-based design).
/// </summary>
public interface IBorrowable
{
    bool IsBorrowed { get; }
    DateTime? BorrowDate { get; }
    DateTime? DueDate { get; }

    void BorrowItem(int durationDays = 14);
    void ReturnItem();
    int GetOverdueDays();
}
