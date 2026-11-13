using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment2.DTOs.Cancellations;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.Repositories;

namespace PracticeAssignment2.Controllers;

[ApiController]
[Authorize]
public class CancellationsController : ControllerBase
{
    private readonly ICancellationRepository _cancellationRepository;
    private readonly IBookingRepository _bookingRepository;

    public CancellationsController(
        ICancellationRepository cancellationRepository,
        IBookingRepository bookingRepository)
    {
        _cancellationRepository = cancellationRepository;
        _bookingRepository = bookingRepository;
    }

    /// <summary>
    /// Cancel a booking with automated tiered refund calculation (Owner or Admin).
    /// </summary>
    [HttpPost("api/bookings/{bookingId:int}/cancel")]
    [ProducesResponseType(typeof(ApiResponseDto<CancellationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelBooking(int bookingId, [FromBody] CancelBookingRequestDto request, CancellationToken ct)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = string.Equals(User.FindFirstValue(ClaimTypes.Role), "Admin", StringComparison.OrdinalIgnoreCase);

        // Fetch user's bookings to verify ownership if not Admin
        if (!isAdmin)
        {
            var userBookings = await _bookingRepository.GetBookingsByUserIdAsync(currentUserId, ct);
            if (!userBookings.Any(b => b.BookingId == bookingId))
            {
                return Forbid();
            }
        }

        var cancellation = await _cancellationRepository.CancelBookingAsync(bookingId, currentUserId, request?.CancellationReason, ct);
        return Ok(ApiResponseDto<CancellationResponseDto>.Ok(cancellation, "Booking cancelled successfully."));
    }

    /// <summary>
    /// Get cancellation breakdown and refund details for a booking (Owner or Admin).
    /// </summary>
    [HttpGet("api/cancellations/{bookingId:int}")]
    [ProducesResponseType(typeof(ApiResponseDto<CancellationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCancellationDetails(int bookingId, CancellationToken ct)
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = string.Equals(User.FindFirstValue(ClaimTypes.Role), "Admin", StringComparison.OrdinalIgnoreCase);

        var details = await _cancellationRepository.GetCancellationDetailsAsync(bookingId, ct);
        if (details == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"No cancellation found for Booking ID {bookingId}."
            });
        }

        if (!isAdmin && details.CancelledByUserId != currentUserId)
        {
            return Forbid();
        }

        return Ok(ApiResponseDto<CancellationResponseDto>.Ok(details));
    }

    private int GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out var id) ? id : 0;
    }
}
