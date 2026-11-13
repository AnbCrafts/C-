using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment2.Data;
using PracticeAssignment2.DTOs.Bookings;
using PracticeAssignment2.DTOs.Cancellations;
using PracticeAssignment2.DTOs.Common;

namespace PracticeAssignment2.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public BookingRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<BookingResponseDto> CreateBookingAsync(int userId, CreateBookingRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Booking_Create", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = request.RoomId });
        cmd.Parameters.Add(new SqlParameter("@CheckInDate", SqlDbType.Date) { Value = request.CheckInDate.Date });
        cmd.Parameters.Add(new SqlParameter("@CheckOutDate", SqlDbType.Date) { Value = request.CheckOutDate.Date });
        cmd.Parameters.Add(new SqlParameter("@NumberOfGuests", SqlDbType.Int) { Value = request.NumberOfGuests });

        var refParam = new SqlParameter("@BookingReference", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
        var idParam = new SqlParameter("@BookingId", SqlDbType.Int) { Direction = ParameterDirection.Output };
        cmd.Parameters.Add(refParam);
        cmd.Parameters.Add(idParam);

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapBooking(reader);
        }

        throw new InvalidOperationException("Failed to create booking.");
    }

    public async Task<List<BookingResponseDto>> GetBookingsByUserIdAsync(int userId, CancellationToken ct = default)
    {
        var list = new List<BookingResponseDto>();
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Booking_GetByUser", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapBooking(reader));
        }

        return list;
    }

    public async Task<BookingDetailResponseDto?> GetBookingByReferenceAsync(string bookingReference, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Booking_GetByReference", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@BookingReference", SqlDbType.NVarChar, 50) { Value = bookingReference });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
        {
            return null;
        }

        var detail = new BookingDetailResponseDto
        {
            BookingId = reader.GetInt32(reader.GetOrdinal("BookingId")),
            BookingReference = reader.GetString(reader.GetOrdinal("BookingReference")),
            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
            CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? null : reader.GetString(reader.GetOrdinal("CustomerName")),
            CustomerEmail = reader.IsDBNull(reader.GetOrdinal("CustomerEmail")) ? null : reader.GetString(reader.GetOrdinal("CustomerEmail")),
            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
            RoomNumber = reader.IsDBNull(reader.GetOrdinal("RoomNumber")) ? null : reader.GetString(reader.GetOrdinal("RoomNumber")),
            RoomTypeName = reader.IsDBNull(reader.GetOrdinal("RoomTypeName")) ? null : reader.GetString(reader.GetOrdinal("RoomTypeName")),
            CheckInDate = reader.GetDateTime(reader.GetOrdinal("CheckInDate")),
            CheckOutDate = reader.GetDateTime(reader.GetOrdinal("CheckOutDate")),
            TotalNights = reader.GetInt32(reader.GetOrdinal("TotalNights")),
            NumberOfGuests = reader.GetInt32(reader.GetOrdinal("NumberOfGuests")),
            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
            BookingStatus = reader.GetString(reader.GetOrdinal("BookingStatus")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
        };

        // If cancelled, check if cancellation columns are available
        int cancellationIdOrdinal = -1;
        try { cancellationIdOrdinal = reader.GetOrdinal("CancellationId"); } catch { }
        if (cancellationIdOrdinal >= 0 && !reader.IsDBNull(cancellationIdOrdinal))
        {
            detail.CancellationDetails = new CancellationResponseDto
            {
                CancellationId = reader.GetInt32(cancellationIdOrdinal),
                BookingId = detail.BookingId,
                BookingReference = detail.BookingReference,
                TotalAmount = detail.TotalAmount,
                CancelledByUserId = reader.GetInt32(reader.GetOrdinal("CancelledByUserId")),
                CancelledByName = reader.IsDBNull(reader.GetOrdinal("CancelledByName")) ? null : reader.GetString(reader.GetOrdinal("CancelledByName")),
                CancellationReason = reader.IsDBNull(reader.GetOrdinal("CancellationReason")) ? null : reader.GetString(reader.GetOrdinal("CancellationReason")),
                RefundAmount = reader.GetDecimal(reader.GetOrdinal("RefundAmount")),
                CancellationCharge = reader.GetDecimal(reader.GetOrdinal("CancellationCharge")),
                CancelledAt = reader.GetDateTime(reader.GetOrdinal("CancelledAt")),
                CheckInDate = detail.CheckInDate,
                CheckOutDate = detail.CheckOutDate
            };
        }

        return detail;
    }

    public async Task<PagedResultDto<BookingResponseDto>> GetAllBookingsAsync(BookingFilterRequestDto filter, CancellationToken ct = default)
    {
        var items = new List<BookingResponseDto>();
        int totalRecords = 0;

        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Booking_GetAll", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = (object?)filter.StartDate?.Date ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = (object?)filter.EndDate?.Date ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 20) { Value = (object?)filter.Status ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = filter.PageNumber });
        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = filter.PageSize });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
            items.Add(MapBooking(reader));
        }

        return new PagedResultDto<BookingResponseDto>(items, totalRecords, filter.PageNumber, filter.PageSize);
    }

    private static BookingResponseDto MapBooking(SqlDataReader reader)
    {
        return new BookingResponseDto
        {
            BookingId = reader.GetInt32(reader.GetOrdinal("BookingId")),
            BookingReference = reader.GetString(reader.GetOrdinal("BookingReference")),
            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
            CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? null : reader.GetString(reader.GetOrdinal("CustomerName")),
            CustomerEmail = reader.IsDBNull(reader.GetOrdinal("CustomerEmail")) ? null : reader.GetString(reader.GetOrdinal("CustomerEmail")),
            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
            RoomNumber = reader.IsDBNull(reader.GetOrdinal("RoomNumber")) ? null : reader.GetString(reader.GetOrdinal("RoomNumber")),
            RoomTypeName = reader.IsDBNull(reader.GetOrdinal("RoomTypeName")) ? null : reader.GetString(reader.GetOrdinal("RoomTypeName")),
            CheckInDate = reader.GetDateTime(reader.GetOrdinal("CheckInDate")),
            CheckOutDate = reader.GetDateTime(reader.GetOrdinal("CheckOutDate")),
            TotalNights = reader.GetInt32(reader.GetOrdinal("TotalNights")),
            NumberOfGuests = reader.GetInt32(reader.GetOrdinal("NumberOfGuests")),
            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
            BookingStatus = reader.GetString(reader.GetOrdinal("BookingStatus")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
        };
    }
}
