using PracticeAssignment2.DTOs.RoomTypes;

namespace PracticeAssignment2.Repositories;

public interface IRoomTypeRepository
{
    Task<RoomTypeResponseDto> CreateAsync(CreateRoomTypeRequestDto request, CancellationToken ct = default);
    Task<List<RoomTypeResponseDto>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<RoomTypeResponseDto?> GetByIdAsync(int roomTypeId, CancellationToken ct = default);
    Task<RoomTypeResponseDto?> UpdateAsync(int roomTypeId, UpdateRoomTypeRequestDto request, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(int roomTypeId, CancellationToken ct = default);
}
