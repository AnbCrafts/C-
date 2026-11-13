using System.ComponentModel.DataAnnotations;

namespace PracticeAssignment2.DTOs.Amenities;

public class CreateAmenityRequestDto
{
    [Required(ErrorMessage = "Amenity name is required.")]
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string AmenityName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Icon code cannot exceed 50 characters.")]
    public string? IconCode { get; set; }
}

public class UpdateAmenityRequestDto
{
    [Required(ErrorMessage = "Amenity name is required.")]
    [StringLength(100, ErrorMessage = "Amenity name cannot exceed 100 characters.")]
    public string AmenityName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [StringLength(50, ErrorMessage = "Icon code cannot exceed 50 characters.")]
    public string? IconCode { get; set; }

    public bool IsActive { get; set; } = true;
}

public class AmenityResponseDto
{
    public int AmenityId { get; set; }
    public string AmenityName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconCode { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AssignRoomAmenitiesRequestDto
{
    [Required(ErrorMessage = "Amenity IDs list is required.")]
    [MinLength(1, ErrorMessage = "At least one AmenityId must be provided.")]
    public List<int> AmenityIds { get; set; } = new();
}
