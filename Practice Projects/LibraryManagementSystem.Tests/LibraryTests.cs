using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using Xunit;

namespace LibraryManagementSystem.Tests;

public class LibraryTests
{
    [Fact]
    public void Book_CalculateLateFee_ShouldReturnCorrectAmount()
    {
        // Arrange
        LibraryItem book = new Book("Clean Code", "Robert C. Martin", 2008, "Tech", "12345", 400);

        // Act
        decimal fee = book.CalculateLateFee(5); // 5 days overdue

        // Assert: $0.50 * 5 = $2.50
        Assert.Equal(2.50m, fee);
    }

    [Fact]
    public void Magazine_CalculateLateFee_ShouldReturnCorrectAmount()
    {
        // Arrange
        LibraryItem magazine = new Magazine("IEEE Tech", "IEEE", 2024, 10, "IEEE Press");

        // Act
        decimal fee = magazine.CalculateLateFee(4); // 4 days overdue

        // Assert: $0.25 * 4 = $1.00
        Assert.Equal(1.00m, fee);
    }

    [Fact]
    public void Audiobook_CalculateLateFee_ShouldReturnCorrectAmount()
    {
        // Arrange
        LibraryItem audiobook = new Audiobook("Atomic Habits", "James Clear", 2018, 300, "James Clear");

        // Act
        decimal fee = audiobook.CalculateLateFee(2); // 2 days overdue

        // Assert: $0.75 * 2 = $1.50
        Assert.Equal(1.50m, fee);
    }

    [Fact]
    public void BorrowItem_ShouldUpdateIsBorrowedAndDueDate()
    {
        // Arrange
        LibraryItem book = new Book("Refactoring", "Martin Fowler", 2019, "Tech", "98765", 450);

        // Act
        book.BorrowItem(14);

        // Assert
        Assert.True(book.IsBorrowed);
        Assert.NotNull(book.DueDate);
        Assert.True(book.DueDate > DateTime.Now);
    }

    [Fact]
    public void BorrowItem_AlreadyBorrowed_ShouldThrowException()
    {
        // Arrange
        LibraryItem book = new Book("Refactoring", "Martin Fowler", 2019, "Tech", "98765", 450);
        book.BorrowItem(14);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => book.BorrowItem(7));
    }

    [Fact]
    public void LibraryManager_SearchByTitle_ShouldReturnMatchingItems()
    {
        // Arrange
        var testFile = $"test_repo_{Guid.NewGuid():N}.json";
        try
        {
            var repo = new LibraryRepository(testFile);
            var manager = new LibraryManager(repo);

            // Act
            var results = manager.SearchByTitle("Clean");

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, item => item.Title.Contains("Clean", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
        }
    }
}
