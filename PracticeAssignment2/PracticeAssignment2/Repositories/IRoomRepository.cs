using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.DTOs.Rooms;
using PracticeAssignment2.DTOs.Search;

namespace PracticeAssignment2.Repositories;

public interface IRoomRepository
{
    Task<RoomResponseDto> CreateAsync(CreateRoomRequestDto request, CancellationToken ct = default);
    Task<List<RoomResponseDto>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<RoomDetailResponseDto?> GetByIdAsync(int roomId, CancellationToken ct = default);
    Task<RoomResponseDto?> UpdateAsync(int roomId, UpdateRoomRequestDto request, CancellationToken ct = default);
    Task<bool> UpdateStatusAsync(int roomId, string status, CancellationToken ct = default);
    Task<PagedResultDto<RoomSearchResultDto>> SearchAvailableRoomsAsync(RoomSearchRequestDto request, CancellationToken ct = default);
}
