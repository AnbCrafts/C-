using System.ComponentModel.DataAnnotations;
using PracticeAssignment2.DTOs.Amenities;

namespace PracticeAssignment2.DTOs.Rooms;

public class CreateRoomRequestDto
{
    [Required(ErrorMessage = "Room number is required.")]
    [StringLength(20, ErrorMessage = "Room number cannot exceed 20 characters.")]
    public string RoomNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Room type ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Valid RoomTypeId is required.")]
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "Floor number is required.")]
    [Range(-5, 200, ErrorMessage = "Floor number must be between -5 and 200.")]
    public int FloorNumber { get; set; }

    [RegularExpression("^(Available|Occupied|UnderMaintenance)$", ErrorMessage = "Status must be 'Available', 'Occupied', or 'UnderMaintenance'.")]
    public string Status { get; set; } = "Available";
}

public class UpdateRoomRequestDto
{
    [Required(ErrorMessage = "Room number is required.")]
    [StringLength(20, ErrorMessage = "Room number cannot exceed 20 characters.")]
    public string RoomNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Room type ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Valid RoomTypeId is required.")]
    public int RoomTypeId { get; set; }

    [Required(ErrorMessage = "Floor number is required.")]
    [Range(-5, 200, ErrorMessage = "Floor number must be between -5 and 200.")]
    public int FloorNumber { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Available|Occupied|UnderMaintenance)$", ErrorMessage = "Status must be 'Available', 'Occupied', or 'UnderMaintenance'.")]
    public string Status { get; set; } = "Available";

    public bool IsActive { get; set; } = true;
}

public class UpdateRoomStatusRequestDto
{
    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Available|Occupied|UnderMaintenance)$", ErrorMessage = "Status must be 'Available', 'Occupied', or 'UnderMaintenance'.")]
    public string Status { get; set; } = string.Empty;
}

public class RoomResponseDto
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public string? RoomTypeDescription { get; set; }
    public decimal BasePricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
}

public class RoomDetailResponseDto : RoomResponseDto
{
    public List<AmenityResponseDto> Amenities { get; set; } = new();
}
