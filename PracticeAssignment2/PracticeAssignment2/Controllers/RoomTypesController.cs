using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.DTOs.RoomTypes;
using PracticeAssignment2.Repositories;

namespace PracticeAssignment2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypesController(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    /// <summary>
    /// Retrieve all room types.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<RoomTypeResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false, CancellationToken ct = default)
    {
        var list = await _roomTypeRepository.GetAllAsync(includeInactive, ct);
        return Ok(ApiResponseDto<List<RoomTypeResponseDto>>.Ok(list));
    }

    /// <summary>
    /// Retrieve a room type by RoomTypeId.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponseDto<RoomTypeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var roomType = await _roomTypeRepository.GetByIdAsync(id, ct);
        if (roomType == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"RoomType with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<RoomTypeResponseDto>.Ok(roomType));
    }

    /// <summary>
    /// Create a new room type (Admin only).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<RoomTypeResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateRoomTypeRequestDto request, CancellationToken ct)
    {
        var created = await _roomTypeRepository.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponseDto<RoomTypeResponseDto>.Ok(created, "Room type created successfully."));
    }

    /// <summary>
    /// Update an existing room type (Admin only).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<RoomTypeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomTypeRequestDto request, CancellationToken ct)
    {
        var updated = await _roomTypeRepository.UpdateAsync(id, request, ct);
        if (updated == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"RoomType with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<RoomTypeResponseDto>.Ok(updated, "Room type updated successfully."));
    }

    /// <summary>
    /// Soft delete a room type (Admin only). Blocked if active future bookings exist.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken ct)
    {
        var success = await _roomTypeRepository.SoftDeleteAsync(id, ct);
        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"RoomType with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<bool>.Ok(true, "Room type deactivated successfully."));
    }
}
