using System.ComponentModel.DataAnnotations;

namespace PracticeAssignment2.DTOs.Cancellations;

public class CancelBookingRequestDto
{
    [StringLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters.")]
    public string? CancellationReason { get; set; }
}

public class CancellationResponseDto
{
    public int CancellationId { get; set; }
    public int BookingId { get; set; }
    public string? BookingReference { get; set; }
    public decimal TotalAmount { get; set; }
    public int CancelledByUserId { get; set; }
    public string? CancelledByName { get; set; }
    public string? CancellationReason { get; set; }
    public decimal RefundAmount { get; set; }
    public decimal CancellationCharge { get; set; }
    public DateTime CancelledAt { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
