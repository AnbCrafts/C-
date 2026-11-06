using System.ComponentModel.DataAnnotations;

namespace WebAPI_PracticeProject_2.DTOs
{
    public class CreateQueryUserDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? FullName { get; set; }

        [MaxLength(15)]
        public string? MobileNo { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? StateName { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public string? QueryText { get; set; }

        public bool? IsActive { get; set; } = true;

        public decimal? QueryAmount { get; set; }
    }

    public class UpdateQueryUserDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? FullName { get; set; }

        [MaxLength(15)]
        public string? MobileNo { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? StateName { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public string? QueryText { get; set; }

        public bool IsActive { get; set; } = true;

        public decimal? QueryAmount { get; set; }
    }
}
