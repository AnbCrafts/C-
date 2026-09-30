using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

/// <summary>
/// Interface for repository layer managing library items storage and persistence.
/// </summary>
public interface ILibraryRepository
{
    List<LibraryItem> GetAll();
    LibraryItem? GetById(string id);
    void Add(LibraryItem item);
    bool Remove(string id);
    void SaveChanges();
    void LoadData();
}
