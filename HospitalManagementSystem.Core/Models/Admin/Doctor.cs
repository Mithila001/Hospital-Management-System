using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Models.Admin
{
    public class Doctor : StaffMember
    {
        public string MedicalRegistrationNumber { get; set; }
        public string MedicalCouncilName { get; set; }
        public string Specializations { get; set; }
        public string SubSpecializations { get; set; }
        public int YearsOfExperience_Doc { get; set; }
        public string ProfessionalMemberships { get; set; }
        public DateTime? LicenseExpiryDate_Doc { get; set; }
        public string IndemnityInsuranceDetails { get; set; }

        // Education & Certification
        public string Qualifications { get; set; }
        public int? YearOfGraduation { get; set; }
        public string Certifications_Doc { get; set; }

        // Schedule & Leave
        public string ConsultationHours { get; set; }
        public string OnCallPreferences { get; set; }
        public int TotalLeaveEntitlement { get; set; }
        public int LeaveTaken { get; set; }

        public string PublicationsJson { get; set; }
    }
}
