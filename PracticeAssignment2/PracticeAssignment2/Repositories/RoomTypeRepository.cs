using System.Data;
using Microsoft.Data.SqlClient;
using PracticeAssignment2.Data;
using PracticeAssignment2.DTOs.RoomTypes;

namespace PracticeAssignment2.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RoomTypeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RoomTypeResponseDto> CreateAsync(CreateRoomTypeRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomType_Create", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.NVarChar, 100) { Value = request.TypeName });
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)request.Description ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@BasePricePerNight", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = request.BasePricePerNight });
        cmd.Parameters.Add(new SqlParameter("@MaxOccupancy", SqlDbType.Int) { Value = request.MaxOccupancy });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapRoomType(reader);
        }

        throw new InvalidOperationException("Failed to create room type.");
    }

    public async Task<List<RoomTypeResponseDto>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var list = new List<RoomTypeResponseDto>();
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomType_GetAll", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@IncludeInactive", SqlDbType.Bit) { Value = includeInactive });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            list.Add(MapRoomType(reader));
        }

        return list;
    }

    public async Task<RoomTypeResponseDto?> GetByIdAsync(int roomTypeId, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomType_GetById", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomTypeId", SqlDbType.Int) { Value = roomTypeId });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapRoomType(reader);
        }

        return null;
    }

    public async Task<RoomTypeResponseDto?> UpdateAsync(int roomTypeId, UpdateRoomTypeRequestDto request, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomType_Update", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomTypeId", SqlDbType.Int) { Value = roomTypeId });
        cmd.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.NVarChar, 100) { Value = request.TypeName });
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object?)request.Description ?? DBNull.Value });
        cmd.Parameters.Add(new SqlParameter("@BasePricePerNight", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = request.BasePricePerNight });
        cmd.Parameters.Add(new SqlParameter("@MaxOccupancy", SqlDbType.Int) { Value = request.MaxOccupancy });
        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = request.IsActive });

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return MapRoomType(reader);
        }

        return null;
    }

    public async Task<bool> SoftDeleteAsync(int roomTypeId, CancellationToken ct = default)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync(ct);
        using var cmd = new SqlCommand("usp_RoomType_SoftDelete", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add(new SqlParameter("@RoomTypeId", SqlDbType.Int) { Value = roomTypeId });

        var affected = await cmd.ExecuteNonQueryAsync(ct);
        return affected > 0;
    }

    private static RoomTypeResponseDto MapRoomType(SqlDataReader reader)
    {
        return new RoomTypeResponseDto
        {
            RoomTypeId = reader.GetInt32(reader.GetOrdinal("RoomTypeId")),
            TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
            BasePricePerNight = reader.GetDecimal(reader.GetOrdinal("BasePricePerNight")),
            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
        };
    }
}
