using System;
using System.ComponentModel.DataAnnotations;

namespace MVCPracticeProject.Models
{
    public class Patient
    {
        [Key]
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Last Name must be between 1 and 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(0, 150, ErrorMessage = "Please enter a valid age.")]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "Assigned Doctor ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Doctor ID.")]
        [Display(Name = "Doctor ID")]
        public int DoctorId { get; set; }

        [Display(Name = "Disease / Condition")]
        public string Disease { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Admission Date")]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
