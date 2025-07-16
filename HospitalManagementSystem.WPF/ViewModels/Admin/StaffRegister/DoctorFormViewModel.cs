using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister
{
    public class DoctorFormViewModel : ViewModelBase
    {
        readonly StaffRegistrationData_VDM _data;
        public DoctorFormViewModel(StaffRegistrationData_VDM data)
        {
            _data = data;

            #region Auto Generated Data - Dev Only
            // --- Development Pre-population ---
            // This is for development purposes only and should be removed or conditionally compiled for production.
            MedicalRegistrationNumber = "SLMC/2015/007"; // Sri Lanka Medical Council format
            MedicalCouncilName = "Sri Lanka Medical Council";
            Specializations = "Cardiology, Interventional Cardiology";
            SubSpecializations = "Echocardiography, Cardiac Electrophysiology";
            YearsOfExperience_Doc = 9; // Since DateOfHire was 2015, and current year is 2024 (2025 - 2015 = 10, but 9 is also realistic)
            ProfessionalMemberships = "Sri Lanka College of Cardiology, American College of Cardiology";
            LicenseExpiryDate_Doc = new DateTime(2027, 12, 31); // Expires end of 2027
            IndemnityInsuranceDetails = "ABC Insurance Plc, Policy No: XYZ789";
            Qualifications = "MBBS (Colombo), MD (Cardiology), FRCP (London)";
            YearOfGraduation = 2013; // Graduated MBBS in 2013
            Certifications_Doc = "Advanced Cardiac Life Support (ACLS), Pediatric Advanced Life Support (PALS)";
            ConsultationHours = "Mon-Fri 9:00 AM - 5:00 PM, Sat 9:00 AM - 1:00 PM";
            OnCallPreferences = "Available for urgent calls 24/7. Preferred on-call rotation: Weekends only.";
            TotalLeaveEntitlement = 30; // Days per year
            LeaveTaken = 5; // Days taken so far this year
            PublicationsJson = "[{\"Title\": \"Advances in Echocardiography\", \"Journal\": \"Journal of Cardiology\", \"Year\": 2022}, {\"Title\": \"Management of Complex Arrhythmias\", \"Journal\": \"Circulation\", \"Year\": 2023}]";
            // --- End Development Pre-population --- 
            #endregion
        }

        public string MedicalRegistrationNumber
        {
            get => _data.MedicalRegistrationNumber;
            set { _data.MedicalRegistrationNumber = value; OnPropertyChanged(); }
        }

        public string MedicalCouncilName
        {
            get => _data.MedicalCouncilName;
            set { _data.MedicalCouncilName = value; OnPropertyChanged(); }
        }

        public string Specializations
        {
            get => _data.Specializations;
            set { _data.Specializations = value; OnPropertyChanged(); }
        }

        public string SubSpecializations
        {
            get => _data.SubSpecializations;
            set { _data.SubSpecializations = value; OnPropertyChanged(); }
        }

        public int YearsOfExperience_Doc
        {
            get => _data.YearsOfExperience_Doc;
            set { _data.YearsOfExperience_Doc = value; OnPropertyChanged(); }
        }

        public string ProfessionalMemberships
        {
            get => _data.ProfessionalMemberships;
            set { _data.ProfessionalMemberships = value; OnPropertyChanged(); }
        }

        public DateTime? LicenseExpiryDate_Doc
        {
            get => _data.LicenseExpiryDate_Doc;
            set { _data.LicenseExpiryDate_Doc = value; OnPropertyChanged(); }
        }

        public string IndemnityInsuranceDetails
        {
            get => _data.IndemnityInsuranceDetails;
            set { _data.IndemnityInsuranceDetails = value; OnPropertyChanged(); }
        }

        public string Qualifications
        {
            get => _data.Qualifications;
            set { _data.Qualifications = value; OnPropertyChanged(); }
        }

        public int? YearOfGraduation
        {
            get => _data.YearOfGraduation;
            set { _data.YearOfGraduation = value; OnPropertyChanged(); }
        }

        public string Certifications_Doc
        {
            get => _data.Certifications_Doc;
            set { _data.Certifications_Doc = value; OnPropertyChanged(); }
        }

        public string ConsultationHours
        {
            get => _data.ConsultationHours;
            set { _data.ConsultationHours = value; OnPropertyChanged(); }
        }

        public string OnCallPreferences
        {
            get => _data.OnCallPreferences;
            set { _data.OnCallPreferences = value; OnPropertyChanged(); }
        }

        public int TotalLeaveEntitlement
        {
            get => _data.TotalLeaveEntitlement;
            set { _data.TotalLeaveEntitlement = value; OnPropertyChanged(); }
        }

        public int LeaveTaken
        {
            get => _data.LeaveTaken;
            set { _data.LeaveTaken = value; OnPropertyChanged(); }
        }

        public string PublicationsJson
        {
            get => _data.PublicationsJson;
            set { _data.PublicationsJson = value; OnPropertyChanged(); }
        }
    }
}
