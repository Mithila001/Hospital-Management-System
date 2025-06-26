using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Enums.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.ViewDataModels.Admin
{
    public class StaffRegistration_VDM
    {
        public StaffRole? SelectedRole { get; set; }
        public StaffMember_VDM Personal { get; } = new StaffMember_VDM();
        public Doctor_VDM Doctor { get; } = new Doctor_VDM();
        public Nurse_VDM Nurse { get; } = new Nurse_VDM();

        // placeholders for ReceptionistInfo, OtherStaffInfo…

    }

    public class StaffMember_VDM
    {
        public int Id { get; set; }

        // Personal Identification
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public Gender? Gender { get; set; }
        public string Nationality { get; set; }      // large list → keep string
        public string NationalIdNumber { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public BloodGroup? BloodGroup { get; set; }

        // Contact Information
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string Email { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactPhone { get; set; }

        // Address Information
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        // Employment Details
        public string EmployeeId { get; set; }
        public DateTime? DateOfHire { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
        public string ReportingManager { get; set; }

        // Bank Details
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankSwiftCode { get; set; }
        public string BankAccountHolder { get; set; }
    }

    public class Doctor_VDM
    {
        public string MedicalRegistrationNumber { get; set; }
        public string MedicalCouncilName { get; set; }
        public string Specializations { get; set; }
        public string SubSpecializations { get; set; }
        public int YearsOfExperience { get; set; }
        public string ProfessionalMemberships { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string IndemnityInsuranceDetails { get; set; }

        // Education & Certification
        public string Qualifications { get; set; }
        public int? YearOfGraduation { get; set; }
        public string Certifications { get; set; }

        // Schedule & Leave
        public string ConsultationHours { get; set; }
        public string OnCallPreferences { get; set; }
        public int TotalLeaveEntitlement { get; set; }
        public int LeaveTaken { get; set; }

        public string PublicationsJson { get; set; }
    }

    public class Nurse_VDM
    {
        public string NursingRegistrationNumber { get; set; }
        public string NursingCouncilName { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string Specialization { get; set; }
        public string Certifications { get; set; }
        public int YearsOfExperience { get; set; }
        public string EducationalQualifications { get; set; }
        public string ClinicalSkills { get; set; }

        // Example enum for shift preference:
        // public ShiftPattern ShiftPreference { get; set; }
        public string ShiftPreferences { get; set; }
    }
}
