using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment2.Data;
using PracticeAssignment2.DTOs.Amenities;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.DTOs.Rooms;
using PracticeAssignment2.DTOs.Search;

namespace PracticeAssignment2.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RoomRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RoomResponseDto> CreateAsync(CreateRoomRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Room_Create", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomNumber", SqlDbType.NVarChar, 20) { Value = request.RoomNumber });
        cmd.Parameters.Add(new SqlParameter("@RoomTypeId", SqlDbType.Int) { Value = request.RoomTypeId });
        cmd.Parameters.Add(new SqlParameter("@FloorNumber", SqlDbType.Int) { Value = request.FloorNumber });
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = string.IsNullOrWhiteSpace(request.Status) ? "Available" : request.Status });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapRoom(reader);
        }

        throw new InvalidOperationException("Failed to create room.");
    }

    public async Task<List<RoomResponseDto>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var list = new List<RoomResponseDto>();
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Room_GetAll", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@IncludeInactive", SqlDbType.Bit) { Value = includeInactive });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapRoom(reader));
        }

        return list;
    }

    public async Task<RoomDetailResponseDto?> GetByIdAsync(int roomId, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Room_GetById", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = roomId });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
        {
            return null;
        }

        var detail = new RoomDetailResponseDto
        {
            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
            RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
            FloorNumber = reader.GetInt32(reader.GetOrdinal("FloorNumber")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            RoomTypeId = reader.GetInt32(reader.GetOrdinal("RoomTypeId")),
            RoomTypeName = reader.GetString(reader.GetOrdinal("RoomTypeName")),
            RoomTypeDescription = reader.IsDBNull(reader.GetOrdinal("RoomTypeDescription")) ? null : reader.GetString(reader.GetOrdinal("RoomTypeDescription")),
            BasePricePerNight = reader.GetDecimal(reader.GetOrdinal("BasePricePerNight")),
            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
            Amenities = new List<AmenityResponseDto>()
        };

        // Second result set: Room Amenities
        if (await reader.NextResultAsync(ct))
        {
            while (await reader.ReadAsync(ct))
            {
                detail.Amenities.Add(new AmenityResponseDto
                {
                    AmenityId = reader.GetInt32(reader.GetOrdinal("AmenityId")),
                    AmenityName = reader.GetString(reader.GetOrdinal("AmenityName")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    IconCode = reader.IsDBNull(reader.GetOrdinal("IconCode")) ? null : reader.GetString(reader.GetOrdinal("IconCode")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }
        }

        return detail;
    }

    public async Task<RoomResponseDto?> UpdateAsync(int roomId, UpdateRoomRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Room_Update", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = roomId });
        cmd.Parameters.Add(new SqlParameter("@RoomNumber", SqlDbType.NVarChar, 20) { Value = request.RoomNumber });
        cmd.Parameters.Add(new SqlParameter("@RoomTypeId", SqlDbType.Int) { Value = request.RoomTypeId });
        cmd.Parameters.Add(new SqlParameter("@FloorNumber", SqlDbType.Int) { Value = request.FloorNumber });
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = request.Status });
        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = request.IsActive });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapRoom(reader);
        }

        return null;
    }

    public async Task<bool> UpdateStatusAsync(int roomId, string status, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Room_UpdateStatus", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = roomId });
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = status });

        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected > 0;
    }

    public async Task<PagedResultDto<RoomSearchResultDto>> SearchAvailableRoomsAsync(RoomSearchRequestDto request, CancellationToken ct = default)
    {
        var items = new List<RoomSearchResultDto>();
        int totalRecords = 0;

        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Room_SearchAvailable", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@CheckInDate", SqlDbType.Date) { Value = request.CheckInDate.Date });
        cmd.Parameters.Add(new SqlParameter("@CheckOutDate", SqlDbType.Date) { Value = request.CheckOutDate.Date });
        cmd.Parameters.Add(new SqlParameter("@Guests", SqlDbType.Int) { Value = (object?)request.Guests ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@RoomTypeId", SqlDbType.Int) { Value = (object?)request.RoomTypeId ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@MinPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = (object?)request.MinPrice ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@MaxPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = (object?)request.MaxPrice ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@AmenityIds", SqlDbType.NVarChar, -1) { Value = (object?)request.AmenityIds ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = request.PageNumber });
        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = request.PageSize });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));

            items.Add(new RoomSearchResultDto
            {
                RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
                FloorNumber = reader.GetInt32(reader.GetOrdinal("FloorNumber")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                RoomTypeId = reader.GetInt32(reader.GetOrdinal("RoomTypeId")),
                RoomTypeName = reader.GetString(reader.GetOrdinal("RoomTypeName")),
                RoomTypeDescription = reader.IsDBNull(reader.GetOrdinal("RoomTypeDescription")) ? null : reader.GetString(reader.GetOrdinal("RoomTypeDescription")),
                BasePricePerNight = reader.GetDecimal(reader.GetOrdinal("BasePricePerNight")),
                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                TotalNights = reader.GetInt32(reader.GetOrdinal("TotalNights")),
                TotalStayPrice = reader.GetDecimal(reader.GetOrdinal("TotalStayPrice")),
                Amenities = new List<AmenityResponseDto>()
            });
        }

        return new PagedResultDto<RoomSearchResultDto>(items, totalRecords, request.PageNumber, request.PageSize);
    }

    private static RoomResponseDto MapRoom(SqlDataReader reader)
    {
        return new RoomResponseDto
        {
            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
            RoomNumber = reader.GetString(reader.GetOrdinal("RoomNumber")),
            FloorNumber = reader.GetInt32(reader.GetOrdinal("FloorNumber")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
            RoomTypeId = reader.GetInt32(reader.GetOrdinal("RoomTypeId")),
            RoomTypeName = reader.GetString(reader.GetOrdinal("RoomTypeName")),
            RoomTypeDescription = reader.IsDBNull(reader.GetOrdinal("RoomTypeDescription")) ? null : reader.GetString(reader.GetOrdinal("RoomTypeDescription")),
            BasePricePerNight = reader.GetDecimal(reader.GetOrdinal("BasePricePerNight")),
            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
        };
    }
}
