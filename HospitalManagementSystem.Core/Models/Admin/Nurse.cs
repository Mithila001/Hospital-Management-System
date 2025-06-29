using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Models.Admin
{
    public class Nurse : StaffMember
    {
        public string NursingRegistrationNumber { get; set; }
        public string NursingCouncilName { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string Specialization { get; set; }
        public string Certifications_Nurce { get; set; }
        public int YearsOfExperience_Nurce { get; set; }
        public string EducationalQualifications { get; set; }
        public string ClinicalSkills { get; set; }

        // Example enum for shift preference:
        // public ShiftPattern ShiftPreference { get; set; }
        public string ShiftPreferences { get; set; }
    }
}
