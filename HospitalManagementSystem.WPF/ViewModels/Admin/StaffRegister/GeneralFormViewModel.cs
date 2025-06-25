using HospitalManagementSystem.Core.Enums;
using HospitalManagementSystem.Core.Enums.Admin;
using HospitalManagementSystem.Core.Models.Admin;
using HospitalManagementSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.WPF.ViewModels.Admin.StaffRegister
{
    public class GeneralFormViewModel : ViewModelBase
    {
        // Property to hold the StaffMember data
        private StaffMember _staffMember;
        public StaffMember StaffMember
        {
            get => _staffMember;
            set
            {
                _staffMember = value;
                OnPropertyChanged(nameof(StaffMember));
            }
        }
        // Property to hold the selected role from the StaffRole enum
        public ObservableCollection<StaffRole> Roles { get; }
        StaffRole? _selectedRole;
        public StaffRole? SelectedRole
        {
            get => _selectedRole;
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        // Properties for Enums used in the StaffMember model
        public ObservableCollection<Gender> Genders { get; }
        public ObservableCollection<MaritalStatus> MaritalStatuses { get; }
        public ObservableCollection<BloodGroup> BloodGroups { get; }
        public ObservableCollection<EmploymentStatus> EmploymentStatuses { get; }

        public GeneralFormViewModel()
        {
            Roles = new ObservableCollection<StaffRole>(
                Enum.GetValues(typeof(StaffRole)).Cast<StaffRole>());

            // Populate ObservableCollections for other enums
            Genders = new ObservableCollection<Gender>(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            MaritalStatuses = new ObservableCollection<MaritalStatus>(Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>());
            BloodGroups = new ObservableCollection<BloodGroup>(Enum.GetValues(typeof(BloodGroup)).Cast<BloodGroup>());
            EmploymentStatuses = new ObservableCollection<EmploymentStatus>(Enum.GetValues(typeof(EmploymentStatus)).Cast<EmploymentStatus>());
        }

    }
}
