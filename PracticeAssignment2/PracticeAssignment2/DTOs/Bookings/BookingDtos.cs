using System.ComponentModel.DataAnnotations;
using PracticeAssignment2.DTOs.Amenities;
using PracticeAssignment2.DTOs.Cancellations;

namespace PracticeAssignment2.DTOs.Bookings;

public class CreateBookingRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "RoomId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Valid RoomId is required.")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Check-in date is required.")]
    public DateTime CheckInDate { get; set; }

    [Required(ErrorMessage = "Check-out date is required.")]
    public DateTime CheckOutDate { get; set; }

    [Required(ErrorMessage = "Number of guests is required.")]
    [Range(1, 50, ErrorMessage = "Number of guests must be at least 1.")]
    public int NumberOfGuests { get; set; }

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
    }
}

public class BookingResponseDto
{
    public int BookingId { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public int RoomId { get; set; }
    public string? RoomNumber { get; set; }
    public string? RoomTypeName { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int TotalNights { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalAmount { get; set; }
    public string BookingStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class BookingDetailResponseDto : BookingResponseDto
{
    public CancellationResponseDto? CancellationDetails { get; set; }
    public List<AmenityResponseDto> RoomAmenities { get; set; } = new();
}

public class BookingFilterRequestDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
}
