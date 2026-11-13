using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeAssignment2.DTOs.Amenities;
using PracticeAssignment2.DTOs.Common;
using PracticeAssignment2.Repositories;

namespace PracticeAssignment2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenitiesController(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    /// <summary>
    /// Retrieve all active amenities.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseDto<List<AmenityResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false, CancellationToken ct = default)
    {
        var list = await _amenityRepository.GetAllAsync(includeInactive, ct);
        return Ok(ApiResponseDto<List<AmenityResponseDto>>.Ok(list));
    }

    /// <summary>
    /// Add a new amenity to the master catalog (Admin only).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<AmenityResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateAmenityRequestDto request, CancellationToken ct)
    {
        var created = await _amenityRepository.CreateAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, ApiResponseDto<AmenityResponseDto>.Ok(created, "Amenity created successfully."));
    }

    /// <summary>
    /// Update an amenity's details (Admin only).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<AmenityResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAmenityRequestDto request, CancellationToken ct)
    {
        var updated = await _amenityRepository.UpdateAsync(id, request, ct);
        if (updated == null)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Amenity with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<AmenityResponseDto>.Ok(updated, "Amenity updated successfully."));
    }

    /// <summary>
    /// Soft delete an amenity (Admin only).
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken ct)
    {
        var success = await _amenityRepository.SoftDeleteAsync(id, ct);
        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = $"Amenity with ID {id} was not found."
            });
        }

        return Ok(ApiResponseDto<bool>.Ok(true, "Amenity deactivated successfully."));
    }
}
