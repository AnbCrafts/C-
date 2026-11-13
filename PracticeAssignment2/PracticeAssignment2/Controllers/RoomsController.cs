using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment2.DTOs.Amenities;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.DTOs.Rooms;
using PracticeAssignment2.DTOs.Search;
using PracticeAssignment2.Repositories;

namespace PracticeAssignment2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IAmenityRepository _amenityRepository;

    public RoomsController(IRoomRepository roomRepository, IAmenityRepository amenityRepository)
    {
        _roomRepository = roomRepository;
        _amenityRepository = amenityRepository;
    }

    /// <summary>
    /// Retrieve all rooms joined with RoomType details.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<RoomResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false, CancellationToken ct = default)
    {
        var list = await _roomRepository.GetAllAsync(includeInactive, ct);
        return Ok(ApiResponseDto<List<RoomResponseDto>>.Ok(list));
    }

    /// <summary>
    /// Search for available rooms based on check-in/out dates, price, guests, and required amenities.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponseDto<PagedResultDto<RoomSearchResultDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] RoomSearchRequestDto request, CancellationToken ct)
    {
        var result = await _roomRepository.SearchAvailableRoomsAsync(request, ct);
        return Ok(ApiResponseDto<PagedResultDto<RoomSearchResultDto>>.Ok(result));
    }

    /// <summary>
    /// Get room details by RoomId including mapped amenities.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponseDto<RoomDetailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var room = await _roomRepository.GetByIdAsync(id, ct);
        if (room == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Room with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<RoomDetailResponseDto>.Ok(room));
    }

    /// <summary>
    /// Create a new physical room (Admin only).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<RoomResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto request, CancellationToken ct)
    {
        var created = await _roomRepository.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponseDto<RoomResponseDto>.Ok(created, "Room created successfully."));
    }

    /// <summary>
    /// Update an existing room (Admin only).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<RoomResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomRequestDto request, CancellationToken ct)
    {
        var updated = await _roomRepository.UpdateAsync(id, request, ct);
        if (updated == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Room with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<RoomResponseDto>.Ok(updated, "Room updated successfully."));
    }

    /// <summary>
    /// Transition room status between Available and UnderMaintenance (Admin only).
    /// </summary>
    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRoomStatusRequestDto request, CancellationToken ct)
    {
        var success = await _roomRepository.UpdateStatusAsync(id, request.Status, ct);
        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Room with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<bool>.Ok(true, $"Room status updated to {request.Status}."));
    }

    /// <summary>
    /// Bulk assign amenities to a room (Admin only).
    /// </summary>
    [HttpPost("{roomId:int}/amenities")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<List<AmenityResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AssignAmenities(int roomId, [FromBody] AssignRoomAmenitiesRequestDto request, CancellationToken ct)
    {
        var amenities = await _amenityRepository.AssignAmenitiesToRoomAsync(roomId, request.AmenityIds, ct);
        return Ok(ApiResponseDto<List<AmenityResponseDto>>.Ok(amenities, "Amenities assigned successfully."));
    }

    /// <summary>
    /// Get all active amenities assigned to a specific room.
    /// </summary>
    [HttpGet("{roomId:int}/amenities")]
    [ProducesResponseType(typeof(ApiResponseDto<List<AmenityResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoomAmenities(int roomId, CancellationToken ct)
    {
        var amenities = await _amenityRepository.GetAmenitiesByRoomIdAsync(roomId, ct);
        return Ok(ApiResponseDto<List<AmenityResponseDto>>.Ok(amenities));
    }

    /// <summary>
    /// Remove a specific amenity from a room (Admin only).
    /// </summary>
    [HttpDelete("{roomId:int}/amenities/{amenityId:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAmenity(int roomId, int amenityId, CancellationToken ct)
    {
        var success = await _amenityRepository.RemoveAmenityFromRoomAsync(roomId, amenityId, ct);
        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Amenity mapping for room {roomId} and amenity {amenityId} was not found."
            });
        }

        return Ok(ApiResponseDto<bool>.Ok(true, "Amenity unmapped from room successfully."));
    }
}
