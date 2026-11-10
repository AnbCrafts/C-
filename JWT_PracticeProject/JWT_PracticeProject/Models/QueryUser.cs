using System.ComponentModel.DataAnnotations;

namespace JWT_PracticeProject.Models
{
    public class QueryUser
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(150)]
        public string? FullName { get; set; }

        [StringLength(15)]
        public string? MobileNo { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? StateName { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public string? QueryText { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        [Range(0, 9999999.99, ErrorMessage = "Query Amount must be a valid amount")]
        public decimal? QueryAmount { get; set; }
    }
}
