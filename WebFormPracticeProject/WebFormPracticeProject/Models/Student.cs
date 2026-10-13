using System;

namespace WebFormPracticeProject.Models
{
    [Serializable]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Course { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
