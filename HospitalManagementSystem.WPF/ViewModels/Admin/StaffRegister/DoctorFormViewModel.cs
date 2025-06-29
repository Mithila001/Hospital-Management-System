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
            => _data = data;

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
