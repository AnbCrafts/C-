using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCPracticeProject.Models;

namespace MVCPracticeProject.ViewModels
{
    public class DoctorPatientViewModel
    {
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
