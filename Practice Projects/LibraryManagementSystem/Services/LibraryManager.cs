using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

/// <summary>
/// Domain service encapsulating business rules for borrowing, returning,
/// searching, and calculating overdue fines.
/// </summary>
public class LibraryManager
{
    private readonly ILibraryRepository _repository;

    public LibraryManager(ILibraryRepository repository)
    {
        _repository = repository;
    }

    public void AddItem(LibraryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _repository.Add(item);
    }

    public List<LibraryItem> GetAllItems() => _repository.GetAll();

    public LibraryItem? GetById(string id) => _repository.GetById(id);

    public List<LibraryItem> SearchByTitle(string titleQuery)
    {
        return _repository.GetAll()
            .Where(i => i.Title.Contains(titleQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<LibraryItem> SearchByAuthor(string authorQuery)
    {
        return _repository.GetAll()
            .Where(i => i.Author.Contains(authorQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public (bool Success, string Message) BorrowItem(string id, int days = 14)
    {
        var item = _repository.GetById(id);
        if (item == null)
            return (false, $"Item with ID '{id}' was not found.");

        try
        {
            item.BorrowItem(days);
            _repository.SaveChanges();
            return (true, $"Successfully borrowed '{item.Title}'. Due date: {item.DueDate:yyyy-MM-dd}.");
        }
        catch (InvalidOperationException ex)
        {
            return (false, ex.Message);
        }
    }

    public (bool Success, string Message, decimal LateFee) ReturnItem(string id)
    {
        var item = _repository.GetById(id);
        if (item == null)
            return (false, $"Item with ID '{id}' was not found.", 0m);

        try
        {
            int overdueDays = item.GetOverdueDays();
            // Polymorphism in action: invokes the derived class implementation of CalculateLateFee
            decimal lateFee = item.CalculateLateFee(overdueDays);

            item.ReturnItem();
            _repository.SaveChanges();

            string feeMsg = overdueDays > 0 
                ? $" Item was overdue by {overdueDays} days. Late fee accrued: ${lateFee:F2}." 
                : " Returned on time (no late fee).";

            return (true, $"Successfully returned '{item.Title}'.{feeMsg}", lateFee);
        }
        catch (InvalidOperationException ex)
        {
            return (false, ex.Message, 0m);
        }
    }

    public List<(LibraryItem Item, int OverdueDays, decimal LateFee)> GetOverdueItems()
    {
        var overdueItems = new List<(LibraryItem, int, decimal)>();

        foreach (var item in _repository.GetAll())
        {
            int overdueDays = item.GetOverdueDays();
            if (overdueDays > 0)
            {
                // Polymorphic method call
                decimal fee = item.CalculateLateFee(overdueDays);
                overdueItems.Add((item, overdueDays, fee));
            }
        }

        return overdueItems;
    }
}
