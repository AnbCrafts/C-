using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment2.Data;
using PracticeAssignment2.DTOs.Amenities;

namespace PracticeAssignment2.Repositories;

public class AmenityRepository : IAmenityRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AmenityRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<AmenityResponseDto> CreateAsync(CreateAmenityRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Amenity_Create", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@AmenityName", SqlDbType.NVarChar, 100) { Value = request.AmenityName });
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)request.Description ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@IconCode", SqlDbType.NVarChar, 50) { Value = (object?)request.IconCode ?? DBNull.Value });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapAmenity(reader);
        }

        throw new InvalidOperationException("Failed to create amenity.");
    }

    public async Task<List<AmenityResponseDto>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var list = new List<AmenityResponseDto>();
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Amenity_GetAll", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@IncludeInactive", SqlDbType.Bit) { Value = includeInactive });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapAmenity(reader));
        }

        return list;
    }

    public async Task<AmenityResponseDto?> UpdateAsync(int amenityId, UpdateAmenityRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Amenity_Update", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@AmenityId", SqlDbType.Int) { Value = amenityId });
        cmd.Parameters.Add(new SqlParameter("@AmenityName", SqlDbType.NVarChar, 100) { Value = request.AmenityName });
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)request.Description ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@IconCode", SqlDbType.NVarChar, 50) { Value = (object?)request.IconCode ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = request.IsActive });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapAmenity(reader);
        }

        return null;
    }

    public async Task<bool> SoftDeleteAsync(int amenityId, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_Amenity_SoftDelete", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@AmenityId", SqlDbType.Int) { Value = amenityId });

        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected > 0;
    }

    public async Task<List<AmenityResponseDto>> AssignAmenitiesToRoomAsync(int roomId, List<int> amenityIds, CancellationToken ct = default)
    {
        var list = new List<AmenityResponseDto>();
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomAmenities_Assign", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = roomId });
        cmd.Parameters.Add(new SqlParameter("@AmenityIds", SqlDbType.NVarChar, -1) { Value = string.Join(",", amenityIds) });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapAmenity(reader));
        }

        return list;
    }

    public async Task<List<AmenityResponseDto>> GetAmenitiesByRoomIdAsync(int roomId, CancellationToken ct = default)
    {
        var list = new List<AmenityResponseDto>();
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomAmenities_GetByRoomId", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = roomId });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapAmenity(reader));
        }

        return list;
    }

    public async Task<bool> RemoveAmenityFromRoomAsync(int roomId, int amenityId, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomAmenities_Remove", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomId", SqlDbType.Int) { Value = roomId });
        cmd.Parameters.Add(new SqlParameter("@AmenityId", SqlDbType.Int) { Value = amenityId });

        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected > 0;
    }

    private static AmenityResponseDto MapAmenity(SqlDataReader reader)
    {
        return new AmenityResponseDto
        {
            AmenityId = reader.GetInt32(reader.GetOrdinal("AmenityId")),
            AmenityName = reader.GetString(reader.GetOrdinal("AmenityName")),
            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
            IconCode = reader.IsDBNull(reader.GetOrdinal("IconCode")) ? null : reader.GetString(reader.GetOrdinal("IconCode")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
        };
    }
}
