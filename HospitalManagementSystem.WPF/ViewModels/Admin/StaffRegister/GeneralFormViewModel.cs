using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Enums.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.Core.Models.Admin.ViewDataModels;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister
{
    public class GeneralFormViewModel : ViewModelBase
    {
        readonly StaffRegistrationData_VDM _data;

        public ObservableCollection<StaffRole> Roles { get; }
        public ObservableCollection<Gender> Genders { get; }
        public ObservableCollection<MaritalStatus> MaritalStatuses { get; }
        public ObservableCollection<BloodGroup> BloodGroups { get; }
        public ObservableCollection<EmploymentStatus> EmploymentStatuses { get; }

        public GeneralFormViewModel(StaffRegistrationData_VDM data)
        {
            _data = data;

            Roles = new ObservableCollection<StaffRole>(
                Enum.GetValues(typeof(StaffRole)).Cast<StaffRole>());

            Genders = new ObservableCollection<Gender>(
                Enum.GetValues(typeof(Gender)).Cast<Gender>());
            MaritalStatuses = new ObservableCollection<MaritalStatus>(
                Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>());
            BloodGroups = new ObservableCollection<BloodGroup>(
                Enum.GetValues(typeof(BloodGroup)).Cast<BloodGroup>());
            EmploymentStatuses = new ObservableCollection<EmploymentStatus>(
                Enum.GetValues(typeof(EmploymentStatus)).Cast<EmploymentStatus>());
        }

        public StaffRole SelectedRole
        {
            get => _data.SelectedRole;
            set { _data.SelectedRole = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get => _data.FirstName;
            set { _data.FirstName = value; OnPropertyChanged(); }
        }

        public string MiddleName
        {
            get => _data.MiddleName;
            set { _data.MiddleName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _data.LastName;
            set { _data.LastName = value; OnPropertyChanged(); }
        }

        public DateTime? DOB
        {
            get => _data.DOB;
            set { _data.DOB = value; OnPropertyChanged(); }
        }

        public Gender Gender
        {
            get => _data.Gender;
            set { _data.Gender = value; OnPropertyChanged(); }
        }

        // …and so on for all the other “#region Staff Member” fields…
        // e.g. Nationality, NationalIdNumber, MaritalStatus, BloodGroup, etc.
    }
}
