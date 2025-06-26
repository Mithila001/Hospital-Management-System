using HospitalManagementSystem.WPF.ViewDataModels.Admin;
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
        readonly StaffRegistration_VDM _staffVDM;
        public DoctorFormViewModel(StaffRegistration_VDM staffData)
            => _staffVDM = staffData;

        public string MedicalRegistrationNumber
        {
            get => _staffVDM.Doctor.MedicalRegistrationNumber;
            set { _staffVDM.Doctor.MedicalRegistrationNumber = value; OnPropertyChanged(); }
        }
    }
}
