using PracticeAssignment2.DTOs.Amenities;

namespace PracticeAssignment2.Repositories;

public interface IAmenityRepository
{
    Task<AmenityResponseDto> CreateAsync(CreateAmenityRequestDto request, CancellationToken ct = default);
    Task<List<AmenityResponseDto>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<AmenityResponseDto?> UpdateAsync(int amenityId, UpdateAmenityRequestDto request, CancellationToken ct = default);
    Task<bool> SoftDeleteAsync(int amenityId, CancellationToken ct = default);
    Task<List<AmenityResponseDto>> AssignAmenitiesToRoomAsync(int roomId, List<int> amenityIds, CancellationToken ct = default);
    Task<List<AmenityResponseDto>> GetAmenitiesByRoomIdAsync(int roomId, CancellationToken ct = default);
    Task<bool> RemoveAmenityFromRoomAsync(int roomId, int amenityId, CancellationToken ct = default);
}
