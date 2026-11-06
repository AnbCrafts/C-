using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_PracticeProject_2.Models
{
    [Table("QueryUser", Schema = "labTest")]
    public class QueryUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

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

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? QueryAmount { get; set; }
    }
}
