using System.ComponentModel.DataAnnotations;

namespace PracticeAssignment2.DTOs.RoomTypes;

public class CreateRoomTypeRequestDto
{
    [Required(ErrorMessage = "Room type name is required.")]
    [StringLength(100, ErrorMessage = "Room type name cannot exceed 100 characters.")]
    public string TypeName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Base price per night is required.")]
    [Range(0.01, 100000.00, ErrorMessage = "Base price per night must be greater than 0.")]
    public decimal BasePricePerNight { get; set; }

    [Required(ErrorMessage = "Max occupancy is required.")]
    [Range(1, 50, ErrorMessage = "Max occupancy must be at least 1.")]
    public int MaxOccupancy { get; set; }
}

public class UpdateRoomTypeRequestDto
{
    [Required(ErrorMessage = "Room type name is required.")]
    [StringLength(100, ErrorMessage = "Room type name cannot exceed 100 characters.")]
    public string TypeName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Base price per night is required.")]
    [Range(0.01, 100000.00, ErrorMessage = "Base price per night must be greater than 0.")]
    public decimal BasePricePerNight { get; set; }

    [Required(ErrorMessage = "Max occupancy is required.")]
    [Range(1, 50, ErrorMessage = "Max occupancy must be at least 1.")]
    public int MaxOccupancy { get; set; }

    public bool IsActive { get; set; } = true;
}

public class RoomTypeResponseDto
{
    public int RoomTypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
