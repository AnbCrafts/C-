using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment2.Data;
using PracticeAssignment2.DTOs.Cancellations;

namespace PracticeAssignment2.Repositories;

public class CancellationRepository : ICancellationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CancellationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CancellationResponseDto> CancelBookingAsync(int bookingId, int cancelledByUserId, string? cancellationReason, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Booking_Cancel", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int) { Value = bookingId });
        cmd.Parameters.Add(new SqlParameter("@CancelledByUserId", SqlDbType.Int) { Value = cancelledByUserId });
        cmd.Parameters.Add(new SqlParameter("@CancellationReason", SqlDbType.NVarChar, 500) { Value = (object?)cancellationReason ?? DBNull.Value });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapCancellation(reader);
        }

        throw new InvalidOperationException("Failed to cancel booking.");
    }

    public async Task<CancellationResponseDto?> GetCancellationDetailsAsync(int bookingId, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Cancellation_GetDetails", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@BookingId", SqlDbType.Int) { Value = bookingId });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapCancellation(reader);
        }

        return null;
    }

    private static CancellationResponseDto MapCancellation(SqlDataReader reader)
    {
        return new CancellationResponseDto
        {
            CancellationId = reader.GetInt32(reader.GetOrdinal("CancellationId")),
            BookingId = reader.GetInt32(reader.GetOrdinal("BookingId")),
            BookingReference = reader.IsDBNull(reader.GetOrdinal("BookingReference")) ? null : reader.GetString(reader.GetOrdinal("BookingReference")),
            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
            CancelledByUserId = reader.GetInt32(reader.GetOrdinal("CancelledByUserId")),
            CancelledByName = reader.IsDBNull(reader.GetOrdinal("CancelledByName")) ? null : reader.GetString(reader.GetOrdinal("CancelledByName")),
            CancellationReason = reader.IsDBNull(reader.GetOrdinal("CancellationReason")) ? null : reader.GetString(reader.GetOrdinal("CancellationReason")),
            RefundAmount = reader.GetDecimal(reader.GetOrdinal("RefundAmount")),
            CancellationCharge = reader.GetDecimal(reader.GetOrdinal("CancellationCharge")),
            CancelledAt = reader.GetDateTime(reader.GetOrdinal("CancelledAt")),
            CheckInDate = reader.GetDateTime(reader.GetOrdinal("CheckInDate")),
            CheckOutDate = reader.GetDateTime(reader.GetOrdinal("CheckOutDate"))
        };
    }
}
