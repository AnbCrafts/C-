using System.ComponentModel.DataAnnotations;

namespace PracticeAssignment.Models.DTOs;

public class AssetUpdateDto
{
    [Required(ErrorMessage = "AssetName is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "AssetName must be between 2 and 100 characters.")]
    public string AssetName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "PurchaseDate is required.")]
    public DateTime PurchaseDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters.")]
    public string Status { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.0, 10000000.0, ErrorMessage = "Price must be non-negative.")]
    public decimal Price { get; set; }
}
