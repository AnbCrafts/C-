using PracticeAssignment2.DTOs.Bookings;
using PracticeAssignment2.DTOs.Common;

namespace PracticeAssignment2.Repositories;

public interface IBookingRepository
{
    Task<BookingResponseDto> CreateBookingAsync(int userId, CreateBookingRequestDto request, CancellationToken ct = default);
    Task<List<BookingResponseDto>> GetBookingsByUserIdAsync(int userId, CancellationToken ct = default);
    Task<BookingDetailResponseDto?> GetBookingByReferenceAsync(string bookingReference, CancellationToken ct = default);
    Task<PagedResultDto<BookingResponseDto>> GetAllBookingsAsync(BookingFilterRequestDto filter, CancellationToken ct = default);
}
