using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.DTOs.Admin;               // StaffCredentials
using HospitalManagementSystem.Core.Interfaces.Admin;        // IStaffRegistrationRepository
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels; // StaffRegistrationData_VDM
using HospitalManagementSystem.Core.Models.Admin;            // StaffMember, Doctor, Nurse
using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.DataAccess;                   // ApplicationDbContext
using Microsoft.EntityFrameworkCore;                         // Entry()

namespace HospitalManagementSystem.DataAccess.Repositories.Admin
{
    public class StaffRegistrationRepository : IStaffRegistrationRepository
    {
        private readonly ApplicationDbContext _db;

        public StaffRegistrationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<StaffCredentials> RegisterAsync(StaffRegistrationData_VDM data)
        {
            // 1) Choose the correct subclass
            StaffMember staff;
            if (data.SelectedRole == StaffRole.Doctor)
            {
                staff = new Doctor
                {
                    MedicalRegistrationNumber = data.MedicalRegistrationNumber,
                    MedicalCouncilName = data.MedicalCouncilName,
                    Specializations = data.Specializations,
                    SubSpecializations = data.SubSpecializations,
                    YearsOfExperience_Doc = data.YearsOfExperience_Doc,
                    ProfessionalMemberships = data.ProfessionalMemberships,
                    LicenseExpiryDate_Doc = data.LicenseExpiryDate_Doc,
                    IndemnityInsuranceDetails = data.IndemnityInsuranceDetails,
                    Qualifications = data.Qualifications,
                    YearOfGraduation = data.YearOfGraduation,
                    Certifications_Doc = data.Certifications_Doc,
                    ConsultationHours = data.ConsultationHours,
                    OnCallPreferences = data.OnCallPreferences,
                    TotalLeaveEntitlement = data.TotalLeaveEntitlement,
                    LeaveTaken = data.LeaveTaken,
                    PublicationsJson = data.PublicationsJson
                };
            }
            else if (data.SelectedRole == StaffRole.Nurse)
            {
                staff = new Nurse
                {
                    NursingRegistrationNumber = data.NursingRegistrationNumber,
                    NursingCouncilName = data.NursingCouncilName,
                    LicenseExpiryDate = data.LicenseExpiryDate_Nurce,
                    Specialization = data.Specialization,
                    Certifications_Nurce = data.Certifications_Nurce,
                    YearsOfExperience_Nurce = data.YearsOfExperience_Nurce,
                    EducationalQualifications = data.EducationalQualifications,
                    ClinicalSkills = data.ClinicalSkills,
                    ShiftPreferences = data.ShiftPreferences
                };
            }
            else
            {
                // default to base for other roles
                staff = new StaffMember();
            }

            // 2) Bulk‑map the common properties
            _db.StaffMembers.Add(staff);
            _db.Entry(staff).CurrentValues.SetValues(data);

            // 3) Override generated fields
            var userName = $"{data.FirstName}{data.LastName}";
            var plainPassword = GenerateRandomPassword(8);
            staff.UserName = userName;
            staff.PasswordHash = HashPassword(plainPassword);
            staff.StaffRole = data.SelectedRole;

            await _db.SaveChangesAsync(); // single insert into StaffMembers table

            return new StaffCredentials
            {
                UserName = userName,
                Password = plainPassword
            };
        }

        // ─── Helpers ───

        string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rnd = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[rnd.Next(chars.Length)]).ToArray());
        }

        string HashPassword(string plain)
        {
            // placeholder—swap in a real hasher later
            return plain;
        }
    }
}
