using PracticeAssignment2.DTOs.Cancellations;

namespace PracticeAssignment2.Repositories;

public interface ICancellationRepository
{
    Task<CancellationResponseDto> CancelBookingAsync(int bookingId, int cancelledByUserId, string? cancellationReason, CancellationToken ct = default);
    Task<CancellationResponseDto?> GetCancellationDetailsAsync(int bookingId, CancellationToken ct = default);
}
