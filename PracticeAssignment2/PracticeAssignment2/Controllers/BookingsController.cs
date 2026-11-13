using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment2.DTOs.Bookings;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.Repositories;

namespace PracticeAssignment2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;

    public BookingsController(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    /// <summary>
    /// Create a new room reservation (Atomic execution with UPDLOCK race condition safety).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseDto<BookingResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequestDto request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0)
            return Unauthorized();

        var booking = await _bookingRepository.CreateBookingAsync(userId, request, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponseDto<BookingResponseDto>.Ok(booking, "Booking created successfully."));
    }

    /// <summary>
    /// Retrieve reservations for the currently authenticated user.
    /// </summary>
    [HttpGet("my-bookings")]
    [ProducesResponseType(typeof(ApiResponseDto<List<BookingResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyBookings(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        if (userId <= 0)
            return Unauthorized();

        var list = await _bookingRepository.GetBookingsByUserIdAsync(userId, ct);
        return Ok(ApiResponseDto<List<BookingResponseDto>>.Ok(list));
    }

    /// <summary>
    /// Retrieve a booking by its human-readable reference (Owner or Admin).
    /// </summary>
    [HttpGet("{bookingReference}")]
    [ProducesResponseType(typeof(ApiResponseDto<BookingDetailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReference(string bookingReference, CancellationToken ct)
    {
        var booking = await _bookingRepository.GetBookingByReferenceAsync(bookingReference, ct);
        if (booking == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Booking with reference '{bookingReference}' was not found."
            });
        }

        var isOwner = booking.UserId == GetCurrentUserId();
        var isAdmin = string.Equals(User.FindFirstValue(ClaimTypes.Role), "Admin", StringComparison.OrdinalIgnoreCase);

        if (!isOwner && !isAdmin)
            return Forbid();

        return Ok(ApiResponseDto<BookingDetailResponseDto>.Ok(booking));
    }

    /// <summary>
    /// Retrieve all hotel bookings with optional filters (Admin only).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<PagedResultDto<BookingResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll([FromQuery] BookingFilterRequestDto filter, CancellationToken ct)
    {
        var pagedResult = await _bookingRepository.GetAllBookingsAsync(filter, ct);
        return Ok(ApiResponseDto<PagedResultDto<BookingResponseDto>>.Ok(pagedResult));
    }

    private int GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out var id) ? id : 0;
    }
}
