using AutoMapper;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Mappers.Admin
{
    public class StaffRegistrationMapper : Profile
    {
        public StaffRegistrationMapper()
        {
            // Mapping from StaffRegistrationData_VDM to StaffMember (base class)
            CreateMap<StaffRegistrationData_VDM, StaffMember>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) // will set by the repository
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // will set by the repository
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.DOB))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.NationalIdNumber, opt => opt.MapFrom(src => src.NationalIdNumber))
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => src.MaritalStatus))
                .ForMember(dest => dest.BloodGroup, opt => opt.MapFrom(src => src.BloodGroup))
                .ForMember(dest => dest.PrimaryPhone, opt => opt.MapFrom(src => src.PrimaryPhone))
                .ForMember(dest => dest.SecondaryPhone, opt => opt.MapFrom(src => src.SecondaryPhone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmergencyContactName, opt => opt.MapFrom(src => src.EmergencyContactName))
                .ForMember(dest => dest.EmergencyContactRelationship, opt => opt.MapFrom(src => src.EmergencyContactRelationship))
                .ForMember(dest => dest.EmergencyContactPhone, opt => opt.MapFrom(src => src.EmergencyContactPhone))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.StaffRole, opt => opt.MapFrom(src => src.SelectedRole))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.DateOfHire, opt => opt.MapFrom(src => src.DateOfHire))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.EmploymentStatus, opt => opt.MapFrom(src => src.EmploymentStatus))
                .ForMember(dest => dest.ReportingManager, opt => opt.MapFrom(src => src.ReportingManager))
                .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
                .ForMember(dest => dest.BankAccountNumber, opt => opt.MapFrom(src => src.BankAccountNumber))
                .ForMember(dest => dest.BankSwiftCode, opt => opt.MapFrom(src => src.BankSwiftCode))
                .ForMember(dest => dest.BankAccountHolder, opt => opt.MapFrom(src => src.BankAccountHolder));

            // Mapping from StaffRegistrationData_VDM to Doctor
            CreateMap<StaffRegistrationData_VDM, Doctor>()
                .IncludeBase<StaffRegistrationData_VDM, StaffMember>() // Inherit mappings from StaffMember
                .ForMember(dest => dest.MedicalRegistrationNumber, opt => opt.MapFrom(src => src.MedicalRegistrationNumber))
                .ForMember(dest => dest.MedicalCouncilName, opt => opt.MapFrom(src => src.MedicalCouncilName))
                .ForMember(dest => dest.Specializations, opt => opt.MapFrom(src => src.Specializations))
                .ForMember(dest => dest.SubSpecializations, opt => opt.MapFrom(src => src.SubSpecializations))
                .ForMember(dest => dest.YearsOfExperience_Doc, opt => opt.MapFrom(src => src.YearsOfExperience_Doc))
                .ForMember(dest => dest.ProfessionalMemberships, opt => opt.MapFrom(src => src.ProfessionalMemberships))
                .ForMember(dest => dest.LicenseExpiryDate_Doc, opt => opt.MapFrom(src => src.LicenseExpiryDate_Doc))
                .ForMember(dest => dest.IndemnityInsuranceDetails, opt => opt.MapFrom(src => src.IndemnityInsuranceDetails))
                .ForMember(dest => dest.Qualifications, opt => opt.MapFrom(src => src.Qualifications))
                .ForMember(dest => dest.YearOfGraduation, opt => opt.MapFrom(src => src.YearOfGraduation))
                .ForMember(dest => dest.Certifications_Doc, opt => opt.MapFrom(src => src.Certifications_Doc))
                .ForMember(dest => dest.ConsultationHours, opt => opt.MapFrom(src => src.ConsultationHours))
                .ForMember(dest => dest.OnCallPreferences, opt => opt.MapFrom(src => src.OnCallPreferences))
                .ForMember(dest => dest.TotalLeaveEntitlement, opt => opt.MapFrom(src => src.TotalLeaveEntitlement))
                .ForMember(dest => dest.LeaveTaken, opt => opt.MapFrom(src => src.LeaveTaken))
                .ForMember(dest => dest.PublicationsJson, opt => opt.MapFrom(src => src.PublicationsJson));

            // Mapping from StaffRegistrationData_VDM to Nurse
            CreateMap<StaffRegistrationData_VDM, Nurse>()
                .IncludeBase<StaffRegistrationData_VDM, StaffMember>() // Inherit mappings from StaffMember
                .ForMember(dest => dest.NursingRegistrationNumber, opt => opt.MapFrom(src => src.NursingRegistrationNumber))
                .ForMember(dest => dest.NursingCouncilName, opt => opt.MapFrom(src => src.NursingCouncilName))
                .ForMember(dest => dest.LicenseExpiryDate, opt => opt.MapFrom(src => src.LicenseExpiryDate_Nurce))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.Certifications_Nurce, opt => opt.MapFrom(src => src.Certifications_Nurce))
                .ForMember(dest => dest.YearsOfExperience_Nurce, opt => opt.MapFrom(src => src.YearsOfExperience_Nurce))
                .ForMember(dest => dest.EducationalQualifications, opt => opt.MapFrom(src => src.EducationalQualifications))
                .ForMember(dest => dest.ClinicalSkills, opt => opt.MapFrom(src => src.ClinicalSkills))
                .ForMember(dest => dest.ShiftPreferences, opt => opt.MapFrom(src => src.ShiftPreferences));





        }

    }
}
