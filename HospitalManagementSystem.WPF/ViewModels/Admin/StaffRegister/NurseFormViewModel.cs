using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister
{
    public class NurseFormViewModel : ViewModelBase
    {
        readonly StaffRegistrationData_VDM _data;
        public NurseFormViewModel(StaffRegistrationData_VDM data)
            => _data = data;

        public string NursingRegistrationNumber
        {
            get => _data.NursingRegistrationNumber;
            set { _data.NursingRegistrationNumber = value; OnPropertyChanged(); }
        }

        public string NursingCouncilName
        {
            get => _data.NursingCouncilName;
            set { _data.NursingCouncilName = value; OnPropertyChanged(); }
        }

        public DateTime? LicenseExpiryDate_Nurce
        {
            get => _data.LicenseExpiryDate_Nurce;
            set { _data.LicenseExpiryDate_Nurce = value; OnPropertyChanged(); }
        }

        public string Specialization
        {
            get => _data.Specialization;
            set { _data.Specialization = value; OnPropertyChanged(); }
        }

        public string Certifications_Nurce
        {
            get => _data.Certifications_Nurce;
            set { _data.Certifications_Nurce = value; OnPropertyChanged(); }
        }

        public int YearsOfExperience_Nurce
        {
            get => _data.YearsOfExperience_Nurce;
            set { _data.YearsOfExperience_Nurce = value; OnPropertyChanged(); }
        }

        public string EducationalQualifications
        {
            get => _data.EducationalQualifications;
            set { _data.EducationalQualifications = value; OnPropertyChanged(); }
        }

        public string ClinicalSkills
        {
            get => _data.ClinicalSkills;
            set { _data.ClinicalSkills = value; OnPropertyChanged(); }
        }

        public string ShiftPreferences
        {
            get => _data.ShiftPreferences;
            set { _data.ShiftPreferences = value; OnPropertyChanged(); }
        }
    }
}
