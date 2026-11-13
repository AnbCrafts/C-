using System.ComponentModel.DataAnnotations;
using PracticeAssignment2.DTOs.Amenities;

namespace PracticeAssignment2.DTOs.Search;

public class RoomSearchRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Check-in date is required.")]
    public DateTime CheckInDate { get; set; }

    [Required(ErrorMessage = "Check-out date is required.")]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 50, ErrorMessage = "Guests must be at least 1.")]
    public int? Guests { get; set; }

    public int? RoomTypeId { get; set; }

    [Range(0, 100000, ErrorMessage = "Min price must be non-negative.")]
    public decimal? MinPrice { get; set; }

    [Range(0, 100000, ErrorMessage = "Max price must be non-negative.")]
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Optional comma-separated amenity IDs (e.g., "1,2,5").
    /// </summary>
    public string? AmenityIds { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; set; } = 10;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckInDate.Date < DateTime.Today)
        {
            yield return new ValidationResult(
                "Check-in date cannot be in the past.",
                new[] { nameof(CheckInDate) });
        }

        if (CheckOutDate.Date <= CheckInDate.Date)
        {
            yield return new ValidationResult(
                "Check-out date must be strictly after the check-in date.",
                new[] { nameof(CheckOutDate) });
        }

        if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice.Value > MaxPrice.Value)
        {
            yield return new ValidationResult(
                "MinPrice cannot be greater than MaxPrice.",
                new[] { nameof(MinPrice), nameof(MaxPrice) });
        }
    }
}

public class RoomSearchResultDto
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public string? RoomTypeDescription { get; set; }
    public decimal BasePricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public int TotalNights { get; set; }
    public decimal TotalStayPrice { get; set; }
    public List<AmenityResponseDto> Amenities { get; set; } = new();
}
